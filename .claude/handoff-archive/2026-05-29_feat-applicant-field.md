# Handoff — 2026-05-29 00:00

## 我們上次做到哪

完成 **申請者欄位(Roadmap #4)**:`Expense` 新增 `ApplicantId`,Create 存入登入者 GUID,Index Employee 只撈自己的單子。
另外做了 UI 美工調整(畫面樣式 + 密碼明碼切換)。
PR #3 走完完整流程,main 乾淨。

---

## 完成的工作

- `Expense.cs`:新增 `public string? ApplicantId { get; set; }`
- Migration:`20260529070325_ExpenseApplicantId`
- `ExpensesController.cs`:
  - `Create POST`:`expense.ApplicantId = User.FindFirstValue(ClaimTypes.NameIdentifier)`
  - `Index()`:Employee 查詢加 `e.ApplicantId == userId` 條件
- View 美工調整:Register、Create、Delete、Edit、Index、Login 頁面樣式更新,Login 加密碼明碼切換
- 兩個 commit(功能 + style)分開下

---

## 進行中的工作

無。

---

## 下次起手建議

Roadmap #5:**補件退件** — Manager 退件後,Employee 可以修改並重送。

需要討論:
- 哪些 status 下 Employee 可以編輯?(目前 Edit 沒有限制 Employee 改 Submitted/Approved 的單)
- 退件後 Employee 改完重送,要重置 status 為 Submitted 嗎?

---

## 對下個 session 的提醒

- `User.FindFirstValue(ClaimTypes.NameIdentifier)` 取 GUID UserId,`User.Identity.Name` 取 username(email),兩個不同
- `git add .` 會一次 stage 全部,要分 commit 時記得一個個 add
- commit message 前綴:`feat:` 新功能、`fix:` 修 bug、`style:` 純樣式/格式
- Amber 的 git 操作已經相當流暢,`git add` 指定檔案 + `git commit` + `git push` + PR 流程都能獨立完成
- `git pull` = fetch + merge,PR merge 後本機 pull 就自動跟上了

---

## 環境狀態快照

- **git**: `main`,已與 origin/main 同步(PR #3 merged),無進行中 feature branch
- **DB**: 6 個 migration 累積(最新:ExpenseApplicantId),Expenses table 含 Status、RejectionReason、IsDeleted、ApplicantId
- **可跑性**: `dotnet run` → `http://localhost:5242`
