using UnityEngine;

public class EventEntityBehaviour : MonoBehaviour
{
    [SerializeField]
    private MessageBehaviour messageBehaviour;
    private void OnCollisionEnter(Collision collision)
    {
        messageBehaviour.OpenMessage();
        ChooseNewLocation();
    }

    private void ChooseNewLocation()
    {
        int randX = Random.Range(-4, 4);
        int randY = Random.Range(-4, 4);
        var vector3 = new Vector3(randX, 0, randY);
        Debug.Log("new position: " + vector3);
        transform.position = vector3;
    }

}
