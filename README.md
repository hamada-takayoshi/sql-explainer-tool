# SQL説明ツール

## 概要

本アプリは、SQLクエリを人間が理解しやすい文章に変換する  
C#製のデスクトップアプリケーションです。

主に以下の用途を想定しています：

- SQLの学習補助
- クエリ内容の理解支援
- 複雑なSQLの可読化

---

## 主な機能（初期スコープ）

- SQLクエリの入力
- SQLの簡易解析
- 人間が読める形式での説明表示

※ 最初は最小構成で実装し、段階的に機能追加を行う

---

## 技術スタック

- C# / .NET（最新安定版）
- WPF または WinForms（初期実装はシンプルなUI）

---

## 使い方（開発者向け）

1. 本リポジトリをベースに開発を行う
2. AGENTS.md に従って実装を進める
3. 小さな単位で変更・検証を行う
4. 必要に応じて README を更新する

---

### 初期指示（Codex用）

```text
Read this repository and propose a minimal implementation plan for a C# desktop application that explains SQL queries.
Do not code yet.
```

※日本語訳：
このリポジトリを読み取り、SQLを説明するC#デスクトップアプリの最小構成の実装計画を作成してください。
まだコードは書かないでください。

次に：

```text
Implement the smallest working version.
The application should allow input of an SQL query and display a simple explanation.
Keep changes minimal and reviewable.
```

※日本語訳：
最小構成で動作するバージョンを実装してください。
SQLを入力でき、その内容を簡単に説明表示するアプリにしてください。
変更は最小限にし、レビューしやすい形にしてください。

---

## 開発方針

- シンプルな構成を優先する
- 不要な依存関係を追加しない
- 小さく作って段階的に改善する
- 常にビルド可能な状態を保つ

---

## 今後の拡張例

- SQL構文ごとの詳細説明
- ハイライト表示
- エラーチェック
- 複雑なクエリ対応（JOIN / サブクエリ等）

---

## 注意事項

- 本ツールは学習用途を主目的とする
- 完全なSQLパーサーではなく、段階的に精度を上げる
