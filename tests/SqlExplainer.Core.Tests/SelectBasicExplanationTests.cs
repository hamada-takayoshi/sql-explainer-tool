using SqlExplainer.Core.Abstractions;
using SqlExplainer.Core.Models;
using SqlExplainer.Core.Services;
using Xunit;

namespace SqlExplainer.Core.Tests;

/// <summary>
/// 基本的なSELECT文説明の正常系・異常系を検証するテストです。
/// </summary>
public sealed class SelectBasicExplanationTests
{
    /// <summary>
    /// SELECT句とFROM句を含む解析結果から、期待する要約文と句説明文が生成されることをテストします。
    /// </summary>
    [Fact]
    public void Explain_SelectWithFrom_ReturnsExpectedSummaryAndClauseExplanation()
    {
        var parser = new StubParser(new SqlParseResult(true, "SELECT id FROM users", new[] { "SELECT", "FROM" }, string.Empty));
        var sut = new ExplanationService(parser);

        var result = sut.Explain("SELECT id FROM users");

        Assert.True(result.IsSuccess);
        Assert.Equal("このSQLはデータを取得するSELECT文です。", result.SummaryText);
        Assert.Equal("SELECT句で取得列を指定し、FROM句で対象テーブルを指定しています。", result.ClauseExplanationText);
        Assert.Equal("OK", result.MessageText);
    }

    /// <summary>
    /// パーサが失敗した場合に、説明サービスがエラーメッセージをそのまま返すことをテストします。
    /// </summary>
    [Fact]
    public void Explain_ParserFailure_PropagatesError()
    {
        var parser = new StubParser(new SqlParseResult(false, string.Empty, Array.Empty<string>(), "SQLを解析できませんでした。"));
        var sut = new ExplanationService(parser);

        var result = sut.Explain("broken sql");

        Assert.False(result.IsSuccess);
        Assert.Equal("SQLを解析できませんでした。", result.MessageText);
    }

    /// <summary>
    /// 固定の <see cref="SqlParseResult"/> を返すテスト用パーサです。
    /// </summary>
    /// <param name="parseResult">返却する解析結果。</param>
    private sealed class StubParser(SqlParseResult parseResult) : ISqlParser
    {
        /// <summary>
        /// 引数に関係なくコンストラクタで受け取った解析結果を返します。
        /// </summary>
        /// <param name="sql">入力SQL（未使用）。</param>
        /// <returns>固定の解析結果。</returns>
        public SqlParseResult Parse(string? sql) => parseResult;
    }
}
