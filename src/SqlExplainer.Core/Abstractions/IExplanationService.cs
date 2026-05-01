using SqlExplainer.Core.Models;

namespace SqlExplainer.Core.Abstractions;

/// <summary>
/// ユーザー向けのSQL説明文を生成するインターフェースです。
/// </summary>
public interface IExplanationService
{
    SqlExplanationResult Explain(string? sql);
}
