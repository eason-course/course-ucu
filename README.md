# eason-course

Unity 3D 角色動作教學專案：從角色移動開始，一步一步做到揮劍攻擊。

## 環境

- Unity **6000.0.68f1**(URP)
- 輸入用舊版 Input Manager(`Input.GetAxis`)
- 下載時要有 [Git LFS](https://git-lfs.com/)，模型和貼圖才會正確下載：
  ```
  git lfs install
  git clone https://github.com/Kinsliner/eason-course.git
  ```

## 步驟內容

| 步驟 | 場景 | 內容 |
|---|---|---|
| Step0 | `Step0/Scenes/00_Move.unity` | 半成品：用 `transform.Translate` 移動、2D Blend Tree 動畫、攝影機跟隨 |
| Step1 | `Step1/Scenes/01_Move4Dir.unity` | 改用 CharacterController：可以跳、可以站上箱子、掉落動作 |
| Step2 | `Step2/Scenes/02_Attack.unity` | 左鍵兩段揮劍、木樁敵人、頭上血條、Animation Event 控制攻擊判定時間 |

## 操作

- **WASD / 方向鍵**：移動
- **滑鼠**：轉動攝影機
- **空白鍵**：跳躍(Step1 起)
- **左鍵**：攻擊(Step2)

## 載入步驟

Step1、Step2 的完成內容放在 `Assets/_Step1~`、`Assets/_Step2~`。資料夾名稱結尾是 `~`，所以 Unity 不會讀取它們。

要看完成版時，在 Unity 選單點 **Tools → 教學 → 載入步驟 → StepN**，專案裡就會多出一個 `Assets/StepN` 資料夾。

- 想重新載入：先把 `Assets/StepN` 刪掉，再點一次選單。
- Step1 和 Step2 不要同時載入：兩個步驟的檔案 GUID 相同，同時載入會互相衝突。看完一個就先刪掉，再載入另一個。

## 素材

- 角色、武器、木樁：Synty POLYGON 系列
- 動作：Synty Animation 系列
