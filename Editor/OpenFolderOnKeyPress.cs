using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class KeyPressFileOpener
{
    static KeyPressFileOpener()
    {
        // Đăng ký hàm Update để gọi thường xuyên
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        // Kiểm tra nếu tổ hợp phím Control + P được nhấn và chỉ thực hiện khi đang ở chế độ Play
        if (Application.isPlaying && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Tổ hợp phím Control + P đã được nhấn - Mở file đầu tiên trong folder...");

            // Đường dẫn tới folder trong Assets
            string folderPath = "Assets/Scenes/EventTrungThu2024"; // Thay bằng đường dẫn chính xác của bạn
            string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });

            if (guids.Length > 0)
            {
                // Tải asset đầu tiên
                string firstAssetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                Object firstAsset = AssetDatabase.LoadAssetAtPath<Object>(firstAssetPath);

                if (firstAsset != null)
                {
                    // Chọn asset đầu tiên trong Project window
                    Selection.activeObject = firstAsset;
                    EditorGUIUtility.PingObject(firstAsset);
                    Debug.Log($"Đã mở file: {firstAssetPath}");
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy file nào trong folder.");
            }
        }
    }
}
