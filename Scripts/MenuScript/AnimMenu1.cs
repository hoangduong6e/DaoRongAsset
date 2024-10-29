using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMenu1 : MonoBehaviour
{
    private void OnEnable()
    {
        // Lấy RectTransform của menu
        RectTransform rectTransform = GetComponent<RectTransform>();

        // Thiết lập vị trí ban đầu của RectTransform
        Vector2 startPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 580);
        rectTransform.anchoredPosition = startPosition;

        // Di chuyển RectTransform từ vị trí ban đầu về vị trí Y = 0
        Vector2 endPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        rectTransform.LeanMoveLocal(endPosition, 0.6f); // Thời gian di chuyển là 0.5 giây 
    }
}
