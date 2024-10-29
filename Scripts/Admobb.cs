
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;

public class Admobb : MonoBehaviour
{
    private InterstitialAd interstitial_Ad;
    private RewardedAd rewardedAd;

    private string interstitial_Ad_ID;
    private string rewardedAd_ID;
    ThongBaoChon tbchon;
    public string namequaxem;
    void Start()
    {
#if UNITY_ANDROID
        rewardedAd_ID = "ca-app-pub-9753380623490837/1396393170";
        #endif
#if UNITY_IOS
        rewardedAd_ID = "ca-app-pub-9753380623490837/5828314627";
#endif
        // interstitial_Ad_ID = "ca-app-pub-3940256099942544/1033173712";
        MobileAds.Initialize(initStatus => { });
        //  RequestInterstitial();
        // RequestRewardedVideo();
        // GetComponent<Material>().s
        // GetComponent<Material>().renderer.material = newMat;
        // 
    }

    private void RequestInterstitial()
    {
        interstitial_Ad = new InterstitialAd(interstitial_Ad_ID);
        interstitial_Ad.OnAdLoaded += HandleOnAdLoaded;
        AdRequest request = new AdRequest.Builder().Build();
        interstitial_Ad.LoadAd(request);
    }
    public void RequestRewardedVideo()
    {
        debug.Log("yeu cau video");
        rewardedAd = new RewardedAd(rewardedAd_ID);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    public void ShowInterstitial()
    {
        if (interstitial_Ad.IsLoaded())
        {
            interstitial_Ad.Show();
            RequestInterstitial();
        }

    }

    public void BtnXemQuangCao()
    {
        namequaxem = "quabian";
        tbchon = AllMenu.ins.GetCreateMenu("MenuXacNhan", GameObject.FindGameObjectWithTag("trencung"), false).GetComponent<ThongBaoChon>();
        tbchon.gameObject.SetActive(true);
        tbchon.txtThongBao.text = "Xem hết quảng cáo để nhận quà bí ẩn";
        tbchon.btnChon.onClick.AddListener(ShowRewardedVideo);
    }

    public void ShowRewardedVideo()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
        else
        {
            CrGame.ins.OnThongBaoNhanh("Chưa có quảng cáo");
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        RequestRewardedVideo();
        CrGame.ins.OnThongBaoNhanh("lỗi khi tải quảng cáo");
        // tbchon.gameObject.SetActive(false);
        // crgame.BtnXemQuangCao.SetActive(false);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        RequestRewardedVideo();
        //  CrGame.ins.OnThongBaoNhanh("Tắt quảng cáo sẽ không được nhận phần thưởng");
        //tbchon.gameObject.SetActive(false);
        //crgame.BtnXemQuangCao.SetActive(false);
        //net.socket.Emit("XemQuangCaoXong");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //  RequestRewardedVideo();
        // tbchon.gameObject.SetActive(false);
        CrGame.ins.BtnXemQuangCao.SetActive(false);
        NetworkManager.ins.socket.Emit("XemQuangCaoXong", JSONObject.CreateStringObject(namequaxem));

    }
}