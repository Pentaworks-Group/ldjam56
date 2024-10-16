using System;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Environment
{
    public class MoonBehaviour : MonoBehaviour
    {
        private const Single MoonRotationInDays = 27.322f;
        private float moonDaysElapsed;
        private float timeOfDay;

        [SerializeField]
        private Transform moon;
        [SerializeField]
        private float secondsPerMinute = 60.0f;

        private float secondsPerHour;
        private float secondsPerDay;

        private void Start()
        {
            secondsPerHour = secondsPerMinute * 60.0f;
            secondsPerDay = secondsPerHour * 24.0f;
        }

        private void Update()
        {
            timeOfDay += Time.deltaTime;

            moonDaysElapsed += timeOfDay / secondsPerDay;

            UpdateMoon();
        }

        private void UpdateMoon()
        {
            var moonAngle = (360 / MoonRotationInDays) * moonDaysElapsed;

            moon.transform.localEulerAngles = new Vector3(0, 0, moonAngle);
        }
    }
}
