using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] float time = 10;
    [SerializeField] private float damage;
    Vector3 plInitPos = Vector3.zero;
    Vector3 direction;
    MainCharacter player;

    private void Awake()
    {
        player = FindObjectOfType<MainCharacter>();
        plInitPos = player.transform.position;
        direction = (plInitPos - transform.position).normalized;
        Destroy(gameObject, time);
    }
    void FixedUpdate()
    {
        transform.right = direction;
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MainCharacter>())
        {
            collision.transform.GetComponent<MainCharacter>().TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
