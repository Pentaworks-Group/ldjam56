using UnityEngine;

namespace Assets.Scripts.Scenes.MovementTest
{
    public class KeyInputHandler : MonoBehaviour
    {
        [SerializeField]
        private Mover mover;

        private float newDirectionX = 0;
        private float newDirectionY = 0;
        private float newDirectionZ = 0;
        private float oldDirectionX = 0;
        private float oldDirectionY = 0;
        private float oldDirectionZ = 0;

        private float newViewX = 0;
        private float newViewY = 0;
        private float newViewZ = 0;
        private float oldViewX = 0;
        private float oldViewY = 0;
        private float oldViewZ = 0;
        void Update()
        {
            PlanarMovement();
            View();
            
        }


        private void View()
        {
            bool isPressed = false;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                newViewX = +1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                newViewX = -1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                newViewY = -1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                newViewY = +1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.Keypad2))
            {
                newViewZ = +1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.Keypad1))
            {
                newViewZ = -1;
                isPressed = true;
            }

            if (isPressed)
            {
                if (newViewX != oldViewX || newViewY != oldViewY || newViewZ != oldViewZ)
                {
                    UpdateView();
                }
            }
            else if (oldViewX != 0 || oldViewY != 0 || oldViewZ != 0)
            {
                oldViewX = 0;
                oldViewY = 0;
                oldViewZ = 0;
                mover.StopViewing();
            }
            else
            {
                newViewX = 0;
                newViewY = 0;
                newViewZ = 0;
            }
        }

        private void PlanarMovement()
        {
            bool isPressed = false;
            if (Input.GetKey(KeyCode.W))
            {
                newDirectionX = +1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                newDirectionX = -1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                newDirectionY = -1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                newDirectionY = +1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.Z))
            {
                newDirectionZ = -1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.X))
            {
                newDirectionZ = +1;
                isPressed = true;
            }

            if (isPressed)
            {
                if (newDirectionX != oldDirectionX || newDirectionY != oldDirectionY || newDirectionZ != oldDirectionZ)
                {
                    UpdateDirection();
                }
            }
            else if (oldDirectionX != 0 || oldDirectionY != 0 || oldDirectionZ != 0)
            {
                oldDirectionX = 0;
                oldDirectionY = 0;
                oldDirectionZ = 0;
                mover.StopMoving();
            }
            else
            {
                newDirectionX = 0;
                newDirectionY = 0;
                newDirectionY = 0;
            }
        }

        private void UpdateDirection()
        {
            var newDirVector = new Vector3(newDirectionX, newDirectionY, newDirectionZ);
            mover.UpdateMoveDirection(newDirVector);
            oldDirectionX = newDirectionX;
            oldDirectionY = newDirectionY;
            oldDirectionZ = newDirectionZ;
        }

        private void UpdateView()
        {
            var newDirVector = new Vector3(newViewX, newViewY, newViewZ);
            mover.UpdateViewDirection(newDirVector);
            oldViewX = newViewX;
            oldViewY = newViewY;
            oldViewZ = newViewZ;
        }
    }
}
