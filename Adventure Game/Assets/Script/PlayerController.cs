using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   // Variables related to player character movement
   public InputAction MoveAction;
   Rigidbody2D rigidbody2d;
   Vector2 move;
   public float speed = 3.0f;

   // Variables related to the health system
   public int maxHealth = 5;
   int currentHealth;
   public int health { get { return currentHealth; }}

   // Variables related to temporary invincibility
   public float timeInvincible = 2.0f;
   bool isInvincible;
   float damageCooldown;

   // Variables related to animation
   Animator animator;
   Vector2 moveDirection = new Vector2(1,0);

   // NPC commands
   public InputAction talkAction;

   // Variables related to projectile
   public GameObject projectilePrefab;
   public InputAction launchAction;
   public float ProjectileSpeed = 300;

   // Varibales related to Player Audio
   AudioSource audioSource;
   public AudioClip launchingProjectile;
   public AudioClip PlayerHit;
   public AudioClip HealthPack;


  // Start is called before the first frame update
  void Start() {
     MoveAction.Enable();
     launchAction.Enable();
     talkAction.Enable();

     launchAction.performed += Launch;
     talkAction.performed += FindFriend;

     rigidbody2d = GetComponent<Rigidbody2D>();
     animator = GetComponent<Animator>();
     audioSource = GetComponent<AudioSource>();

     currentHealth = maxHealth;
  }
 
  // Update is called once per frame
  void Update() {
      move = MoveAction.ReadValue<Vector2>();

      if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y,0.0f)) {
         moveDirection.Set(move.x, move.y);
         moveDirection.Normalize();
      }

      animator.SetFloat("Look X", moveDirection.x);
      animator.SetFloat("Look Y", moveDirection.y);
      animator.SetFloat("Speed", move.magnitude);

      if (isInvincible) {
         damageCooldown -= Time.deltaTime;
         if (damageCooldown < 0)
         isInvincible = false;
      }
   }

   // FixedUpdate has the same call rate as the physics system
   void FixedUpdate() {
      Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
      rigidbody2d.MovePosition(position);
   }

   public void ChangeHealth (int amount) {
      if (amount < 0)
         {
            if (isInvincible)
               return;
          
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(PlayerHit);
         } else {
            PlaySound(HealthPack);
         }

      currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
      UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
   }

   void Launch(InputAction.CallbackContext context) {
      GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

      Projectile projectile = projectileObject.GetComponent<Projectile>();
      projectile.Launch(moveDirection, ProjectileSpeed);
      animator.SetTrigger("Launch");

      PlaySound(launchingProjectile);
   }

   void FindFriend(InputAction.CallbackContext context) {
      RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

      if (hit.collider != null) {
         NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

         if (character != null) {
            UIHandler.instance.DisplayDialogue();
         }
      }
   }

   public void PlaySound(AudioClip clip) {
      audioSource.PlayOneShot(clip);
   }
}
