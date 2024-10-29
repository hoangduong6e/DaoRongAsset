
using UnityEditor;
using UnityEngine;
using System;

public class CreateAssetBundles : MonoBehaviour
{
    public static bool test = false;
    [MenuItem("Assets/Build tất cả assetbundle")]
    private static void BuildAllAssetBundles()
    {
#if UNITY_ANDROID
		string assetBundleDirectoryPath = Application.dataPath + "/../AssetsBundles/Android";
#endif
#if UNITY_IOS
        string assetBundleDirectoryPath = Application.dataPath + "/../AssetsBundles/IOS";
#endif
        try
        {
			BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath,BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		}
		catch (Exception e)
		{
			debug.LogError("Lỗi build assetbundle " + e);
		}
    }
}

//#if UNITY_ANDROID && !UNITY_EDITOR
//// mã ở đây
//#endif
