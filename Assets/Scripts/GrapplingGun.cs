using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

    private Camera mainCamera;
    private bool isGrappling = false;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [HideInInspector] public Vector2? grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    private bool isGamepad;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
    }

    void RotateGun(Vector3 lookPoint)
    {
        Vector3 distanceVector = lookPoint;
        if (!isGamepad)
        {
            distanceVector = (lookPoint - gunPivot.position).normalized;
        }

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void SetGrapplePoint()
    {
        int layerMask = LayerMask.GetMask("Grappable");
        Vector2 distanceVector = transform.position - gunPivot.position;

        RaycastHit2D _hit = Physics2D.Raycast(gunPivot.position, distanceVector.normalized, maxDistnace, layerMask);

        if (_hit)
        {
            grapplePoint = _hit.point;
            grappleDistanceVector = grapplePoint.Value - (Vector2)gunPivot.position;
            grappleRope.enabled = true;
        }
        else
        {
            grapplePoint = null;
        } 
    }

    public void Grapple()
    {
        switch (launchType)
        {
            case LaunchType.Physics_Launch:
                m_springJoint2D.connectedAnchor = grapplePoint.Value;

                Vector2 distanceVector = grapplePoint.Value - (Vector2)transform.position;

                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
                break;
        }
    }

    public void OnGrapple()
    {
        if (!isGrappling)
        {
            SetGrapplePoint();
            isGrappling = true;
        }
        
        else if (isGrappling)
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
            isGrappling = false;
        }
    }

    public void OnCrosshair(Vector2 direction)
    {
        if (Time.timeScale == 0)
            return;

        if (isGamepad)
        {
            RotateGun(direction);
        }
        else
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            Vector2 crosshairPos = mainCamera.ScreenToWorldPoint(direction);
            RotateGun(crosshairPos);
        }
    }

    public bool GetIsGrappling()
    {
        return isGrappling;
    }

    public void SetIsGamepad(bool value)
    {
        isGamepad = value;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistnace);
        }

        if (gunPivot != null && transform.position != null)
        {
            // Draw a line from player to the grappling gun
            Gizmos.color = Color.red;
            Gizmos.DrawLine(gunPivot.position, transform.position);
        }

        if (grapplePoint != null && transform.position != null)
        {
            // Draw a line from player to the grappling gun
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(grapplePoint.Value, gunPivot.position);
        }
    }
}
