//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;
//using SIDGIN.Patcher.Client;

//public class CheckUpdate : MonoBehaviour
//{
//    public ClientSettings client;
//     //public GameObject objtest;
//    // Start is called before the first frame update
//    void Awake()
//    {
//        StartCoroutine(Getid());
//        //SpriteRenderer sprite = GetComponent<SpriteRenderer>();
//        //debug.Log(sprite.bounds.size.y);
//        //// GameObject test = Instantiate(objtest, transform.position, Quaternion.identity) as GameObject;
//        //objtest.transform.position = new Vector3(sprite.transform.position.x, sprite.bounds.min.y + 0.4f);
//        IEnumerator Getid()
//        {
//            UnityWebRequest www = new UnityWebRequest("http://202.92.6.223:4567/linkdatanew");
//            //UnityWebRequest www = new UnityWebRequest("http://192.168.0.107:4567/linkdatanew");
//            www.downloadHandler = new DownloadHandlerBuffer();
//            yield return www.SendWebRequest();

//            if (www.result != UnityWebRequest.Result.Success)
//            {
//                debug.Log(www.error);
//            }
//            else
//            {
//                // Show results as text
//                debug.Log(www.downloadHandler.text);
//                client.appId = www.downloadHandler.text;
//                SGPatcherControl sgcontroller = GetComponent<SGPatcherControl>();
//                sgcontroller.CheckUpdate();
//            }
//        }
//    }
//}
