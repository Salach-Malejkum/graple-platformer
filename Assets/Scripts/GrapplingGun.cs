using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistnace = 20;

    private Camera mainCamera;
    private bool isGrapplingTest = false;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [HideInInspector] public Vector2? grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        mainCamera = Camera.main;

    }

    private void Update()
    {
        if (isGrapplingTest)
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint.Value, false);
            }
            else
            {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistnace = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint.Value - firePointDistnace;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = (lookPoint - gunPivot.position).normalized;

        float angle = Mathf.Atan2(lookPoint.y, lookPoint.x) * Mathf.Rad2Deg;
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
                // Vector2 distanceVector = grapplePoint - (Vector2)gunPivot.position;
                print(distanceVector);

                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
                break;
            case LaunchType.Transform_Launch:
                m_rigidbody.gravityScale = 0;
                m_rigidbody.linearVelocity = Vector2.zero;
                break;
        }
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

    private void OnGrapple()
    {
        if (!isGrapplingTest)
        {
            SetGrapplePoint();
            isGrapplingTest = true;
        }
        
        else if (isGrapplingTest)
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
            isGrapplingTest = false;
        }
    }

    private void OnCrosshair(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();

        Vector2 crosshairPos = mainCamera.ScreenToWorldPoint(input);
        RotateGun(input, true);
    }
}
