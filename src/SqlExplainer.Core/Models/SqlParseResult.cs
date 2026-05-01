namespace SqlExplainer.Core.Models;

/// <summary>
/// 単体テストでモック可能なパース結果を表します。
/// </summary>
public sealed record SqlParseResult(
    bool IsSuccess,
    string NormalizedSql,
    IReadOnlyList<string> Clauses,
    string ErrorMessage
);
