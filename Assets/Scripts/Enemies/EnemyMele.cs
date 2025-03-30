using UnityEngine;

public class EnemyMele : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeDirect;
    [SerializeField] private float life;
    [SerializeField] private float maxLife;
    [SerializeField] private float damage;
    private float timeNextDirect;
    bool isMovingLeft = false;

    void Start()
    {

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
        if (life <= 0)
        {
            Die();
        }
        timeNextDirect += Time.fixedDeltaTime;

        if (timeNextDirect >= timeDirect)
        {
            isMovingLeft = !isMovingLeft;
            timeNextDirect = 0;
            transform.localRotation = isMovingLeft ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }

        if (isMovingLeft)
        {
            transform.Translate(Vector3.left * speed * Time.fixedDeltaTime, Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.fixedDeltaTime, Space.World);
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MainCharacter>())
        {
            collision.gameObject.GetComponent<MainCharacter>().TakeDamage(damage);
        }
    }
}
