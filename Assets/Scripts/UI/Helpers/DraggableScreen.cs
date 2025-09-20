using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Helpers
{
    public class DraggableScreen : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Vector2 _offset;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, _canvas.worldCamera, out _offset);
            
            Debug.Log("OnBeginDrag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, _canvas.worldCamera, out var localPosition))
            {
                Debug.Log("OnDrag");
                
                _rectTransform.anchoredPosition = localPosition - _offset;
            }
        }
    }
}