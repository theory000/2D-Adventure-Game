using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public bool vertical;
    public float speed;
    Rigidbody2D rigidbody2d;

    // Timer to change axis
    public float changetime = 3.0f;
    float timer;
    int direction = -1;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changetime;
    }

    void Update() {
        timer -= Time.deltaTime;
        if (timer < 0) {
            direction = -direction;
            timer = changetime;
        }
    }

    void FixedUpdate() {
        Vector2 position = rigidbody2d.position;

        if (vertical) {
            position.y = position.y + speed * direction * Time.deltaTime;
        } else {
            position.x = position.x + speed * direction * Time.deltaTime;
        }

        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        
        if (player != null) {
            player.ChangeHealth(-1);
        }
    }

}