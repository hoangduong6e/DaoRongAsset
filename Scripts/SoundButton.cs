using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SoundButton : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData data)
    {
        AudioManager.SoundClick();
    }    
}
