using EZ_Pooling;
using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PVEManager : MonoBehaviour
{
    //  public static Dictionary<string, string> DicNameSkillDra = new Dictionary<string, string>() { };
    public static GameObject GetSkillDra(string namedra)
    {
        GameObject obj = Resources.Load("GameData/SkillDra/skill" + namedra) as GameObject;
        if (obj != null)
        {
            return obj;
        }
        return Resources.Load("GameData/SkillDra/skillRongMaTroi") as GameObject;
    }
    public static void GetScale(Transform tf)
    {
        Vector3 Scale;
        Scale = tf.localScale;
        string Team = tf.transform.parent.name;
        if (Team == "TeamXanh")
        {
           // TeamDich = VienChinh.vienchinh.TeamDo;
            Scale.x = -Scale.x;
            tf.localScale = Scale;
        }
        else
        {
           // TeamDich = VienChinh.vienchinh.TeamXanh;
            //chiso.ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
        }
    }
    //public static Vector3 GetRandomTFtrieuHoi(string team, string Idlle,DraHeight draheight)
    //{
    //    Vector3 randomvec = new Vector3();
    //    if (team == "TeamXanh")
    //    {
    //        if (Idlle == "Idlle")
    //        {
    //            randomvec = ;
    //        }   
    //        else
    //        {
    //            randomvec = new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0.5f,1.5f), VienChinh.vienchinh.TruXanh.transform.position.z);
    //        }
    //    }
    //    else
    //    {
    //        float xcong = 1.8f;
    //        //if (VienChinh.vienchinh.chedodau == "Online") xcong += 2;
    //        if (Idlle == "Idlle")
    //        {
    //            randomvec = new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - xcong, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(-3f,-1), VienChinh.vienchinh.TruDo.transform.position.z);
    //        }
    //        else
    //        {
    //            randomvec = new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - xcong, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0.5f,1.5f), VienChinh.vienchinh.TruDo.transform.position.z);
    //        }    
    //    }
    //    return randomvec;
    //}

    //private static readonly Dictionary<string, Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>> VecHeight =
    //     new Dictionary<string, Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>>
    //     {
    //         {
    //             "TeamXanh", new Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>
    //             {
    //                 {
    //                     "Idlle", new Dictionary<DraHeight, Func<Vector3>>
    //                     {
    //                         { DraHeight.DEFAULT, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(-2.3f,-1f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                         { DraHeight.NuTamXuan, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0f,0.5f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                     }
    //                 },
    //                 {
    //                     "Flying", new Dictionary<DraHeight, Func<Vector3>>
    //                     {
    //                         { DraHeight.DEFAULT, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0f,0.5f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                         { DraHeight.HacLong, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(2.6f,2.9f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                         { DraHeight.LMX_PH_2DAU, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y +Random.Range(-0.3f,1.2f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                         { DraHeight.RongLua, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0.3f,1f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                         { DraHeight.PH, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0.2f,1.8f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                         { DraHeight._2DAU, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y +Random.Range(-0.5f,0.8f), VienChinh.vienchinh.TruXanh.transform.position.z) },
    //                         { DraHeight.MaThach, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y +Random.Range(-0.8f,0f), VienChinh.vienchinh.TruXanh.transform.position.z) },

    //                     }
    //                 }
    //             }
    //         },
    //         {
    //             "TeamDo", new Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>
    //             {
    //                 {
    //                     "Idlle", new Dictionary<DraHeight, Func<Vector3>>
    //                     {
    //                         { DraHeight.DEFAULT, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(-2.3f,-1), VienChinh.vienchinh.TruDo.transform.position.z) },
    //                         { DraHeight.NuTamXuan, () => new Vector3(VienChinh.vienchinh.TruXanh.transform.position.x + 2, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0f,0.5f), VienChinh.vienchinh.TruXanh.transform.position.z) },

    //                     }
    //                 },
    //                 {
    //                     "Flying", new Dictionary<DraHeight, Func<Vector3>>
    //                     {
    //                         { DraHeight.DEFAULT, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0f,0.5f), VienChinh.vienchinh.TruDo.transform.position.z) },
    //                         { DraHeight.HacLong, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(2.6f,2.9f), VienChinh.vienchinh.TruDo.transform.position.z) },
    //                         { DraHeight.LMX_PH_2DAU, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y +Random.Range(-0.3f,1.2f), VienChinh.vienchinh.TruDo.transform.position.z) },
    //                         { DraHeight.RongLua, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(1.3f,1.5f), VienChinh.vienchinh.TruDo.transform.position.z) },
    //                         { DraHeight.PH, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y + Random.Range(0.2f,1.8f), VienChinh.vienchinh.TruDo.transform.position.z) },
    //                         { DraHeight._2DAU, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y + +Random.Range(-0.5f,0.8f), VienChinh.vienchinh.TruDo.transform.position.z) },
    //                         { DraHeight.MaThach, () => new Vector3(VienChinh.vienchinh.TruDo.transform.position.x - 1.8f, VienChinh.vienchinh.TruXanh.transform.position.y + +Random.Range(-0.8f,0f), VienChinh.vienchinh.TruDo.transform.position.z) },

    //                     }
    //                 }
    //             }
    //         }
    //     };

    public static float XTruTeamXanh = 17.40f;
    public static float XTruTeamDo = 42.82f;

     public static float YTruTeamXanh = -1.70f;
    public static Dictionary<Team, float> XTruTeam = new Dictionary<Team, float> {
        {Team.TeamXanh, XTruTeamXanh },
        {Team.TeamDo,  XTruTeamDo },
    };

    private static readonly Dictionary<string, Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>> VecHeight =
     new Dictionary<string, Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>>
     {
            {
                "TeamXanh", new Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>
                {
                    {
                        "Idlle", new Dictionary<DraHeight, Func<Vector3>>
                        {
                            { DraHeight.DEFAULT, () => new Vector3(XTruTeamXanh, YTruTeamXanh + Random.Range(-2.3f,-1f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                            { DraHeight.NuTamXuan, () => new Vector3(XTruTeamXanh, YTruTeamXanh + Random.Range(0f,0.5f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                        }
                    },
                    {
                        "Flying", new Dictionary<DraHeight, Func<Vector3>>
                        {
                            { DraHeight.DEFAULT, () => new Vector3(XTruTeamXanh, YTruTeamXanh + Random.Range(0f,0.5f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                            { DraHeight.HacLong, () => new Vector3(XTruTeamXanh, YTruTeamXanh + Random.Range(2.6f,2.9f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                            { DraHeight.LMX_PH_2DAU, () => new Vector3(XTruTeamXanh, YTruTeamXanh +Random.Range(-0.3f,1.2f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                            { DraHeight.RongLua, () => new Vector3(XTruTeamXanh, YTruTeamXanh + Random.Range(0.3f,1f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                            { DraHeight.PH, () => new Vector3(XTruTeamXanh, YTruTeamXanh + Random.Range(0.2f,1.8f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                            { DraHeight._2DAU, () => new Vector3(XTruTeamXanh, YTruTeamXanh +Random.Range(-0.5f,0.8f), VienChinh.vienchinh.TruXanh.transform.position.z) },
                            { DraHeight.MaThach, () => new Vector3(XTruTeamXanh, YTruTeamXanh +Random.Range(-0.8f,0f), VienChinh.vienchinh.TruXanh.transform.position.z) },

                        }
                    }
                }
            },
            {
                "TeamDo", new Dictionary<string, Dictionary<DraHeight, Func<Vector3>>>
                {
                    {
                        "Idlle", new Dictionary<DraHeight, Func<Vector3>>
                        {
                            { DraHeight.DEFAULT, () => new Vector3(XTruTeamDo, YTruTeamXanh + Random.Range(-2.3f,-1), VienChinh.vienchinh.TruDo.transform.position.z) },
                            { DraHeight.NuTamXuan, () => new Vector3(XTruTeamDo, YTruTeamXanh + Random.Range(0f,0.5f), VienChinh.vienchinh.TruDo.transform.position.z) },

                        }
                    },
                    {
                        "Flying", new Dictionary<DraHeight, Func<Vector3>>
                        {
                            { DraHeight.DEFAULT, () => new Vector3(XTruTeamDo, YTruTeamXanh + Random.Range(0f,0.5f), VienChinh.vienchinh.TruDo.transform.position.z) },
                            { DraHeight.HacLong, () => new Vector3(XTruTeamDo, YTruTeamXanh + Random.Range(2.6f,2.9f), VienChinh.vienchinh.TruDo.transform.position.z) },
                            { DraHeight.LMX_PH_2DAU, () => new Vector3(XTruTeamDo, YTruTeamXanh +Random.Range(-0.3f,1.2f), VienChinh.vienchinh.TruDo.transform.position.z) },
                            { DraHeight.RongLua, () => new Vector3(XTruTeamDo, YTruTeamXanh + Random.Range(1.3f,1.5f), VienChinh.vienchinh.TruDo.transform.position.z) },
                            { DraHeight.PH, () => new Vector3(XTruTeamDo, YTruTeamXanh + Random.Range(0.2f,1.8f), VienChinh.vienchinh.TruDo.transform.position.z) },
                            { DraHeight._2DAU, () => new Vector3(XTruTeamDo, YTruTeamXanh + +Random.Range(-0.5f,0.8f), VienChinh.vienchinh.TruDo.transform.position.z) },
                            { DraHeight.MaThach, () => new Vector3(XTruTeamDo, YTruTeamXanh + +Random.Range(-0.8f,0f), VienChinh.vienchinh.TruDo.transform.position.z) },

                        }
                    }
                }
            }
     };

    public static void TrieuHoiDra(JSONObject data, string team, Vector3 randomvec = new Vector3())
    {
        GTrieuHoiDra(data, team,randomvec);
    }
   public static GameObject GTrieuHoiDra(JSONObject data, string team, Vector3 randomvec = new Vector3())
    {
        string nameObject = data["nameobject"].str;
        //  debug.Log("rong1");
        // GameObject hieuung = Instantiate(vienchinh.HieuUngTrieuHoi, new Vector3(randomvec.x, 6, randomvec.z), Quaternion.identity) as GameObject;
        GameObject rongtrieuhoi = Instantiate(Inventory.GetObjRong(nameObject + "2"),Vector3.zero, Quaternion.identity);
        SetScaleDragon(nameObject, byte.Parse(data["sao"].ToString()), rongtrieuhoi.transform);

        //string _team = "";
        //if (VienChinh.vienchinh.Teamthis != Team.TeamXanh)
        //{
        //    _team = team == "TeamXanh" ? "TeamDo" : "TeamXanh";
        //}
        DraInstantiate draInstantiate = rongtrieuhoi.GetComponent<DraInstantiate>();


        //Transform child0 = rongtrieuhoi.transform.GetChild(0);
        //Transform child1 = rongtrieuhoi.transform.GetChild(1);
        //Vector3 vec1 = child0.localScale;
        //Vector3 vec2 = child1.localScale;
        //vec1 = new Vector3(vec1.x * 1.07f, vec1.y * 1.07f, vec1.z);
        //vec2 = new Vector3(vec1.x * 1.07f, vec1.y * 1.07f, vec1.z);

        //child0.transform.localScale = vec1;
        //child1.transform.localScale = vec2;

        if (randomvec == default)
        {
            randomvec = VecHeight[team][draInstantiate.Idlle][draInstantiate.draheight]();//GetRandomTFtrieuHoi(team, draInstantiate.Idlle,draInstantiate.draheight);
            //randomvec.x = 0;
        } 
        else if(VienChinh.vienchinh.Teamthis != Team.TeamXanh)
        {
            randomvec.x = PVPManager.XTeam[team == "TeamXanh"?"r":"b"];
        }
            
        rongtrieuhoi.transform.position = randomvec;
        /// rongtrieuhoi.transform.position = draInstantiate.PositionPVERandom(team);
        //debug.Log("rong3");
        Animator anim = rongtrieuhoi.GetComponent<Animator>();
        //debug.Log("rong4");
        //if (anim.runtimeAnimatorController == null)
        //{
        //    rongtrieuhoi.GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(nameObject[1]);//SGResources.Load<RuntimeAnimatorController>( nameObject[1]);
        //}
        //    debug.Log("rong5");
        string id = data["id"].str;

        // DragonController dra = rongtrieuhoi.GetComponent<DragonController>();
        //   debug.Log("rong6");x
        // dra.tienhoa = byte.Parse(e.data["xanhtrieuhoi"]["tienhoa"].ToString());
        Transform tfteam = null;
        string teamonl = "b";
        if (team == "TeamXanh")
        {
            tfteam = VienChinh.vienchinh.TeamXanh.transform;
        }
        else
        {
            teamonl = "r";
            tfteam = VienChinh.vienchinh.TeamDo.transform;
            if (Setting.cauhinh == CauHinh.CauHinhCao) GiaoDienPVP.ins.AddHienRong(nameObject);
        }
        if (ThongKeDame.Set && data["namerong"])
        {
            ThongKeDame.Data[team][nameObject][id] = new JSONClass();
            ThongKeDame.Data[team][nameObject][id].Add("dame", "0");
            ThongKeDame.Data[team][nameObject][id].Add("netranh", "0");
            ThongKeDame.Data[team][nameObject][id].Add("chimang", "0");
            ThongKeDame.Data[team][nameObject][id].Add("hoiphuc", "0");
            ThongKeDame.Data[team][nameObject][id].Add("chongchiu", "0");
            // ThongKeDame.Data[team][nameObject][id].Add("sao", data["sao"].ToString());
            ThongKeDame.Data[team][nameObject][id].Add("tenhienthi", data["namerong"].str);
            //  debug.Log(ThongKeDame.Data.ToString());
        }
        rongtrieuhoi.name = id;
        rongtrieuhoi.transform.SetParent(tfteam.transform, true);
        rongtrieuhoi.transform.SetSiblingIndex(tfteam.transform.childCount - 1);
        anim.Play("Spawn");

        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online)
        //{
        //    draInstantiate.DraInsPVP(data, id);
        //}
        //else draInstantiate.DraInsPVE(data);

        /////
          draInstantiate.DraInsOnline(data, id);
          PVPManager.AddDragonTF(teamonl,id,rongtrieuhoi.transform);
        /////
        if (ReplayData.Record)
        {
            ReplayData.AddNewDragon(id, team, nameObject, randomvec, data["sao"].ToString());
        }
      //  VienChinh.vienchinh.SetMucTieuTeamDo();//setmuctieuuu
       // VienChinh.vienchinh.SetMucTieuTeamXanh();
        return rongtrieuhoi;
    }


    public static void SetScaleDragon(string nameobject, byte saorong,Transform rong)
    {
        if (nameobject == "RongRua")//|| nameobject == "RongRua2"
        {
            if (saorong >= 30)
            {
                rong.transform.GetChild(0).transform.localScale = Vector3.one;
                rong.transform.GetChild(1).transform.localScale = Vector3.one;
            }
        }
        //else
        //{
        //    Transform child0 = rong.transform.GetChild(0);
        //    Transform child1 = rong.transform.GetChild(1);
        //    Vector3 vec1 = child0.localScale;
        //    Vector3 vec2 = child1.localScale;
        //    vec1 = new Vector3(vec1.x * 1.2f, vec1.y * 1.2f, vec1.z);
        //    vec2 = new Vector3(vec1.x * 1.2f, vec1.y * 1.2f, vec1.z);

        //    child0.transform.localScale = vec1;
        //    child1.transform.localScale = vec2;
        //}
    }

    public static GameObject TrieuHoiDraReplay(JSONNode data)
    {
      //  debug.Log(data.ToString());
        string nameObject = data["nameobject"].AsString;
    //    debug.Log("rong1");
        
        // GameObject hieuung = Instantiate(vienchinh.HieuUngTrieuHoi, new Vector3(randomvec.x, 6, randomvec.z), Quaternion.identity) as GameObject;
        GameObject rongtrieuhoi = Instantiate(Inventory.GetObjRong(nameObject + "2"), new Vector3(data["x"].AsFloat, data["y"].AsFloat,90), Quaternion.identity);
        SetScaleDragon(nameObject, byte.Parse(data["sao"].AsString), rongtrieuhoi.transform);


        //debug.Log("rong3");
        Animator anim = rongtrieuhoi.GetComponent<Animator>();


        //debug.Log("rong4");
        //if (anim.runtimeAnimatorController == null)
        //{
        //    rongtrieuhoi.GetComponent<Animator>().runtimeAnimatorController = Inventory.LoadAnimator(nameObject[1]);//SGResources.Load<RuntimeAnimatorController>( nameObject[1]);
        //}
        //   debug.Log("rong5");
        string id = data["id"].AsString;
        rongtrieuhoi.name = id;
        // DragonController dra = rongtrieuhoi.GetComponent<DragonController>();
       // debug.Log("rong6");
        // dra.tienhoa = byte.Parse(e.data["xanhtrieuhoi"]["tienhoa"].ToString());
        Transform tfteam = null;
        if (data["Team"].AsString == "TeamXanh")
        {
            tfteam = VienChinh.vienchinh.TeamXanh.transform;
        }
        else tfteam = VienChinh.vienchinh.TeamDo.transform;

        rongtrieuhoi.transform.SetParent(tfteam.transform, true);
        rongtrieuhoi.transform.SetSiblingIndex(tfteam.transform.childCount - 1);
        anim.Play("Spawn");
        DraInstantiate draInstantiate = rongtrieuhoi.GetComponent<DraInstantiate>();
        draInstantiate.DraInsReplay(nameObject, data["time"].AsFloat);

      
        return rongtrieuhoi;
    }
    public static void GetUpdateMove(Transform tf, Team team)
    {
        DragonPVEController dragonPVEController = tf.GetComponent<DragonPVEController>();
        Transform parent = tf.parent;
        if (!VienChinh.vienchinh.DanhOnline)
        {
            dragonPVEController.ActionAttack += dragonPVEController.ABSAnimatorAttack;
        }
            
        // Thêm hành động vào actionUpdate dựa trên team
        if (team == Team.TeamXanh)
        {
            dragonPVEController.actionUpdate += MoveTeamXanh;
            //dragonPVEController.actionUpdate += MoveTeamDo;

            //   dragonPVEController.team = Team.TeamXanh;
        }
        else
        {
            dragonPVEController.actionUpdate += MoveTeamDo;
           // dragonPVEController.actionUpdate += MoveTeamXanh;
            // dragonPVEController.team = Team.TeamDo;
            if (Setting.cauhinh == CauHinh.CauHinhCao) dragonPVEController.actionUpdate += TruHienRongTeamDo;
        }

        // Nếu đang ghi lại, thêm hành động ghi lại vào actionUpdate
        if (ReplayData.Record)
        {
            dragonPVEController.actionUpdate += Record;
        }

        // Định nghĩa các hành động cần thiết

        void MoveTeamXanh()
        {
            dragonPVEController.Target = VienChinh.vienchinh.muctieuxanh.transform;

            Vector3 targetPosition = dragonPVEController.Target.position;
            float tamdanhxa = dragonPVEController.tamdanhxa;
            float speed = dragonPVEController.speed * Time.deltaTime;
            //if (dragonPVEController.walking)
            //{
              
            //}
            if (parent.position.x < targetPosition.x - tamdanhxa)
            {
                parent.position += Vector3.right * speed;
                if (!dragonPVEController.DanganimAttack)
                {
                    dragonPVEController.AnimatorRun();
                    //if (parent.position.x > VienChinh.vienchinh.muctieudo.transform.position.x)
                    //{
                    //  //  VienChinh.vienchinh.SetMucTieuTeamDo();//setmuctieuuu
                    //   // Debug.Log("SetMucTieuTeamXanhgg");
                    //}
                }
                //VienChinh.vienchinh.SetMucTieuTeamXanh();//setmuctieuuu
               // Debug.Log("parent: " + parent.position.x + " muctieudo: " + VienChinh.vienchinh.muctieudo.transform.position.x);
            }
            else
            {
                dragonPVEController.AnimatorAttack();///////////////////////
            }
        }

        void MoveTeamDo()// Team đỏ di chuyển sang trái
        {
            dragonPVEController.Target = VienChinh.vienchinh.muctieudo.transform;

            Vector3 targetPosition = dragonPVEController.Target.position;
            float tamdanhxa = dragonPVEController.tamdanhxa;
            float speed = dragonPVEController.speed * Time.deltaTime;

            if (parent.position.x > targetPosition.x + tamdanhxa)
            {
                parent.position += Vector3.left * speed;
                if (!dragonPVEController.DanganimAttack)
                {
                    dragonPVEController.AnimatorRun();

                    //if (parent.position.x < VienChinh.vienchinh.muctieuxanh.transform.position.x)
                    //{
                    //   // VienChinh.vienchinh.SetMucTieuTeamXanh();//setmuctieuuu
                    //  //  Debug.Log("SetmucTieuTeamDo");
                    //}

                  //  Debug.Log("parent: " + parent.position.x + " muctieuxanh: " + VienChinh.vienchinh.muctieuxanh.transform.position.x);
                }
                // VienChinh.vienchinh.SetMucTieuTeamDo();//setmuctieuuu
            }
            else
            {
                dragonPVEController.AnimatorAttack();//////////////////////
            }


        }

        void Record()
        {
            ReplayData.AddPositionDragon(parent.gameObject.name, parent.position.x, dragonPVEController.animplay);
          //  if (dragonPVEController.nameobj == "NamNguSac32") debug.Log("AddPositionDragon: x " + parent.position.x);
        }

        void TruHienRongTeamDo()
        {
            if (parent.position.x <= GiaoDienPVP.ins.HienRongObj.transform.position.x + 0.5f)
            {
                GiaoDienPVP.ins.AddHienRong(dragonPVEController.nameobj, -1);
                dragonPVEController.actionUpdate -= TruHienRongTeamDo;
            }
        }
    }
    //public static void GetUpdateMove(Transform tf,string nameteam)
    //{
    //    DragonPVEController dragonPVEController = tf.GetComponent<DragonPVEController>();
    //    Transform parent = tf.transform.transform.parent;    
    //    if (nameteam == "TeamXanh")
    //    {
    //        dragonPVEController.actionUpdate += MoveTeamDo;
    //    }
    //    else
    //    {
    //        dragonPVEController.actionUpdate += MoveTeamXanh;
    //        dragonPVEController.actionUpdate += TruHienRongTeamDo;
    //    }

    //    if(ReplayData.Record)
    //    {
    //        dragonPVEController.actionUpdate += Record;
    //    }
    //    void MoveTeamXanh()
    //    {
    //        dragonPVEController.Target = VienChinh.vienchinh.muctieudo.transform;
    //        //dragonPVEController.Muctieu = VienChinh.vienchinh.muctieudo;
    //       // if (dragonPVEController.Target == null) return;
    //        if (parent.position.x > dragonPVEController.Target.transform.position.x + dragonPVEController.tamdanhxa)
    //        {
    //            parent.position += Vector3.left * dragonPVEController.speed * Time.deltaTime;
    //            dragonPVEController.AnimatorRun();
    //        }
    //        else dragonPVEController.AnimatorAttack();
    //    }

    //    void MoveTeamDo()
    //    {
    //        dragonPVEController.Target = VienChinh.vienchinh.muctieuxanh.transform;
    //        // chiso.Muctieu = VienChinh.vienchinh.muctieuxanh;
    //       // if (dragonPVEController.Target == null) return;
    //        if (parent.position.x < dragonPVEController.Target.transform.position.x - dragonPVEController.tamdanhxa)
    //        {
    //            parent.position += Vector3.right * dragonPVEController.speed * Time.deltaTime;
    //            dragonPVEController.AnimatorRun();
    //        }
    //        else dragonPVEController.AnimatorAttack();

    //    }
    //    void Record()
    //    {
    //        ReplayData.AddPositionDragon(parent.gameObject.name, parent.transform.position.x,dragonPVEController.animplay);
    //    }
    //    void TruHienRongTeamDo()
    //    {
    //      //  debug.Log("tru hien rong");
    //        if (parent.position.x <= GiaoDienPVP.ins.HienRongObj.transform.position.x + 0.5f)
    //        {
    //            GiaoDienPVP.ins.AddHienRong(dragonPVEController.nameobj, -1);
    //            dragonPVEController.actionUpdate -= TruHienRongTeamDo;
    //        }
    //    }
    //}
    //public static List<Transform> GetDraDungTruoc(byte sodra, Transform team, Vector2 phamvi)
    //{
    //    List<Transform> dradungtruoc = new List<Transform>();
    //    Transform dungdau = null;
    //    if (team.name == "TeamXanh")
    //    {
    //        dungdau = VienChinh.vienchinh.muctieudo.transform;

    //    }
    //    else
    //    {
    //        dungdau = VienChinh.vienchinh.muctieuxanh.transform;
    //    }
    //    dradungtruoc.Add(dungdau);
    //    for (int i = 1; i < team.transform.childCount; i++)
    //    {
    //        if (Mathf.Abs(dungdau.transform.position.x - team.transform.GetChild(i).transform.position.x) <= phamvi.x &&
    //            Mathf.Abs(dungdau.transform.position.y - team.transform.GetChild(i).transform.position.y) <= phamvi.y)
    //        {
    //            if (dradungtruoc.Contains(team.transform.GetChild(i)) == false)
    //            {
    //                dradungtruoc.Add(team.transform.GetChild(i));
    //            }
    //            // debug.Log("Contains" + dradungtruoc.Contains(team.transform.GetChild(i)));
    //            //debug.Log(dradungtruoc[dradungtruoc.Count - 1].transform.name + " và " + team.transform.GetChild(i).transform.name + " Cách nhau " + (Mathf.Abs(dradungtruoc[dradungtruoc.Count - 1].transform.position.x - team.transform.GetChild(i).transform.position.x)));
    //            if (dradungtruoc.Count >= sodra) return dradungtruoc;
    //        }
    //    }
    //    return dradungtruoc;
    //}

    //public static List<Transform> GetDraDungTruoc(Transform team, Vector2 phamvi) // không có giới hạn rồng
    //{
    //    List<Transform> dradungtruoc = new List<Transform>();
    //    Transform dungdau = null;
    //    if (team.name == "TeamXanh")
    //    {
    //        dungdau = VienChinh.vienchinh.muctieudo.transform;

    //    }
    //    else
    //    {
    //        dungdau = VienChinh.vienchinh.muctieuxanh.transform;
    //    }
    //    dradungtruoc.Add(dungdau);
    //    for (int i = 1; i < team.transform.childCount; i++)
    //    {
    //        if (Mathf.Abs(dungdau.transform.position.x - team.transform.GetChild(i).transform.position.x) <= phamvi.x &&
    //            Mathf.Abs(dungdau.transform.position.y - team.transform.GetChild(i).transform.position.y) <= phamvi.y)
    //        {
    //            if (dradungtruoc.Contains(team.transform.GetChild(i)) == false)
    //            {
    //                dradungtruoc.Add(team.transform.GetChild(i));
    //            }
    //        }
    //    }
    //    return dradungtruoc;
    //}



    public static List<Transform> GetDraDungTruoc(byte sodra, Transform team, Vector2 phamvi)
    {
        List<Transform> dradungtruoc = new List<Transform>();
        Transform dungdau = null;

        if (team.name == "TeamXanh")
        {
            dungdau = VienChinh.vienchinh.muctieudo.transform;
        }
        else
        {
            dungdau = VienChinh.vienchinh.muctieuxanh.transform;
        }
       

        List<Transform> potentialTransforms = new List<Transform>();
        
        for (int i = 1; i < team.transform.childCount; i++)
        {
            Transform childTransform = team.transform.GetChild(i);

            if (Mathf.Abs(dungdau.transform.position.x - childTransform.transform.position.x) <= phamvi.x &&
                Mathf.Abs(dungdau.transform.position.y - childTransform.transform.position.y) <= phamvi.y)
            {
                potentialTransforms.Add(childTransform);
            }
        }

        // Sắp xếp danh sách các transform theo khoảng cách từ dungdau
        potentialTransforms.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(dungdau.position, a.position);
            float distanceB = Vector3.Distance(dungdau.position, b.position);
            return distanceA.CompareTo(distanceB);
        });

        // Giới hạn danh sách theo sodra
        for (int i = 0; i < Mathf.Min(sodra, potentialTransforms.Count); i++)
        {
            dradungtruoc.Add(potentialTransforms[i]);
        }

        if(dradungtruoc.Count == 0)
        {
            dradungtruoc.Add(dungdau);
        }    
     //   Debug.Log("Count dra đứng trước " + team.ToString() + " là:" + dradungtruoc.Count);
        return dradungtruoc;
    }

    public static List<Transform> GetDraDungTruoc(Transform team, Vector2 phamvi)
    {
        List<Transform> dradungtruoc = new List<Transform>();
        Transform dungdau = null;

        if (team.name == "TeamXanh")
        {
            dungdau = VienChinh.vienchinh.muctieudo.transform;
        }
        else
        {
            dungdau = VienChinh.vienchinh.muctieuxanh.transform;
        }

        dradungtruoc.Add(dungdau);

        for (int i = 1; i < team.transform.childCount; i++)
        {
            Transform childTransform = team.transform.GetChild(i);

            if (Mathf.Abs(dungdau.transform.position.x - childTransform.transform.position.x) <= phamvi.x &&
                Mathf.Abs(dungdau.transform.position.y - childTransform.transform.position.y) <= phamvi.y)
            {
                if (!dradungtruoc.Contains(childTransform))
                {
                    dradungtruoc.Add(childTransform);
                }
            }
        }

        return dradungtruoc;
    }


    public static Transform GetRongManhNhat(Team teamMinh)
    {
        float hpWeight = 50f; // Trọng số ưu tiên HP so với dame để lấy rồng mạnh nhất
        List<Transform> strongestDragons = new List<Transform>();
        float maxCombinedStrength = 0;

        // Duyệt qua các con rồng trong TeamXanh
        Transform TeamTf = null;
        if(teamMinh == Team.TeamXanh)
        {
            TeamTf = VienChinh.vienchinh.TeamDo.transform;
        }    
        else TeamTf = VienChinh.vienchinh.TeamXanh.transform;
        for (int i = 1; i < TeamTf.childCount; i++)
        {
            // Tìm DragonPVEController
            DragonPVEController dra = TeamTf.GetChild(i)
                .transform.Find("SkillDra")
                .GetComponent<DragonPVEController>();

            float hp = dra.hp;
            float maxhp = dra.Maxhp;
            float dame = dra.dame;

            // Kiểm tra nếu máu >= 50%
            if (hp >= maxhp * 0.5f)
            {
                // Tính sức mạnh tổng hợp
                float combinedStrength = dame + (hp / maxhp) * hpWeight;

                // So sánh chỉ số sức mạnh
                if (combinedStrength > maxCombinedStrength)
                {
                    // Nếu mạnh hơn, làm mới danh sách
                    strongestDragons.Clear();
                    strongestDragons.Add(dra.transform.parent);
                    maxCombinedStrength = combinedStrength;
                }
                else if (combinedStrength == maxCombinedStrength)
                {
                    // Nếu bằng sức mạnh cao nhất, thêm vào danh sách
                    strongestDragons.Add(dra.transform.parent);
                }
            }
        }

        // Trả về ngẫu nhiên một Transform trong danh sách mạnh nhất
        if (strongestDragons.Count > 0)
        {
            int randomIndex = Random.Range(0, strongestDragons.Count);
            return strongestDragons[randomIndex];
        }
        if (TeamTf.transform.childCount > 1)
        {
            return TeamTf.transform.GetChild(1);
        }    
            // Nếu không tìm thấy con rồng nào, trả về null

            return null;
    }

    //public static void InstantiateHieuUngChu(string nameeff, Transform tfins,float destroy = 2f,bool setOnline = false)
    //{
    //    if(nameeff == "chimang")
    //    {
    //        string team = tfins.transform.parent.transform.parent.name;
    //        if (team == "TeamDo")
    //        {
    //            //   debug.Log(tfins.transform.parent.name + " gây ra chí mạng đỏ");
    //            nameeff = "chimangdich";
    //        }
    //        ThongKeDame.AddThongKe(new ThongKeDame.CData(team, tfins.GetComponent<DragonPVEController>().nameobj, tfins.transform.parent.name, 1, ThongKeDame.EType.chimang));
    //    }    

    //    ReplayData.AddHieuUngChu(tfins.transform.parent.name,nameeff);
    //    if(VienChinh.vienchinh.chedodau == "Online" && !setOnline)
    //    {
    //        JSONObject newjson = new JSONObject();
    //        newjson.AddField("id", tfins.transform.parent.name);
    //        newjson.AddField("hieuungchu", nameeff);
    //        DauTruongOnline.ins.AddUpdateData(newjson);
    //        return;
    //    }
    //    Transform obj = Inventory.ins.GetEffect(nameeff).transform;
    //    Transform txt = EZ_PoolManager.Spawn(obj, tfins.transform.position, Quaternion.identity);
    //    Vector3 vec = new Vector3(tfins.transform.position.x, tfins.transform.position.y + 0.5f, 100);
    //    txt.transform.position = vec;
    //    txt.gameObject.SetActive(true);
    //    EZ_PoolManager.Despawn(txt, destroy);
    //}
    public static void InstantiateHieuUngChu(string nameeff, Transform tfins, float destroy = 2f)
    {
   
        var parentTransform = tfins.transform.parent;
        var team = parentTransform.parent.name;

        if (nameeff == "chimang")
        {
            if (team == "TeamDo")
            {
                nameeff = "chimangdich";
            }
            //ThongKeDame.AddThongKe(new ThongKeDame.CData(
            //    team,
            //    tfins.GetComponent<DragonPVEController>().nameobj,
            //    parentTransform.name,
            //    1,
            //    ThongKeDame.EType.chimang
            //));
        }

        ReplayData.AddHieuUngChu(parentTransform.name, nameeff);
        //if (VienChinh.vienchinh.chedodau == CheDoDau.Online && !setOnline)
        //{
        //    JSONObject newjson = new JSONObject();
        //    newjson.AddField("id", parentTransform.name);
        //    newjson.AddField("hieuungchu", nameeff);
        //    DauTruongOnline.ins.AddUpdateData(newjson);
        //    return;
        //}
        if (Setting.cauhinh != CauHinh.CauHinhCao) return;
        Transform obj = Inventory.ins.GetEffect(nameeff).transform;
        Transform txt = EZ_PoolManager.Spawn(obj, tfins.transform.position, Quaternion.identity);

        txt.transform.position = new Vector3(tfins.transform.position.x, tfins.transform.position.y + 0.5f, 100);
        txt.gameObject.SetActive(true);
        EZ_PoolManager.Despawn(txt, destroy);
    }
    public static bool GetTyLeDayLui(byte sao)
    {
        bool daylui = false;
        if (sao < 10)
        {
            if (Random.Range(0, 100) > 40) daylui = true;
        }
        else if (sao >= 10 && sao < 15)
        {
            if (Random.Range(0, 100) > 30) daylui = true;
        }
        else if (sao >= 15 && sao < 20)
        {
            if (Random.Range(0, 100) > 20) daylui = true;
        }
        else if (sao >= 20)
        {
            if (Random.Range(0, 100) > 10) daylui = true;
        }
        return daylui;
    }    
    public static void RotationSkill(Transform tf, Transform target)
    {
        Vector3 vectorToTarget = target.transform.position - tf.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg; //- rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        tf.transform.rotation = q;// Quaternion.Slerp(tf.transform.rotation, q, Time.deltaTime * 15);
    }    
}
