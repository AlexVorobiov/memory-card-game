using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class TopMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private TMP_Text achievedScoreText;
    [SerializeField] private TMP_Text totalScoreText;

    private SignalBus _signalBus;
    private GameSettings _gameSettings;

    [Inject]
    public void Construct(SignalBus signalBus, GameSettings gameSettings)
    {
        _signalBus = signalBus;
        _gameSettings = gameSettings;
    }

    private void Start()
    {
        _signalBus.Subscribe<RoundStartSignal>(OnStartGame);
        _signalBus.Subscribe<CardsMatchedSignal>(OnCardsMatchedSignal);
        _signalBus.Subscribe<ShowMainMenuSignal>(OnMainMenuShow);
    }

    private void OnMainMenuShow()
    {
        Hide();
        achievedScoreText.text = "0";   
    }

    private void OnStartGame()
    {
        achievedScoreText.text = "0";
        totalScoreText.text = (_gameSettings.TotalCards / 2).ToString();
        Show();
    }

    public void Show()
    {
        scorePanel.transform.DOLocalMoveY(0, 1).SetEase(Ease.OutBounce);
    }
    
    public void Hide()
    {
        scorePanel.transform.DOLocalMoveY(50, 1).SetEase(Ease.OutBounce);
    }

    private void OnCardsMatchedSignal(CardsMatchedSignal signal)
    {
        achievedScoreText.text = signal.CurrentMatches.ToString();
    }
    
    public void OnGameMenuButtonClicked()
    {
        _signalBus.Fire(new ShowGameMenuSignal());
    }
}