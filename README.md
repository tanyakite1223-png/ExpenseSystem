# ExpenseSystem 報銷系統

以 ASP.NET Core MVC + Web API 實作的報銷流程管理系統，支援角色分流審核、複合表單送出、動態費用明細管理。

---

## 功能概覽

### 申請者（Employee）
- 新增報銷申請：主檔 + 多筆費用明細一次送出
- 費用明細支援動態新增 / 刪除列
- 送審、退件後補件重送

### 主管（Manager）
- 審核申請：核准 / 退件（可填寫退件原因）/ 退回補件
- 管理專案清單（啟用 / 封存）
- 刪除申請（軟刪除）

---

## 狀態流程

```
建立
 ↓
Draft（草稿）→ Submitted（待審）→ Approved（核准）
                     ↓
                     ├─→ Rejected（駁回，終止）
                     │
                     └─→ Returned（退回補件）→ 申請者修改後重送 → Submitted
```

---

## 技術棧

| 分類 | 技術 |
|------|------|
| 框架 | ASP.NET Core MVC + Web API（.NET 10） |
| ORM | Entity Framework Core 10 |
| 資料庫 | SQL Server LocalDB |
| 認證授權 | ASP.NET Core Identity + Cookie Auth |
| 角色管理 | RBAC（Manager / Employee） |
| API 文件 | Scalar |
| 前端 | Razor Views + 自訂 CSS + Vanilla JavaScript |

---

## 設計重點

- **MVC + Web API 雙介面**：MVC 處理頁面操作，RESTful API 獨立供外部使用，透過 Scalar 提供互動式文件
- **複合表單**：主檔與多筆費用明細一次送出，EF Core Graph Behavior 自動處理 INSERT / UPDATE / DELETE
- **動態明細列**：前端 Vanilla JS 動態新增 / 移除列，Edit POST 以 Reconcile 三態（更新 / 新增 / 刪除）同步資料庫
- **條件式驗證**：選擇「統一發票」時，發票號碼自動變為必填（Server-side 驗證）
- **軟刪除**：報銷申請不實際刪除，以 `IsDeleted` 標記

---

## 快速啟動

### 前置條件

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB（隨 Visual Studio 安裝，或單獨安裝 [SQL Server Express](https://www.microsoft.com/zh-tw/sql-server/sql-server-downloads)）

### 步驟

```bash
# 1. Clone 專案
git clone <repo-url>
cd ExpenseSystem/ExpenseSystem

# 2. 建立並初始化資料庫
dotnet ef database update

# 3. 啟動
dotnet run
```

瀏覽器開啟：`http://localhost:5242/Login/Index`

API 文件：`http://localhost:5242/scalar/v1`

---

## 預設測試帳號

啟動後系統自動建立以下測試帳號：

| 帳號 | 密碼 | 角色 | 可用功能 |
|------|------|------|---------|
| admin | @Admin123 | Manager | 審核、刪除、專案管理 |
| Amber | @Amber123 | Employee | 新增申請、送審、補件 |

> 也可透過「註冊」頁面自行建立新帳號（預設指派 Employee 角色）
