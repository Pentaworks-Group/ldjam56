
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody beeBody;

    [SerializeField]
    private float _updateInterval = 0.1f;
    private float _lastUpdate = 0;

    private Vector3 currentMoveDirection = Vector3.zero;
    private bool _isMoving = false;

    private Vector3 currentViewDirection = Vector3.zero;
    private bool _isViewing = false;

    private void Awake()
    {
        beeBody = GetComponent<Rigidbody>();
        beeBody.linearDamping = 1.0f;
        beeBody.angularDamping = 1.0f;
    }

    void Update()
    {
        if (_lastUpdate > _updateInterval)
        {
            if (_isMoving)
            {
                MoveBee();
            }
            if (_isViewing)
            {
                ViewBee();
            }
        }
        else
        {
            _lastUpdate += Time.deltaTime;
        }
    }

    public void UpdateMoveDirection(Vector2 direction)
    {
        currentMoveDirection = new Vector3(0, -direction.x, -direction.y);
        _isMoving = true;
    }

    public void StopMoving()
    {
        _isMoving = false;
    }

    public void UpdateViewDirection(Vector2 direction)
    {
        currentViewDirection = new Vector3(-direction.x, 0, -direction.y);
        _isViewing = true;
    }

    public void StopViewing()
    {
        _isViewing = false;
    }
    public void Clear()
    {
        transform.position = new Vector3(0, 1, 0);
        currentMoveDirection = Vector2.zero;
        beeBody.linearVelocity = Vector2.zero;
        beeBody.angularVelocity = Vector2.zero;
    }

    private void MoveBee()
    {
        beeBody.AddRelativeForce(currentMoveDirection);
    }

    private void ViewBee()
    {
        beeBody.AddRelativeTorque(currentViewDirection);
    }
}
