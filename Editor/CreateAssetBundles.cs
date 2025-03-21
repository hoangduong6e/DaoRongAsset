﻿using UnityEditor;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

public class CreateEncryptedAssetBundles : MonoBehaviour
{
    public static bool test = false;

    [MenuItem("Assets/Build tất cả assetbundle (đã mã hóa)")]
    private static void BuildAllEncryptedAssetBundles()
    {
#if UNITY_ANDROID
        string assetBundleDirectoryPath = Application.dataPath + "/../AssetsBundles/Android";
#endif
#if UNITY_IOS
string assetBundleDirectoryPath = Application.dataPath + "/../AssetsBundles/IOS";
#endif

        try
        {
            // Build asset bundle

            string[] oldFiles = Directory.GetFiles(assetBundleDirectoryPath);
            foreach (string file in oldFiles)
            {
                File.Delete(file);
            }
            Debug.Log("Đã xóa tất cả các asset bundle cũ trong thư mục.");


            BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

            // Mã hóa asset bundle
            string[] files = Directory.GetFiles(assetBundleDirectoryPath).Where(file => !file.EndsWith(".manifest")).ToArray();
            foreach (string file in files)
            {
                byte[] fileBytes = File.ReadAllBytes(file);
                byte[] encryptedBytes = AssetBundleManager.EncryptData(fileBytes);
                File.WriteAllBytes(file, encryptedBytes);
                Debug.Log($"Đã mã hóa {Path.GetFileName(file)}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Lỗi build hoặc mã hóa asset bundle: " + e.Message);
        }
    }
}