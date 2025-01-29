using Assets.Scripts.Scenes.Game.Bee;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Hazards
{
    public class DirectionalDraft : HazardBaseBehaviour
    {

        private Vector3 gravity;
        private float strength = 5f;

        private void Start()
        {
            Init();
        }

        protected override void OnEnter(Collider collision)
        {
            mover.SetGravity(gravity);
        }

        protected override void OnExit(Collider collision)
        {
            mover.SetGravity(-gravity);
        }

        public override void Init()
        {
            //random scale
            var scaleFactor = Random.Range(0f, 2f);
            //Debug.Log("scale: " + scaleFactor);
            var scaleVector = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            transform.localScale += scaleVector;
            transform.GetChild(0).localScale += scaleVector;

            //random rotation, limit to max 30 degree down 
            var xRotation = Random.Range(-210, 30);
            var yRotation = Random.Range(0, 360);
            var zRotation = Random.Range(0, 360);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);

            //set gravity
            gravity = transform.forward * strength; 

            //attempts do move forward/back
            var before = transform.position;
            var changeVector = transform.forward * scaleVector.x;
            if (xRotation > 0 || xRotation < -180)
            {
                transform.position -= changeVector;
            }
            else
            {
                transform.position += changeVector;
            }

            //Debug.LogFormat("forwad: {0}, factor: {1}, changeVector: {2}, scale: {3}", transform.forward, scaleFactor, changeVector, transform.localScale);
            //Debug.LogFormat("Before: {0}, after: {1}, xRotation: {2}", before, transform.position, xRotation);
        }

    }
}