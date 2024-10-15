using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteAlways]
    public class SkyboxController : MonoBehaviour
    {
        [SerializeField]
        private Transform sun = default;

        [SerializeField]
        private Transform moon = default;

        void LateUpdate()
        {
            // Directions are defined to point towards the object

            // Sun
            Shader.SetGlobalVector("_SunDir", -sun.transform.forward);

            // Moon
            Shader.SetGlobalVector("_MoonDir", -moon.transform.forward);
            Shader.SetGlobalMatrix("_MoonSpaceMatrix", new Matrix4x4(-moon.transform.forward, -moon.transform.up, -moon.transform.right, Vector4.zero).transpose);
        }
    }
}
