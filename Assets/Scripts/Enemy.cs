using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public int maxHealth;
    int currentHealth;
    public GameObject player;
    bool facingRight = false;
    public Animator animator;
  
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        Vector3 objectPosition = transform.position;
        if (player.transform.position.x >= objectPosition.x && !facingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = true;
        } else if (player.transform.position.x < objectPosition.x && facingRight)
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = false;
        }
    }

    public void takeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        animator.SetBool("IsDead", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        //GetComponent<SpriteRenderer>().enabled = false;
        GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>().enabled = false;
        this.enabled = false;
    }
}
