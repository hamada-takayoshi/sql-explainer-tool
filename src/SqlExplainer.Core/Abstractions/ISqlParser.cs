using SqlExplainer.Core.Models;

namespace SqlExplainer.Core.Abstractions;

/// <summary>
/// 生のSQL文字列を中間表現へ変換するインターフェースです。
/// </summary>
/// <remarks>
/// 実装詳細（手書きパーサ / 外部パッケージ利用）を隠蔽するための境界です。
/// 呼び出し側はこの契約のみへ依存し、実装差し替え時も変更不要であることを意図します。
/// </remarks>
public interface ISqlParser
{
    /// <summary>
    /// SQL文字列を解析し、説明生成に必要な中間結果を返します。
    /// </summary>
    /// <param name="sql">解析対象のSQL文字列。</param>
    /// <returns>解析結果。</returns>
    SqlParseResult Parse(string? sql);
}
