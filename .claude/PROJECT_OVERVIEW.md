# ExpenseSystem 專案總覽

> 這份檔案是專案的「全景圖」。新 session 不一定每次讀,但需要對整體狀況有印象時就回來翻。
>
> **更新時機**:有結構性變化(新 entity、新功能上線、git 劇本完成等)才更新,日常進度推進不動這份。
>
> **版本**:v1.0 — 骨架版,Amber 進來定版

---

## 1. 專案目的

ExpenseSystem(報銷系統)從 ASP.NET Core 教學專案,正在轉型為 Amber 的**履歷作品**。

**現階段目標**:
- 變成一個「拿得出手」的 portfolio 專案
- 涵蓋 ASP.NET Core MVC + Web API + EF Core + 認證授權 + 部署的完整實作
- 經歷實務常見的 git 情境
- 自然導入 JavaScript 前端互動

---

## 2. 技術棧

- **.NET 10** + ASP.NET Core MVC
- **EF Core 10.0.7** + SQL Server LocalDB
- **ASP.NET Core Identity** 認證授權
- **Scalar** 作為 API 文件介面
- **前端**:目前是 Razor + Bootstrap(預設),未來會加 vanilla JS
- **版控**:git + GitHub(HTTPS + Git Credential Manager)

---

## 3. 目錄結構

```
D:\Workshops\Amber\ExpenseSystem\          ← git repo 根
├── CLAUDE.md                              ← 實戰場規範書
├── .claude/
│   ├── PROJECT_OVERVIEW.md                ← 本檔
│   ├── current-handoff.md                 ← 最新 handoff
│   └── handoff-archive/
├── .gitignore
├── README.md
└── ExpenseSystem\                         ← .NET 專案本體(注意:多一層同名資料夾)
    ├── ExpenseSystem.csproj
    ├── Program.cs
    ├── Controllers\
    │   ├── ExpensesController.cs          ← MVC CRUD
    │   ├── ApiExpensesController.cs       ← Web API CRUD
    │   ├── LoginController.cs             ← 登入/登出(Identity)
    │   └── AccountController.cs          ← Register
    ├── Models\
    │   ├── Expense.cs
    │   ├── LoginViewModel.cs
    │   └── RegisterViewModel.cs
    ├── Data\
    │   └── ExpenseDbContext.cs            ← 繼承 IdentityDbContext
    ├── Views\
    │   ├── _ViewImports.cshtml
    │   ├── Expenses\
    │   │   ├── Index.cshtml
    │   │   ├── Create.cshtml
    │   │   ├── Edit.cshtml
    │   │   └── Delete.cshtml
    │   ├── Login\
    │   │   └── Index.cshtml
    │   └── Account\
    │       └── Register.cshtml
    └── Migrations\
        ├── InitialCreate
        ├── FixAmountPrecision
        └── AddIdentityTables
```

---

## 4. 資料模型(目前)

### Expense
| 欄位 | 型別 | 說明 |
|------|------|------|
| ExpenseId | int (PK) | |
| Title | string (Required) | |
| Amount | decimal(18,2) | |
| ExpenseDate | DateTime | |
| Description | string? | |
| Status | ExpenseStatus (enum) | Draft/Submitted/Approved/Rejected |
| RejectionReason | string? | Manager 退件時填寫 |
| IsDeleted | bool | 軟刪除標記 |
| ApplicantId | string? | 申請者 UserId(對應 AspNetUsers) |
| CreatedAt | DateTime | 系統自動寫入,使用者不可見不可改 |

### Project
| 欄位 | 型別 | 說明 |
|------|------|------|
| ProjectId | int (PK) | |
| ProjectName | string | |
| IsActive | bool | |

### ExpenseDetail
| 欄位 | 型別 | 說明 |
|------|------|------|
| ExpenseDetailId | int (PK) | |
| ExpenseDate | DateTime | |
| Amount | decimal(18,2) | |
| Description | string | |
| ExpenseId | int (FK → Expense) | NOT NULL |
| ProjectId | int (FK → Project) | NOT NULL |

### 關聯
- Expense 1 — 多 ExpenseDetail
- Project 1 — 多 ExpenseDetail

### ExpenseStatus (enum)
`Draft` / `Submitted` / `Returned` / `Approved` / `Rejected`,含中文 `[Display]` 標註

---

## 5. 已實作功能

- ✅ Expense CRUD(MVC)— Index、Create、Edit、Delete
- ✅ Expense Web API — GetAll、GetById、Create、Update、Delete
- ✅ Scalar API 文件介面(`http://localhost:5242/scalar/v1`)
- ✅ CORS 設定(開發用 AllowAll)
- ✅ ASP.NET Core Identity 認證(登入、登出、Cookie Auth)
- ✅ RBAC — Manager 可刪除/審核,Employee 可新增/編輯
- ✅ Seed 資料 — admin(Manager)/ Amber(Employee)
- ✅ 新帳號註冊自動指派 Employee 角色
- ✅ Expense Status 欄位 — 狀態流程、角色分流審核、退件原因、軟刪除
- ✅ Edit server-side 授權控管 — Employee 只能修改自己的 Draft/Returned 單,Manager 不限
- ✅ Index 清單顯示優化 — 分頁(server-side)、流水號、金額無小數、日期只顯示日期
- ✅ 資料模型擴充(第一棒) — 新增 Project、ExpenseDetail model 與一對多關聯;Expense 加 CreatedAt;種子資料「一般支出」;Include 查詢驗證 JOIN 成立

