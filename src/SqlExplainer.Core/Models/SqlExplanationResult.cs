namespace SqlExplainer.Core.Models;

/// <summary>
/// UI表示用に説明サービスから返却されるDTOです。
/// </summary>
public sealed record SqlExplanationResult(
    bool IsSuccess,
    string SummaryText,
    string ClauseExplanationText,
    IReadOnlyList<string> Warnings,
    string MessageText
);
