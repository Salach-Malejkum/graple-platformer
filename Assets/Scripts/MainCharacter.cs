using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float maxLife;
    [SerializeField] private float damage;
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
            Diying();
        }
    }
    void Diying()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMele>())
        {
            collision.transform.GetComponent<EnemyMele>().TakeDamage(damage);
        }
    }
}
