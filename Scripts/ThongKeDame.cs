using SimpleJSON;
using UnityEngine;

public class ThongKeDame : MonoBehaviour
{
    public static bool Set = false;
    public static JSONClass Data = new JSONClass();
    public static void SetEnableThongKe()
    {
        if (VienChinh.vienchinh.chedodau == CheDoDau.Online || VienChinh.vienchinh.chedodau == CheDoDau.Replay || VienChinh.vienchinh.chedodau == CheDoDau.ThuThach) Set = false;
        else Set = true;
    }    
    public static void AddThongKe(CData data)
    {
        if (!Set) return;
        if (Data[data.team][data.nameobject].ISNull) return;
        string stype = data.type.ToString();
        float damecong = Data[data.team][data.nameobject][data.id][stype].AsFloat + data.cong;
        Data[data.team][data.nameobject][data.id][stype] = damecong.ToString();
    }
    public static void ResetThongKe()
    {
        Data = new JSONClass();
    }
    public class CData
    {
        public string team, nameobject, id;
        public float cong;
        public EType type;
        public CData(string Team, string Nameobject, string Id, float Cong, EType Type)
        {
            team = Team;
            nameobject = Nameobject;
            id = Id;
            cong = Cong;
            type = Type;
        }
    }
    public enum EType
    {
        dame,
        netranh,
        chimang,
        hoiphuc,
        chongchiu
    }    

}

