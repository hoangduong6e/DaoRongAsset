using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : MonoBehaviour
{
    private string encryptionKey = "1111111111111111"; // Khóa mã hóa, cần 16, 24 hoặc 32 ký tự

    // Đường dẫn đầy đủ tới file được lưu trong `persistentDataPath`
    private string GetEncryptedFilePath(string fileName) => Path.Combine(Application.persistentDataPath, fileName);

    // Hàm mã hóa dữ liệu
    private byte[] EncryptData(byte[] data)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = new byte[16]; // IV mặc định

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }
        }
    }

    // Hàm giải mã dữ liệu
    private byte[] DecryptData(byte[] encryptedData)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = new byte[16]; // IV mặc định

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream ms = new MemoryStream(encryptedData))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        cs.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }
        }
    }

    // Tải `AssetBundle` từ server
    public IEnumerator DownloadAssetBundle(string url, string fileName, Action onSuccess = null, Action<string> onError = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    byte[] encryptedData = EncryptData(www.downloadHandler.data);
                    File.WriteAllBytes(GetEncryptedFilePath(fileName), encryptedData);
                    Debug.Log($"AssetBundle downloaded and encrypted as {fileName}");
                    onSuccess?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error encrypting or saving AssetBundle: {e.Message}");
                    onError?.Invoke(e.Message);
                }
            }
            else
            {
                Debug.LogError($"Error downloading AssetBundle: {www.error}");
                onError?.Invoke(www.error);
            }
        }
    }

    // Tải `AssetBundle` từ bộ nhớ
   public IEnumerator LoadAssetBundle(string fileName, Action<AssetBundle> onSuccess, Action<string> onError)
{
    string filePath = GetEncryptedFilePath(fileName);

    if (!File.Exists(filePath))
    {
        Debug.LogError($"AssetBundle file not found: {filePath}");
        onError?.Invoke("File not found");
        yield break;
    }

    byte[] decryptedData = null;

    // Đọc và giải mã dữ liệu trong try-catch
    try
    {
        byte[] encryptedData = File.ReadAllBytes(filePath);
        decryptedData = DecryptData(encryptedData);
    }
    catch (Exception e)
    {
        Debug.LogError($"Error decrypting AssetBundle: {e.Message}");
        onError?.Invoke(e.Message);
        yield break;
    }

    // Tải AssetBundle từ dữ liệu đã giải mã
    AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(decryptedData);
    yield return request;

    if (request.assetBundle != null)
    {
        Debug.Log($"AssetBundle {fileName} loaded successfully");
        onSuccess?.Invoke(request.assetBundle);
    }
    else
    {
        Debug.LogError($"Failed to load AssetBundle: {fileName}");
        onError?.Invoke("Failed to load AssetBundle");
    }
}


    // Xóa `AssetBundle` khỏi bộ nhớ
    public void DeleteAssetBundle(string fileName)
    {
        string filePath = GetEncryptedFilePath(fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Deleted AssetBundle file: {filePath}");
        }
        else
        {
            Debug.LogWarning($"File not found for deletion: {filePath}");
        }
    }

    // Test ví dụ
    private void Start()
    {
        string testUrl = "https://daorongmobile.online/DaoRongData2/IOS/animtienhoa"; // URL của AssetBundle
        string testFileName = "animatorkhungavttrungthu2024";

        StartCoroutine(DownloadAssetBundle(testUrl, testFileName, 
            onSuccess: () => Debug.Log("Download and save successful"),
            onError: (error) => Debug.LogError($"Download error: {error}")
        ));

        StartCoroutine(LoadAssetBundle(testFileName, 
            onSuccess: (assetBundle) =>
            {
                Debug.Log("AssetBundle loaded successfully");
                // Sử dụng AssetBundle ở đây, ví dụ:
                GameObject prefab = assetBundle.LoadAsset<GameObject>("animtienhoa");
                Instantiate(prefab);
            },
            onError: (error) => Debug.LogError($"Load error: {error}")
        ));
    }
}
