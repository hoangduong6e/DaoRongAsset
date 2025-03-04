using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraFlyIsland : DraInstantiate
{
    // Start is called before the first frame update
    public string attackQuaBong = "Attack";
    void Start()
    {
        //DraInstantiate draInstantiate = GetComponent<DraInstantiate>();
        //draInstantiate.DraInsIsland();
        //DraInsIsland(new DataDragonIsland("abc","abc"));
    }
    public override void DraInsIsland(DataDragonIsland data)
    {
        if(!GetComponent<DragonFlyIsland>())
        {
            DragonFlyIsland DraflyIsland = gameObject.AddComponent<DragonFlyIsland>();
             DraflyIsland.attackQuaBong = attackQuaBong;
            InsCanvasDraIsland(data);
        }
        // Destroy(GetComponent<DraInstantiate>());
    }
}
