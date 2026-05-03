using System.Text.RegularExpressions;
using SqlExplainer.Core.Abstractions;
using SqlExplainer.Core.Models;

namespace SqlExplainer.Core.Services;

/// <summary>
/// SQL文字列を正規化し、説明生成で利用する主要句を抽出するパーサです。
/// </summary>
public sealed class SqlParser : ISqlParser
{
    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    private static readonly string[] ClauseKeywords =
    [
        "SELECT",
        "FROM",
        "JOIN",
        "WHERE",
        "GROUP BY",
        "HAVING",
        "ORDER BY"
    ];

    /// <summary>
    /// 入力SQLを正規化し、主要句を抽出した中間結果を返します。
    /// </summary>
    /// <param name="sql">解析対象のSQL文字列。</param>
    /// <returns>解析成否、正規化SQL、抽出句、メッセージを含む結果。</returns>
    public SqlParseResult Parse(string? sql)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            return new SqlParseResult(false, string.Empty, Array.Empty<string>(), "SQLが空です。");
        }

        var normalized = Normalize(sql);
        if (normalized.Length == 0)
        {
            return new SqlParseResult(false, string.Empty, Array.Empty<string>(), "SQLが空です。");
        }

        var clauses = ExtractClauses(normalized);
        if (clauses.Count == 0)
        {
            return new SqlParseResult(false, normalized, Array.Empty<string>(), "SQL構文が不正です。");
        }

        return new SqlParseResult(true, normalized, clauses, "OK");
    }

    private static string Normalize(string sql)
    {
        var normalizedWhitespace = WhitespaceRegex.Replace(sql.Trim(), " ");
        return normalizedWhitespace.TrimEnd(';');
    }

    private static IReadOnlyList<string> ExtractClauses(string normalizedSql)
    {
        var upper = normalizedSql.ToUpperInvariant();
        var found = new List<string>();

        foreach (var clauseKeyword in ClauseKeywords)
        {
            if (!ContainsClause(upper, clauseKeyword))
            {
                continue;
            }

            found.Add(clauseKeyword);
        }

        return found;
    }

    private static bool ContainsClause(string sqlUpper, string clauseKeyword)
    {
        var pattern = $@"(?<![A-Z0-9_]){Regex.Escape(clauseKeyword)}(?![A-Z0-9_])";
        return Regex.IsMatch(sqlUpper, pattern);
    }
}
