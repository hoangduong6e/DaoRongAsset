using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D.Animation;

public class Setting : MonoBehaviour
{
    public static bool amthanh = true;
    public static bool hieuungamthanh = true;
    public static CauHinh cauhinh = CauHinh.CauHinhCao;
    public static string GetStrCauHinh(CauHinh cauhinh)
    {
        switch (cauhinh)
        {
            case CauHinh.CauHinhCao:
                return "Cấu hình cao";
            case CauHinh.CauHinhTrungBinh:
                return "Cấu hình trung bình";
            case CauHinh.CauHinhThap:
                return "Cấu hình thấp";
        }
        return "";
    }
    public class CauHinhThap
    {
        public static void OffspriteSkin(Transform Rong)
        {
            Transform child0 = Rong.transform.GetChild(0);
            for (int i = 0; i < child0.transform.childCount; i++)
            {
                SpriteSkin spriteSkin = child0.transform.GetChild(i).GetComponent<SpriteSkin>();
                if (spriteSkin != null)
                {
                    spriteSkin.enabled = false;
                    spriteSkin.alwaysUpdate = false;
                }
            }
        }
    }
}
public enum CauHinh
{
    CauHinhThap,
    CauHinhTrungBinh,
    CauHinhCao,
}
