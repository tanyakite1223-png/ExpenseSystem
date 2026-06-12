# Handoff — 2026-05-21 22:00

## 我們上次做到哪

完成 **Register 頁面**。員工可自行填表單建帳號,不再依賴 seed 資料。
commit 直接在 main 上(忘記開 branch),已 push 到 GitHub。

---

## 完成的工作

- `RegisterViewModel.cs` — UserName、Email、Password、ConfirmPassword(含 DataAnnotations)
- `AccountController.cs` — GET/POST Register,使用 `UserManager<IdentityUser>.CreateAsync(user, password)` 建帳號
- `Views/Account/Register.cshtml` — 表單含 validation summary(ModelOnly)
- `Views/Login/Index.cshtml` — 加入「註冊」連結,導向 `/Account/Register`
- git 劇本:commit message 寫壞 → 用 `git commit --amend` + `git push --force` 修正

---

## 進行中的工作

無。

---

## 下次起手建議

Roadmap #2:**Expense 加 Status 欄位**(Draft / Submitted / Approved / Rejected)

建議討論範圍:
- Status 用什麼型別存(string? enum? int?)
- 需不需要 Migration
- Views 要不要顯示 Status

---

## 對下個 session 的提醒

- Amber 這次忘記先開 branch 再動 code,下次起手就問「你現在在哪個 branch?」
- commit message 反覆拉鋸,不夠直接說出「做了什麼」— 下次碰到繼續練
- `git commit --amend` + `git push --force` 今天第一次做,概念有了但不一定記熟,下次碰到不用重頭解釋

---

## 環境狀態快照

- **git**: `main`,最新 commit `19a7b78`,本機與 origin/main 同步
- **branch**: 無進行中的 feature branch
- **DB**: 同上次(migration 未動,AspNetUsers 可能有測試帳號新增)
- **可跑性**: `dotnet run` → `http://localhost:5242`,登入 `/Login/Index`,註冊 `/Account/Register`
