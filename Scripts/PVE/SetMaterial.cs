
using UnityEngine;

public class SetMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    public string materialName;
    void Awake()
    {
        Renderer rend = GetComponent<Renderer>();
        Shader shader = Shader.Find("Shader Graphs/"+ materialName);
        rend.material.shader = shader;
        Destroy(this);
    }
}
