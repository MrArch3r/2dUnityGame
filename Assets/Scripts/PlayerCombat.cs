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

    public PlayerSpawner playerSpawner;

    void Start()
    {
        playerSpawner = GameObject.FindWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth = playerHealth - 1;
            animator.SetTrigger("Hurt");
            if (playerHealth <= 0)
            {
                Destroy(gameObject);
                playerSpawner.RespawnPlayer();
            }
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
