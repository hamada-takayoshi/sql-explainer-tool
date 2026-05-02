using SqlExplainer.Core.Abstractions;
using SqlExplainer.Core.Models;
using SqlExplainer.Core.Services;
using Xunit;

namespace SqlExplainer.Core.Tests;

/// <summary>
/// 長文SQLや改行・空白揺れを想定した境界値ケースを検証するテストです。
/// </summary>
public sealed class BoundaryInputTests
{
    /// <summary>
    /// 長文SQL想定でもSELECT/FROMを含む解析結果なら安定して成功結果を返すことを確認します。
    /// </summary>
    [Fact]
    public void Explain_LongSqlLikeInput_ReturnsStableSuccess()
    {
        var parser = new StubParser(new SqlParseResult(
            true,
            new string('X', 4096),
            new[] { "SELECT", "FROM" },
            string.Empty));
        var sut = new ExplanationService(parser);

        var result = sut.Explain(new string('X', 4096));

        Assert.True(result.IsSuccess);
        Assert.Equal("このSQLはデータを取得するSELECT文です。", result.SummaryText);
        Assert.Equal("SELECT句で取得列を指定し、FROM句で対象テーブルを指定しています。", result.ClauseExplanationText);
    }

    /// <summary>
    /// 改行や余分な空白を含む入力想定でも、SELECT/FROMを含む場合は固定説明が安定して返ることを確認します。
    /// </summary>
    [Fact]
    public void Explain_NewLineAndWhitespaceVariant_ReturnsClauseExplanation()
    {
        var parser = new StubParser(new SqlParseResult(
            true,
            "SELECT\n  id\nFROM   users\nWHERE  id > 10",
            new[] { "SELECT", "FROM", "WHERE" },
            string.Empty));
        var sut = new ExplanationService(parser);

        var result = sut.Explain("SELECT\n  id\nFROM   users\nWHERE  id > 10");

        Assert.True(result.IsSuccess);
        Assert.Equal("SELECT句で取得列を指定し、FROM句で対象テーブルを指定しています。", result.ClauseExplanationText);
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
