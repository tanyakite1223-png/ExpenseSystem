# Handoff — 2026-06-07

## 我們上次做到哪

完成 **新帳號註冊自動指派 Employee 角色**。PR #9 merge 完成,main 乾淨。

`AccountController.Register` POST 在 `CreateAsync` 成功後呼叫 `_userManager.AddToRoleAsync(user, "Employee")`,新帳號會自動寫入 `AspNetUserRoles`。

---

## 完成的工作

- `AccountController.cs`:Register POST 加入 `AddToRoleAsync`,新帳號自動取得 Employee 角色

---

## 進行中的工作

無。

---

## 下次起手建議

**server-side 權限控管**:Edit GET 目前沒擋,Employee 可以用 URL 直接改狀態不是 Returned 的單。UI 有隱藏按鈕但 server 端沒防。這是安全性問題,下次處理。

---

## 對下個 session 的提醒

- `ApplicantId` 在 DB 存 GUID,顯示時即時查 UserManager 轉成 username
- 測試帳號:admin(Manager)、Amber(Employee),也可以自行 Register 新帳號測試(現在有 role 了)

---

## 環境狀態快照

- **git**: `main`,PR #9 merged,已與 origin/main 同步,乾淨
- **DB**: 6 個 migration,無變化
- **可跑性**: `dotnet run` → `http://localhost:5242`
- **測試帳號**: admin(Manager)、Amber(Employee)
