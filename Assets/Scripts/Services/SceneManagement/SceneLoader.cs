using Cysharp.Threading.Tasks;
using SceneManagment;
using Services.SceneManagement.Enums;
using UniRx;
using UnityEngine;
using VContainer;

namespace Services.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private SceneGroup _sceneGroup;
        [SerializeField] private float _fadeDuration = 0.4f;
        [SerializeField] private float _minSlider = 0.05f;
        [SerializeField] private float _fillSpeed = 0.5f;

        private ScreenService _screenService;
        private float _targetProgress;
        
        private readonly ReactiveProperty<bool> _isLoading = new();
        private readonly ReactiveProperty<float> _progress = new();
        
        public IReadOnlyReactiveProperty<bool> IsLoading => _isLoading;
        public IReadOnlyReactiveProperty<float> FillAmount => _progress;
        
        [Inject]
        private void Construct(SceneResources sceneResources, ScreenService screenService)
        {
            _screenService = screenService;
            _screenService.Construct(sceneResources);
        }
        
        public async UniTask LoadScene(TypeScene typeScene)
        {
            ChangeParameters();
            LoadingProgress loadingProgress = new LoadingProgress();
            loadingProgress.Progressed += value => _targetProgress = Mathf.Max(value, _targetProgress);
            _isLoading.Value = true;
            await _screenService.LoadScene(_sceneGroup, loadingProgress, typeScene);
            _isLoading.Value = false;
        }

        private void Update()
        {
            if(_isLoading.Value == false) 
                return;

            var currentFillAmount = FillAmount.Value;
            var progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            var dynamicFillSpeed = progressDifference * _fillSpeed;

            _progress.Value = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        private void ChangeParameters()
        {
            _progress.Value = _minSlider;
            
            _targetProgress = 1f;
        }
    }
}