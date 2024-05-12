using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 6f;
    private bool isFacingRight = true;
    public float smoothTime = 0.05f;

    public Animator animator;
    private bool hasPlayedJumpAfterHurt = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public PlayerSpawner playerSpawner;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawner = GameObject.FindWithTag("PlayerSpawner").GetComponent<PlayerSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal * speed));

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower); 
        }

        if (!IsGrounded())
        {
            if (!hasPlayedJumpAfterHurt && animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            {
                StartCoroutine(PlayJumpAfterHurt());
                hasPlayedJumpAfterHurt = true;
            } else
            {
                animator.SetBool("IsJumping", true);
            }
        } else
        {
           animator.SetBool("IsJumping", false);
           hasPlayedJumpAfterHurt = false;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        Flip();

        if (transform.position.y < -3)
        {
            Destroy(gameObject);
            playerSpawner.RespawnPlayer();
        }
    }

    private IEnumerator PlayJumpAfterHurt()
    {
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("IsJumping", true);
    }

    private void FixedUpdate()
    {
        float targetSpeed = horizontal * speed;

        if (IsGrounded()) 
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetSpeed, smoothTime), rb.velocity.y);
        } else 
        {
            if (horizontal != 0f)
            {
                //float strafeForceX = speed * (horizontal > 0f ? 1f : -1f);
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetSpeed, smoothTime), rb.velocity.y);
                //rb.AddForce(new Vector2(strafeForceX, 0f), ForceMode2D.Force);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void SpikeHit() 
    {   
        float spikeHorizontalForce = 2f;
        rb.velocity = new Vector2(spikeHorizontalForce * (isFacingRight ? 1f : -1f), jumpingPower * 0.75f); 
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}