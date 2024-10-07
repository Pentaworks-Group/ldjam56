namespace Assets.Scripts.Model
{
    public class TimedSpeedEvent
    {
        public float speedFactor;
        public float time;

        public TimedSpeedEvent(System.Single speedFactor, System.Single time)
        {
            this.speedFactor = speedFactor;
            this.time = time;
        }
    }
}
