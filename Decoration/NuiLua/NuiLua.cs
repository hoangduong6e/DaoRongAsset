using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum _mauNgoc
{
    Base,
    ngocluc,
    ngoclam,
    ngocdo,
    ngocvang,
    ngoctim
}
  ;

public class NuiLua : MonoBehaviour
{
    public static NuiLua Instance;
  
    public _mauNgoc MauNgoc;
    public GameObject _nuiLua;
    public GameObject _particles;
    public GameObject _twinkle;
    Material _material;
    private void Awake()
    {
        SpriteRenderer spriteRenderer = _nuiLua.GetComponent<SpriteRenderer>();
        _material = spriteRenderer.material;
        if (!Friend.ins.QuaNha) Instance = this;
        else
        {
            Friend.nuiluaFriend = this;
        } 
    }
    private void OnEnable()
    {
        if(Friend.ins.QuaNha && Friend.ins.timeNuiThanBi > 1) MauNgoc = (_mauNgoc)Enum.Parse(typeof(_mauNgoc), Friend.ins.MauNgocNuiThanBi);
    }
    private void FixedUpdate()
    {
        switch (MauNgoc)
        {
            case _mauNgoc.Base:
                _material.SetFloat("_HueFLoat", 0);
                _particles.SetActive(false);
                _twinkle.SetActive(true);
                break;
            case _mauNgoc.ngocluc:
                _material.SetFloat("_HueFLoat", -70);
                _particles.SetActive(true);
                _twinkle.SetActive(false);
                break;
            case _mauNgoc.ngoclam:
            
                _material.SetFloat("_HueFLoat", 50);
                _particles.SetActive(true);
                _twinkle.SetActive(false);
                break;
            case _mauNgoc.ngocdo:
                _material.SetFloat("_HueFLoat", 170);
                _particles.SetActive(true);
                _twinkle.SetActive(false);
                break;
            case _mauNgoc.ngocvang:
                _material.SetFloat("_HueFLoat", -120);
                _particles.SetActive(true);
                _twinkle.SetActive(false);
                break;
            case _mauNgoc.ngoctim:
                _material.SetFloat("_HueFLoat", 120);
                _particles.SetActive(true);
                _twinkle.SetActive(false);
                break;
        }
    }
}
