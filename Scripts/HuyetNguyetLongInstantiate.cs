using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuyetNguyetLongInstantiate : DraInstantiate
{
    // Start is called before the first frame update
    public ParticleSystem effduoi;
    void Start()
    {
        //DraInstantiate draInstantiate = GetComponent<DraInstantiate>();
        //draInstantiate.DraInsIsland();
       // DraInsIsland(new DataDragonIsland("abc", "abc"));
    }
    public override void DraInsIsland(DataDragonIsland data)
    {
        if (!GetComponent<HuyetNguyetLongFlyIsland>())
        {
            HuyetNguyetLongFlyIsland DramoveIsland = gameObject.AddComponent<HuyetNguyetLongFlyIsland>();
            DramoveIsland.scaleduoi = effduoi;
            InsCanvasDraIsland(data);
        }
        // Destroy(GetComponent<DraInstantiate>());
    }
}

