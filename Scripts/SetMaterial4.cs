using UnityEngine;

public class SetMaterial4 : MonoBehaviour
{
    public string paThparticleMaterial;  // Đường dẫn đến Material của ParticleSystem
    public string namEspriteMaterial;    // Đường dẫn đến Material của SpriteRenderer

    void Start()
    {
        // Lấy ParticleSystem
        ParticleSystem myParticleSystem = GetComponent<ParticleSystem>();

        if (myParticleSystem != null)
        {
            var particleRenderer = myParticleSystem.GetComponent<Renderer>();

            if (particleRenderer != null)
            {
                // Load Material cho ParticleSystem
                Material particleMaterial = Resources.Load("Materials/" + paThparticleMaterial) as Material;

                if (particleMaterial != null)
                {
                    // Gán Material cho ParticleSystem
                    particleRenderer.material = particleMaterial;
                    Debug.Log("Đã gán Material cho ParticleSystem.");

                    // Sau khi gán thành công, tiếp tục gán Material cho SpriteRenderer
                  
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy Material cho ParticleSystem.");
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy Renderer cho ParticleSystem.");
            }
        }
        AssignMaterialToSpriteRenderer();
    }

    // Hàm gán Material cho SpriteRenderer
    void AssignMaterialToSpriteRenderer()
    {
        // Load Material cho SpriteRenderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Material spriteMaterial = Resources.Load("Materials/" + namEspriteMaterial) as Material;

            if (spriteMaterial != null)
            {
                // Gán Material cho SpriteRenderer
                spriteRenderer.material = spriteMaterial;
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
