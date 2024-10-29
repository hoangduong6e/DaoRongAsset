using System.Collections;
using UnityEngine;

public class SocConDiChuyen : MonoBehaviour
{
    public Transform[] points;  // Mảng các điểm mà nhân vật sẽ di chuyển qua
    public float speed = 2.0f;
    public float attackDuration = 1.0f; // Thời gian thực hiện animation tấn công
    private Animator anim;
    public GameObject Nuoc,bong;
    private void Start()
    {
        StartCoroutine(MoveAndAttack());
    }

    private IEnumerator MoveAndAttack()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Transform PointRandom = points[i].transform.GetChild(Random.Range(0, points[i].transform.childCount - 1));
            if(i == 2 && points[i].name == "pontnhatren")
            {
                //xoay nguoi
                Vector3 localScale = transform.localScale;
                transform.localScale = new Vector3(-localScale.x, localScale.y);
            }
            else if (i == 3 && points[i].name == "pont4")
            {
                Nuoc.SetActive(true);
                bong.SetActive(false);
                speed = 1.5f;
            }
            yield return StartCoroutine(MoveToPoint(PointRandom));
        }
        if (points[points.Length - 1].name == "pontnhatren")
        {
            gameObject.SetActive(false);
            EventManager.ins.StartDelay(delegate {
                EventDaiChienThuyQuai ev = EventManager.ins.GetComponent<EventDaiChienThuyQuai>();
                ev.ThaSocGiap();
                Destroy(gameObject);
            }, 0.75f);
        }
        else
        {
            anim = GetComponent<Animator>();
            anim.Play("Attack");
          //  yield return new WaitForSeconds(1.8f);
         //   Destroy(gameObject);
        }    
        // Thực hiện animation tấn công tại điểm cuối cùng
      //  animator.SetTrigger("Attack");
      //  yield return new WaitForSeconds(attackDuration); // Chờ cho animation tấn công kết thúc
    }

    private IEnumerator MoveToPoint(Transform target)
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
    private byte update = 0;
    public void UpdateStateAnimation()
    {
        update += 1;
        if (update >= 2)
        {
         //   EventDaiChienThuyQuai.ins.SendHp();
            Destroy(gameObject);
        }
    }    
    public void Attack()
    {
        EventDaiChienThuyQuai.ins.DanhBachTuoc(1);
    }
}
