using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class CamBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject firstPersonCam;

        [SerializeField]
        private GameObject thirdPersonCam;

        public void ToggleCam()
        {
            if (firstPersonCam.activeSelf)
            {
                thirdPersonCam.SetActive(true);
                firstPersonCam.SetActive(false);
            }
            else
            {
                thirdPersonCam.SetActive(false);
                firstPersonCam.SetActive(true);
            }
        }
    }
}