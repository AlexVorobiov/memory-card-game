using System;
using UnityEngine;
using Zenject;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject container;
    private SignalBus _signalBus;

    private void Start()
    {
        _signalBus.Subscribe<ShowMainMenuSignal>(OnMainMenuShow);
    }

    private void OnMainMenuShow()
    {
        container.SetActive(true);
    }

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void StartGame()
    {
        _signalBus.Fire<StartGameSignal>();
        container.SetActive(false);
    }
}