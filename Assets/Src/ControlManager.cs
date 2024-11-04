using System;
using Zenject;

/// <summary>
/// Manages control states within the game, particularly flipping cards.
/// Utilizes Zenject's SignalBus for subscribing to game events.
/// Implements IInitializable for initialization logic.
/// </summary>
public class ControlManager : IInitializable, IDisposable
{
    /// <summary>
    /// Indicates whether cards can be flipped.
    /// </summary>
    public bool CanFlip { get; private set; } = true;
    
    // Reference to Zenject's SignalBus for event subscription.
    private readonly SignalBus _signalBus;
    
    /// <summary>
    /// Initializes a new instance of the ControlManager class.
    /// </summary>
    /// <param name="signalBus">The SignalBus instance for subscribing to signals.</param>
    public ControlManager(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    
    /// <summary>
    /// Subscribes to relevant signals on initialization.
    /// </summary>
    public void Initialize()
    {
        // Subscribe to CardsSelectSignal to disable card flipping.
        _signalBus.Subscribe<CardsSelectSignal>(DisableFlipping);
        
        // Subscribe to CardsMatchedSignal to enable card flipping.
        _signalBus.Subscribe<CardsMatchedSignal>(EnableFlipping);
        _signalBus.Subscribe<RoundStartSignal>(EnableFlipping);
        _signalBus.Subscribe<CardsDidNotMatchSignal>(EnableFlipping);
        _signalBus.Subscribe<RoundEndedSignal>(DisableFlipping);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<CardsSelectSignal>(DisableFlipping);
        _signalBus.Unsubscribe<CardsMatchedSignal>(EnableFlipping);
        _signalBus.Unsubscribe<CardsDidNotMatchSignal>(EnableFlipping);
        _signalBus.Unsubscribe<RoundEndedSignal>(DisableFlipping);
    }
    
    private void EnableFlipping()
    {
        CanFlip = true;
    }
    
    private void DisableFlipping()
    {
        CanFlip = false;
    }
    
}
