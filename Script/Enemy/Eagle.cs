using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float speed = 2f;

    private Rigidbody2D rb;
    private Collider2D coli;
    private bool facingLeft = true;


    // Start is called before the first frame update
   void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        coli = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        left = GameManager.Ins.mainCam.transform.position.x - 2f;
        right = GameManager.Ins.mainCam.transform.position.x + 2f;
        this.Move();
    }
    private void Move()
    {
        if (facingLeft)
        {

            if (transform.position.x > left)
            {
                // make sure sprite is facing location, and if it is not, then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                // test to see if we are on the ground, if so jump

                rb.velocity = new Vector2(-speed, rb.velocity.y);


            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < right)
            {
                // make sure sprite is facing location, and if it is not, then face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                // test to see if we are on the ground, if so jump
                rb.velocity = new Vector2(speed, rb.velocity.y);

            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
