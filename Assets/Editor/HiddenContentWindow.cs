// 教學用：每個步驟的完成內容藏在 Assets/_StepN~(Unity 會忽略結尾是 ~ 的資料夾)，需要時再載入回原位
// 每個步驟資料夾裡有 manifest.txt，每行一個相對於 Assets 的路徑，記錄這個步驟包含哪些東西
// 連同 .meta 一起搬，GUID 不變，載入後場景和 Prefab 的參照會自動接回
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class HiddenContentWindow : EditorWindow
{
    const string AssetsRoot = "Assets";

    [MenuItem("Tools/教學/載入步驟")]
    static void Open() => GetWindow<HiddenContentWindow>("載入步驟");

    void OnGUI()
    {
        // 步驟資料夾：Assets/_Step1~、Assets/_Step2~ ...，視窗裡顯示成 Step1、Step2
        var steps = Directory.GetDirectories(AssetsRoot, "_Step*~").Select(Path.GetFileName).OrderBy(n => n).ToList();
        if (steps.Count == 0)
        {
            EditorGUILayout.HelpBox("Assets 底下沒有 _StepN~ 步驟資料夾。", MessageType.Warning);
            return;
        }

        foreach (var step in steps)
        {
            var items = ReadManifest(step);
            using (new EditorGUILayout.HorizontalScope("box"))
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    if (items.Count == 0)
                    {
                        EditorGUILayout.LabelField(DisplayName(step), EditorStyles.boldLabel);
                        EditorGUILayout.HelpBox($"缺少 {step}/manifest.txt，或裡面沒有內容。", MessageType.Warning);
                        continue;
                    }
                    bool hidden = items.All(p => IsHidden(step, p));
                    bool loaded = items.All(IsLoaded);
                    string state = hidden ? "已隱藏" : loaded ? "已載入" : "部分載入";
                    EditorGUILayout.LabelField($"{DisplayName(step)}　({state})", EditorStyles.boldLabel);
                    foreach (var p in items) EditorGUILayout.LabelField("  " + p, EditorStyles.miniLabel);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(loaded))
                            if (GUILayout.Button("載入", GUILayout.Height(26))) Load(step, items);
                        using (new EditorGUI.DisabledScope(hidden))
                            if (GUILayout.Button("藏起來", GUILayout.Height(26))) Hide(step, items);
                    }
                }
            }
        }
    }

    static List<string> ReadManifest(string step)
    {
        var path = $"{AssetsRoot}/{step}/manifest.txt";
        return File.Exists(path)
            ? File.ReadAllLines(path).Select(l => l.Trim()).Where(l => l.Length > 0 && !l.StartsWith("#")).ToList()
            : new List<string>();
    }

    static string DisplayName(string step) => step.Trim('_', '~');   // _Step1~ → Step1
    static string AssetPath(string path) => $"{AssetsRoot}/{path}";
    static string HiddenPath(string step, string path) => $"{AssetsRoot}/{step}/{path}";
    static bool Exists(string p) => File.Exists(p) || Directory.Exists(p);
    static bool IsHidden(string step, string path) => Exists(HiddenPath(step, path));
    static bool IsLoaded(string path) => Exists(AssetPath(path));

    static void Load(string step, List<string> items)
    {
        foreach (var p in items.Where(p => IsHidden(step, p)))
            Move(HiddenPath(step, p), AssetPath(p));
        AssetDatabase.Refresh();
    }

    static void Hide(string step, List<string> items)
    {
        // 正在開的場景要藏起來時，先讓使用者存檔，再換成空場景
        var active = EditorSceneManager.GetActiveScene().path;
        if (items.Any(p => AssetPath(p) == active))
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        }

        foreach (var p in items.Where(IsLoaded))
            Move(AssetPath(p), HiddenPath(step, p));
        AssetDatabase.Refresh();
    }

    // 搬檔案或資料夾，連同 .meta 一起搬
    static void Move(string from, string to)
    {
        if (Exists(to))
        {
            Debug.LogError($"[載入步驟] 目的地已經有東西，沒有搬動：{to}");
            return;
        }
        Directory.CreateDirectory(Path.GetDirectoryName(to));
        if (Directory.Exists(from)) Directory.Move(from, to);
        else File.Move(from, to);
        if (File.Exists(from + ".meta")) File.Move(from + ".meta", to + ".meta");
    }
}
