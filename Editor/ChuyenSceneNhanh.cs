#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneSwitcherWindow : EditorWindow
{
    [MenuItem("Window/Scene Switcher")]
    public static void ShowWindow()
    {
        GetWindow<SceneSwitcherWindow>("Scene Switcher");
    }

    private void Update()
    {
        // Kiểm tra nếu một tổ hợp phím được nhấn (ví dụ: Ctrl + Shift + S)
        Event e = Event.current;
        if (e != null && e.type == EventType.KeyDown && e.control && e.shift && e.keyCode == KeyCode.P)
        {
            SwitchScene("YourSceneName");
            e.Use(); // Ngăn sự kiện này khỏi tác động khác
        }
    }

    private void SwitchScene(string sceneName)
    {
        // Kiểm tra nếu cảnh hiện tại đã được lưu
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // Chuyển sang cảnh mới
            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");
        }
    }
}
#endif
