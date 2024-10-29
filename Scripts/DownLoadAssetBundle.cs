//using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownLoadAssetBundle : MonoBehaviour
{
    // Start is called before the first frame update
    public static string link = "";
#if UNITY_ANDROID
    public static string linkdown = "/DaoRongData2/Android/";
#endif
#if UNITY_IOS
  public static string linkdown = "/DaoRongData2/IOS/";
#endif

    public static DownLoadAssetBundle ins;
    public static Dictionary<string, RuntimeAnimatorController> dataKhungavt = new Dictionary<string, RuntimeAnimatorController>();
    public static Dictionary<string, GameObject> ObjectRong = new Dictionary<string, GameObject>();

    public static AssetBundle bundleDragon;

    private void Start()
    {
        ins = this;
        //StartCoroutine(DownLoadAllRong());
        //DownLoadObjectRong("rongvang2");
      //  linkdown = link + linkdown;
    }
    public static string SetLinkDown { set { link = value; linkdown = "https://" + link + linkdown;} }
    public void DownAssetBundleAnimator(Image img, string namekhung)
    {
        string nameasset = "animator" + namekhung.ToLower();
        if (dataKhungavt.ContainsKey(nameasset))
        {
            debug.Log("load from cache");
            StartCoroutine(addAnim(dataKhungavt[nameasset]));
            return;
        }
        StartCoroutine(GetAssetBundle());
        IEnumerator GetAssetBundle()
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(linkdown + nameasset);
            // DownloadHandler handle = www.downloadHandler;
            www.SendWebRequest();
            while (!www.isDone)
            {
                debug.Log("Downloading... " + (int)(www.downloadProgress * 100f) + "%");
                yield return new WaitForSeconds(0.2f);
            }
            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                debug.Log("down ok");
                RuntimeAnimatorController tmpController = bundle.LoadAsset("Khung") as RuntimeAnimatorController;

                //  RuntimeAnimatorController tmpController = bundle.LoadAsset("AssetsBundles") as RuntimeAnimatorController;
                StartCoroutine(addAnim(tmpController));
                dataKhungavt.Add(nameasset, tmpController);
                //  debug.LogError("name controller " + tmpController.name);
                // RuntimeAnimatorController controller = Instantiate(tmpController);
                //  bundle.Unload(true);
                // save(handle.data, "");
            }

        }
        IEnumerator addAnim(RuntimeAnimatorController anim)
        {
            Animator animator = null;
            if (!img.GetComponent<Animator>())
            {
                animator = img.AddComponent<Animator>();
            }
            else animator = img.GetComponent<Animator>();
            animator.runtimeAnimatorController = anim;
            yield return new WaitUntil(() => animator.runtimeAnimatorController != null);
         //   yield return new WaitForSeconds(0.6f);
            img.SetNativeSize();
        }
    }

    public void DownLoadObjectRong(string namerong)
    {
        string nameasset = namerong.ToLower();
        if (ObjectRong.ContainsKey(nameasset))
        {
            debug.Log("load from cache");
            addObjRong(ObjectRong[nameasset]);
        }
        StartCoroutine(GetAssetBundle());
        IEnumerator GetAssetBundle()
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(linkdown + nameasset);
            // DownloadHandler handle = www.downloadHandler;
            www.SendWebRequest();
            while (!www.isDone)
            {
                debug.Log("Downloading... " + (int)(www.downloadProgress * 100f) + "%");
                yield return new WaitForSeconds(.01f);
            }
            if (www.result != UnityWebRequest.Result.Success)
            {
                debug.Log(www.error);
            }
            else
            {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                debug.Log("down ok");
                GameObject g = bundle.LoadAsset(namerong) as GameObject;

                //  RuntimeAnimatorController tmpController = bundle.LoadAsset("AssetsBundles") as RuntimeAnimatorController;
                addObjRong(g);
                GameObject abc = Instantiate(g, transform.position, Quaternion.identity);
                //  Instantiate(g);
                //  debug.LogError("name controller " + tmpController.name);
                // RuntimeAnimatorController controller = Instantiate(tmpController);
                //bundle.Unload(true);
                // save(handle.data, "");
            }

        }
        void addObjRong(GameObject g)
        {
            ObjectRong.Add(namerong, g);
        }
    }
    void save(byte[] data, string path)
    {
        //Create the Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        try
        {
            File.WriteAllBytes(path, data);
            debug.Log("Saved Data to: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            debug.LogWarning("Failed To Save Data to: " + path.Replace("/", "\\"));
            debug.LogWarning("Error: " + e.Message);
        }
    }


  
}
