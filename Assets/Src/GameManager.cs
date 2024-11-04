using System;
using System.Collections.Generic;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;
using Src.Settings;


public class GameManager : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;

    private readonly RoundManager _roundManager;
    private readonly CardManager _cardManager;

    private readonly EffectManager _effectManager;

    public GameManager(
        SignalBus signalBus,
        RoundManager roundManager,
        CardManager cardManager,
        EffectManager effectManager)
    {
        _signalBus = signalBus;
        _effectManager = effectManager;
        _roundManager = roundManager;
        _cardManager = cardManager;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<StartGameSignal>(OnGameStart);
        _signalBus.Subscribe<TimerEndedSignal>(OnTimerEnded);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<StartGameSignal>(OnGameStart);
        _signalBus.Unsubscribe<TimerEndedSignal>(OnTimerEnded);
    }

    private void OnTimerEnded()
    {
        _roundManager.EndRound();
    }

    private async void OnGameStart()
    {
        _roundManager.Init();
        _cardManager.Init();
        await _cardManager.ShowCardsBriefly();
        _roundManager.StartRound();
    }
}