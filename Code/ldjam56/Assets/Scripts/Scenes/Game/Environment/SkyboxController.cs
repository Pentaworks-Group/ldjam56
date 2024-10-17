using System;

using Assets.Scripts.Model;

using GameFrame.Core.Extensions;

using UnityEngine;

using UnityVector3 = UnityEngine.Vector3;
using UnityVector4 = UnityEngine.Vector4;

namespace Assets.Scripts
{
    //[ExecuteAlways]
    public class SkyboxController : MonoBehaviour
    {
        private const Single MoonRotationInDays = 27.322f;
        private World world;

        [SerializeField]
        private Single speed = 15f;

        [SerializeField]
        private Transform sun = default;

        [SerializeField]
        private Transform moon = default;

        private void Start()
        {
            this.world = Base.Core.Game.State?.World;

            if (this.world != default)
            {
                this.sun.eulerAngles = this.world.SunAngles.ToUnity();
                this.moon.eulerAngles = this.world.MoonAngles.ToUnity();
            }
        }

        private void Update()
        {
            if (this.world != default)
            {
                var angle = speed * Time.deltaTime;

                UpdateSun(angle);
                UpdateMoon(angle);
            }
        }

        private void UpdateSun(Single angle)
        {
            UpdateTransform(sun, angle);

            this.world.SunAngles = moon.eulerAngles.ToFrame();
        }

        private void UpdateMoon(Single angle)
        {
            UpdateTransform(moon, angle / MoonRotationInDays);

            this.world.MoonAngles = moon.position.ToFrame();
        }

        private void UpdateTransform(Transform transformToChange, Single angle)
        {
            transformToChange.RotateAround(UnityVector3.zero, UnityVector3.forward, angle);
            transformToChange.LookAt(UnityVector3.zero);
        }

        void LateUpdate()
        {
            if (this.world != default)
            {
                // Sun
                Shader.SetGlobalVector("_SunDir", -sun.transform.forward);

                // Moon
                Shader.SetGlobalVector("_MoonDir", -moon.transform.forward);
                Shader.SetGlobalMatrix("_MoonSpaceMatrix", new Matrix4x4(-moon.transform.forward, -moon.transform.up, -moon.transform.right, UnityVector4.zero).transpose);
            }
        }
    }
}
