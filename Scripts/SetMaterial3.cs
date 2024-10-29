
using UnityEngine;

public class SetMaterial3 : MonoBehaviour
{
    // Start is called before the first frame update
    public string path;
    public string materialName;
    void Awake()
    {
        Renderer rend = GetComponent<Renderer>();
        Shader shader = Shader.Find(path + materialName);
        rend.material.shader = shader;
        Destroy(this);
    }
}
