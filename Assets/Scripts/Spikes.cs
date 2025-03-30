using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MainCharacter>())
        {
            collision.gameObject.GetComponent<MainCharacter>().TakeDamage(damage);
        }
    }
}
