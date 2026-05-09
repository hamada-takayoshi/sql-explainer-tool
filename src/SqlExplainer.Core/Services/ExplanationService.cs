using SqlExplainer.Core.Abstractions;
using SqlExplainer.Core.Models;

namespace SqlExplainer.Core.Services;

/// <summary>
/// <see cref="ISqlParser"/> の解析結果をもとに、画面表示用の説明文を生成します。
/// </summary>
public sealed class ExplanationService(ISqlParser parser) : IExplanationService
{
    private static readonly IReadOnlyDictionary<string, string> ClauseExplanations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["SELECT"] = "SELECT句で取得する列を指定しています。",
        ["FROM"] = "FROM句で対象テーブルを指定しています。",
        ["JOIN"] = "JOIN句でテーブルを結合しています。",
        ["WHERE"] = "WHERE句で抽出条件を指定しています。",
        ["GROUP BY"] = "GROUP BY句で集計単位を指定しています。",
        ["HAVING"] = "HAVING句で集計結果に対する条件を指定しています。",
        ["ORDER BY"] = "ORDER BY句で並び順を指定しています。"
    };

    /// <summary>
    /// SQL文字列を説明文へ変換します。
    /// </summary>
    /// <param name="sql">説明対象のSQL文字列。</param>
    /// <returns>説明結果DTO。</returns>
    public SqlExplanationResult Explain(string? sql)
    {
        var parse = parser.Parse(sql);

        if (!parse.IsSuccess)
        {
            return new SqlExplanationResult(false, string.Empty, string.Empty, Array.Empty<string>(), parse.ErrorMessage);
        }

        var summary = BuildSummary(parse.Clauses);
        var clauseText = BuildClauseExplanation(parse.Clauses);

        return new SqlExplanationResult(true, summary, clauseText, Array.Empty<string>(), "OK");
    }

    private static string BuildSummary(IReadOnlyList<string> clauses)
    {
        var hasSelect = clauses.Any(c => string.Equals(c, "SELECT", StringComparison.OrdinalIgnoreCase));
        return hasSelect
            ? "このSQLはデータを取得するSELECT文です。"
            : "このSQLの種別を判定できませんでした。";
    }

    private static string BuildClauseExplanation(IReadOnlyList<string> clauses)
    {
        var explanations = new List<string>();

        foreach (var clause in clauses)
        {
            if (ClauseExplanations.TryGetValue(clause, out var text))
            {
                explanations.Add(text);
                continue;
            }

            explanations.Add($"{clause}句が含まれています。");
        }

        return string.Join(" ", explanations);
    }
}
