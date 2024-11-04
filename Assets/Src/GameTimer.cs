using System;
using UnityEngine;
using Zenject;

public class GameTimer : MonoBehaviour
{
    private float _timeLimit = 60f; // Time limit in seconds
    private float _elapsedTime; // Elapsed time
    private bool _isRunning; // Timer running state
    
    private SignalBus _signalBus;
    
    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    
    private void Start()
    {
        _signalBus.Subscribe<StartTimerSignal>(OnStartTimer);
    }

    private void OnStartTimer(StartTimerSignal signal)
    {
        _timeLimit = signal.Duration;
        StartTimer();
    }
    
    private void Update()
    {
        if (!_isRunning) return;
        // Update the elapsed time
        _elapsedTime += Time.deltaTime;

        // Calculate the remaining time
        var remainingTime = _timeLimit - _elapsedTime;

        // If the timer has run out, stop the timer and do something
        if (!(remainingTime <= 0)) return;
        remainingTime = 0;
        StopTimer();
        _signalBus.Fire<StartTimerSignal>();
        // You can add code here to handle what happens when the time runs out
        Debug.Log("Time's up!");
    }

    private void FixedUpdate()
    {
        var remainingTime = _timeLimit - _elapsedTime;
        _signalBus.Fire(new UpdateTimerSignal()
        {
            RemainingTime = remainingTime
        });
    }

    public void StartTimer()
    {
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _elapsedTime = 0f;
    }
}