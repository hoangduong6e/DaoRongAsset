using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class DLIMGR : MonoBehaviour
{
    // Start is called before the first frame update
    //  public List<string> allIMGR = new List<string>();
   // public MR mr;
    void Start()
    {
      //  StartCoroutine(GetAssetBundle());
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    if(transform.GetChild(i).name != "Icon" && transform.GetChild(i).name != "vong tron " && transform.GetChild(i).name != "bong")
        //    {
        //        StartCoroutine(DownloadImage(transform.GetChild(i).name, transform.GetChild(i).GetComponent<SpriteRenderer>()));
        //    }
        //}
    }

    //IEnumerator DownloadImage(string namefile, SpriteRenderer spriteRenderer)
    //{
    //    string link = "http://" + LoginFacebook.ins.ServerChinh + "/LIMGR/name/" + gameObject.name + "/namemanh/" + namefile;
    //    debug.Log("linkkkkk " + link);
    //    UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
    //    yield return request.SendWebRequest();
    //    if (request.isNetworkError || request.isHttpError)
    //        debug.Log(request.error);
    //    else
    //    {
    //        debug.Log(request.downloadHandler);
    //        Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
    //        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
    //        spriteRenderer.sprite = sprite;
    //        debug.Log("Download mảnh " + namefile + " thành công");
    //        //img.sprite = sprite;
    //        //img.SetNativeSize();
    //    }
    //}

    //IEnumerator GetAssetBundle()
    //{
    //    string nameasset = "manh" + gameObject.name.ToLower();
    //    UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(DownLoadAssetBundle.linkdown + nameasset);
    //    // DownloadHandler handle = www.downloadHandler;
    //    www.SendWebRequest();
    //    while (!www.isDone)
    //    {
    //        debug.Log("Downloading... " + (int)(www.downloadProgress * 100f) + "%");
    //        yield return new WaitForSeconds(.01f);
    //    }
    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        debug.Log(www.error);
    //    }
    //    else
    //    {
    //        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
    //        //debug.Log("down ok");
            
    //        //Texture2D tex = bundle.LoadAsset<Texture2D>("Mom");
         

    //        //Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

    //        //transform.Find(tex.name).GetComponent<SpriteRenderer>().sprite = mySprite ;
    //        //debug.Log(tex.name);
    //        //  GameObject g = bundle.LoadAsset(namerong) as GameObject;

    //        //bundle.Unload(true);
    //        // save(handle.data, "");


    //        Texture2D[] alltex = bundle.LoadAllAssets<Texture2D>();
    //        for (int i = 0; i < alltex.Length; i++)
    //        {
    //            Sprite spritemanh = Sprite.Create(alltex[i], new Rect(0.0f, 0.0f, alltex[i].width, alltex[i].height), new Vector2(0.5f, 0.5f), 100.0f);
    //            spritemanh.name = alltex[i].name;
    //            transform.Find(alltex[i].name).GetComponent<SpriteRenderer>().sprite = spritemanh;
    //            debug.Log(alltex[i].name);
    //        }
    //    }

    }

