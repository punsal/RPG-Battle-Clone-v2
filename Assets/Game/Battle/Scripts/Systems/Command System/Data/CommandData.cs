using System;

namespace Game.Battle.Scripts.Systems.Command_System.Data
{
    [Serializable]
    public struct CommandData
    {
        public int minutes;
        public float seconds;

        public void SetTime(float duration)
        {
            minutes = (int) (duration / 60f);
            seconds = duration % 60f;
        }

        public void SetTime(CommandData data)
        {
            minutes = data.minutes;
            seconds = data.seconds;
        }

        public float GetTime()
        {
            return minutes * 60f + seconds;
        }

        public void Tick(float duration)
        {
            var currentTime = GetTime();
            SetTime(currentTime - duration);
        }

        public override string ToString()
        {
            return $"{minutes} : {((int)seconds):D2}";
        }
    }
}