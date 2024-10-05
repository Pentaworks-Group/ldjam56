using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class MouseHandler : MonoBehaviour
{
    [SerializeField]
    private Mover mover;

    void Update()
    {
        var rotationX = Input.GetAxis("Mouse X");
        var rotationY = Input.GetAxis("Mouse Y");
        if (rotationX != 0 || rotationY != 0)
        {
            Debug.Log("Test");
            mover.UpdateViewDirection(new Vector3(-rotationX, rotationY, 0));
        }
        else
        {
            mover.StopViewing();
        }

    }
}
