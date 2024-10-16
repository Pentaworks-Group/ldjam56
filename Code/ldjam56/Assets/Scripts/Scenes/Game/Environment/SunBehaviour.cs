using System;

using Assets.Scripts.Model;

using UnityEngine;

namespace Assets.Scripts
{
    public class SunBehaviour : MonoBehaviour
    {
        private World world;

        [SerializeField, Range(0, 43200)]
        private float timeOfDay = 0.0f;

        [SerializeField]
        private float secondsPerMinute = 60.0f;

        [SerializeField]
        private Transform sun;

        [HideInInspector]
        public float secondsPerHour;
        [HideInInspector]
        public float secondsPerDay;


        private Int32 currentDay = 1;
        public Int32 currentMonth = 1;
        private Int32 currentYear = 1;

        public float timeMultiplier = 1;

        private void Start()
        {
            this.world = Base.Core.Game.State?.World;

            if (this.world != default)
            {
                secondsPerHour = secondsPerMinute * 60.0f;
                secondsPerDay = secondsPerHour * 24.0f;
            }
        }

        private void Update()
        {
            if (this.world != default && Base.Core.Game.IsRunning)
            {
                var elapsed = Time.deltaTime;

                world.TimeElapsed += elapsed;
                world.Date = world.Date.AddSeconds(elapsed);

                SunUpdate();

                timeOfDay += Time.deltaTime * timeMultiplier;

                if (timeOfDay >= 360)
                {
                    currentDay += 1;

                    if (currentDay > 30)
                    {
                        currentDay = 1;
                        currentMonth += 1;

                        if (currentMonth > 12)
                        {
                            currentYear += 1;
                            currentMonth = 1;
                        }
                    }

                    timeOfDay = 0;
                }
            }
        }

        private void SunUpdate()
        {
            print("Date and Time : " + currentYear + "-" + currentMonth + "-" + currentDay + " : time = " + timeOfDay);

            //sun.transform.localRotation = Quaternion.Euler((timeOfDay / 24) * 360 - 0, -30, 0);
            sun.transform.localEulerAngles = SunAngle();
        }

        private Vector3 SunAngle()
        {
            //30,0,0 = sunrise
            //90,0,0 = High noon
            //180,0,0 = sunset
            //-90,0,0 = Midnight
            //0,90,0 = summer
            //0,40,0 = winter
            int tempMonth;

            switch (currentMonth)
            {
                case 8:
                    tempMonth = 6;
                    break;
                case 9:
                    tempMonth = 5;
                    break;
                case 10:
                    tempMonth = 4;
                    break;
                case 11:
                    tempMonth = 3;
                    break;
                case 12:
                    tempMonth = 2;
                    break;
                default:
                    tempMonth = currentMonth;
                    break;
            }

            return new Vector3(-timeOfDay, (tempMonth * 40 * 0.18f) + 40, 0);
        }
    }
}
