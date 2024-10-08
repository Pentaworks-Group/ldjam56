using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class MumbleBeeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject homeSweetHome;

        [SerializeField]
        private AudioSource audioSource;

        private Vector3 nextWaypoint;

        private readonly float distToWaypointReq = 0.5f;
        private float speed = 5f;
        private float rotationSpeed = 10f;
        private float radius = 12;
        private bool isOut = false;

        private Vector3 rotationAxis = Vector3.up;

        private void Start()
        {
            GameFrame.Base.Audio.Ambience.VolumeChanged.AddListener(OnAmbienceVolumeChanged);
            Base.Core.Game.OnPauseToggled.AddListener(OnGamePaused);

            ChooseNextWayPoint();
        }

        private void OnAmbienceVolumeChanged(System.Single newVolume)
        {
            audioSource.volume = newVolume;
        }

        private void OnGamePaused(System.Boolean isPaused)
        {
            if (isPaused)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.UnPause();
            }
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

            if (Random.value < 0.5f)
            {
                rotationAxis = Vector3.down;
            }

            radius *= Random.Range(0.8f, 1.2f);
            speed *= Random.Range(0.9f, 1.1f);
            rotationSpeed *= Random.Range(0.7f, 1.3f);
        }

        private void FlyAround()
        {
            transform.RotateAround(homeSweetHome.transform.position, rotationAxis, rotationSpeed * Time.deltaTime);

            var randVector = new Vector3(Random.Range(-.01f, .01f), Random.Range(-.01f, .01f), Random.Range(-.01f, .01f));

            transform.position += randVector;
        }

        private void FlyOut()
        {
            var dir = nextWaypoint - transform.position;
            if (dir.sqrMagnitude < distToWaypointReq)
            {
                isOut = true;
                transform.LookAt(homeSweetHome.transform);
                transform.Rotate(rotationAxis * 90);
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
            d *= radius;

            this.nextWaypoint = new Vector3(p.x + d.x + Random.Range(-1f, 1f), p.y + Random.Range(-1f, 1f) + 4, p.z + d.z + Random.Range(-1f, 1f));
        }
    }
}
