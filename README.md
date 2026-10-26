# Allagan Tools (TW) — 繁中服移植版

本專案是 Dalamud 插件 [Allagan Tools（InventoryTools）](https://github.com/Critical-Impact/InventoryTools) 的 **FFXIV 繁體中文伺服器（台服）** 移植版。

Allagan Tools 可以追蹤所有角色／雇員／部隊箱的物品、搜尋與篩選物品、規劃製作清單，並在物品說明框顯示物品存放位置與市場價格。

## 版本說明

| 項目 | 內容 |
|---|---|
| 基於版本 | 原版 [13.1.9](https://github.com/Critical-Impact/InventoryTools/releases/tag/13.1.9)（最後一個 API13 版本） |
| Dalamud API | API13 |
| 分支 | `tw-api13` |
| 程式修改 | 插件顯示名稱與 RepoUrl；修正物品總數溢位（見下方） |

原版主線已更新到 API15，無法在目前的台服 Dalamud 上載入，因此本移植版固定在 13.1.9。

## 已在台服確認可用的功能

- 背包、雇員物品追蹤（需先在傳喚鈴打開雇員一次）
- 物品說明框顯示存放位置與數量
- 市場價格（Universalis 台服資料，陸行鳥資料中心）

## 程式修改

- **物品總數溢位修正**：原版跨角色、雇員加總物品數量時使用 32 位元整數，總數超過 2,147,483,647（約 21.47 億）會溢位顯示錯誤，金幣最容易遇到。本版改用 64 位元（`long`）加總，影響 Quantity／Available 欄位、數量篩選與物品說明框的擁有數量。修改處皆以 `TW port` 註解標示。

## 注意事項

- **市場價格預設不會自動下載**，價格欄會顯示 0。
  請到設定 → **Market Board** → 勾選 **Automatically download prices?**。
- 市場價格資料來自 Universalis，由玩家上傳。冷門物品的價格可能較久未更新。

## 編譯方式

```bat
git clone --recurse-submodules https://github.com/pipipi314159/InventoryTools.git
cd InventoryTools
git checkout tw-api13
git submodule update --init --recursive
set DALAMUD_HOME=<台服 Dalamud 的 Injector 資料夾>
dotnet build InventoryTools\InventoryTools.csproj -c Release
```

產生的插件位於 `InventoryTools\bin\Release\InventoryTools.dll`，在 Dalamud 設定 → 實驗性功能 → 開發插件位置加入即可。

## 授權與致謝

- 原作者：[Critical-Impact](https://github.com/Critical-Impact)
- 授權：GPL-3.0，與原版相同（見 [LICENSE](LICENSE)）
- 問題回報：請在本 repo 開 Issue，**不要回報給原作者**，因為原作者不支援台服。
