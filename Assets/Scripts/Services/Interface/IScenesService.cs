using System;
using Cysharp.Threading.Tasks;
using Services.SceneManagement;
using Services.SceneManagement.Enums;

namespace Services.Interface
{
    public interface IScenesService
    {
        UniTask LoadScene(SceneGroup sceneGroup, IProgress<float> progress, TypeScene typeScene);
    }
}