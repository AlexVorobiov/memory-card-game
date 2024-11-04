using Src.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UniRx;
using Zenject;

namespace Src.MVVM.Views
{
    public class RoundView : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeLeftText;

        private RoundViewModel _viewModel;

        [Inject]
        public void Construct(RoundViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.TimeLeft.Subscribe(time =>
            {
                timeLeftText.text = time;
            }).AddTo(this);
        }
    }
}