using SqlExplainer.Core.Abstractions;
using SqlExplainer.Core.Models;

namespace SqlExplainer.Core.Services;

/// <summary>
/// <see cref="ISqlParser"/> の解析結果をもとに、画面表示用の説明文を生成する最小実装です。
/// </summary>
public sealed class ExplanationService(ISqlParser parser) : IExplanationService
{
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

        var hasSelect = parse.Clauses.Any(c => string.Equals(c, "SELECT", StringComparison.OrdinalIgnoreCase));
        var hasFrom = parse.Clauses.Any(c => string.Equals(c, "FROM", StringComparison.OrdinalIgnoreCase));

        var summary = hasSelect
            ? "このSQLはデータを取得するSELECT文です。"
            : "このSQLの種別を判定できませんでした。";

        var clauseText = hasSelect && hasFrom
            ? "SELECT句で取得列を指定し、FROM句で対象テーブルを指定しています。"
            : string.Join(" / ", parse.Clauses);

        return new SqlExplanationResult(true, summary, clauseText, Array.Empty<string>(), "OK");
    }
}
