# Handoff — 2026-05-28 00:00

## 我們上次做到哪

完成 **送審功能**:Employee 在 Index 頁面可以把 Draft 單子送審變 Submitted。
PR #2 走完完整流程:開 branch → commit → push → GitHub PR → merge → 刪 branch → main pull。

---

## 完成的工作

- `ExpensesController.cs`:
  - 新增獨立 `Submitted(int id)` action(`[HttpPost]`) — 把指定 expense 的 status 改為 Submitted
  - `Index()`:依 role 分流查詢 — Manager 看 `Status != Draft`,Employee 看全部(未刪除)
- `Views/Expenses/Index.cshtml`:
  - 送審、修改 改為 `<form>` + `<button>` 形式(取代原本的 `<a>`)
  - 刪除也改為 `<form>` + `<button>`
  - `@if (!User.IsInRole("Manager"))` 包住送審 button — Manager 不看到送審
  - 修改 button 加條件 class:`User.IsInRole("Manager") ? "" : "EditBtn"`
  - JS:`querySelectorAll(".SubmitButton, .EditBtn")` + `forEach` 判斷 status,隱藏已審核/已拒絕/審核中的 button

---

## 進行中的工作

無。

---

## 下次起手建議

Roadmap #4:**申請者欄位** — 記錄是誰送的單子,Index 只顯示自己的申請單。

需要:
- `Expense.cs` 新增 `UserId` 或 `ApplicantName` 欄位
- Migration
- `Create POST` 存入目前登入 user 的 id
- `Index()` Employee 只撈自己的單子

---

## 對下個 session 的提醒

- `querySelectorAll` vs `querySelector` 今天搞清楚了(All 回傳集合,需要 forEach)
- `data-*` attribute 在 JS 裡讀取是全小寫(`dataset.statusbtn` 不是 `dataset.statusBtn`)
- CSS 多選器語法:`.ClassA, .ClassB` 可以一起選
- `<a>` 做 POST 不行,要用 `<form method="post">` — 今天踩到學會了
- Amber 對設計決策的思考過程比較慢(例如 Manager 要看哪些 status 繞了一圈),但最終判斷力是對的,不需要擔心
- commit message 格式:`feat:` 前綴已熟悉,文字描述還在練

---

## 環境狀態快照

- **git**: `main`,已與 origin/main 同步(PR #2 merged),無進行中 feature branch
- **DB**: 5 個 migration 累積(最新:RenameIsDeleted),Expenses table 含 Status、RejectionReason、IsDeleted
- **可跑性**: `dotnet run` → `http://localhost:5242`
