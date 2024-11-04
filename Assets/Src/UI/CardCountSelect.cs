using TMPro;
using UnityEngine;
using Zenject;

public class CardCountSelect : MonoBehaviour
{
    [SerializeField] private GameObject selectedIcon;
    [SerializeField] private GameObject focusedIcon;
    [SerializeField] private TMP_Text cardCountText;
    
    private SignalBus _signalBus;
    
    public int CardCount { get; private set; }
    
    public void SetCardCount(int count)
    {
        CardCount = count;
        cardCountText.text = count.ToString();
    }

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    
    public void Select()
    {
        selectedIcon.SetActive(true);
        focusedIcon.SetActive(true);
    }
    
    public void UnSelect()
    {
        selectedIcon.SetActive(false);
        focusedIcon.SetActive(false);
    }

    public void OnClick()
    {
        Select();
        _signalBus.Fire(new CardCountSelectedSignal() { CardCount = CardCount });
    }
}
