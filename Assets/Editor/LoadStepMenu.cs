// 教學用：每個步驟的完成內容藏在 Assets/_StepN~(Unity 會忽略結尾是 ~ 的資料夾)
// 從選單載入時，把 _StepN~ 複製一份成 StepN，Unity 就看得到了；_StepN~ 原封不動保留，之後可以再載入
using System.IO;
using UnityEditor;
using UnityEngine;

public static class LoadStepMenu
{
    [MenuItem("Tools/教學/載入步驟/Step1")]
    static void Step1() => Load("Step1");

    [MenuItem("Tools/教學/載入步驟/Step2")]
    static void Step2() => Load("Step2");

    static void Load(string step)
    {
        string hidden = $"Assets/_{step}~";
        string target = $"Assets/{step}";
        if (!Directory.Exists(hidden))
        {
            Debug.LogError($"[載入步驟] 找不到 {hidden}。");
            return;
        }
        if (Directory.Exists(target))
        {
            Debug.LogError($"[載入步驟] {target} 已經存在，沒有載入。要重新載入請先刪掉它。");
            return;
        }

        // 連同 .meta 一起複製，GUID 不變，參照會自動接回
        foreach (var file in Directory.GetFiles(hidden, "*", SearchOption.AllDirectories))
        {
            string copy = target + file.Substring(hidden.Length);
            Directory.CreateDirectory(Path.GetDirectoryName(copy));
            File.Copy(file, copy);
        }
        AssetDatabase.Refresh();
        Debug.Log($"[載入步驟] {step} 載入完成。");
    }
}
