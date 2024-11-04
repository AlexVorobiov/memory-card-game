using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

public class CardManager
{
    private readonly CardFactory _cardFactory;
    private readonly GameSettings _gameSettings;
    private readonly SignalBus _signalBus;
    private readonly RoundManager _roundManager;
    private readonly EffectManager _effectManager;

    private List<Card> _cards = new();
    private Card _firstFlippedCard;
    private Card _secondFlippedCard;

    public CardManager(CardFactory cardFactory, GameSettings gameSettings, SignalBus signalBus,
        RoundManager roundManager, EffectManager effectManager)
    {
        _cardFactory = cardFactory;
        _gameSettings = gameSettings;
        _signalBus = signalBus;
        _roundManager = roundManager;
        _effectManager = effectManager;
        _signalBus.Subscribe<ShowMainMenuSignal>(OnMainMenuShow);
    }

    private void OnMainMenuShow()
    {
        ClearBoard();
    }

    public void Init()
    {
        ClearBoard();
        InitBoard();
        InitializeCards();
    }


    private void InitializeCards()
    {
        _cards = _cardFactory.CreateCards(_gameSettings.TotalCards);
        foreach (var card in _cards)
        {
            card.OnCardFlipped.Subscribe(OnCardFlipped).AddTo(card);
        }
    }

    public async UniTask ShowCardsBriefly()
    {
        await UniTask.Delay(_gameSettings.DelayBeforeShowingCards * 1000);
        foreach (var card in _cards)
        {
            card.Flip();
        }

        await UniTask.Delay(_roundManager.MemorizeTime * 1000);

        foreach (var card in _cards)
        {
            card.Flip();
        }
    }

    private void ClearBoard()
    {
        foreach (var card in _cards)
        {
            card.Dispose();
        }
        _cards.Clear();
    }

    private void InitBoard()
    {
        _signalBus.Fire(new BoardSizeSignal()
        {
            Columns = _gameSettings.Columns,
            Rows = _gameSettings.Rows,
            TotalCards = _gameSettings.TotalCards
        });
    }

    private void OnCardFlipped(Card flippedCard)
    {
        if (!_roundManager.IsRoundActive)
            return;
        if (_firstFlippedCard == null)
        {
            _firstFlippedCard = flippedCard;
        }
        else if (_secondFlippedCard == null)
        {
            _secondFlippedCard = flippedCard;
            CheckMatch().Forget();
        }

        if (_firstFlippedCard && _secondFlippedCard)
            _signalBus.Fire<CardsSelectSignal>();
    }

    private async UniTaskVoid CheckMatch()
    {
        await UniTask.Delay(500);
        if (_firstFlippedCard.CardId == _secondFlippedCard.CardId)
        {
            _firstFlippedCard.SetMatched();
            _secondFlippedCard.SetMatched();
            _roundManager.MatchFound();
            _effectManager.PlayEffect1(_firstFlippedCard.transform.position);
            _effectManager.PlayEffect2(_secondFlippedCard.transform.position);
        }
        else
        {
            _firstFlippedCard.Flip();
            _secondFlippedCard.Flip();
            _signalBus.Fire<CardsDidNotMatchSignal>();
        }

        _firstFlippedCard = null;
        _secondFlippedCard = null;
    }

    public void OnHintButtonClicked()
    {
        ShowCardsBriefly().Forget();
    }
}