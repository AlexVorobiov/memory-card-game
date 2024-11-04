using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popup;
    private SignalBus _signalBus;
    private RoundManager _roundManager;
    
    
    [Inject]
    public void Construct(SignalBus signalBus, RoundManager roundManager)
    {
        _signalBus = signalBus;
        _roundManager = roundManager;
    }
    
    private void Start()
    {
        _signalBus.Subscribe<ShowGameMenuSignal>(OnShowGameMenuSignal);
        _signalBus.Subscribe<HideGameMenuSignal>(OnHideGameMenuSignal);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<ShowGameMenuSignal>(OnShowGameMenuSignal);
        _signalBus.Unsubscribe<HideGameMenuSignal>(OnHideGameMenuSignal);
    }

    private void OnHideGameMenuSignal()
    {
        popup.SetActive(false);
    }

    private void OnShowGameMenuSignal()
    {
        popup.SetActive(true);
    }


    
    public void OnCloseButtonClicked()
    {
        popup.SetActive(false);
    }
    
    public void OnMainMenuButtonClicked()
    {
        _signalBus.Fire<ShowMainMenuSignal>();
        _roundManager.TerminateRound();
        popup.SetActive(false);
    }
}
