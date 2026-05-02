using SqlExplainer.Core.Abstractions;
using SqlExplainer.Core.Models;
using SqlExplainer.Core.Services;
using Xunit;

namespace SqlExplainer.Core.Tests;

/// <summary>
/// WHERE/JOIN/ORDER BY を含む正常系の句説明テキスト生成を検証するテストです。
/// </summary>
public sealed class ClauseExplanationTests
{
    /// <summary>
    /// SELECT/FROMを含む場合、追加句があっても固定のSELECT/FROM説明文が優先されることを確認します。
    /// </summary>
    [Fact]
    public void Explain_WithWhereJoinOrderBy_ReturnsSelectFromFixedExplanation()
    {
        var parser = new StubParser(new SqlParseResult(
            true,
            "SELECT u.id FROM users u INNER JOIN orders o ON o.user_id = u.id WHERE u.is_active = 1 ORDER BY u.id",
            new[] { "SELECT", "FROM", "JOIN", "WHERE", "ORDER BY" },
            string.Empty));
        var sut = new ExplanationService(parser);

        var result = sut.Explain("SELECT ...");

        Assert.True(result.IsSuccess);
        Assert.Equal("このSQLはデータを取得するSELECT文です。", result.SummaryText);
        Assert.Equal("SELECT句で取得列を指定し、FROM句で対象テーブルを指定しています。", result.ClauseExplanationText);
        Assert.Equal("OK", result.MessageText);
    }

    /// <summary>
    /// FROM句を欠くSELECT句のみの解析結果では、固定文ではなく句一覧が返ることを確認します。
    /// </summary>
    [Fact]
    public void Explain_SelectOnly_ReturnsClauseListText()
    {
        var parser = new StubParser(new SqlParseResult(true, "SELECT 1", new[] { "SELECT" }, string.Empty));
        var sut = new ExplanationService(parser);

        var result = sut.Explain("SELECT 1");

        Assert.True(result.IsSuccess);
        Assert.Equal("SELECT", result.ClauseExplanationText);
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
