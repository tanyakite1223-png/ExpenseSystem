# Handoff — 初始版(實戰場啟用)

> 這份是實戰場 Claude Code 的第一個 handoff。
> Amber 第一個 session 開始時讀這份。

---

## 我們上次做到哪

這是**實戰場的第一次 session**,還沒有「上次」。

過去脈絡:Amber 在 Project Chat 完成了 C# 基礎、ASP.NET Core MVC、EF Core、Web API、認證授權的觀念與基本實作。ExpenseSystem 專案已建立並推上 GitHub,目前 main 分支有完整的 Expense CRUD(MVC + Web API)。

現在進入新階段:**離開 Project Chat,以「資深同事」的方式陪她實戰**,直到她準備好正式找工作。

---

## 第一個 session 的目標 — Onboarding(上工)

這個 session **不是寫 code**,而是讓 Amber 跟你(Claude Code)一起把實戰場機制建立起來。具體要做的事:

### Step 1:讀文件,確認你理解

請你讀:
1. `CLAUDE.md`(實戰場規範書 — 你的工作手冊)
2. 本檔(`current-handoff.md`)
3. `.claude/PROJECT_OVERVIEW.md`(專案總覽)

讀完後,**用你自己的話**跟 Amber 確認:
- 你是誰、你的角色是什麼
- handoff 機制怎麼運作
- 什麼狀況該建議她回 Project Chat

如果文件有不清楚的地方,跟 Amber 一起討論,把改良建議寫在 session 結束的 handoff 中。

### Step 2:跟 Amber 一起把 PROJECT_OVERVIEW.md 補完

目前 PROJECT_OVERVIEW.md 是骨架版,有幾個區塊待 Amber 自己決定:

1. **§4 資料模型**:把 Expense 的欄位列出來,順便畫個簡單的 entity 文字圖
2. **§6 功能 Roadmap**:這是最重要的。**讓 Amber 決定**接下來想做什麼功能、做的順序、為什麼這樣排
   - 你可以建議、可以提醒「這個太大要拆」「這個順序會撞 git 劇本」等等
   - 但**不要替她決定**。這是她的履歷作品
3. **§10 學習目標追蹤**:她自評目前各領域的程度,以後做完 feature 來更新

### Step 3:確認 git 工作紀律

跟 Amber 確認:
- 從現在開始,不在 main 直接 commit
- 開 feature/* 或 fix/* branch
- 完成後 merge 回 main(用 PR 還是直接 merge,她自己選並體會差別)

### Step 4:寫 Onboarding 結束的 handoff

把上面討論的成果寫成新的 `current-handoff.md`,把這份初始版歸檔到 `handoff-archive/2026-MM-DD_initial-onboarding.md`。

---

## 對你(Claude Code)的特別提醒

1. **這個 session 是「機制建立」,不是「寫 code」**。不要急著建議「我們來寫個功能」
2. **Amber 是第一次用這套機制**,她可能不熟。耐心走,她有疑問就停下來討論
3. **她可能會質疑機制的某些部分**,這是好事(本來就是要被質疑的)。認真聽,寫進改良提案
4. 第一次互動,**人格定位很重要**。記得你是資深同事,不是老師。但 onboarding 場景下,可以稍微多解釋一點 — 第二個 session 才正式進入「資深同事」模式

---

## 環境狀態快照

- **git**:`main`,最新 commit `6aaaed7`(test: 測試分支功能),本機與 origin/main 同步
- **DB**:`ExpenseSystem`,migration 全套用(InitialCreate + FixAmountPrecision),Expenses 表有 8 筆資料
- **NuGet**:EF Core 10.0.7、Scalar.AspNetCore
- **可跑性**:`dotnet run` → `http://localhost:5242`,Scalar UI 在 `/scalar/v1`

---

## 下次起手建議(給 onboarding 結束後的下個 session)

待本 session 結束時由你寫,通常會包含:
- Amber 在 roadmap 上選的第一個 feature 是什麼
- 第一個 feature 預計怎麼拆
- 開哪個 branch
