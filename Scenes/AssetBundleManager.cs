using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AssetBundleManager : MonoBehaviour
{
    public static JSONNode infoasbundle;
    public static string encryptionKey = "1111111111111111"; // Khóa mã hóa, cần 16, 24 hoặc 32 ký tự

    public static byte[] EncryptData(byte[] data)
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
    public static byte[] DecryptData(byte[] encryptedData)
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

     // Mã hóa dữ liệu (byte[])


    // Mã hóa chuỗi
    public static string EncryptString(string plainText)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] encryptedBytes = EncryptData(plainBytes);
        string base64String = Convert.ToBase64String(encryptedBytes); // Chuyển sang Base64 để dễ lưu trữ

        return base64String.Replace("/", "_").Replace("\\", "-");
    }

    // Giải mã chuỗi
    public static string DecryptString(string encryptedText)
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
        byte[] decryptedBytes = DecryptData(encryptedBytes);
        string base64String =  Encoding.UTF8.GetString(decryptedBytes);
        return base64String.Replace("/", "_").Replace("\\", "-");
    }

    
    // Đường dẫn đầy đủ tới file được lưu trong `persistentDataPath`
    private string GetEncryptedFilePath(string fileName) => Path.Combine(Application.persistentDataPath, fileName);

    // Hàm mã hóa dữ liệu

    // Tải `AssetBundle` từ server
    public IEnumerator DownloadAssetBundle(string url, string fileName, Action onSuccess = null, Action<string> onError = null, Action<float> updateProcess = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Bắt đầu tải AssetBundle
            www.SendWebRequest();

            // Theo dõi tiến trình tải xuống
            while (!www.isDone)
            {
                // Nếu có hàm callback updateProcess, gọi để cập nhật tiến trình
                updateProcess?.Invoke(www.downloadProgress); // downloadProgress trả về giá trị từ 0.0f đến 1.0f
               // debug.Log($"Downloading... {www.downloadProgress * 100f}%");

                yield return null; // Đợi đến khung hình tiếp theo
            }

            // Khi quá trình tải hoàn tất, gọi callback với tiến trình = 1.0
            updateProcess?.Invoke(1f);

            // Kiểm tra kết quả tải xuống
            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    byte[] encryptedData = EncryptData(www.downloadHandler.data); // Mã hóa dữ liệu tải xuống
                    File.WriteAllBytes(GetEncryptedFilePath(fileName), encryptedData); // Ghi dữ liệu vào file
                    debug.Log($"AssetBundle downloaded and encrypted as {fileName}");
                    onSuccess?.Invoke(); // Gọi callback thành công
                }
                catch (Exception e)
                {
                    debug.LogError($"Error encrypting or saving AssetBundle: {e.Message}");
                    onError?.Invoke(e.Message); // Gọi callback khi lỗi
                }
            }
            else
            {
                debug.LogError($"Error downloading AssetBundle: {www.error}");
                onError?.Invoke(www.error); // Gọi callback khi lỗi tải xuống
            }
        }
    }

    // Tải `AssetBundle` từ bộ nhớ

    private bool CheckFileContains(string fileName)
    {
        string filePath = GetEncryptedFilePath(fileName);

        return File.Exists(filePath);
    }


    public static Dictionary<string,AssetBundle> allAssetLoaded = new();
    public IEnumerator LoadAssetBundle(string fileName, Action<AssetBundle> onSuccess, Action<string> onError)
    {
        debug.Log("LoadAssetBundle: " + fileName);
        fileName = EncryptString(fileName);
        if(allAssetLoaded.ContainsKey(fileName))
        {
             onSuccess?.Invoke(allAssetLoaded[fileName]);
            yield break;
        }
        string filePath = GetEncryptedFilePath(fileName);

        if (!File.Exists(filePath))
        {
            debug.LogError($"AssetBundle file not found: {filePath}");
            onError?.Invoke("File not found");
            yield break;
        }

        byte[] decryptedData = null;//

        // Đọc và giải mã dữ liệu trong try-catch
        try
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            decryptedData = DecryptData(encryptedData);
        }
        catch (Exception e)
        {
            debug.LogError($"Error decrypting AssetBundle: {e.Message}");
            onError?.Invoke(e.Message);
            yield break;
        }

        // Tải AssetBundle từ dữ liệu đã giải mã
        AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(decryptedData);
        yield return request;

        if (request.assetBundle != null)
        {
            debug.Log($"AssetBundle {fileName} loaded successfully");
            onSuccess?.Invoke(request.assetBundle);
            allAssetLoaded.Add(fileName,request.assetBundle);
        }
        else
        {
            debug.LogError($"Failed to load AssetBundle: {fileName}");
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
            debug.Log($"Deleted AssetBundle file: {filePath}");
        }
        else
        {
            debug.LogWarning($"File not found for deletion: {filePath}");
        }
    }
    public static AssetBundleManager ins;
    // Test ví dụ
    private void Awake()
    {
        ins = this;
    }
    private void Start()
    {
        //string testUrl = "https://daorongmobile.online/DaoRongData3/IOS/animtienhoa"; // URL của AssetBundle
        //string testFileName = "animtienhoa";


        //debug.Log(CheckFileContains(testFileName) ? "Đã tổn tại file " + testFileName : "Không tồn tại file: " + testFileName);
        //StartCoroutine(DownloadAssetBundle(testUrl, testFileName,
        //    onSuccess: () => debug.Log("Download and save successful"),
        //    onError: (error) => debug.LogError($"Download error: {error}"),
        //    updateProcess: (update) => debug.Log($"Download process: {update}")
        //));

        //StartCoroutine(DownloadAssetBundleKoMaHoa(testUrl, testFileName,
        //    onSuccess: () => debug.Log("Download and save successful"),
        //    onError: (error) => debug.LogError($"Download error: {error}"),
        //    updateProcess: (update) => debug.Log($"Download process: {update}")
        //));
        //StartCoroutine(LoadAssetBundle(testFileName, 
        //    onSuccess: (assetBundle) =>
        //    {
        //        debug.Log("AssetBundle loaded successfully");
        //        // Sử dụng AssetBundle ở đây, ví dụ:
        //        GameObject prefab = assetBundle.LoadAsset<GameObject>("animtienhoa");
        //        Instantiate(prefab);
        //    },
        //    onError: (error) => debug.LogError($"Load error: {error}")
        //));


    }


    public IEnumerator DownloadAssetBundleKoMaHoa(string url, string fileName, Action onSuccess = null, Action<string> onError = null, Action<float> updateProcess = null)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Bắt đầu yêu cầu tải xuống
            www.SendWebRequest();
            // Theo dõi tiến trình tải xuống
            while (!www.isDone)
            {
                // Nếu có hàm callback updateProcess, gọi để cập nhật tiến trình
                updateProcess?.Invoke(www.downloadProgress); // downloadProgress trả về giá trị từ 0.0f đến 1.0f
                   debug.Log($"Downloading ko mã hóa... {www.downloadProgress * 100f}%");

                yield return null; // Đợi đến khung hình tiếp theo
            }
            // Kiểm tra kết quả tải
            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    // Lưu dữ liệu tải về vào file trực tiếp (không mã hóa)
                    File.WriteAllBytes(GetFilePath(fileName), www.downloadHandler.data);
                    debug.Log($"AssetBundle downloaded and saved as {fileName}");
                    onSuccess?.Invoke(); // Gọi callback thành công (nếu có)
                }
                catch (Exception e)
                {
                    // Xử lý lỗi khi lưu file
                    debug.LogError($"Error saving AssetBundle: {e.Message}");
                    onError?.Invoke(e.Message); // Gọi callback lỗi (nếu có)
                }
            }
            else
            {
                // Xử lý lỗi tải xuống
                debug.LogError($"Error downloading AssetBundle: {www.error} " + url);
                onError?.Invoke(www.error); // Gọi callback lỗi (nếu có)
            }
        }
    }

    // Hàm hỗ trợ lấy đường dẫn lưu file
    private string GetFilePath(string fileName)
    {
        // Đường dẫn lưu file trong Application.persistentDataPath
        return Path.Combine(Application.persistentDataPath, fileName);
    }


    public IEnumerator CheckAndDownLoadAll(Action Success = null,Action<string> onError = null, Action<float> updateProcess = null, Action<string> status = null)
    {
        //     updateText?.Invoke("Đang kiểm tra bản cập nhật...");

        int count = infoasbundle.Count;
            
        double process = 0;

        foreach (string id in infoasbundle.AsObject.Keys)
        {
            debug.Log("check id la: " + id);
            float vercurrent = 0;
          //  string id = infoasbundle[i]["id"].AsString;
            float ver = infoasbundle[id]["ver"].AsFloat;
            //string name = infoasbundle[id]["name"].AsString;
            string namefile = EncryptString(id);
            if (PlayerPrefs.HasKey(namefile)) vercurrent = PlayerPrefs.GetFloat(namefile);
            bool checkContainFile = CheckFileContains(namefile);
            debug.Log("file tồn tại " + checkContainFile.ToString() + " ver cũ: " + vercurrent + ", ver mới: " + ver);
            if (vercurrent != ver || !checkContainFile)
            {
                if (checkContainFile)
                {
                    DeleteAssetBundle(namefile);
                    debug.Log("cập nhật: " + id);
                    ChangeStatus("Đang cập nhật.. ");
                }
                else ChangeStatus("Đang tải dữ liệu: ");
                if(ver > 0)
                {
                   yield return DownloadAssetBundleKoMaHoa(DownLoadAssetBundle.linkdown + id, namefile, ThanhCong, Error, UpdateProcess);
                    process += Math.Floor((double)90 / count);
                   PlayerPrefs.SetFloat(namefile, ver);
                }
                else PlayerPrefs.DeleteKey(namefile);
        

            }
            else count -= 1;

            void UpdateProcess(float f)
            {
                double invoke = process + Math.Round(f * 100f / count, 2);
                //process = 50 + invoke;

                debug.Log("invoke: " + invoke);
                updateProcess?.Invoke((float)invoke);
            }
        }

        //    for (int i = 0; i < infoasbundle.Count;i++)
        //{
         
        //}
        if(process != 0) updateProcess?.Invoke(100f);
        void ThanhCong()
        {
            debug.Log("tải thành công");
            Success?.Invoke();
        }
        void Error(string err)
        {
            onError?.Invoke(err);
        }
       void ChangeStatus(string s)
        {
            status?.Invoke(s);
        }
    }
}
