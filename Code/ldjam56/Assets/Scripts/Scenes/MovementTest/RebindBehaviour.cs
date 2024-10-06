using System;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindBehaviour : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Text bindingText;
    private InputAction inputAction;

    public void Init(InputAction inputAction)
    {
        this.inputAction = inputAction;
        nameText.text = inputAction.name;
        bindingText.text = inputAction.GetBindingDisplayString();
        gameObject.SetActive(true);
    }

}
