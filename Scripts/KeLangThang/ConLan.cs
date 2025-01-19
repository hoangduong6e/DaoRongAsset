
using UnityEngine;

public class ConLan:KeLangThang
{
  
    public class Builder
    {
        ConLan _conLan = null;
        public Builder(JSONObject data)
        {
             GameObject instan = KeLangThangManager.ins.LoadObject("ConLan");
            Transform Dao = CrGame.ins.AllDao.transform.Find("BGDao" + int.Parse(data["dao"].ToString()));
            if (Dao != null)
            {
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
                    if (data["nhan"])
                    {
                        _conLan.nhan = float.Parse(data["nhan"].ToString());
                        _conLan.speed = _conLan.nhan;
                    }

            }
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