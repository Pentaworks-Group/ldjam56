using UnityEngine;

public class MessageBehaviour : MonoBehaviour
{
    public void CloseMessage()
    {
        gameObject.SetActive(false);
    }

    public void OpenMessage()
    {
        gameObject.SetActive(true);
    }
}
