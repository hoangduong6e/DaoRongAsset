
using UnityEngine;

public class SetMaterial2 : MonoBehaviour
{
    public string[] allShader;
    void Awake()
    {
        //     allRenderer[i].material.shader = Shader.Find("Shader Graphs/" + allShader[i]);
        Renderer allrender = gameObject.GetComponent<Renderer>();

        for (int i = 0; i < allrender.materials.Length; i++)
        {
            allrender.materials[i].shader = Resources.Load("Shaders/" + allShader[i]) as Shader;//Shader.Find("Shader Graphs/" + allShader[i]);

            //debug.Log(allrender.materials[i].name);
        }
        //Material[] allMaterial = gameObject.GetComponents<Material>();
       // debug.Log("có " + allMaterial.Length + " Material");

        //for (int i = 0; i < allShader.Length; i++)
        //{
        //    allRenderer[i].material.shader = Shader.Find("Shader Graphs/" + allShader[i]);
        //}
        //Renderer rend = GetComponent<Renderer>();
        //Shader shader =  Shader.Find("Shader Graphs/" + "Electricity");
        //rend.material.shader = shader;

       // GetComponent<ParticleSystemRenderer>().material.shader = Shader.Find("Shader Graphs/" + "Electricity");
       // gameObject.GetComponents<Renderer>()[0].material.shader = Shader.Find("Shader Graphs/Electricity");
       //// debug.Log("naame material 1" + gameObject.GetComponents<Renderer>()[1].material.name);
       // gameObject.AddComponent<Renderer>().material.shader = Shader.Find("Shader Graphs/LightRail");
       // //Destroy(this);
    }
}