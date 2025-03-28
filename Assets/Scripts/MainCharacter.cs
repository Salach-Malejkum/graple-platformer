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
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private GameObject smokeVFX;

    [Header("Grappling")]
    [SerializeField] private GrapplingGun grapplingGun;

    private bool isGrounded = true;
    private float moveInput;
    private Rigidbody2D rb2d;

    [Header("Pause menu")]
    [SerializeField] private GameObject pauseMenu;
    private AudioSource audioSource;


    private void Awake()
    {
        life = maxLife;
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
        if (grapplingGun.GetIsGrappling())
            return;

        rb2d.linearVelocityX = moveInput * speed;

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

    private void OnGrapple()
    {
        grapplingGun.OnGrapple();
    }

    private void OnCrosshair(InputValue inputValue)
    {
        Vector2 direction = inputValue.Get<Vector2>();
        grapplingGun.OnCrosshair(direction);
    }

    private void OnControlsChanged(PlayerInput obj)
    {
        switch (obj.currentControlScheme) 
        {
            case "Gamepad":
                grapplingGun.SetIsGamepad(true);
                break;
            default:
                grapplingGun.SetIsGamepad(false);
                break;
        }
    }

    private void OnPause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Step()
    {
        audioSource.PlayOneShot(stepSound);

        Vector3 smokePos = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        GameObject smoke = Instantiate(smokeVFX, smokePos, Quaternion.identity);
        smoke.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;
        Destroy(smoke, 0.8f);
    }
}
