using UnityEngine;

public class EnemyMele : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int timeDirect;
    [SerializeField] private float life;
    [SerializeField] private float maxLife;
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
}
