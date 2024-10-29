using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollBai : MonoBehaviour
{
    public void DoneRoll()
    {
        GiaoDienRuongThanBi gdruong = transform.parent.transform.parent.GetComponent<GiaoDienRuongThanBi>();
        gdruong.DonePhatBai();
    }
}
