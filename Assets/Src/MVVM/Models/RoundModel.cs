namespace Src.MVVP.Models
{
    public class RoundModel
    {
        public int RoundTime { get; private set; }

        public void SetTime(int time)
        {
            RoundTime = time;
        }
        public void DecreaseTime()
        {
            if (RoundTime > 0)
                RoundTime--;
        }
    }
}