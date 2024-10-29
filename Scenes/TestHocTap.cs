using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class TestHocTap : LopAo
{
    private delegate void TestDelegate();
    private Action TestAction;
    public int testframeUpdate = 0;
    public int testframeFixedUpdate = 0;
    public float time;
    public float Fixedtime;
    // public int intlopao { get; set; }
    // Start is called before the first frame update
    public Texture2D imageToEncode;
    void Start()
    {
        //TestDelegate testdele = TestDele;
        // testdele();

        //TestAction += TestAction1;
        //TestAction += TestAction2;
        //TestAction += TestAction1;
        //TestAction();
        //   ITest1 testHocTap = this;
        //   testHocTap.ITestDebug();
        // debug.Log("Dragon start");
        //   IDragonFly fly = GetComponent<TestHocTap>();
        //   fly.IFly();
        //Animal cat = new Cat();
        //cat.Keu();
        //Animal Dog = new Dog();
        //Dog.Keu();
        //LopAo lopao = this;
        //lopao.LopKhongAo();
        //lopao.LopAoo();
        //   imageToEncode = GetComponent<Texture2D>();
        //   byte[] imageBytes = imageToEncode.EncodeToPNG();
        //  string encodedImage = System.Convert.ToBase64String(imageBytes);
        //  debug.Log("encodedImage " + encodedImage);

        //StartCoroutine(StartTest());
    }

    private void Update()
    {
   //     time += Time.deltaTime;
      //  testframeUpdate++;
     //   debug.Log(Time.deltaTime);
    }
    private void FixedUpdate()
    {
        Fixedtime += Time.fixedDeltaTime;
        testframeFixedUpdate++;
        Debug.Log(Time.fixedDeltaTime);
    }
    public override void LopAoo()
    {
        debug.Log("Lớp rất rất ảo ma canada");
    }
    void TestDele()
    {
        debug.Log("<color=red>TestDele</color>");
    }
    void TestAction1()
    {
        debug.Log("<color=lime>Test Action 1</color>");
    }
    void TestAction2()
    {
        debug.Log("<color=yellow>Test Action 2</color>");
    }
    public void ITestDebug()
    {
        debug.Log("<color=yellow>Định nghĩa lại ITest Debug</color>");
    }
}
interface IDragonFly
{
    void IFly()
    {
        debug.Log("<color=yellow>ITest IDragonFly</color>");
    }
}
interface IDragonRun
{
    void IRun();
}
public class DragonControllere:MonoBehaviour
{
    public string namedragon;
    public int level;
}
public class Animal
{
    public virtual void Keu()
    {
        debug.Log("Animal kêu");
    }
}
public class Dog : Animal
{
    public override void Keu()
    {
        debug.Log("Dog kêu");
    }
}
public class Cat: Animal
{ 
    public void Keu()
    {
        debug.Log("Cat kêu");
    }
}
public class Duck : Animal
{
    public void Keu()
    {
        debug.Log("Cat kêu");
    }
}
public abstract class LopAo : MonoBehaviour
{
    //public int intlopao { get; set; }
    public void LopKhongAo()
    {
        debug.Log("Lớp không ảo");
    }
    public abstract void LopAoo();
        
}
