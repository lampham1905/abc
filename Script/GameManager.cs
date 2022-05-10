using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Analytics;
public class GameManager : MonoBehaviour
{
    public float Posx;
    public PlayerController playerPrefab;
    public Platform platformPrefab;

    // random Enenmy
    public EnemyRandom enemyPrefab;
    public int randomEnemy;
    public float randomPosxPrefab;


    public float minSpawnX;
    public float maxSpawnX;
    public float minSpawnY;
    public float maxSpawnY;
    public CamController mainCam;
    PlayerController m_player;
    public int m_score;
    float randomPlaformX;
    public float powerBarUp;
    public int scoreSave;
    bool m_isGameStarted;
    public TextMeshProUGUI greatText;
    public TextMeshProUGUI perfectText;
    public TextMeshProUGUI ooopsText;
    //wind
    public GameObject windUp;
    public GameObject windDown;
    public Text speedWindUpText;
    public Text speedWindDownText;
    public bool IsGameStarted
    {
        get => m_isGameStarted; 
    }
    public static GameManager Ins;
    private void Awake()
    {
        Ins = this;
    }

    void Start()
    {
        
        GameGUIManger.Ins.UpdateScoreCounting(m_score);
      // GameGUIManger.Ins.ShowGameGUI(false);
        GameGUIManger.Ins.UpdatePowerBar(0, 1);
    }
    private void Update()
    {
        {
            scoreSave = Pref.bestScore;
            checkWind();
            //CheckEagle();
        }
    }
    public  void PlayGame()
    {
        StartCoroutine(PlatformInit());
        GameGUIManger.Ins.ShowGameGUI(true);
      
    }

    IEnumerator PlatformInit()
    {
        Platform platformClone = null;
        float posXplayer = Random.Range(minSpawnX, maxSpawnX);
        if (platformPrefab)
        {
            platformClone = Instantiate(platformPrefab, new Vector2(posXplayer, Random.Range(minSpawnY, maxSpawnY)), Quaternion.identity);
            platformClone.id = platformClone.gameObject.GetInstanceID();
        }
        yield return new WaitForSeconds(0.5f);
        if (platformPrefab)
        {
            m_player = Instantiate(playerPrefab, new Vector2(posXplayer, 0), Quaternion.identity);
            m_player.lastPlatformId = platformClone.id;
        }
        if (platformPrefab)
        {
            float spawnX = m_player.transform.position.x + Random.Range(2f,3.5f);
            float spawnY = Random.Range(minSpawnY, maxSpawnY);
            Platform platformClone2 = Instantiate(platformPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
            platformClone2.id = platformClone2.gameObject.GetInstanceID();
        }
        yield return new WaitForSeconds(0.5f);
        m_isGameStarted = true;
    }

    public void CreatePlatform()
    {
        randomPlaformX = Random.Range(3.5f, 4f);
        if(!platformPrefab || !m_player) return;
        
        float spawnX = m_player.transform.position.x + randomPlaformX;
        randomPosxPrefab = spawnX;
        float spawnY = Random.Range(minSpawnY, maxSpawnY);
        Platform platformClone = Instantiate(platformPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
        platformClone.id = platformClone.gameObject.GetInstanceID();
    }
    public void CreatePlatformAndLerp(float playerXpos)
    {
        CreatePlatform();
        CreateEnemyRandom();
        if (mainCam)
        {
            mainCam.LerpTrigeer(playerXpos + randomPlaformX);
        }
       
    }
    public void CreateEnemyRandom(){
        randomEnemy = Random.Range(0,3);
        if(randomEnemy == 2){
            float spawnX = ((PlayerController.Ins.PosXPlayer + randomPosxPrefab) / 2) ;
            Posx = spawnX;
            EnemyRandom enemyClone = Instantiate(enemyPrefab, new Vector2(spawnX, Random.Range(-4,-1)), Quaternion.identity);
        }
    }
    public void AddScore(int add)
    {
        m_score+= add;
        Pref.bestScore = m_score;
        GameGUIManger.Ins.UpdateScoreCounting(m_score);
        // firebase analytics
        FirebaseAnalytics.LogEvent("Add_score", new Parameter("score", m_score));
    }
    public void checkWind()
    {
        if (PlayerController.speedWind == 0f)
        {

            windUp.gameObject.SetActive(false);
            windDown.gameObject.SetActive(false);
        }
        else if (PlayerController.speedWind > 0)
        {
            windUp.gameObject.SetActive(true);
            windDown.gameObject.SetActive(false);
        }
        else if(PlayerController.speedWind < 0)
        {
            windUp.gameObject.SetActive(false);
            windDown.gameObject.SetActive(true);
        }
        speedWindDownText.text = Mathf.Abs(PlayerController.speedWind).ToString("0.00");
        speedWindUpText.text = Mathf.Abs(PlayerController.speedWind).ToString("0.00");

    }
    private void DeleteGreatText()
    {
        greatText.gameObject.SetActive(false);

    }
    public void GreatText()
    {
        greatText.gameObject.SetActive(true);
        Invoke("DeleteGreatText", 1f);
    }
    public void PerfectText()
    {
        perfectText.gameObject.SetActive(true);
        Invoke("DeletePerfectText", 1f);
    }
    public void DeletePerfectText()
    {
        perfectText.gameObject.SetActive(false);
    }
    public void OoopsText()
    {
        ooopsText.gameObject.SetActive(true);
        Invoke("DeleteOoopsText", 1f);
    }
    public void DeleteOoopsText()
    {
        ooopsText.gameObject.SetActive(false);
    }
    
    // Reward when watch video

    public void RevivalPlayer(){
         m_player = Instantiate(playerPrefab, new Vector2(PlayerController.Ins.posXPlaform, 0), Quaternion.identity);        
    }
    
}
