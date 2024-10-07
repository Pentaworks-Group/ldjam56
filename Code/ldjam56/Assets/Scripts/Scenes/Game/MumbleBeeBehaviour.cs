using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class MumbleBeeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject homeSweetHome;

        private Vector3 nextWaypoint;

        private float distToWaypointReq = 0.5f;
        [SerializeField]
        private float speed = 10f;
        private bool isOut = false;

        private void Start()
        {
            ChooseNextWayPoint();
        }

        private void Update()
        {
            if (isOut)
            {
                FlyAround();
            }
            else
            {
                FlyOut();
            }
        }


        public void Init(GameObject homeSweetHome)
        {
            this.homeSweetHome = homeSweetHome;
            var hP = homeSweetHome.transform.position;
            transform.position = new Vector3(hP.x + 1, hP.y, hP.z + 1);
        }

        private void FlyAround()
        {
            transform.RotateAround(homeSweetHome.transform.position, Vector3.up, speed * Time.deltaTime);
        }

        private void FlyOut()
        {
            var dir = nextWaypoint - transform.position;
            if (dir.sqrMagnitude < distToWaypointReq)
            {

                isOut = true;
            }
            else
            {
                dir.Normalize();
                transform.position += dir * Time.deltaTime * speed;
            }
        }


        private void ChooseNextWayPoint()
        {

            var p = homeSweetHome.transform.position;
            var d = new Vector3(Random.value, 0, Random.value);
            d.Normalize();
            d *= 15;
            this.nextWaypoint = new Vector3(p.x + d.x + Random.Range(-1f, 1f), p.y + Random.Range(-1f, 1f) + 3, p.z + d.z + Random.Range(-1f, 1f));
        }

    }
}
