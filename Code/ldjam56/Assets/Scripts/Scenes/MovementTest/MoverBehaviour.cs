
using UnityEngine;
using UnityEngine.InputSystem;

public class MoverBehaviour : MonoBehaviour
{
    private Rigidbody beeBody;

    //[SerializeField]
    //private float _updateInterval = 0.1f;
    //private float _lastUpdate = 0;

    private Vector3 currentMoveDirection = Vector3.zero;
    private bool _isMoving = false;

    private Vector3 currentViewDirection = Vector3.zero;
    private bool _isViewing = false;

    private Quaternion initRotation;
    private Vector3 initPosition;

    private Vector3 gravity = new Vector3(0, -.02F, 0);

    private float moveFactor = 100f;
    private float viewFactor = 50f;



    private void Awake()
    {
        beeBody = GetComponent<Rigidbody>();
        beeBody.linearDamping = 1.0f;
        beeBody.angularDamping = 2.0f;

        initPosition = transform.position;
        initRotation = transform.rotation;
    }

    void Update()
    {
        if (_isMoving)
        {
            MoveBee();
        }
        if (_isViewing)
        {
            ViewBee();
        }
        beeBody.AddForce(gravity);
    }

    public void UpdateMoveDirection(Vector3 direction)
    {
        //currentMoveDirection = new Vector3(direction.y, -direction.z, -direction.x);
        currentMoveDirection = new Vector3(-direction.x, direction.y, -direction.z);
        currentMoveDirection.Normalize();
        currentMoveDirection *= moveFactor;
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    public void UpdateViewDirection(Vector3 direction)
    {
        //currentViewDirection = new Vector3(direction.x, -direction.y, -direction.z);
        //currentViewDirection = new Vector3(direction.x, direction.y, direction.z);
        currentViewDirection = new Vector3(-direction.z, direction.x, -direction.y);
        currentViewDirection.Normalize();
        currentViewDirection *= viewFactor;
        _isViewing = true;
    }

    public void StopViewing()
    {
        _isViewing = false;
    }
    public void Clear()
    {
        transform.position = initPosition;
        transform.rotation = initRotation;
        currentMoveDirection = Vector2.zero;
        beeBody.linearVelocity = Vector2.zero;
        beeBody.angularVelocity = Vector2.zero;
    }

    private void MoveBee()
    {
        beeBody.AddRelativeForce(currentMoveDirection * Time.deltaTime);
    }

    private void ViewBee()
    {
        beeBody.AddRelativeTorque(currentViewDirection * Time.deltaTime);
    }
}
