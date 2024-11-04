using Flexalon;
using UnityEngine;
using Zenject;

public class BoardManager : MonoBehaviour
{
    private SignalBus _signalBus;
    
    private FlexalonGridLayout _gridLayout;
    
    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    
    private void Start()
    {
        _signalBus.Subscribe<BoardSizeSignal>(OnBoardSizeSignal);
        _gridLayout = GetComponent<FlexalonGridLayout>();
    }

    private void OnBoardSizeSignal(BoardSizeSignal signal)
    {
        _gridLayout.Columns = (uint)signal.Columns;
        _gridLayout.Rows = (uint)signal.Rows;
    }
}
