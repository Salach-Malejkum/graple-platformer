using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float minimumX;
    [SerializeField] private float maximumX;
    [SerializeField] private float minimumY;
    [SerializeField] private float maximumY;


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (Mathf.Clamp(transform.position.x, minimumX, maximumX), Mathf.Clamp(transform.position.y, minimumY, maximumY), -10);

    }
}
