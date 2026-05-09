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
    /// WHERE/JOIN/ORDER BYなどの複数句を含む場合に、各句の説明文が連結されることを確認します。
    /// </summary>
    [Fact]
    public void Explain_WithWhereJoinOrderBy_ReturnsAllClauseExplanations()
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
        Assert.Equal("SELECT句で取得する列を指定しています。 FROM句で対象テーブルを指定しています。 JOIN句でテーブルを結合しています。 WHERE句で抽出条件を指定しています。 ORDER BY句で並び順を指定しています。", result.ClauseExplanationText);
        Assert.Equal("OK", result.MessageText);
    }

    /// <summary>
    /// FROM句を欠くSELECT句のみの解析結果でも、SELECT句の自然文が返ることを確認します。
    /// </summary>
    [Fact]
    public void Explain_SelectOnly_ReturnsClauseListText()
    {
        var parser = new StubParser(new SqlParseResult(true, "SELECT 1", new[] { "SELECT" }, string.Empty));
        var sut = new ExplanationService(parser);

        var result = sut.Explain("SELECT 1");

        Assert.True(result.IsSuccess);
        Assert.Equal("SELECT句で取得する列を指定しています。", result.ClauseExplanationText);
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
