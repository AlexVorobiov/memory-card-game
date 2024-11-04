using System;
using System.Collections.Generic;
using Src.Settings;
using Zenject;


public class RoundManager
{
    private readonly GameSettings _gameSettings;
    private readonly List<RoundSettings> _roundSettings;
    private readonly SignalBus _signalBus;

    private int _totalMatches;
    private int _currentMatches;

    public int TimeLimit { get; private set; }

    public int MemorizeTime { get; private set; }

    public bool IsRoundActive { get; private set; }

    public RoundManager(GameSettings gameSettings, List<RoundSettings> roundSettings, SignalBus signalBus)
    {
        _gameSettings = gameSettings;
        _roundSettings = roundSettings;
        _signalBus = signalBus;
    }

    public void Init()
    {
        var currentRoundSettings = _roundSettings.Find(s => s.numberOfCards == _gameSettings.TotalCards);
        TimeLimit = currentRoundSettings ? currentRoundSettings.timeLimit : 60;
        MemorizeTime = currentRoundSettings ? currentRoundSettings.timeToMemorize : 5;

        _totalMatches = _gameSettings.TotalCards / 2;
        _currentMatches = 0;
    }

    public void StartRound()
    {
        _signalBus.Fire(new RoundStartSignal() { RoundTimeLimit = TimeLimit });
        IsRoundActive = true;
    }
    
    public void TerminateRound()
    {
        IsRoundActive = false;
        _signalBus.Fire(new RoundEndedSignal());
    }

    public void EndRound()
    {
        IsRoundActive = false;
        _signalBus.Fire(new RoundEndedSignal());
        if (_currentMatches != _totalMatches)
        {
            _signalBus.Fire(new GameOverSignal());
        }
    }

    public void MatchFound()
    {
        _currentMatches++;
        if (_currentMatches == _totalMatches)
        {
            _signalBus.Fire<RoundCompletedSignal>();
            EndRound();
        }

        _signalBus.Fire(new CardsMatchedSignal()
        {
            CurrentMatches = _currentMatches
        });
    }
}