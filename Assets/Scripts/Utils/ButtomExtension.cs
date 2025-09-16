using R3;
using UnityEngine.UI;

namespace Utils
{
    public static class ButtonExtension
    {
        public static Observable<Unit> OnClickAsObservable(this Button button)
        {
            return button.onClick.AsObservable(button.GetDestroyCancellationToken());
        }
    }
}м