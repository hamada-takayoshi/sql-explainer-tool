using SqlExplainer.Core.Models;

namespace SqlExplainer.Core.Abstractions;

/// <summary>
/// 生のSQL文字列を中間表現へ変換するインターフェースです。
/// </summary>
public interface ISqlParser
{
    SqlParseResult Parse(string? sql);
}
