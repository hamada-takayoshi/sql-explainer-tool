# テスト方針（最小）

本ドキュメントは、MVP（SELECT / FROM / WHERE 対応）に対する最小の確認シナリオを定義する。  
P5-02 の完了条件として、各シナリオに期待結果を明記する。

## 1. 対象範囲

- 対象: SQL説明ツール MVP
- 目的: DoD の「基本動作確認」を主観なく実施できる状態にする
- 手法: 手動確認（UI操作ベース） + 最小単体テスト（メソッド単位）

## 2. 前提条件

- アプリが起動できること
- SQL入力欄、実行ボタン、説明表示欄、メッセージ欄が表示されていること
- 実装スコープは `SELECT` / `FROM` / `WHERE` のみであること
- 単体テストでは `SqlExplanationService.Explain(string sql)` を直接呼び出せること

## 3. 最小単体テスト（メソッドレベル）

UIの動作確認だけでは不具合の切り分けが難しいため、
`SqlExplanationService.Explain` に対して最小限の入力/出力契約テストを追加する。

### UT-01 正常系（SELECT / FROM / WHERE）

- 入力例:
  - `SELECT name FROM users WHERE age > 20`
- 検証観点:
  - `IsSuccess = true`
  - `ClauseExplanationText` が空でない
  - `MessageText` が成功系メッセージである

### UT-02 空入力

- 入力例:
  - `""`（空文字）
- 検証観点:
  - `IsSuccess = false`
  - `MessageText` に入力不足を示す文言が含まれる
  - `SummaryText` / `ClauseExplanationText` が空（またはUI表示対象外として扱える値）

### UT-03 未対応入力（JOIN を含む）

- 入力例:
  - `SELECT u.name FROM users u JOIN departments d ON u.department_id = d.id`
- 検証観点:
  - `Warnings` に未対応構文を示す要素が含まれる
  - 通常の成功説明のみを返さない（`MessageText` で注意喚起される）

### UT-04 前後空白を含む入力

- 入力例:
  - `  SELECT name FROM users  `
- 検証観点:
  - 前後空白が原因で失敗しない
  - `IsSuccess = true`

## 4. 手動確認シナリオ（UIレベル）

### S-01 正常系（SELECT / FROM / WHERE）

- 入力例:
  - `SELECT name FROM users WHERE age > 20`
- 手順:
  1. SQL入力欄に入力例を貼り付ける
  2. 実行ボタンを押す
- 期待結果:
  - 説明表示欄に「何を取得するか（SELECT）」「どこから取得するか（FROM）」「どの条件で絞るか（WHERE）」に相当する説明が表示される
  - メッセージ欄にエラーが表示されない

### S-02 空入力

- 入力例:
  - `""`（空文字）
- 手順:
  1. SQL入力欄を空のままにする（または空文字を入力する）
  2. 実行ボタンを押す
- 期待結果:
  - メッセージ欄に入力を促すエラーメッセージが表示される
  - 説明表示欄には有効な説明文が表示されない（既存説明をクリア、または更新しない）

### S-03 未対応入力（JOIN を含む）

- 入力例:
  - `SELECT u.name FROM users u JOIN departments d ON u.department_id = d.id`
- 手順:
  1. SQL入力欄に入力例を貼り付ける
  2. 実行ボタンを押す
- 期待結果:
  - メッセージ欄に未対応構文を含む旨（JOIN未対応）が表示される
  - 説明表示欄には未対応であることが分かる表示となる（通常の成功説明を出さない）

## 5. 判定ルール

- 単体テスト（UT-01〜UT-04）がすべてPassであること
- 手動シナリオ（S-01〜S-03）がすべてPassであること
- 1つでも満たさない場合は Fail とする
- DoD 判定時は UT / S の結果を記録する

## 6. 記録フォーマット（例）

- 実施日: YYYY-MM-DD
- 実施者: <name>
- UT-01: Pass / Fail（メモ）
- UT-02: Pass / Fail（メモ）
- UT-03: Pass / Fail（メモ）
- UT-04: Pass / Fail（メモ）
- S-01: Pass / Fail（メモ）
- S-02: Pass / Fail（メモ）
- S-03: Pass / Fail（メモ）
- 総合: Pass / Fail
