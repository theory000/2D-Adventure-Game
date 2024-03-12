using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    // Varibles related to Audio
    // public AudioClip collectedClip;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(1);
            Destroy(gameObject);
            // controller.PlaySound(collectedClip);
            Debug.Log(controller.health);
        }
        else
        {
            Debug.Log("You already have full health!");
        }

    }

}