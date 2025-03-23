using UnityEngine;

public class PlatformTranslator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private Vector3 finalPos;

    private float movement;

    // Update is called once per frame
    void Update()
    {
        movement = movement + speed * Time.deltaTime;
        transform.position = Vector3.Lerp(initialPos, finalPos, Mathf.PingPong(movement, 1));
        
    }
}
