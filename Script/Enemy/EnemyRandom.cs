using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandom : MonoBehaviour
{
    public static EnemyRandom Ins;
    void Awake(){
        Ins = this;
    }
    [SerializeField] private float bottom;
    [SerializeField] private float top;
    [SerializeField] private float speed = 5f;
    [SerializeField] Animator anim;
     private Rigidbody2D rb;
    private Collider2D coli;
    private bool facingLeft = true;
    
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        coli = GetComponent<Collider2D>();  
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        this.Move();
    }
    private void Move()
    {
        if (facingLeft)
        {

            if (transform.position.y > bottom)
            {
                // make sure sprite is facing location, and if it is not, then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                // test to see if we are on the ground, if so jump
               
                 rb.velocity = new Vector2(rb.velocity.x, -speed);
                    
                
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.y < top)
            {
                // make sure sprite is facing location, and if it is not, then face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                // test to see if we are on the ground, if so jump
                 rb.velocity = new Vector2(rb.velocity.x, speed);

            }
            else
            {
                facingLeft = true;
            }
        }
    }

    private  void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Limit"){
            EnemyDeathAnim();
        }
    }
    public void EnemyDeathAnim(){
            anim.SetTrigger("EnemyDeath");
    }
    private void EnemyDeath(){
        Destroy(this.gameObject);
    }

}
