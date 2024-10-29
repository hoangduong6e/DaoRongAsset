using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDatTen : MonoBehaviour
{
    // Start is called before the first frame update
    NetworkManager net;
    void Start()
    {
        net = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NetworkManager>();
    }

    public void DatTen(InputField input)
    {
        if (input.text != "")
        {
            net.socket.Emit("SetName", JSONObject.CreateStringObject(input.text));
        }
    }
}
