using UnityEngine;

namespace Assets.Scripts.Scenes.MovementTest
{
    public class KeyInputHandler : MonoBehaviour
    {
        [SerializeField]
        private Mover Mover;

        private float newDirectionX = 0;
        private float newDirectionY = 0;
        private float oldDirectionX = 0;
        private float oldDirectionY = 0;

        private float newViewX = 0;
        private float newViewY = 0;
        private float oldViewX = 0;
        private float oldViewY = 0;
        void Update()
        {
            PlanarMovement();
            View();
            
        }


        private void View()
        {
            bool isPressed = false;
            if (Input.GetKey(KeyCode.Q))
            {
                newViewX = +1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.E))
            {
                newViewX = -1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.R))
            {
                newViewY = -1;
                isPressed = true;
            }
            if (Input.GetKey(KeyCode.F))
            {
                newViewY = +1;
                isPressed = true;
            }

            if (isPressed)
            {
                if (newViewX != oldViewX || newViewY != oldViewY)
                {
                    UpdateView();
                }
            }
            else if (oldViewX != 0 || oldViewY != 0)
            {
                oldViewX = 0;
                oldViewY = 0;
                Mover.StopViewing();
            }
            else
            {
                newViewX = 0;
                newViewY = 0;
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

            if (isPressed)
            {
                if (newDirectionX != oldDirectionX || newDirectionY != oldDirectionY)
                {
                    UpdateDirection();
                }
            }
            else if (oldDirectionX != 0 || oldDirectionY != 0)
            {
                oldDirectionX = 0;
                oldDirectionY = 0;
                Mover.StopMoving();
            }
            else
            {
                newDirectionX = 0;
                newDirectionY = 0;
            }
        }

        private void UpdateDirection()
        {
            var newDirVector = new Vector2(newDirectionX, newDirectionY);
            Mover.UpdateMoveDirection(newDirVector);
            oldDirectionX = newDirectionX;
            oldDirectionY = newDirectionY;
        }

        private void UpdateView()
        {
            var newDirVector = new Vector2(newViewX, newViewY);
            Mover.UpdateViewDirection(newDirVector);
            oldViewX = newViewX;
            oldViewY = newViewY;
        }
    }
}
