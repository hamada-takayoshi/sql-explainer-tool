using SqlExplainer.Core.Services;
using Xunit;

namespace SqlExplainer.Core.Tests;

/// <summary>
/// <see cref="SqlParser"/> の前処理（正規化・句抽出）を検証するテストです。
/// </summary>
public sealed class SqlParserTests
{
    /// <summary>
    /// 空文字入力時に失敗し、期待メッセージを返すことを確認します。
    /// </summary>
    [Fact]
    public void Parse_EmptySql_ReturnsError()
    {
        var sut = new SqlParser();

        var result = sut.Parse("  ");

        Assert.False(result.IsSuccess);
        Assert.Equal("SQLが空です。", result.ErrorMessage);
    }

    /// <summary>
    /// 改行や余分な空白を含むSQLが正規化され、主要句が抽出されることを確認します。
    /// </summary>
    [Fact]
    public void Parse_WhitespaceAndNewLine_NormalizesAndExtractsClauses()
    {
        var sut = new SqlParser();

        var result = sut.Parse("SELECT\n id  FROM   users\n WHERE  id > 1 ORDER   BY id;");

        Assert.True(result.IsSuccess);
        Assert.Equal("SELECT id FROM users WHERE id > 1 ORDER BY id", result.NormalizedSql);
        Assert.Equal(new[] { "SELECT", "FROM", "WHERE", "ORDER BY" }, result.Clauses);
    }

    /// <summary>
    /// SQL句が見つからない入力では構文エラー扱いになることを確認します。
    /// </summary>
    [Fact]
    public void Parse_NoKnownClause_ReturnsSyntaxError()
    {
        var sut = new SqlParser();

        var result = sut.Parse("foo bar baz");

        Assert.False(result.IsSuccess);
        Assert.Equal("SQL構文が不正です。", result.ErrorMessage);
    }
}
