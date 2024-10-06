using UnityEngine;

namespace Assets.Scripts.Scenes.Game.InputPad
{
    public class CommandRelayBehaviour : MonoBehaviour
    {
        public virtual void SetDirection(Vector3 direction) { }
        public virtual void Stop() { }
    }
}