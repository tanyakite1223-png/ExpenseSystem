# Handoff — 2026-06-12

## 我們上次做到哪

完成 **報銷單主檔/明細/專案 — 資料模型與關聯(第一棒)**。PR #12 merge 完成,補充 commit 也已 push,main 乾淨。

---

## 完成的工作

### 新增 model

- `Models/Project.cs`:ProjectId(PK)、ProjectName、IsActive、`List<ExpenseDetail>? ExpenseDetails`
- `Models/ExpenseDetail.cs`:ExpenseDetailId(PK)、ExpenseDate、Amount(decimal(18,2))、Description、ExpenseId(FK)、ProjectId(FK)、`Expense? Expense`、`Project? Project`
- `Models/Expense.cs`:加了 `CreatedAt`(DateTime)和 `List<ExpenseDetail>? ExpenseDetails`

### Migration

- `20260610154149_AddProjectAndExpenseDetail`:建出 Projects、ExpenseDetails 兩張表 + 外鍵 + index,並在 Expenses 加 CreatedAt 欄位

### Controller

- `ExpensesController.Create()` POST:加了 `expense.CreatedAt = DateTime.UtcNow`
- `ExpensesController.Edit()` POST:加了 `expense.CreatedAt = _expense.CreatedAt`(防止 Update 覆蓋原始建立時間)

### 種子資料

- `Program.cs`:啟動時若 Projects 為空,插入一筆「一般支出」(`IsActive = true`,Id 由 DB 自動產生)

### Include 驗證

- 暫時程式已刪除,驗證結果:`Include(ExpenseDetails).ThenInclude(Project)` 成功查回一對多關聯,Console 印出正確

---

## 重要設計決策紀錄

- Navigation property 要加 `?`(nullable),否則 non-nullable reference type 不在 form 裡會讓 ModelState 失敗
- EF Core PK 自動辨識規則:屬性名稱必須是 `Id` 或 `{ClassName}Id`,`DetailId` 不符合所以改成 `ExpenseDetailId`
- 種子資料不用 `HasData()` 是因為 `HasData` 必須手動指定 Id;Runtime 插入才能讓 DB 自動產生 Id

---

## 下次起手建議

這是「第一棒」,資料層打好了。下一步(第二棒以後)方向:

1. **ExpenseDetail CRUD**:新增/編輯報銷單時能新增明細(UI 流程待設計)
2. **Project CRUD**:管理專案清單的介面(之後當 SelectListItem 用)

上一個 session 討論過「類型專屬表」的複雜設計,Project Chat 這次刻意先做最基本的版本。下個 session 開始前建議先確認 Project Chat 的第二棒任務方向。

---

## 對下個 session 的提醒

- Amber 對 navigation property 的「一對多方向」需要多想一下(哪邊放 List、哪邊放單筆),但靠反問能自己想通
- EF Core 的 PK 命名規則她踩過了,下次新增 model 可以先問她「PK 你打算怎麼命名」確認方向
- `AddRange` vs `Add` 她習慣用 `AddRange` 即使只有一筆,可以在適當時機提一下規範
- Amber 不小心把 terminal output 貼回 terminal 造成一堆 PowerShell 錯誤,提醒她貼指令時確認是指令不是 output

---

## 環境狀態快照

- **git**: `main`,PR #12 merged + 補充 commit(0c29554),已與 origin/main 同步,乾淨
- **DB**: 10 個 migration,Projects 表有一筆「一般支出」,ExpenseDetails 表有測試資料(Include 驗證用,不影響功能)
- **可跑性**: `dotnet run` → `http://localhost:5242`,Create/Edit/Index 功能正常
- **測試帳號**: admin(Manager)、Amber(Employee)
