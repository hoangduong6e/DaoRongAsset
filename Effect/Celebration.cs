using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebration : MonoBehaviour
{
    public ParticleSystem celebrate;
    public int SoNgoiSao = 20;
    public int SoLanNo = 5;
    private GameObject menu;
    //private void Start()
    //{
    //    menu = transform.parent.gameObject;
    //    transform.SetParent(menu.transform);
    //    transform.SetAsFirstSibling();
    //}
    private void OnEnable()
    {
        //debug.Log("Start!!!");
        //for (int i = 0; i < SoLanNo; i++)
        //{
        //    Invoke("Celebrate", 0.5f * (i + 1));
        //}
        menu = transform.parent.gameObject;
     
        
        StartCoroutine(delay());
    }
    //private void OnDisable()
    //{
    //    transform.SetParent(AllMenu.ins.transform);
    //}
    private IEnumerator delay()
    {
      //  int i = 0;
        while (menu.activeSelf)
        {
            Celebrate();
            yield return new WaitForSeconds(Random.Range(0.3f,1f));
         //   i++;
         //   if (i > 2) i = 0;
        }
        yield return new WaitForSeconds(5f);
        transform.SetParent(menu.transform);
        transform.SetAsFirstSibling();
    }    
    //public void StartCelebrate()
    //{
    //    debug.Log("Start!!!");
    //    for (int i = 0; i < SoLanNo; i++)
    //    {
    //        Invoke("Celebrate", 0.5f * (i + 1));
    //    }
    //}
    void Celebrate()
    {
        var EmitPos = celebrate.shape;
        EmitPos.position = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0f);
        celebrate.Emit(SoNgoiSao);
    }
}
