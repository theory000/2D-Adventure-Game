using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool aggressive = true;

    // Public variables
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
  
    // Private variables
    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = -1;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;  
    }


    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        if(!aggressive) {
            return;
        }

        timer -= Time.deltaTime;


        if (timer < 0) {
            direction = -direction;
            timer = changeTime;
        }
     
        Vector2 position = rigidbody2d.position;
     
        if (vertical) {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }


        rigidbody2d.MovePosition(position);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();


        if (player != null)
        {
           player.ChangeHealth(-1);
        }
    }

    public void Fix() {
        aggressive = false;
        animator.SetTrigger("Fixed");
        rigidbody2d.simulated = false;
    }

}