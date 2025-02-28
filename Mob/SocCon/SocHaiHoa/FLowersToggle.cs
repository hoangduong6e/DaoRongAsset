using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLowersToggle : MonoBehaviour
{
    public List<GameObject> Flowers = new List<GameObject>();

    private void Start()
    {
        Flowers.Add(transform.Find("GameObject/Flowers_1").gameObject);
        Flowers.Add(transform.Find("GameObject/Flowers_2").gameObject);
    }

    public void ToggleOnFlowers(int index)
    {
        Flowers[index].SetActive(true);   
    }

    public void ToggleOffFlowers()
    {
        foreach (GameObject flower in Flowers)
        {
            flower.SetActive(false);
        }
    }

}
