using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float damage;
    private bool isOnSpikes = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOnSpikes = true;
        StartCoroutine(DealDamageEverySomeTime(collision.gameObject.GetComponent<MainCharacter>()));
    }

    private IEnumerator DealDamageEverySomeTime(MainCharacter mainCharacter)
    {
        while (isOnSpikes)
        {
            mainCharacter.TakeDamage(damage);
            yield return new WaitForSeconds(1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnSpikes = false;
    }
}
