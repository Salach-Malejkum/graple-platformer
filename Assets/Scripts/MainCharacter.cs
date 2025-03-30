using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private AudioClip ouchSound;

    [Header("Grappling")]
    [SerializeField] private GrapplingGun grapplingGun;

    private bool isGrounded = true;
    private float moveInput;
    private Rigidbody2D rb2d;

    [Header("Pause menu")]
    [SerializeField] private GameObject pauseMenu;
    private AudioSource audioSource;

    [Header("Life UI")]
    [SerializeField] private TextMeshProUGUI lifeText;

    [Header("Die scene load")]
    [SerializeField] private SceneAsset mainMenu;


    private void Awake()
    {
        life = maxLife;
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        lifeText.text = "x " + life;
        StartCoroutine(RedBlink());
        audioSource.PlayOneShot(ouchSound);

        if (life <= 0)
        {
            StartCoroutine(Die());
        }
    }

    void FixedUpdate()
    {
        animator.SetBool("IsGrappling", grapplingGun.GetIsGrappling());
        if (grapplingGun.GetIsGrappling())
        {
            return;
        }

        rb2d.linearVelocityX = moveInput * speed;

        if (!isGrounded)
        {
            animator.SetFloat("YVelocity", rb2d.linearVelocityY);
        }
        else
        {
            animator.SetFloat("YVelocity", 0);
        }

        animator.SetBool("IsGrounded", isGrounded);
    }

    private IEnumerator Die()
    {
        Time.timeScale = 0;
        CameraShake.Instance.Shake(0.2f, 0.05f);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenu.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        if (collision.GetComponent<EnemyMele>())
        {
            collision.transform.GetComponent<EnemyMele>().TakeDamage(damage);
        }
        if (collision.GetComponent<EnemyRange>())
        {
            collision.transform.GetComponent<EnemyRange>().TakeDamage(damage);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovablePlatform")
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovablePlatform") 
        {
            transform.parent = null;
        }
    }

    private IEnumerator RedBlink()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = i % 2 == 0 ? Color.red : Color.white;
            yield return new WaitForSeconds(0.2f);
        }

        spriteRenderer.color = Color.white;
    }
}
