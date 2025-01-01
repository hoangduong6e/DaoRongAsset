using System;
using System.Collections.Generic;
using UnityEngine;

public class TienHoa : MonoBehaviour
{
    public bool Stage;
    public Material GlowMat;
    public float GLowFloat = 1;
    private Dictionary<Renderer, Material> OriginalMaterials = new Dictionary<Renderer, Material>();
    public Action DoneAnimTienHoa;
    private void Awake()
    {
        GlowMat = Resources.Load("Materials/DaoRongMobile_Glow") as Material;
    }
    void Start()
    {
     
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            OriginalMaterials[renderer] = renderer.material;
        }
    }
    public void Glow()
    {
        
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) 
        { 
            renderer.material = GlowMat; 
        }
    }
    public void Dim()
    {
        Stage = true;
        foreach (var material in OriginalMaterials)
        {
            material.Key.material = material.Value;
        }
    }
    private void Update()
    {
        GlowMat.SetFloat("_Glow",GLowFloat);
    }
    public void UpdateAnimTienHoa()
    {
        if (DoneAnimTienHoa != null) DoneAnimTienHoa();
    }    
}
