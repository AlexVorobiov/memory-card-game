using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WinMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popup;

    private SignalBus _signalBus;

    private void Start()
    {
        _signalBus.Subscribe<RoundCompletedSignal>(OnRoundCompleted);
    }

    private void OnRoundCompleted()
    {
        popup.SetActive(true);
    }

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    
    public void OnRestartButtonClicked()
    {
        _signalBus.Fire(new StartGameSignal());
        popup.SetActive(false);
    }
    
    public void OnMainMenuButtonClicked()
    {
        _signalBus.Fire(new ShowMainMenuSignal());
        popup.SetActive(false);
    }
    
    public void OnNextRoundButtonClicked()
    {
        _signalBus.Fire(new RoundStartSignal());
        popup.SetActive(false);
    }
}
