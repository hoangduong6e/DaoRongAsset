using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLogin : MonoBehaviour
{
    // Start is called before the first frame update
    public float timecho = 0;
    public LoginFacebook loginfb;
    Text txttimecho;
    private void Start()
    {
        txttimecho = loginfb.panelloaddao.transform.GetChild(1).GetComponent<Text>();
    }
    private void Update()
    {
        if (timecho > 0)
        {
            timecho -= Time.deltaTime;

            txttimecho.text = "Đang nhập sau " + Mathf.Floor( timecho) + " giây";
            if(timecho <= 1)
            {
                StartCoroutine(loginfb.Loginfb());
                txttimecho.text = "Đang đăng nhập...";
                timecho = 0;
            }
        }
    }
}
