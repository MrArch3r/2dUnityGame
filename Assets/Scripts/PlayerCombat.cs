using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    
    public LayerMask enemyLayers;
    
    public int attackDamage = 1;
    public float attackRate = 0.5f;
    public int playerHealth = 3;
    float nextAttackTime = 0;

    public Transform firePoint;
    public GameObject arrowPrefab;
    public float shootRate = 2f;
    float nextShootTime = 0;

    private PlayerSpawner playerSpawner;
    private PlayerMovement playerMovement;
    private GameObject particleSystemPrefab;

    void Start()
    {
        playerSpawner = GameObject.FindWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        particleSystemPrefab = GameObject.FindWithTag("Death");
    }

    void Update()
    {   
        if (Time.time >= nextShootTime)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                animator.SetTrigger("Shoot");
                nextShootTime = Time.time + 1f / shootRate;
            }
        }
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                meleeAttack();
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;
            }
        } 
        if (playerMovement == null) 
        {
            playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth = playerHealth - 1;
            animator.SetTrigger("Hurt");
        }

        if (collision.gameObject.CompareTag("Spike")) 
        {
            playerHealth = playerHealth - 1;
            animator.SetTrigger("Hurt");
            playerMovement.SpikeHit();
        }

        if (playerHealth <= 0)
        {
            GameObject particleSystem = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
            particleSystem.GetComponent<ParticleSystem>().Play();

            Destroy(gameObject);
            playerSpawner.RespawnPlayer();
        }
    }

    public void Shoot()
    {
        Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
    }

    public void meleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().takeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
