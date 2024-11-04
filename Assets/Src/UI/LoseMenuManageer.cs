using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LoseMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popup;
    private SignalBus _signalBus;
    public void Start()
    {
        _signalBus.Subscribe<GameOverSignal>(OnGameOver);
    }

    private void OnGameOver()
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
}
