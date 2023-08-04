using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Components.VideoPlayer
{
    public class SliderEvent : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler
    {
        [SerializeField] private VideoPlayerController videoPlayerController;
        [SerializeField] private Controls controls;
        
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            videoPlayerController.PauseVideo();
            controls.ShowControls(true, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetVideoTimeValueChange();
        }
        
        private void SetVideoTimeValueChange()
        {
            videoPlayerController.SetTime();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            videoPlayerController.PlayVideo();
            controls.ShowControls(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            videoPlayerController.PauseVideo();
            SetVideoTimeValueChange();
            videoPlayerController.PlayVideo();
        }
    }
}
