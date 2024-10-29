using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraUpdateAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public DragonPVEController DragonPVEControllerr { get; set; }
    public GameObject bongrong;
    public Transform chamdo;

    private void Awake()
    {
        Transform tf0 = transform.GetChild(0);
        Transform icon = tf0.transform.Find("Icon");
        if(icon != null) icon.GetComponent<Renderer>().material.shader = Shader.Find("Shader Graphs/Spawn");
        Transform vongtron = tf0.transform.Find("vong tron ");
        if(vongtron != null) vongtron.GetComponent<Renderer>().material.shader = Shader.Find("Shader Graphs/Glow");
    }
    private void Start()
    {
        Transform bong = transform.GetChild(0).transform.Find("bong");
        if (bong != null) bongrong = bong.gameObject;
    }
    private void Update()
    {
        if (chamdo != null) GiaoDienPVP.ins.minimap.UpdateMapDots(transform, chamdo);
    }
    public void UpdateAnimAttack()
    {
        if (DragonPVEControllerr != null)
        {
            DragonPVEControllerr.UpdateAnimAttack();
        }
    }
    public void UpdateAnimIdle()
    {
        if (DragonPVEControllerr != null)
        {
            DragonPVEControllerr.UpdateAnimIdle();
        }
    }
    public void SpawmComplete()
    {
        debug.Log(DragonPVEControllerr);
        if (DragonPVEControllerr != null)
        {
        //    debug.Log("Spawm complete");
            DragonPVEControllerr.SpawmComplete();
            CreateChamDo();
        } 
            
        else if (GetComponent<ReplayDra>())
        {
            GetComponent<ReplayDra>().SpawmComplete();
            CreateChamDo();
        }
      
    }
    public void CreateChamDo()
    {
        if (Setting.cauhinh != CauHinh.CauHinhCao) return;
        chamdo = GiaoDienPVP.ins.minimap.InitializeChamDo();
        if (transform.parent.name == "TeamXanh") chamdo.GetComponent<Image>().color = new Color32(57, 255, 0, 255);
    }    
    private void OnDestroy()
    {
        if (bongrong != null) Destroy(bongrong);
        if (chamdo != null) Destroy(chamdo.gameObject);
    }
}
