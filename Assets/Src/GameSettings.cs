using System.Collections.Generic;
using Zenject;

public class GameSettings: IInitializable
{
    public readonly List<int> AvailableCardCounts = new()
    {
        16, 18, 20, 24, 28, 30, 32, 36, 40, 42, 48
    };
    
    public readonly Dictionary<int, int> CameraFoVForCards = new()
    {
        { 16, 60 },
        { 18, 60 },
        { 20, 60 },
        { 24, 60 },
        { 28, 65 },
        { 30, 60 },
        { 32, 72 },
        { 36, 70 },
        { 40, 71 },
        { 42, 71 },
        { 48, 72 },
    };

    private readonly Dictionary<int, List<(int Rows, int Cols)>> _configurations = new()
    {
        { 16, new() { (4, 4) } },
        { 18, new() { (6, 3) } },
        { 20, new() { (5, 4) } },
        { 24, new() { (6, 4) } },
        { 28, new() { (7, 4) } },
        { 30, new() { (6, 5) } },
        { 32, new() { (8, 4) } },
        { 36, new() { (6, 6) } },
        { 40, new() { (8, 5) } },
        { 42, new() { (7, 6) } },
        { 48, new() { (8, 6) } },
    };

    public readonly int DelayBeforeShowingCards;

    public GameSettings([Inject(Id = "DelayBeforeShowingCards")] int delayBeforeShowingCards)
    {
        DelayBeforeShowingCards = delayBeforeShowingCards;
    }
    
    public void Initialize()
    {
        Rows = 4;
        Columns = 4;
        TotalCards = 16;
    }

    public int TotalCards { get; set; }
    public int Rows { get; private set; }
    public int Columns {get; private set; }
    
    public void SetCardCount(int count)
    {
        TotalCards = count;
        Rows = _configurations[count][0].Rows;
        Columns = _configurations[count][0].Cols;
    }
}
