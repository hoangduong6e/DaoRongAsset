using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timePvp : MonoBehaviour
{
    Text txtTime;
    // Start is called before the first frame update
    void Start()
    {
        txtTime = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
      //  debug.Log("max time " + gameObject.name + " parent  "  +transform.parent.name + " parentt " + transform.parent.transform.parent);
        if (GiaoDienPVP.ins.maxtime > 0)
        {
            GiaoDienPVP.ins.maxtime -= Time.deltaTime;
            int sec = (int)GiaoDienPVP.ins.maxtime, min = 0;
            while (sec >= 60)
            {
                sec -= 60;
                min += 1;
            }
            txtTime.text = min + ":" + sec;
        }
        else
        {
            if(VienChinh.vienchinh.chedodau == CheDoDau.ThuThach) VienChinh.vienchinh.Thang();
            else VienChinh.vienchinh.Thua();
        }
    }
}
