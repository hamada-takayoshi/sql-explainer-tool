# AGENTS.md

## 目的 (Goal)
シンプルで保守しやすい C# デスクトップアプリケーションを構築する

---

## ルール (Rules)

- 標準的な .NET のプロジェクト構成を使用する
- 依存関係は最小限に抑える (keep dependencies minimal)
- 指定がない場合は WPF または WinForms を使用する
- 常にビルド可能な状態を維持する (ensure buildability)

---

## コード方針 (Code Guidelines)

- クラスは小さく保つ (keep classes small)
- UI とロジックは分離する (separate UI and logic)
- 過剰設計を避ける (avoid over-engineering)
- 必要に応じてテストを追加する (add tests if applicable)

---

## 出力要件 (Output Requirements)

必ず以下を含めること：

- 変更したファイル一覧 (changed files)
- 変更内容の要約 (summary of changes)
- 変更理由 (reason for changes)
- ビルドに関する考慮点 (build considerations)
- 次にやるべきこと (next steps)

---

## 開発フロー (Workflow)

1. リポジトリを分析する (analyze repository)
2. 実装計画を提案する (propose plan)
3. 最小構成の動作するバージョンを実装する (implement minimal working version)
4. ビルドが通ることを確認する (ensure build passes)
5. 必要に応じて README を更新する (update README if needed)
