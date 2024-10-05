using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class MouseBehaviour : MonoBehaviour
{
    [SerializeField]
    private MoverBehaviour mover;

    private bool isRotating = false;

    void Update()
    {
        var rotationX = Input.GetAxis("Mouse X");
        var rotationY = Input.GetAxis("Mouse Y");
        if (rotationX != 0 || rotationY != 0)
        {
            isRotating = true;
            mover.UpdateViewDirection(new Vector3(-rotationX, rotationY, 0));
        }
        else if (isRotating) 
        {
            mover.StopViewing();
            isRotating = false;
        }

    }
}
