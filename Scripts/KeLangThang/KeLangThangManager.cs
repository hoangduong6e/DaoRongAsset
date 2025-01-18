using System.Collections.Generic;


public class KeLangThangManager
{

    private static readonly Dictionary<string, KeLangThangFactory> factories = new Dictionary<string, KeLangThangFactory>
    {
        { "ConLan", new ConLanFactory() },
       // { "KeLangThang", new KeLangThangFactory() }
    };
    public static void CreateAllKeLangThang(JSONObject data)
    {

        foreach (string nameKLT in data["KeLangThang"].keys)
        {
            foreach (string KTL in data["KeLangThang"][nameKLT].keys)
            {
               KeLangThangFactory keLangThangFactory = factories[nameKLT];
                JSONObject dataCreate = data["KeLangThang"][nameKLT][KTL];
                dataCreate.AddField("nameobject",nameKLT);
                  keLangThangFactory.Create(dataCreate);
            }
        }

            
          //  ConLan.Create conlan = new ConLan.Create(e.data);
    }
}//
