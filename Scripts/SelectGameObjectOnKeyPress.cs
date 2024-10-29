#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class SelectGameObjectOnKeyPress : MonoBehaviour
{
    public GameObject targetGameObject; // GameObject bạn muốn chọn

    void Update()
    {
        // Kiểm tra nếu phím bạn muốn (ví dụ phím G) được nhấn
        if (Input.GetKeyDown(KeyCode.G))
        {
            SelectGameObject(targetGameObject);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            //CrGame.ins.OpenGiaoDienLongAp();
            GameObject CanvasVienChinh = GameObject.Find("CanvasVienChinh");
            SelectGameObject(CanvasVienChinh.transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject);
        }
    }

    void SelectGameObject(GameObject go)
    {
        if (go != null)
        {
            // Mở rộng và chọn GameObject trong Hierarchy
            Selection.activeGameObject = go;
            EditorGUIUtility.PingObject(go);
            go.SetActive(true);
            //string str = "";
            //for (int i = 0; i < go.transform.childCount; i++)
            //{
            //    str += i + ": " + go.transform.GetChild(i).name + "\n";
             
            //}
            //debug.Log(str);
        }
    }
}
#endif
