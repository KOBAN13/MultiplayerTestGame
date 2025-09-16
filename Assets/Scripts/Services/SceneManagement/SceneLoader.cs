using Cysharp.Threading.Tasks;
using R3;
using Services.Interface;
using Services.SceneManagement.Enums;
using UI.View;
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

        private IScenesService _scenesService;
        private IScreenService _screenService;
        private float _targetProgress;
        
        private readonly ReactiveProperty<bool> _isLoading = new();
        private readonly ReactiveProperty<float> _progress = new();
        private readonly LoadingProgress _loadingProgress = new();
        
        public ReadOnlyReactiveProperty<bool> IsLoading => _isLoading;
        public ReadOnlyReactiveProperty<float> FillAmount => _progress;
        
        [Inject]
        private void Construct(SceneResources sceneResources, IScenesService scenesService, IScreenService screenService)
        {
            _scenesService = scenesService;
            _screenService = screenService;
            _scenesService.Construct(sceneResources);
        }
        
        public async UniTask LoadScene(TypeScene typeScene)
        {
            ChangeParameters();
            _screenService.OpenLoading<LoadingScreen>();
            _loadingProgress.Progressed += value => _targetProgress = Mathf.Max(value, _targetProgress);
            _isLoading.Value = true;
            await _scenesService.LoadScene(_sceneGroup, _loadingProgress, typeScene);
            _isLoading.Value = false;
        }
        
        private void OnEnable()
        {
            _scenesService.SceneIsLoad.Subscribe(_ => _screenService.Close());
        }

        private void Update()
        {
            if(_isLoading.Value == false) 
                return;

            var currentFillAmount = FillAmount.CurrentValue;
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