using UnityEngine;
using Zenject;

namespace Src.Settings
{
    [CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Memory Game/Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public int delayBeforeShowingCards = 2;
    
        public override void InstallBindings()
        {
            Container.BindInstance(delayBeforeShowingCards).WithId("DelayBeforeShowingCards");
        }
    }
}