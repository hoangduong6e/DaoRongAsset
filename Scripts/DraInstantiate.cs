
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SortingGroup))]
public abstract class DraInstantiate : MonoBehaviour
{
    public string nameSkill, Idlle;
    public DraHeight draheight;
    public void TangScaleRong(Transform rongtrieuhoi)
    {
        Transform child0 = rongtrieuhoi.transform.GetChild(0);
        Transform child1 = rongtrieuhoi.transform.GetChild(1);
        Vector3 vec1 = child0.localScale;
        Vector3 vec2 = child1.localScale;
        vec1 = new Vector3(vec1.x * 1.07f, vec1.y * 1.07f, vec1.z);
        vec2 = new Vector3(vec1.x * 1.07f, vec1.y * 1.07f, vec1.z);

        child0.transform.localScale = vec1;
        child1.transform.localScale = vec2;
    }    
    public abstract void DraInsIsland(DataDragonIsland data);
    public void DraInsPVP(JSONObject e,string id)
    {
        GameObject obj = PVEManager.GetSkillDra(nameSkill); //Resources.Load("GameData/Object/CanvasDraIsland") as GameObject;
        GameObject SkillDra = Instantiate(obj, transform.position, Quaternion.identity, transform);

        Transform obj0 = transform.GetChild(0);
        SkillDra.transform.position = obj0.transform.position;
        SkillDra.name = "SkillDra";
        DragonPVEController dragonPVEController = SkillDra.GetComponent<DragonPVEController>();

        GetComponent<DraUpdateAnimator>().DragonPVEControllerr = dragonPVEController;
        TangScaleRong(transform);

        dragonPVEController.ParseData(e);
        PVEManager.GetScale(transform);
        SetScaleSkill(dragonPVEController.skillObj);
        if (transform.parent.name == "TeamDo")
        {
            dragonPVEController.ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
            dragonPVEController.team = Team.TeamDo;
        }
        else dragonPVEController.team = Team.TeamXanh;
        DauTruongOnline.dicdra.Add(id,transform);
        //dragonPVEController.animIdlle = Idlle;
        SetDraCompleted();
    }
    public void DraInsPVE(JSONObject e)
    {
        GameObject obj = PVEManager.GetSkillDra(nameSkill); //Resources.Load("GameData/Object/CanvasDraIsland") as GameObject;
        GameObject SkillDra = Instantiate(obj, transform.position, Quaternion.identity, transform);

        Transform obj0 = transform.GetChild(0);
        SkillDra.transform.position = obj0.transform.position;
        SkillDra.name = "SkillDra";
        DragonPVEController dragonPVEController = SkillDra.GetComponent<DragonPVEController>();
        GetComponent<DraUpdateAnimator>().DragonPVEControllerr = dragonPVEController;
        TangScaleRong(transform);

        dragonPVEController.ParseData(e);
        PVEManager.GetScale(transform);
        SetScaleSkill(dragonPVEController.skillObj);
        if (transform.parent.name == "TeamDo")
        {
            dragonPVEController.ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
            dragonPVEController.tamdanhxa += Random.Range(0, 2);
            dragonPVEController.speed += Random.Range(0, 1);
            dragonPVEController.team = Team.TeamDo;
        }
        else dragonPVEController.team = Team.TeamXanh;
       // dragonPVEController.animIdlle = Idlle;
        SetDraCompleted();
    }

