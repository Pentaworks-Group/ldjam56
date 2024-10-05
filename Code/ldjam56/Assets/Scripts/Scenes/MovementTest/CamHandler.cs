using UnityEngine;

public class CamHandler : MonoBehaviour
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
