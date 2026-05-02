using SqlExplainer.Core.Abstractions;
using SqlExplainer.Core.Models;
using SqlExplainer.Core.Services;
using Xunit;

namespace SqlExplainer.Core.Tests;

/// <summary>
/// 空文字や不正SQLなど異常系の振る舞いを検証するテストです。
/// </summary>
public sealed class ErrorHandlingTests
{
    /// <summary>
    /// 空文字入力に対してパーサ失敗時のメッセージがそのまま返ることを確認します。
    /// </summary>
    [Fact]
    public void Explain_EmptySql_ReturnsParserErrorMessage()
    {
        var parser = new StubParser(new SqlParseResult(false, string.Empty, Array.Empty<string>(), "SQLが空です。"));
        var sut = new ExplanationService(parser);

        var result = sut.Explain(string.Empty);

        Assert.False(result.IsSuccess);
        Assert.Equal("SQLが空です。", result.MessageText);
    }

    /// <summary>
    /// 不正SQL入力に対してパーサ失敗時のメッセージがそのまま返ることを確認します。
    /// </summary>
    [Fact]
    public void Explain_InvalidSql_ReturnsParserErrorMessage()
    {
        var parser = new StubParser(new SqlParseResult(false, string.Empty, Array.Empty<string>(), "SQL構文が不正です。"));
        var sut = new ExplanationService(parser);

        var result = sut.Explain("SELECT FROM");

        Assert.False(result.IsSuccess);
        Assert.Equal("SQL構文が不正です。", result.MessageText);
    }

    /// <summary>
    /// 固定の <see cref="SqlParseResult"/> を返すテスト用パーサです。
    /// </summary>
    /// <param name="parseResult">返却する解析結果。</param>
    private sealed class StubParser(SqlParseResult parseResult) : ISqlParser
    {
        /// <summary>
        /// 引数に関係なく固定の解析結果を返します。
        /// </summary>
        /// <param name="sql">入力SQL（未使用）。</param>
        /// <returns>固定の解析結果。</returns>
        public SqlParseResult Parse(string? sql) => parseResult;
    }
}
