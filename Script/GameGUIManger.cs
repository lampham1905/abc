 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameGUIManger : MonoBehaviour
{
    public static GameGUIManger Ins;
    private void Awake()
    {
        Ins = this;
    }
    // best socre
    public Text bestScoreText;
    public Text bestScoreTextOver;

    //check Replay
    bool onClickReplay;
    //void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}



    //private void Start()
    //{
      
    //    homeGUI.SetActive(true);
    //}

    public GameObject homeGUI;
    public GameObject gameGUI;
    public GameObject helpGUI;
    public GameObject achiGUI;
    public GameObject overGUI;
    public GameObject LoginGUI;
    public GameObject registerGUI;
    public GameObject eagle;
    public GameObject AdVideoBtn;

    public Text scoreCountingText;
    public Image powerBarSlider;
    public float powerBarUp;
    int scoreCurr;
    
    public void ShowGameGUI(bool isShow)
    {
        if (gameGUI)
        {
            gameGUI.SetActive(isShow);
        

        }
        if (gameGUI)
        {
            homeGUI.SetActive(!isShow);
         
        }
    }
    public void UpdatePowerBar(float curVal, float totalVal)
    {
        if (powerBarSlider)
        {
            powerBarSlider.fillAmount = curVal / totalVal;
        }
    }
    public void UpdateScoreCounting(int score)
    {
        if (scoreCountingText)
        {
            scoreCountingText.text = score.ToString();
        }
    }

    public void ShowAchivementDialog()
    {
        achiGUI.SetActive(true);
        bestScoreText.text = Pref.bestScore.ToString();

    }
    public void showHelpDialog()
    {

        helpGUI.SetActive(true);
       

    }
    
    public void showGameOverDialog()
    {

        overGUI.SetActive(true);
        bestScoreTextOver.text = Pref.bestScore.ToString();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
  public void showLogin(){
      LoginGUI.SetActive(true);
      registerGUI.SetActive(false);
  }
  public void showRegister(){
      registerGUI.SetActive(true);
      LoginGUI.SetActive(false);
  }
  public void CloseLoginGUI(){
      LoginGUI.SetActive(false);
  }
  
    public void Replay()
    {
        onClickReplay = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerController.speedWind = 0;
        // AdManager.instance.RequestInterstitial();
        // AdManager.instance.ShowInterstital();
    }
    public void ReplayRewardAdVideo(){
        onClickReplay = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerController.speedWind = 0;
        GameManager.Ins.m_score = scoreCurr;
    }
   // --------------Admod--------------
    // public void ShowRewardBasedVideo(){
    //    AdManager.instance.RequestVideoAd();
    //     AdManager.instance.ShowRewardBasedVideo();
    //     GameManager.Ins.m_score = scoreCurr;
    // }

    public void RewardBasedVideoAd(){
        //GameManager.Ins.RevivalPlayer();
        //overGUI.SetActive(false);
        ReplayRewardAdVideo();
        
    }
    public void BackToHome()
    {
        GameGUIManger.Ins.ShowGameGUI(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (onClickReplay)
        {
            GameGUIManger.Ins.ShowGameGUI(true);
            GameManager.Ins.PlayGame();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
  
}
