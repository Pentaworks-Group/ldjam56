using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

public class TestBehaviour : MonoBehaviour
{
    private PlayerInput input;

    private RebindBehaviour rebindTemplate;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rebindTemplate = transform.Find("RebindTemplate").GetComponent<RebindBehaviour>();
    }

    private void OnEnable()
    {
        //UpdateBindings();
        //var actopm = input.actions["Look"];
        Debug.Log("=====================");
        //Debug.LogFormat("{0} {1}", actopm.name, actopm.GetBindingDisplayString());
        //var binding = actopm.bindings[1];
        var actopm = input.actions["Look"];
        actopm.ApplyBindingOverride(1, string.Empty);
        //Debug.LogFormat("{0} {1} {2}", binding.name, binding.action, binding.effectivePath);
        //foreach (var action in input.actions)
        //{
        //    Debug.Log(action.name + "  " + action.id);
        //    if (action.name == "Look")
        //    {
        //        foreach (var bind in action.bindings)
        //        {
        //            Debug.LogFormat("{0} {1} {2}", bind.name, bind.action, bind.effectivePath);
        //        }
        //    }
        //}
    }

    public void UpdateBindings()
    {
        Debug.Log(input.actions.Count());
        var rect = rebindTemplate.GetComponent<RectTransform>();
        var height = rect.anchorMax.y - rect.anchorMin.y;
        //var space = 0.03f;
        int cnt = 0;
        foreach (var action in input.actions)
        {
            var newRebind = Instantiate<RebindBehaviour>(rebindTemplate, transform);
            var newRect = newRebind.GetComponent<RectTransform>();
            float maxY = 1 - cnt * height;
            newRect.anchorMin = new Vector2(rect.anchorMin.x, maxY - height);
            newRect.anchorMax = new Vector2(rect.anchorMax.x, maxY);
            newRebind.Init(action);
            cnt++;
        }

    }
}
