using UnityEngine;

namespace Assets.Scripts
{
    public class SunBehaviourCrap : MonoBehaviour
    {
        public float DaySpeed = 2f;
        public float NightSpeed = 15f;

        // Update is called once per frame
        void Update()
        {
            float speed;

            if (transform.position.z >= -1.4)
            {
                speed = DaySpeed;
            }
            else
            {
                speed = NightSpeed;
            }

            transform.RotateAround(Vector3.zero, Vector3.forward, speed * Time.deltaTime);
            transform.LookAt(Vector3.zero);
        }




        //private const Single MoonRotationInDays = 27.322f;

        //[SerializeField]
        //private Int32 dayDurationInMinutes = 1440;

        //[SerializeField]
        //private Single timeAccelerationFactor = 1;

        //private World world;

        //[Range(0, 43200)]
        //public float timeOfDay = 0.0f;

        //public float secondsPerMinute = 60.0f;
        //[HideInInspector]
        //public float secondsPerHour;
        //[HideInInspector]
        //public float secondsPerDay;

        //private float moonDaysElapsed;
        //private Int32 currentDay = 1;
        //public Int32 currectMonth = 1;
        //private Int32 currectYear = 1;

        //public float timeMultiplier = 1;

        //private void Start()
        //{
        //    this.world = Base.Core.Game.State?.World;

        //    if (this.world != default)
        //    {
        //        secondsPerHour = secondsPerMinute * 60.0f;
        //        secondsPerDay = secondsPerHour * 24.0f;
        //    }
        //}

        //private void Update()
        //{
        //    if (this.world != default && Base.Core.Game.IsRunning)
        //    {
        //        var elapsed = Time.deltaTime * timeAccelerationFactor;

        //        world.TimeElapsed += elapsed;
        //        world.Date = world.Date.AddSeconds(elapsed);

        //        SunUpdate();
        //        UpdateMoon();

        //        timeOfDay += Time.deltaTime * timeMultiplier;

        //        moonDaysElapsed += timeOfDay / secondsPerDay;

        //        if (timeOfDay >= 360)
        //        {
        //            currentDay += 1;

        //            if (currentDay > 30)
        //            {
        //                currentDay = 1;
        //                currectMonth += 1;

        //                if (currectMonth > 12)
        //                {
        //                    currectYear += 1;
        //                    currectMonth = 1;
        //                }
        //            }

        //            timeOfDay = 0;
        //        }
        //    }
        //}

        //private void SunUpdate()
        //{
        //    print("Date and Time : " + currectYear + "-" + currectMonth + "-" + currentDay + " : time = " + timeOfDay);

        //    //sun.transform.localRotation = Quaternion.Euler((timeOfDay / 24) * 360 - 0, -30, 0);
        //    sun.transform.localEulerAngles = SunAngle();
        //}

        //private void UpdateMoon()
        //{
        //    var moonAngle = (360 / MoonRotationInDays) * moonDaysElapsed;

        //    moon.transform.localEulerAngles = new Vector3(moonAngle, 0, 0);
        //}

        //private Vector3 SunAngle()
        //{
        //    //30,0,0 = sunrise
        //    //90,0,0 = High noon
        //    //180,0,0 = sunset
        //    //-90,0,0 = Midnight
        //    //0,90,0 = summer
        //    //0,40,0 = winter
        //    int tempMonth;

        //    switch (currectMonth)
        //    {
        //        case 8:
        //            tempMonth = 6;
        //            break;
        //        case 9:
        //            tempMonth = 5;
        //            break;
        //        case 10:
        //            tempMonth = 4;
        //            break;
        //        case 11:
        //            tempMonth = 3;
        //            break;
        //        case 12:
        //            tempMonth = 2;
        //            break;
        //        default:
        //            tempMonth = currectMonth;
        //            break;
        //    }

        //    return new Vector3(-timeOfDay, (tempMonth * 40 * 0.18f) + 40, 0);
        //}

    }
}
