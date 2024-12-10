using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMaterialImage : MonoBehaviour
{
    // Start is called before the first frame update
    public string pathmateral;
    Image img;
    void Start()
    {
        img = GetComponent<Image>();
        if (img != null)
        {
            Material spriteMaterial = Resources.Load("GameData/"+pathmateral) as Material;

            if (spriteMaterial != null)
            {
                // Gán Material cho SpriteRenderer
                img.material = spriteMaterial;
                Debug.Log("Đã gán Material cho SpriteRenderer.");
            }
            else
            {
                Debug.LogWarning("Không tìm thấy Material cho SpriteRenderer.");
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy SpriteRenderer trên đối tượng này.");
        }
    }
}
