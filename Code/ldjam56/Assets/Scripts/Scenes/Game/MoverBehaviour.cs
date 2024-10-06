using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class MoverBehaviour : MonoBehaviour
    {
        private Rigidbody beeBody;

        private Vector3 currentMoveDirection = Vector3.zero;
        private bool _isMoving = false;

        private Vector3 currentViewDirection = Vector3.zero;
        private bool _isViewing = false;

        private Quaternion initRotation;
        private Vector3 initPosition;

        public Vector3 gravity = new Vector3(0, -.02F, 0);

        private float moveFactor = 100f;
        private float viewFactor = 10f;
        private float speed = 1f;

        private float nextEvent = 0;

        private List<SpeedEvent> activeEvents = new List<SpeedEvent>();
        private class SpeedEvent
        {
            public float speedFactor;
            public float time;

            public SpeedEvent(System.Single speedFactor, System.Single time)
            {
                this.speedFactor = speedFactor;
                this.time = time;
            }
        }


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
            if (activeEvents.Count > 0) 
            {
                if (nextEvent < 0)
                {
                    var ev = activeEvents[0];
                    speed /= ev.speedFactor;
                    activeEvents.RemoveAt(0);
                    if (activeEvents.Count > 0)
                    {
                        foreach (var e in activeEvents)
                        {
                            e.time -= Time.deltaTime;
                        }
                        nextEvent = activeEvents[0].time;
                    }
                }
                else
                {
                    foreach (var e in activeEvents)
                    {
                        e.time -= Time.deltaTime;
                    }
                    nextEvent = activeEvents[0].time;
                }
            }

        }

        public void UpdateMoveDirection(Vector3 direction)
        {
            //currentMoveDirection = new Vector3(direction.y, -direction.z, -direction.x);
            direction.Normalize();
            currentMoveDirection = new Vector3(-direction.x * moveFactor, direction.y * moveFactor, -direction.z * moveFactor * speed);
            //currentMoveDirection *= moveFactor;
            _isMoving = true;
        }

        public void SetGravity(Vector3 gravity)
        {
            this.gravity += gravity;
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
            currentViewDirection = new Vector3(currentViewDirection.x * viewFactor, currentViewDirection.y * viewFactor, currentViewDirection.z);
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

        public void AddSpeedBoost(float speedFactor, float time)
        {
            speed *= speedFactor;
            var speedEvent = new SpeedEvent(speedFactor, time);
            activeEvents.Add(speedEvent);

            activeEvents = activeEvents.OrderBy(e=>e.time).ToList();
            nextEvent = activeEvents[0].time;
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
}