using System;
using Src.MVVP.Models;
using UniRx;
using Zenject;

namespace Src.MVVM.ViewModels
{
    public class RoundViewModel
    {
        private readonly RoundModel _model;
        private readonly SignalBus _signalBus;
        private IDisposable _timerSubscription;

        public ReactiveProperty<string> TimeLeft { get; private set; } = new ReactiveProperty<string>("0");

        public RoundViewModel(RoundModel model, SignalBus signalBus)
        {
            _model = model;
            _signalBus = signalBus;
            TimeLeft.Value = _model.RoundTime.ToString();
            _signalBus.Subscribe<RoundStartSignal>(OnRoundStart);
            _signalBus.Subscribe<RoundEndedSignal>(OnRoundEnd);
        }
        
        ~RoundViewModel()
        {
            _signalBus.Unsubscribe<RoundStartSignal>(OnRoundStart);
            _signalBus.Unsubscribe<RoundEndedSignal>(OnRoundEnd);
        }
        
        private void OnRoundStart(RoundStartSignal signal)
        {
            StartTimer(signal.RoundTimeLimit);
        }
        
        private void OnRoundEnd()
        {
            StopTimer();
            TimeLeft.Value = "0";
        }
        
        private void StartTimer(int timeLimit)
        {
            _model.SetTime(timeLimit);
            TimeLeft.Value = _model.RoundTime.ToString();
            _timerSubscription = Observable.Interval(System.TimeSpan.FromSeconds(1))
                .TakeWhile(_ => _model.RoundTime > 0)
                .Subscribe(_ =>
                {
                    _model.DecreaseTime();
                    TimeLeft.Value = _model.RoundTime.ToString();
                    if (_model.RoundTime != 0) return;

                    _signalBus.Fire<TimerEndedSignal>();
                });
        }
        
        private void StopTimer()
        {
            _timerSubscription?.Dispose();
            _timerSubscription = null;
        }
    }
}