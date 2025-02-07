using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;

public class random : MonoBehaviour
{
    public int soRandom;
    public int soChon;
    public int wintime;
    public int play;
    public Text txtWin;
    public Text txtplay;
    public Text state;
    public int[] numberList = new int[] { 1, 3, 5 };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void rannum()
    {
        Random random = new Random();
        int index = UnityEngine.Random.Range(0, numberList.Length);
        soRandom = numberList[index];
    }
    void winn()
    {
        play++;
        wintime++;
        state.text = "Win!";
        txtWin.text = "Số lần thắng: " + wintime;
        txtplay.text = "Số lần chơi: " + play;

    }
    void lose()
    {
        state.text = "Lose!";
        play++;
        txtplay.text = "Số lần chơi: " + play;
    }
    public void One()
    {
        soChon = 2;
        int a = (soChon - soRandom);
        while ((a!= 3) && (a != -3))
        {
            rannum();
            if ( a == 1)
            {
                winn();
            }
            else if (a == -1)
            {
                lose();
            }
        }    
    }
    public void Two()
    {
        soChon = 4;
        int a = (soChon - soRandom);
        while ((a != 3) && (a != -3))
        {
            rannum();
            if (a == 1)
            {
                winn();
            }
            else if (a == -1)
            {
                lose();
            }
        }
    }
    public void Three()
    {
        soChon = 6;
        int a = (soChon - soRandom);
        while ((a != 3) && (a != -3))
        {
            rannum();
            if (a == 1)
            {
                winn();
            }
            else if (a == -1)
            {
                lose();
            }
        }
    }
}
