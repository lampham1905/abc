using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Analytics;

//using Firebase.Extensions;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Ins;
    public bool isDead = false;
    public Vector2 jumpForce;
    public Vector2 jumpForceUp;
    public float minForceX;
    public float maxForceX;
    public float minForceY;
    public float maxForceY;
    public static float speedWind = 0;
    [HideInInspector]
    public int lastPlatformId;
   public float PosXPlayer;
    bool m_didJump;
    bool m_powerSetted;
    //public GameObject windUp;
    //public GameObject windDown;
    public Rigidbody2D m_rb;
    Animator m_anim;
    public BoxCollider2D box;
    public  float m_curPowerBarVal = 0;
    public  float qualityJump;
    private LineRenderer lr;
    public float posXPlaform;
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        lr = GetComponent<LineRenderer>();
        Ins = this;
      
    }
    public  int randomEnemy;
    private void Update()
    {
        if (GameManager.Ins.IsGameStarted)
        {
            SetPower();
            if (Input.GetMouseButtonDown(0))
            {
                SetPower(true);
            }
            if (Input.GetMouseButtonUp(0))
            {
                SetPower(false);
                lr.positionCount = 0;
            }

        }
        PosXPlayer = transform.position.x;
    }
    void SetPower()
    {
        if (m_powerSetted && !m_didJump)
        {
            jumpForce.x += jumpForceUp.x * Time.deltaTime + speedWind/300;
            jumpForce.y += jumpForceUp.y * Time.deltaTime;

            jumpForce.x = Mathf.Clamp(jumpForce.x, minForceX, maxForceX);
            jumpForce.y = Mathf.Clamp(jumpForce.y, minForceY, maxForceY);

            // spacebar 
            m_curPowerBarVal += GameManager.Ins.powerBarUp * Time.deltaTime;
            GameGUIManger.Ins.UpdatePowerBar(m_curPowerBarVal, 1);

            // tranjectory line
            Vector2[] tranjectory = Plot(m_rb, (Vector2)transform.position, jumpForce, 500);
            lr.positionCount = tranjectory.Length;
            Vector3[] positions = new Vector3[tranjectory.Length];
            for(int i = 0; i < tranjectory.Length; i++)
            {
                positions[i] = tranjectory[i];
            }
            lr.SetPositions(positions);
           
            float width = lr.startWidth;
            lr.material.mainTextureScale = new Vector2(1f/ width, 1.0f);

        }
    }
    
    public  Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps){
        Vector2[] results = new Vector2 [steps];

        float timestep = Time.fixedDeltaTime/ Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;
        for(int i = 0; i < steps; i++){
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }
        return results;
    }

    public void SetPower(bool isHoldingMosue)
    {
        m_powerSetted = isHoldingMosue;

        if(!m_powerSetted && !m_didJump)
        {
            Jump();
        }
    }
    void Jump()
    {
        if (!m_rb || jumpForce.x <= 0 || jumpForce.y <= 0) return;

        m_rb.velocity = jumpForce;

        m_didJump = true;

        if (m_anim)
        {
            m_anim.SetBool("didJump", true);


        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagConst.GROUND)
        {
            Platform p = collision.transform.root.GetComponent<Platform>();
            if (m_didJump)
            {
                m_didJump = false;

                if (m_anim)
                {
                    m_anim.SetBool("didJump", false);
                }
                if (m_rb)
                {
                    m_rb.velocity = Vector2.zero;
                }
                jumpForce = Vector2.zero;
                m_curPowerBarVal = 0;
                GameGUIManger.Ins.UpdatePowerBar(m_curPowerBarVal, 1);
            }

            if (p && p.id != lastPlatformId) {
                // Create Enemy Random
                //GameManager.Ins.CreateEnemyRandom();
                lastPlatformId = p.id;
                posXPlaform = p.transform.position.x;
                speedWind = Random.Range(-1f, 1f);
                qualityJump = Mathf.Abs (this.transform.position.x - p.transform.position.x);
                if(qualityJump >= 0.2 && isDead == false)
                {
                    GameManager.Ins.GreatText();
                    m_anim.SetBool("hurtLeft", true);
                    
                    GameManager.Ins.AddScore(1);
                    box.enabled = false;
                    m_anim.SetBool("touchBox", false);
                    
                }
               
                else if(qualityJump < 0.2 && qualityJump > -0.4 && isDead == false)
                {
                    GameManager.Ins.PerfectText();
                    m_anim.SetBool("hurtLeft", false);
                    GameManager.Ins.AddScore(2);
                    box.enabled = true;
                    m_anim.SetTrigger("touchBoxHurt");
                }
                else if(qualityJump < -0.35 && isDead == false)
                {
                    GameManager.Ins.GreatText();
                    m_anim.SetBool("hurtLeft", true);
                    box.enabled = false;
                    GameManager.Ins.AddScore(1);
                }
                // Firebase Analytics
                 FirebaseAnalytics.LogEvent(
                     "AddScore",new Parameter("SpeedWind", speedWind)
                 );

            }
        }
        if(collision.gameObject.tag == TagConst.BOX)
        {
            m_rb.velocity = new Vector2(-2f, 5f);
            m_anim.SetBool("touchBox", true);
            //Invoke("DestroyPlayer", .5f);
            //m_anim.SetTrigger("death");
        }
        if(collision.gameObject.tag == TagConst.OOOPS_ZONE)
        {
            GameManager.Ins.OoopsText();
        }

       
       
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagConst.DEAD_ZONE)
        {
            Invoke("DestroyPlayer", .5f);
            m_anim.SetTrigger("death");
            m_rb.velocity = new Vector2(m_rb.velocity.x, 5f);
           // isDead = true;
           // firebase analytics
           FirebaseAnalytics.LogEvent(
                "Death",
                new Parameter("type", "trap")
            );

            

        }
         if(collision.gameObject.tag == TagConst.ENEMY)
        { 
                EnemyRandom.Ins.EnemyDeathAnim();
                m_rb.velocity = new Vector2(2f, 10f);
                FirebaseAnalytics.LogEvent("Collision Enemy", new Parameter("type", "enemy"));
        }
        }
        
        
    
    //public  void checkWind()
    //{
    //    if(speedWind == 0)
    //    {
    //        windUp.gameObject.SetActive(false);
    //        windDown.gameObject.SetActive(false);
    //    }
    //    else if(speedWind > 0)
    //    {
    //        windUp.gameObject.SetActive(true);
    //        windDown.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        windUp.gameObject.SetActive(false);
    //        windDown.gameObject.SetActive(true);
    //    }
    //}
    public void DestroyPlayer()
    {
        GameGUIManger.Ins.showGameOverDialog();
        Destroy(this.gameObject);
        AdManager.instance.RequestInterstitial();
        AdManager.instance.ShowInterstital();
    }
    private void SpwanEnemy(){
        
    }
    
}