---

## 6. 功能 Roadmap

> 這部分由 Amber 自己決定。Project Chat 給了起手提示,但**最終由 Amber 拍板**這個專案接下來要長什麼樣。
>
> 待 Amber 進入新 chat 時填寫,可能的方向(僅供參考,不限於此):
>
> - 使用者系統(註冊、登入)→ 對應認證授權的學習目標
> - 角色與權限(申請人 / 主管 / 財務)→ RBAC 練習
> - 報銷單明細(主檔 / 明細)→ 一對多關聯
> - 簽核流程(submit → approve → reject)→ 業務邏輯複雜度
> - 報表(月度統計、按部門 / 類別)→ LINQ 練習、可能用前端圖表
> - 附件上傳(收據圖片)→ 檔案處理
> - 通知(email / 站內)→ 整合外部服務
> - 部署到雲端 → CI/CD 練習

### 規劃中

| 優先順序 | Feature | 說明 |
|---------|---------|------|
| 1 | ~~**Register 頁面**~~ | ✅ 完成 |
| 2 | ~~**Expense Status 欄位**~~ | ✅ 完成 |
| 3 | ~~**送件功能**~~ | ✅ 完成 |
| 4 | **審核功能** | Manager 核准 / 退件 — ✅ 已實作(Edit 角色分流) |
| 5 | ~~**補件退件**~~ | ✅ 完成 — Returned 狀態 + Employee 可修改後重送 |

### 已完成

- ✅ Expense CRUD(MVC + Web API)
- ✅ ASP.NET Core Identity 認證
- ✅ RBAC — Manager / Employee 角色分流
- ✅ Register 頁面 — 員工可自行建帳號
- ✅ Expense Status — 狀態欄位、角色分流審核、退件原因、軟刪除
- ✅ 送審功能 — Employee 可從 Index 送審(Draft → Submitted),依角色控制按鈕顯示
- ✅ 申請者欄位 — Expense 記錄 ApplicantId,Employee Index 只顯示自己的單子

---

## 7. 進行中的工作

- **報銷單明細(第二棒)** — 資料層已就緒,待下個 session 規劃 ExpenseDetail CRUD 與 Project 管理介面

未來格式範例:
- **feature/user-auth** — 使用者認證系統
  - 開始日:YYYY-MM-DD
  - 預計範圍:註冊、登入、登出
  - 目前進度:DbContext 已加入 IdentityUser

---

## 8. 已知問題 / 技術債

(目前無)

未來格式範例:
- [P2] Expense.Amount 沒有 server-side 驗證上限
- [P3] Views 的錯誤訊息沒做 i18n

---

## 9. Git 劇本進度

從 CLAUDE.md §5 的劇本庫,記錄哪些已經自然或刻意經歷過。

| 劇本 | 狀態 | 何時 | 備註 |
|------|------|------|------|
| 緊急 bug 中斷(stash + 切 branch) | ⬜ | | |
| 主分支前進了(rebase vs merge) | ⬜ | | |
| 改錯地方(reset vs revert) | ⬜ | | |
| 衝突解決 | ⬜ | | |
| 想拆 commit | ⬜ | | |
| commit message 寫錯(amend) | ✅ | 2026-05-21 | 編輯器誤入 # 註解,用 amend + force push 修正 |
| PR 流程(GitHub PR merge) | ✅ | 2026-05-25 | feature/add-Expense-Status 第一次走完完整 PR 流程 |
| feature branch 寫太久(squash) | ⬜ | | |

---

## 10. 學習目標追蹤

> 避免一直練同類東西。每完成一個 feature,標記它鍛鍊到了什麼。

| 領域 | 累積經驗 |
|------|---------|
| EF Core 進階(N+1、Include、Tracking) | 入門 |
| LINQ 複雜查詢 | 入門 |
| Web API 設計 | 入門 |
| 認證授權 | 概念 |
| 前端 JS | 入門(addEventListener、style.display、disabled、querySelectorAll、forEach、data-* attribute、CSS 多選器) |
| 部署 | 未實作 |
| 測試 | 未開始 |

---

## 11. 筆記變更紀錄

> 由 Claude Code 新增或建議修改的筆記,在這裡留紀錄。

(目前無)

格式範例:
- 2026-05-20 **新增** `DataAccess_EFCoreInclude_關聯查詢.md` — 在實作報銷單明細時,Amber 第一次碰到 N+1,補了這份筆記
- 2026-05-25 **待修正** `DataAccess_Migration.md` — 內容沒提到 `dotnet ef database update --connection`,建議補充

---

## 12. 機制改良紀錄

> handoff 機制本身的演進。Amber 或 Claude Code 提出、Project Chat review 後採納的變更。

(目前無)

格式範例:
- v1.0 → v1.1(2026-05-22)— 由 Amber 提出:`current-handoff.md` 加「卡點」欄位,因為環境問題卡關時,下個 session 起手要能快速看到
