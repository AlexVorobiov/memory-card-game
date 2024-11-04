namespace Src.Settings
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewRoundSettings", menuName = "Memory Game/Round Settings")]
    public class RoundSettings : ScriptableObject
    {
        public int numberOfCards;
        public int timeToMemorize; // Time to memorize in seconds
        public int timeLimit; // Time limit in seconds
    }
}