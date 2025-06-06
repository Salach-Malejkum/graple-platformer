using UnityEngine;
using System.Collections;

public class EnemyRange : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int timeDirect;
    [SerializeField] private float life;
    [SerializeField] private float maxLife;
    [SerializeField] private float damage;
    public float fireRate = 1f;
    [SerializeField] private float shootingRange;
    private float timeNextDirect;
    private float nextFire;
    public GameObject bullet;
    public GameObject bulletParent;
    private bool isMovingLeft = false;
    private Transform player;
    private bool isShooting = false;

    [SerializeField] private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Awake()
    {
        life = maxLife;
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
    }

    void FixedUpdate()
    {
        if (isShooting) return; 

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (life <= 0) Die();

        timeNextDirect += Time.fixedDeltaTime;
        if (timeNextDirect >= timeDirect)
        {
            isMovingLeft = !isMovingLeft;
            timeNextDirect = 0;
            transform.localRotation = isMovingLeft ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }

       
        if (distanceFromPlayer <= shootingRange && CanSeePlayer() && nextFire < Time.time)
        {
            StartCoroutine(StopAndShoot());
        }

        if (!isShooting)
        {
            float moveDirection = isMovingLeft ? -1f : 1f;
            transform.Translate(Vector3.right * moveDirection * speed * Time.fixedDeltaTime, Space.World);
        }
    }

    bool CanSeePlayer()
    {
  
        if (isMovingLeft)
        {
            return player.position.x < transform.position.x;
        }
        else
        {
            return player.position.x > transform.position.x;
        }
    }

    IEnumerator StopAndShoot()
    {
        isShooting = true;
        if (animator) animator.enabled = false;

        Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
        nextFire = Time.time + fireRate;

        yield return new WaitForSeconds(1f);

        isShooting = false;
        if (animator) animator.enabled = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MainCharacter>())
        {
            collision.gameObject.GetComponent<MainCharacter>().TakeDamage(damage);
        }
    }
}

