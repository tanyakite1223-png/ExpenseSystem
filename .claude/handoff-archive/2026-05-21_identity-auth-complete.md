# Handoff — 2026-05-21 00:00

## 我們上次做到哪

完成 **ASP.NET Core Identity 整合**,將原本 hardcode 的認證系統全面換掉。
`feature/identity-auth` branch 已 merge 回 main 並 push 到 GitHub。

---

## 完成的工作

- 安裝 `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `ExpenseDbContext` 改繼承 `IdentityDbContext<IdentityUser>`
- `Program.cs` 換成 `AddIdentity` + `ConfigureApplicationCookie`
- `LoginController` 換成 `SignInManager.PasswordSignInAsync`
- Migration `AddIdentityTables` → 建立 7 張 Identity Table
- `Program.cs` 加入 seed 資料(Manager/Employee 角色 + admin/Amber 帳號)
- 驗證:admin 可刪除、Amber 被擋,功能正常

---

## 進行中的工作

無。`feature/identity-auth` 已完成並 merge。

---

## 下次起手建議

還有兩件 onboarding 事項沒做(這次直接跳進寫 code 了):
1. **補完 PROJECT_OVERVIEW.md 的 Roadmap** — 讓 Amber 決定接下來的 feature 順序
2. **Expense entity 欄位確認** — PROJECT_OVERVIEW.md §4 待補

下一個 feature 方向 Amber 說的是:帳號、權限管理、簽核狀態查詢、補件退件。
Identity 已完成,建議接下來討論:
- **Register 頁面**(目前只有 seed,沒有正式註冊功能)
- 或跳去做**簽核狀態**欄位(Expense 加 Status)

**Amber 決定:下次做 Register 頁面。**

---

## 對下個 session 的提醒

- Amber 對 `Program.cs` 的 DI / scope 概念比較陌生(seed 那段直接給了答案),下次碰到類似場景可以多引導一點
- 命名(table、欄位、branch)是她自己也認為要練的弱點,遇到機會讓她多想
- 這次用 `git merge` 直接 merge,下次可以建議試試 PR 流程對比看看差在哪

---

## 環境狀態快照

- **git**: `main`,最新 commit `05e71f9`,本機與 origin/main 同步
- **branch**: 無進行中的 feature branch
- **DB**: `ExpenseSystem`,migration 全套用(InitialCreate + FixAmountPrecision + AddIdentityTables)
  - AspNetUsers: 2 筆(admin / Amber)
  - AspNetRoles: 2 筆(Manager / Employee)
  - Expenses: 8 筆(原有資料)
- **NuGet**: EF Core 10.0.7、Scalar.AspNetCore、Microsoft.AspNetCore.Identity.EntityFrameworkCore
- **可跑性**: `dotnet run` → `http://localhost:5242`,登入路徑 `/Login/Index`
