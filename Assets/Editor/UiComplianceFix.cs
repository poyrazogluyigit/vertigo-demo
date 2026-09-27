using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// One-shot fix for the PDF's UI naming and RaycastTarget rules.
// Run it once on WheelSpin, review, save the scene, then delete this file.
static class UiComplianceFix
{
    const string OldSlotPrefix = "ui_wheel_slot_";

    static readonly Dictionary<string, string> Renames = new Dictionary<string, string>
    {
        // Changed at runtime, so they need the _value suffix
        { "ui_image_wheel_base", "ui_image_wheel_base_value" },
        { "ui_image_wheel_indicator", "ui_image_wheel_indicator_value" },
        { "ui_image_wheel_rays", "ui_image_wheel_rays_value" },
        { "ui_image_level_current_bg", "ui_image_level_current_bg_value" },
        { "ui_button_gameend_restart", "ui_button_gameend_restart_value" },
        // General to specific: ui_<type>_<area>_<detail>
        { "ui_wheel_spin_root", "ui_group_wheel_spin" },
        { "ui_wheel_slot_group", "ui_group_wheel_slots" },
        { "ui_view_levelPanel", "ui_view_level" },
        { "ui_view_rewardPanel", "ui_view_rewards" },
    };

    // Purely decorative, nothing to click
    static readonly string[] NoRaycast = { "ui_image_bomb" };

    // Scroll lists need a raycast target on their items, or a touch drag never reaches the ScrollRect
    static readonly (string prefab, string child)[] ScrollDragTargets =
    {
        ("Assets/Prefabs/ui_item_reward.prefab", "ui_item_reward"),
        ("Assets/Prefabs/ui_item_reward_card.prefab", "ui_image_reward_icon_value"),
    };

    [MenuItem("Tools/Vertigo/Apply UI Compliance Fix")]
    static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();
        var all = new List<Transform>();
        foreach (GameObject root in scene.GetRootGameObjects())
            all.AddRange(root.GetComponentsInChildren<Transform>(true));

        var log = new StringBuilder("UI compliance fix:\n");
        foreach (Transform t in all)
        {
            string newName = NewName(t.name);
            if (newName != null)
            {
                Undo.RecordObject(t.gameObject, "UI compliance rename");
                log.AppendLine($"  {t.name} -> {newName}");
                t.name = newName;
            }

            if (System.Array.IndexOf(NoRaycast, t.name) >= 0 && t.TryGetComponent(out Image image) && image.raycastTarget)
            {
                Undo.RecordObject(image, "UI compliance raycast");
                image.raycastTarget = false;
                log.AppendLine($"  {t.name}: RaycastTarget off");
            }
        }

        // OnValidate wiring looks children up by name, so rerun it against the new names
        foreach (Transform t in all)
            foreach (MonoBehaviour mb in t.GetComponents<MonoBehaviour>())
            {
                if (mb == null || mb.GetType().Assembly.GetName().Name != "Assembly-CSharp") continue;
                MethodInfo onValidate = mb.GetType().GetMethod("OnValidate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (onValidate == null) continue;
                Undo.RecordObject(mb, "UI compliance rewire");
                onValidate.Invoke(mb, null);
            }

        foreach ((string prefab, string child) in ScrollDragTargets)
            EnableRaycast(prefab, child, log);

        EditorSceneManager.MarkSceneDirty(scene);
        Debug.Log(log.ToString());
    }

    // Prefab edits are saved straight to the asset, outside Undo
    static void EnableRaycast(string prefabPath, string childName, StringBuilder log)
    {
        GameObject root = PrefabUtility.LoadPrefabContents(prefabPath);
        try
        {
            foreach (Image image in root.GetComponentsInChildren<Image>(true))
            {
                if (image.name != childName || image.raycastTarget) continue;
                image.raycastTarget = true;
                PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
                log.AppendLine($"  {prefabPath} / {childName}: RaycastTarget on");
                return;
            }
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    static string NewName(string name)
    {
        if (Renames.TryGetValue(name, out string renamed)) return renamed;
        if (name.StartsWith(OldSlotPrefix) && int.TryParse(name.Substring(OldSlotPrefix.Length), out int index))
            return "ui_item_wheel_slot_" + index;
        return null;
    }
}
