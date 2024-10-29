using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuiDo : MonoBehaviour
{
    private Vector3 _target;
    Camera Camera;
    bool drag = false;
    float ShipSpeed = 8;
    public GameObject dragonao;
    public Transform vitridau;
    private void Start()
    {
        _target = new Vector3(transform.position.x, transform.position.y);

        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Transform vtd = GameObject.Find("ORong (0)").transform;
    }
    public void Update()
    {
        if (drag)
        {
            _target = Camera.ScreenToWorldPoint(Input.mousePosition);
            _target.x += 0.7f;
            _target.z = 0;

            var delta = ShipSpeed * Time.deltaTime;

            delta *= Vector3.Distance(transform.position, _target);

            if (transform.position != _target)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target, delta);
            }
        }
        else
        {
            transform.position = new Vector3(vitridau.transform.position.x, vitridau.transform.position.y);
        }
    }
    public void Drag()
    {
     
        drag = true;
        gameObject.transform.SetParent(GameObject.Find("ImageOdo").transform);
    }
    public void EndDrag()
    {
        drag = false;
        GameObject dra = Instantiate(dragonao, new Vector3(_target.x, _target.y), Quaternion.identity) as GameObject;
        dra.SetActive(true);
        if (transform.position.x > -5 && transform.position.x < 5.05f && transform.position.y < 2.3 && transform.position.y > - 1.9f)
        {
          
          
        }
        else
        {
            gameObject.transform.position = GameObject.Find("Otui (0)").transform.position;
            gameObject.transform.SetParent(GameObject.Find("Otui (0)").transform);
           
        }
    }
}
