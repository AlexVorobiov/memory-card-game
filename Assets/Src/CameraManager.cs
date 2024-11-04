using DG.Tweening;
using UnityEngine;
using Zenject;

public class CameraManager : MonoBehaviour
{
    private SignalBus _signalBus;
    private GameSettings _gameSettings;
    private Camera _camera;

    private const int DefaultCameraFoV = 70;

    [Inject]
    public void Construct(SignalBus signalBus, GameSettings gameSettings)
    {
        _signalBus = signalBus;
        _gameSettings = gameSettings;
    }
    
    private void Start()
    {
        _signalBus.Subscribe<StartGameSignal>(OnStartGame);
        _signalBus.Subscribe<ShowMainMenuSignal>(OnShowMainMenu);
        _camera = GetComponent<Camera>();
    }

    private void OnShowMainMenu()
    {
        const int duration = 1;
        DOTween.To(() => _camera.fieldOfView, x => _camera.fieldOfView = x, DefaultCameraFoV, duration);
    }

    private void OnStartGame()
    {
        var targetFoV = _gameSettings.CameraFoVForCards[_gameSettings.TotalCards];
        const int duration = 1;
        DOTween.To(() => _camera.fieldOfView, x => _camera.fieldOfView = x, targetFoV, duration);
    }
}
