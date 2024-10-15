using UnityEngine;

namespace Assets.Scripts
{
    public class SunBehaviour : MonoBehaviour
    {
        public float DaySpeed = 2f;
        public float NightSpeed = 15f;

        // Update is called once per frame
        void Update()
        {
            float speed;

            if (transform.position.z >= -1.4)
            {
                speed = DaySpeed;
            }
            else
            {
                speed = NightSpeed;
            }

            transform.RotateAround(Vector3.zero, Vector3.forward, speed * Time.deltaTime);
            transform.LookAt(Vector3.zero);
        }
    }
}
