using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class menuinfoitem : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = new Vector2(0.3f, 0.3f);
        transform.LeanScale(new Vector2(1.2f,1.2f), 0.15f);
        transform.SetAsLastSibling();
     //   debug.Log("GameObject disabled by: " + this.GetType().Name);
    }
    public void Disnable(float time)
    {
        StopAllCoroutines();
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(time);
            transform.LeanScale(new Vector2(0.3f, 0.3f), 0.15f);
            yield return new WaitForSeconds(0.15f);
            gameObject.SetActive(false);
        }
    }
}