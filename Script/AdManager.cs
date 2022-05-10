using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
public class AdManager : MonoBehaviour
{
     private BannerView bannerView;
    private InterstitialAd interstitial;
     private RewardedAd rewardBasedVideo;
     bool isRewarded = false;
    public static AdManager instance;

    
     private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
         else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });
        this.RequestBanner();
        this.RequestRewardBasedVideo();
        //Get singleton reward based video ad reference
        //this.rewardBasedVideo = Reward.Instance;

        //RewardBasedVideoAd is a singleton, so handlers should only be registered once.
       this.rewardBasedVideo.OnUserEarnedReward += this.HandleRewardBasedVideoRewarded;
       this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;


      
        

       
    }
    void Update(){
        if(isRewarded == true){
           
            //this.rewardBasedVideoAd.Show();
            Debug.Log("Reward");
        }
    }
    
    private AdRequest CreateAdRequest(){
        return new AdRequest.Builder().Build();
    }

    private void RequestBanner(){
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        
        this.bannerView.LoadAd(this.CreateAdRequest());
    }
    public void RequestInterstitial(){
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        //Clean up interstitial ad before creating a new one
        if(this.interstitial != null){
            this.interstitial.Destroy();
        }
        // Create an interstital
        this.interstitial = new InterstitialAd(adUnitId);

        // Load the interstitial ad
        this.interstitial.LoadAd(this.CreateAdRequest());
    }
    public void ShowInterstital(){
        if(this.interstitial.IsLoaded()){
            this.interstitial.Show();
        }
        else{
            Debug.Log("Interstital Ad is not yet");
        }
    }
    public void RequestRewardBasedVideo(){
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        this.rewardBasedVideo = new RewardedAd(adUnitId);
        this.rewardBasedVideo.LoadAd(this.CreateAdRequest());
    }
    public void ShowRewardBasedVideo(){
        if(this.rewardBasedVideo.IsLoaded()){
            this.rewardBasedVideo.Show();
        }
        else{
            Debug.Log("Reward based video ad is not ready yet");
        }
    }

   

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        this.RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        isRewarded = true;
    }

}
