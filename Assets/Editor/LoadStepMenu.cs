// 教學用：每個步驟的完成內容藏在 Assets/_StepN~(Unity 會忽略結尾是 ~ 的資料夾)
// 從選單載入時，把 _StepN~ 改名成 StepN，Unity 就看得到了；裡面的 .meta 不變，參照會自動接回
using System.IO;
using UnityEditor;
using UnityEngine;

public static class LoadStepMenu
{
    [MenuItem("Tools/教學/載入步驟/Step1")]
    static void Step1() => Load("Step1");

    static void Load(string step)
    {
        string hidden = $"Assets/_{step}~";
        string target = $"Assets/{step}";
        if (!Directory.Exists(hidden))
        {
            Debug.LogWarning($"[載入步驟] 找不到 {hidden}，可能已經載入過了。");
            return;
        }
        if (Directory.Exists(target))
        {
            Debug.LogError($"[載入步驟] {target} 已經存在，沒有載入。");
            return;
        }
        Directory.Move(hidden, target);
        AssetDatabase.Refresh();
        Debug.Log($"[載入步驟] {step} 載入完成。");
    }
}
