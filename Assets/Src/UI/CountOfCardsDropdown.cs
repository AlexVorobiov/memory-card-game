using TMPro;
using UnityEngine;
using Zenject;

public class CountOfCardsDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private GameSettings _gameSettings;

    [Inject]
    public void Construct(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
    }
    
    private void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(OnValueChanged);
        _dropdown.ClearOptions();
        _dropdown.AddOptions(_gameSettings.AvailableCardCounts.ConvertAll(i => i.ToString()));
    }
    
    private void OnValueChanged(int value)
    {
        _gameSettings.SetCardCount(_gameSettings.AvailableCardCounts[value]);
    }
}