    public void DraInsOnline(JSONObject e, string id)
    {
      GameObject obj = PVEManager.GetSkillDra(nameSkill); //Resources.Load("GameData/Object/CanvasDraIsland") as GameObject;
        GameObject SkillDra = Instantiate(obj, transform.position, Quaternion.identity, transform);

        Transform obj0 = transform.GetChild(0);
        SkillDra.transform.position = obj0.transform.position;
        SkillDra.name = "SkillDra";
        DragonPVEController dragonPVEController = SkillDra.GetComponent<DragonPVEController>();

        GetComponent<DraUpdateAnimator>().DragonPVEControllerr = dragonPVEController;
        TangScaleRong(transform);

        dragonPVEController.ParseData(e);
        PVEManager.GetScale(transform);
        SetScaleSkill(dragonPVEController.skillObj);
        if (transform.parent.name == "TeamDo")
        {
            dragonPVEController.ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
            dragonPVEController.team = Team.TeamDo;
        }
        else dragonPVEController.team = Team.TeamXanh;
      //  DauTruongOnline.dicdra.Add(id,transform);
        //dragonPVEController.animIdlle = Idlle;
        SetDraCompleted();
    }
    public void DraInsReplay(string nameObject,float timetrieuhoi)
    {
       // debug.Log("time trieu hoi " + timetrieuhoi + " frame: " + (timetrieuhoi * 0.2f) + " time " + ReplayData.time);
        GameObject obj = PVEManager.GetSkillDra(nameSkill); //Resources.Load("GameData/Object/CanvasDraIsland") as GameObject;
        GameObject SkillDra = Instantiate(obj, transform.position, Quaternion.identity, transform);

        Transform obj0 = transform.GetChild(0);
        SkillDra.transform.position = obj0.transform.position;
        SkillDra.name = "SkillDra";
        DragonPVEController dragonPVEController = SkillDra.GetComponent<DragonPVEController>();
        dragonPVEController.enabled = false;
        ReplayDra replayDra = transform.AddComponent<ReplayDra>();
        replayDra.dragonPVEController = dragonPVEController;
        replayDra.FrameTrieuHoi = ReplayData.time - timetrieuhoi;
        //replayDra.ParseData();
        PVEManager.GetScale(transform);

        TangScaleRong(transform);

        SetScaleSkill(dragonPVEController.skillObj);


        if (transform.parent.name == "TeamDo" && Setting.cauhinh == CauHinh.CauHinhCao)
        {
            GiaoDienPVP.ins.AddHienRong(nameObject,1);
            dragonPVEController.ImgHp.sprite = VienChinh.vienchinh.thanhmaudo;
            replayDra.ActionUpdate += TruHienRongTeamDo;
            void TruHienRongTeamDo()
            {
              //  debug.Log("tru hien rong");
                if (replayDra.transform.position.x <= GiaoDienPVP.ins.HienRongObj.transform.position.x + 0.5f)
                {
                    GiaoDienPVP.ins.AddHienRong(nameObject, -1);
                    replayDra.ActionUpdate -= TruHienRongTeamDo;
                }
            }
            dragonPVEController.team = Team.TeamDo;
        }
        else dragonPVEController.team = Team.TeamXanh;
        //dragonPVEController.animIdlle = Idlle;
        SetDraCompleted();
    }
    protected void InsCanvasDraIsland(DataDragonIsland dataa)
    {
        GameObject obj = Inventory.ins.GetObj("CanvasDraIsland"); //Resources.Load("GameData/Object/CanvasDraIsland") as GameObject;
        GameObject CanvasDraIsland = Instantiate(obj, transform.position, Quaternion.identity, transform);
        //CanvasDraIsland.transform.SetParent(transform,false);
        Transform obj0 = transform.GetChild(0);
        CanvasDraIsland.transform.position = obj0.transform.position;
        CanvasDraIsland.name = "CanvasDraIsland";

        DragonIslandController dragonIslandController = GetComponent<DragonIslandController>();
        Text txtnamerong = CanvasDraIsland.transform.Find("txtNameRong").GetComponent<Text>();
        dragonIslandController.txtNameRong = txtnamerong;
        //Transform head = transform.GetChild(0).transform.Find("Head");
       // txtnamerong.transform.position = new Vector3(txtnamerong.transform.position.x, head.transform.position.y + 0.5f,txtnamerong.transform.position.z);
     //   dragonIslandController.data = dataa;

       // Action[] abc = new Action[] { };    
        EventTrigger trigger = CanvasDraIsland.transform.Find("Btn").GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { dragonIslandController.StartDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.EndDrag;
        entry2.callback.AddListener((data) => { dragonIslandController.StopDrag((PointerEventData)data); });
        trigger.triggers.Add(entry2);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerDown;
        entry3.callback.AddListener((data) => { dragonIslandController.ButtonDown((PointerEventData)data); });
        trigger.triggers.Add(entry3);

        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.PointerUp;
        entry4.callback.AddListener((data) => { dragonIslandController.ButtonUp((PointerEventData)data); });
        trigger.triggers.Add(entry4);

        //debug.Log("Parse data " + dataa.namedra);
        //if (dataa.namedra != "")
        //{
        //    txtnamerong.text = dataa.namedra;
        //    txtnamerong.gameObject.SetActive(true);
        //}
        dragonIslandController.SetNameRong = dataa.namedra;
        gameObject.name = dataa.id;
        SettingCauHinhThap();
        // debug.Log(CanvasDraIsland.name);
    }
    private void SetScaleSkill(GameObject[] allskill)
    {
        if(transform.parent.name == "TeamXanh")
        {
            for (int i = 0; i < allskill.Length; i++)
            {
                Vector3 scale = allskill[i].transform.localScale;
                scale.x *= -1;
                allskill[i].transform.localScale = scale;
            }
        }
        else
        {
            
        }
    }
    private void SetDraCompleted()
    {
        SettingCauHinhThap();
        if (Idlle == "Flying")
        {
            if(draheight == DraHeight.MaThach)
            {
                transform.GetChild(0).transform.Find("bong").transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f);

            }
            else
            {
                transform.GetChild(0).transform.Find("bong").transform.position = new Vector3(transform.position.x, transform.position.y - 2.5f);

            }
        }
        else if (VienChinh.vienchinh.chedodau == CheDoDau.XucTu) //nhoxoa
        {
            Transform bong = transform.GetChild(0).transform.Find("bong");
            Transform child0 = bong.transform.GetChild(0);
            bong.transform.GetChild(0).gameObject.SetActive(false);
            GameObject hieuungboi = Instantiate(Inventory.ins.GetEffect("HieuUngBoi"), transform.position, Quaternion.identity);
            hieuungboi.transform.SetParent(bong.transform);
            hieuungboi.transform.position = new Vector3(child0.transform.position.x, child0.transform.position.y + 0.2f, hieuungboi.transform.position.z);
            hieuungboi.gameObject.SetActive(true);
        }
        Destroy(this);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<BoxCollider2D>());
    }
    private void SettingCauHinhThap()
    {
        if (Setting.cauhinh == CauHinh.CauHinhThap)
        {
            Setting.CauHinhThap.OffspriteSkin(transform);
        }
    }
}
public class DataDragonIsland
{
    public string namedra, id;
    public DataDragonIsland(string NameDra, string Id)
    {
        namedra = NameDra;
        id = Id;
    }
}
public enum DraHeight// chiều cao của trục y của rồng trong trận đấu
{
    DEFAULT,
    HacLong,
    LMX_PH_2DAU,
    RongLua,
    PH,
    _2DAU,
    MaThach
}

