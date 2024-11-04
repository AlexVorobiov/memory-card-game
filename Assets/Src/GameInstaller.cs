using System.Collections.Generic;
using Src.MVVM.ViewModels;
using Src.MVVP.Models;
using Src.Settings;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardParent;
    [SerializeField] private Sprite[] cardSprites;
    
    [SerializeField] private List<RoundSettings> roundSettingsList;
    
    [SerializeField] private ParticleSystem effect1;
    [SerializeField] private ParticleSystem effect2;
    public override void InstallBindings()
    {
        Container.DeclareSignal<CardsSelectSignal>();
        Container.DeclareSignal<CardsMatchedSignal>();
        Container.DeclareSignal<CardsDidNotMatchSignal>();
        Container.DeclareSignal<BoardSizeSignal>();
        Container.DeclareSignal<UpdateTimerSignal>();
        Container.DeclareSignal<RoundStartSignal>();
        Container.DeclareSignal<RoundEndedSignal>();
        Container.DeclareSignal<StartGameSignal>();
        Container.DeclareSignal<TimerEndedSignal>();
        Container.DeclareSignal<GameOverSignal>();
        Container.DeclareSignal<RoundCompletedSignal>();
        Container.DeclareSignal<CardCountSelectedSignal>();
        Container.DeclareSignal<ShowMainMenuSignal>();
        Container.DeclareSignal<ShowGameMenuSignal>();
        Container.DeclareSignal<HideGameMenuSignal>();
        
        Container.BindInterfacesAndSelfTo<GameSettings>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RoundManager>().AsSingle().WithArguments(roundSettingsList).NonLazy();
        Container.BindInterfacesAndSelfTo<CardManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ControlManager>().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<EffectManager>().AsSingle().WithArguments(effect1, effect2);
        
        Container.Bind<CardFactory>().AsSingle().WithArguments(cardPrefab, cardParent, cardSprites);
        
        Container.Bind<RoundModel>().AsSingle();
        Container.Bind<RoundViewModel>().AsSingle();
    }
}
