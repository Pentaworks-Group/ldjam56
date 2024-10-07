using UnityEngine;

namespace Assets.Scripts.Scenes.Credits
{
    public class CameraRotationBehaviour : MonoBehaviour
    {
        public Transform target;
        public float rotationSpeed = 10;

        private void Update()
        {
            transform.LookAt(target);
            transform.RotateAround(target.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }
}
