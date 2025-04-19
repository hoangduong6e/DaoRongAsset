using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendRequest
{
    private Queue<RequestItem> requestQueue = new Queue<RequestItem>();
    private bool isSending = false;

    public void SendServer(JSONClass data, Action<JSONNode> action, bool useAltEvent = false, float timeout = 10f)
    {
        requestQueue.Enqueue(new RequestItem()
        {
            data = data,
            callback = action,
            useAltEvent = useAltEvent,
            timeout = timeout
        });

        if (!isSending)
          NetworkManager.ins.StartCoroutine(ProcessNextRequest());
    }

    // Request item
    private class RequestItem
    {
        public JSONClass data;
        public Action<JSONNode> callback;
        public bool useAltEvent;
        public float timeout;
    }

    // Request processor
    private IEnumerator ProcessNextRequest()
    {
        while (requestQueue.Count > 0)
        {
            isSending = true;
            RequestItem item = requestQueue.Dequeue();

           // CrGame.ins.panelLoadDao.SetActive(true);
            bool responded = false;
            string eventName = item.useAltEvent ? "SendRequest2" : "SendRequest";

            NetworkManager.ins.socket.EmitWithJSONClass(eventName, item.data, (response) =>
            {
                responded = true;
                CrGame.ins.panelLoadDao.SetActive(false);
                Debug.Log("Server response: " + response.ToString());
                item.callback(response[0]);
            });

            float t = 0;
            while (t < item.timeout && !responded)
            {
                t += Time.deltaTime;
                yield return null;
            }

            if (!responded)
            {
                Debug.LogError("Timeout: Không nhận được phản hồi từ server sau " + item.timeout + " giây.");
                CrGame.ins.panelLoadDao.SetActive(false);
                // Gợi ý: hiển thị popup lỗi ở đây
            }

        //  if(!item.useAltEvent) yield return new WaitForSeconds(0.2f); // Khoảng cách giữa các request (có thể điều chỉnh)
        }

        isSending = false;
    }
}
