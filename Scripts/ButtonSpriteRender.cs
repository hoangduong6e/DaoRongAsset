using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSpriteRender : MonoBehaviour , IPointerDownHandler,IPointerUpHandler
{
    // Start is called before the first frame update
    public SpriteRenderer spriteButton;
    public void OnPointerDown(PointerEventData data)
    {
        spriteButton.color = new Color(0.7169812f, 0.7169812f, 0.7169812f,1);
    }
    public void OnPointerUp(PointerEventData data)
    {
        spriteButton.color = new Color(1,1,1,1);
    }
}
