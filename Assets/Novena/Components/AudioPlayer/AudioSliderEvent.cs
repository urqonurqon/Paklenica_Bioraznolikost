using UnityEngine;
using UnityEngine.EventSystems;

namespace Novena.Components.AudioPlayer
{
    public class AudioSliderEvent : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler
    {
        private Novena.Components.AudioPlayer.AudioPlayer _audioPlayer;
        private void Awake()
        {
            _audioPlayer = GetComponentInParent<Novena.Components.AudioPlayer.AudioPlayer>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _audioPlayer.SetTime();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _audioPlayer.Play();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _audioPlayer.Pause();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _audioPlayer.Pause();
            _audioPlayer.SetTime();
            _audioPlayer.Play();
        }
    }
}
