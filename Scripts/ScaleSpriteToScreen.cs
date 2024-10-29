using UnityEngine;

public class ScaleSpriteToScreen
{
    public static void ScaleSprite(SpriteRenderer spriteRenderer, Canvas targetCanvas)
    {
        if (spriteRenderer == null)
        {
            debug.LogError("SpriteRenderer component not found.");
            return;
        }

        // Lấy RectTransform của Canvas
        RectTransform canvasRectTransform = targetCanvas.GetComponent<RectTransform>();
        if (canvasRectTransform == null)
        {
            debug.LogError("Canvas RectTransform component not found.");
            return;
        }

        // Kích thước của Canvas trong đơn vị thế giới (world units)
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // Kích thước của Sprite trong đơn vị thế giới (world units)
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Tính toán tỉ lệ scale để Sprite vừa với kích thước của Canvas
        float scaleFactorX = canvasSize.x / spriteSize.x;
        float scaleFactorY = canvasSize.y / spriteSize.y;

        // Áp dụng scale lên GameObject
        spriteRenderer.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);

        // Đảm bảo SpriteRenderer nằm trong Canvas
        Vector2 canvasPosition = canvasRectTransform.position;
        spriteRenderer.transform.position = new Vector3(canvasPosition.x, canvasPosition.y, spriteRenderer.transform.position.z);
    }
}