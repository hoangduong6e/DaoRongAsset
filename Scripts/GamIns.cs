using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamIns : MonoBehaviour
{
    private float m_minX, m_minY, m_maxX, m_maxY;
    public float minX { get { return m_minX; } }
    public float minY { get { return m_minY; } }
    public float MaxX { get { return m_maxX; } }
    public float MaxY { get { return m_maxY; } }

    //private float limitOffset = 1;
    public static GamIns ins;
    // Start is called before the first frame update
    void Start()
    {
        ins = this;
        m_minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + 1;
        m_maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - 1;
        m_minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + 2;
        m_maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - 1;

    }
    public static string CatDauNgoacKep(string s)
    {
        string[] cat = s.Split('"');
        if (cat.Length > 1)
        {
            return cat[1];
        }

        return cat[0];
    }
    public static string CatDauNgoacKep(string s, bool replacedauphay = false)
    {
        string[] cat = s.Split('"');
        if (cat.Length > 1)
        {
            if (!replacedauphay) return cat[1];
            else return ReplaceDauPhay(cat[1]);
        }

        if (!replacedauphay) return cat[0];
        else return ReplaceDauPhay(cat[0]);
    }
    public static string ReplaceDauPhay(string s)
    {
        return s.Replace(",", ".");
    }    

    public static float GetFPS()
    {
        float fps = (int)(1f / Time.unscaledDeltaTime);
        return fps;
    }
    public static float getSecondsLoad()
    {
        float fps = GetFPS();
        debug.Log("fps hien tai " + fps);
       // return 0.02f;
        if (fps >= 40)
        {
            return 0.01f;
        }
        else if (fps < 40 && fps >= 30)
        {
            return 0.02f;
        }
        else if (fps < 30 && fps >= 20)
        {
            return 0.03f;
        }
        else
        {
            return 0.05f;
        }
    }
    public static void SetNativeSizeAnimator(Animator anim)
    {
        CrGame.ins.StartCoroutine(SetNativeSizeAnim());
        IEnumerator SetNativeSizeAnim()
        {
            yield return new WaitUntil(() => anim.runtimeAnimatorController != null);
            anim.GetComponent<Image>().SetNativeSize();
        }
    }
    public static string FormatCash(double n)
    {
        string result = "";

        if (n < 1e3) return n.ToString();

        if (n >= 1e3 && n < 1e6)
        {
            double value = n / 1e3;
            result = $"{value:F2}K";
            if (value % 1 == 0) result = $"{(int)value}K";
            return result;
        }

        if (n >= 1e6 && n < 1e9)
        {
            double value = n / 1e6;
            result = $"{value:F2}M";
            if (value % 1 == 0) result = $"{(int)value}M";
            return result;
        }

        if (n >= 1e9 && n < 1e12)
        {
            double value = n / 1e9;
            result = $"{value:F2}B";
            if (value % 1 == 0) result = $"{(int)value}B";
            return result;
        }

        if (n >= 1e12)
        {
            double value = n / 1e12;
            result = $"{value:F2}T";
            if (value % 1 == 0) result = $"{(int)value}T";
            return result;
        }

        return result;
    }
    public static void ResizeItem(Image image, float size = 75f)
    {
        // Lấy kích thước gốc của image
        float originalWidth = image.rectTransform.rect.width;
        float originalHeight = image.rectTransform.rect.height;

        // Tính toán tỉ lệ thu nhỏ
        float widthRatio = size / originalWidth;
        float heightRatio = size / originalHeight;
        float minRatio = Mathf.Min(widthRatio, heightRatio);

        // Điều chỉnh kích thước
        image.rectTransform.sizeDelta = new Vector2(originalWidth * minRatio, originalHeight * minRatio);
    }
    public static GameObject Instantiate(GameObject g,Vector3 vec, Quaternion q)
    {
        return Instantiate(g,vec,q);
    }
}
