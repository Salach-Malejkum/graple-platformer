using UnityEngine;

public class EnemyMele : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int timeDirect;
    private float timeNextDirect;
    bool isMovingLeft = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    
    

    void FixedUpdate()
    {
        timeNextDirect += Time.deltaTime;

        if (timeNextDirect >= timeDirect)
        {
            isMovingLeft = !isMovingLeft;
            timeNextDirect = 0;
        }

        if (isMovingLeft)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

}
