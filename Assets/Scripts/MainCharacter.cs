using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacter : MonoBehaviour
{
    [Header("Player properties")]
    [SerializeField] private float life;
    [SerializeField] private float maxLife;
    [SerializeField] private float damage;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    

    [Header("Sprite and animations properties")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isGrounded = true;
    private float moveInput;
    private Rigidbody2D rb2d;


    private void Awake()
    {
        life = maxLife;
        rb2d = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float damage)
    {
        life -= damage;

        if (life <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        rb2d.linearVelocityX = moveInput * speed;
        print(rb2d.linearVelocityY);

        if (!isGrounded)
        {
            animator.SetFloat("YVelocity", rb2d.linearVelocityY);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        if (collision.GetComponent<EnemyMele>())
        {
            collision.transform.GetComponent<EnemyMele>().TakeDamage(damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }

    private void OnMovement(InputValue input)
    {   
        moveInput = input.Get<float>();
        
        animator.SetBool("IsRunning", moveInput != 0);
        FlipSprite();
    }

    private void OnJump()
    {
        if (isGrounded)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    private void FlipSprite()
    {
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
