using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class CountOfCardsSelector : MonoBehaviour
{
    [SerializeField] private GameObject item;
    
    private readonly List<CardCountSelect> _items = new();
    private GameSettings _gameSettings;
    private SignalBus _signalBus;
    private DiContainer _container;

    private CardCountSelect _selectedItem;

    private void Start()
    {
        _signalBus.Subscribe<CardCountSelectedSignal>(OnCardCountSelected);
        foreach (var cardCount in _gameSettings.AvailableCardCounts)
        {
            var createdCards = _container.InstantiatePrefabForComponent<CardCountSelect>(item, transform);
            createdCards.SetCardCount(cardCount);
            _items.Add(createdCards);
        }

        _selectedItem = _items.First();
        _selectedItem.Select();
        _gameSettings.SetCardCount(_selectedItem.CardCount);
    }
    
    private void OnDestroy()
    {
        _signalBus.Unsubscribe<CardCountSelectedSignal>(OnCardCountSelected);
    }

    private void OnCardCountSelected(CardCountSelectedSignal signal)
    {
        _selectedItem.UnSelect();
        _selectedItem = _items.Find(i => i.CardCount == signal.CardCount);
        _selectedItem.Select();
        _gameSettings.SetCardCount(signal.CardCount);
    }

    [Inject]
    public void Init(GameSettings gameSettings, SignalBus signalBus, DiContainer container)
    {
        _gameSettings = gameSettings;
        _signalBus = signalBus;
        _container = container;
    }
}