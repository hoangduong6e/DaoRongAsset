
using UnityEngine;

public class ConLan:KeLangThang
{
 
    public class Builder
    {
        ConLan _conLan;
        public Builder(JSONObject data)
        {
             GameObject instan = Inventory.LoadObjectResource("GameData/EventTet2024/ConLan");
            GameObject Dao = CrGame.ins.AllDao.transform.Find("BGDao" + int.Parse(data["dao"].ToString())).gameObject;
            GameObject RongDao = Dao.transform.Find("RongDao").gameObject;
            string id = data["id"].str;
            if(!RongDao.transform.Find(id))
            {
                GameObject conLan = Instantiate(instan, Vector3.zero, Quaternion.identity, RongDao.transform);
                conLan.name = id;
                Canvas canvas =  conLan.transform.Find("Canvas").GetComponent<Canvas>();
                canvas.sortingLayerName = "RongGiaoDien";
               canvas.transform.localScale = new Vector3(0.02f,0.02f);
                conLan.transform.position = new Vector3(Random.Range(-3,3),Random.Range(-3,3));
                _conLan = conLan.GetComponent<ConLan>();
                _conLan.ID = id;
                _conLan.nameObject = data["nameobject"].str;

            }
        }
        public ConLan Build()
        {
            return _conLan;
        }
    }
}
public class ConLanFactory:KeLangThangFactory
{
    public override KeLangThang Create(JSONObject data)
    {
        return new ConLan.Builder(data).Build();
    }
}