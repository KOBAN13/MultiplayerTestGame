using System;
using Cysharp.Threading.Tasks;
using R3;
using Services.SceneManagement;
using Services.SceneManagement.Enums;

namespace Services.Interface
{
    public interface IScenesService
    {
        UniTask LoadScene(SceneGroup sceneGroup, IProgress<float> progress, TypeScene typeScene);
        void Construct(SceneLoader loader, SceneResources resources);
        Observable<Unit> SceneIsLoad { get; }
    }
}