using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayDra : MonoBehaviour
{
    // Start is called before the first frame update
    private bool replay = false, setFloatAnim = true;
    private int countMove = 0;
    public DragonPVEController dragonPVEController;
  //  private string animplay = "";
    public Action ActionUpdate;
    public float FrameTrieuHoi;
    //void Awake()
    //{
    //    dragonPVEController.anim = GetComponent<Animator>();
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        countMove = (int)(FrameTrieuHoi / 0.2f);
       // debug.Log("frame hien tai " + countMove);// + " frame rong " + ((ReplayData.time - FrameTrieuHoi)/50));
        if (ActionUpdate != null) ActionUpdate();
        if (!replay) return;
        if (ReplayData.speedReplay > 1)
        {
            int CountNhan = countMove + ReplayData.speedReplay;
            if (CountNhan < ReplayData.NodeDataPosition[gameObject.name].Count)
            {
                for (int i = countMove; i < CountNhan; i++)
                {
                    SetData(i);
                }
                // countMove += ReplayData.speedReplay;
               // FrameTrieuHoi += 0.2f * ReplayData.speedReplay;
            }
            else
            {
                SetData(countMove);
             //   FrameTrieuHoi += 0.2f;
                //  countMove += 1;
            }
        }
        else
        {
            SetData(countMove);
            //   countMove += 1;
        }
      
    }
    private void SetData(int i)
    {
 
        if (!replay) return;
        string nameobj = gameObject.name;
     //   debug.Log(ReplayData.NodeDataPosition[nameobj][i].ToString());
        if(ReplayData.CheckData(nameobj, "x", i))
        {
            transform.position = new Vector3(ReplayData.NodeDataPosition[nameobj][i]["x"].AsFloat, transform.position.y, 90);
        }
        if (ReplayData.CheckData(nameobj, "anim",i)) dragonPVEController.anim.Play(ReplayData.NodeDataPosition[nameobj][i]["anim"].AsString);
        if(setFloatAnim)
        {
            dragonPVEController.anim.SetFloat("speedRun", ReplayData.speedReplay);
            dragonPVEController.anim.SetFloat("speedAttack", ReplayData.speedReplay);
        }    
        if (ReplayData.CheckData(nameobj, "fillhp", i)) dragonPVEController.SetHp(ReplayData.NodeDataPosition[nameobj][i]["fillhp"].AsFloat,false);
        if (ReplayData.CheckData(nameobj, "skill", i))
        {
            if (ReplayData.CheckData(nameobj, "target", i) == false) // không có target là auto đứng đầu
            {
                Transform target = null;
             //   int index = ReplayData.NodeDataPosition[nameobj][i]["target"].AsInt;
                if (transform.parent.name == "TeamXanh")
                {
                    target = VienChinh.vienchinh.muctieuxanh.transform;
                }
                else
                {
                   target = VienChinh.vienchinh.muctieudo.transform;
                }
                dragonPVEController.Target = target;
                if (target != null)
                {
                    GameObject skill = dragonPVEController.skillObj[ReplayData.NodeDataPosition[nameobj][i]["skill"].AsInt];
                    //debug.Log("replay skill " + ReplayData.NodeDataPosition[nameobj][i]["skill"].AsInt + " " + gameObject.name);
                    if(skill != null)
                    {
                        skill.SetActive(true);
                    }
                }
            }
          //  debug.Log(ReplayData.NodeDataPosition[nameobj][i].ToString()) ;
            //Transform target = null;
            //int index = ReplayData.NodeDataPosition[nameobj][i]["target"].AsInt;
            //if (transform.parent.name == "TeamXanh")
            //{
            //    if(VienChinh.vienchinh.TeamDo.transform.childCount > index) target = VienChinh.vienchinh.TeamDo.transform.GetChild(ReplayData.NodeDataPosition[nameobj][i]["target"].AsInt).transform;
            //}
            //else
            //{
            //    if (VienChinh.vienchinh.TeamXanh.transform.childCount > index) target = VienChinh.vienchinh.TeamXanh.transform.GetChild(ReplayData.NodeDataPosition[nameobj][i]["target"].AsInt).transform;
            //}
            //dragonPVEController.Target = target;
            //if (target != null)
            //{
            //    GameObject skill = dragonPVEController.skillObj[ReplayData.NodeDataPosition[nameobj][i]["skill"].AsInt];
            //    skill.SetActive(true);
            //} 
        }
        if (ReplayData.CheckData(nameobj, "hieuungchu", i)) PVEManager.InstantiateHieuUngChu(ReplayData.NodeDataPosition[nameobj][i]["hieuungchu"].AsString, transform);
        if (ReplayData.CheckData(nameobj, "timelamcham", i))
        {
            float timelamcham = ReplayData.NodeDataPosition[nameobj][i]["timelamcham"].AsFloat;
            string hieuung = "";
            if (ReplayData.CheckData(nameobj, "hieuunglamcham", i))
            {
                hieuung = ReplayData.NodeDataPosition[nameobj][i]["hieuunglamcham"].AsString;
                debug.Log("hieu ung lam cham " +hieuung );
            }
            setFloatAnim = false;
            float chialamcham = 0;
            string tangtoc = "0";
            if (ReplayData.NodeDataPosition[nameobj][i]["chialamcham"].ToString() != "") chialamcham = ReplayData.NodeDataPosition[nameobj][i]["chialamcham"].AsFloat;

            else if (ReplayData.NodeDataPosition[nameobj][i]["tangtoc"].ToString() != "")
            {
                tangtoc = ReplayData.NodeDataPosition[nameobj][i]["tangtoc"].AsString;
            }
            dataLamCham data = new dataLamCham(timelamcham, hieuung, chialamcham, tangtoc);
            dragonPVEController.LamChamABS(data);
            StartCoroutine(IeSetFloatLamCham(timelamcham));
        }
        if (ReplayData.CheckData(nameobj, "killtru", i)) dragonPVEController.KillTru();
        if (ReplayData.CheckData(nameobj, "BienCuu", i))
        {
            debug.Log("Bien Cuu " + ReplayData.NodeDataPosition[nameobj][i].ToString());
            dragonPVEController.BienCuuABS(ReplayData.NodeDataPosition[nameobj][i]["BienCuu"].AsFloat / ReplayData.speedReplay);
        }    

        if (countMove >= ReplayData.NodeDataPosition[nameobj].Count - 1)
        {
            dragonPVEController.AnimWin();
            done();
   
            replay = false;
        //    Destroy(gameObject);
        }
        FrameTrieuHoi += 0.2f;
        //StartCoroutine(all[0]);
        //all[0] = IeSetFloatLamCham(1);
    }
    private IEnumerator IeSetFloatLamCham(float time)
    {
        yield return new WaitForSeconds(time / ReplayData.speedReplay);
        setFloatAnim = true;
    }
   // public IEnumerator[] all;
    private void done()
    {
        //ReplayData.done += 1;
        //if (ReplayData.done >= ReplayData.NodeDataPosition.Count - 1) ReplayData.DoneReplay();
    }
    public void SpawmComplete()
    {
       // debug.Log("Replay spawm complete");
    //   debug.Log("data replay move " + gameObject.name + " " + ReplayData.NodeDataPosition[gameObject.name].ToString());
        replay = true;
    }
    public void ParseData()
    {
       
    }
    //private void OnDestroy()
    //{
    //    done();
    //}
}
