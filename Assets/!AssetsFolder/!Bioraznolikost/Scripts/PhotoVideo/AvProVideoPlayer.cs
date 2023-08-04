using Novena.DAL;
using RenderHeads.Media.AVProVideo;
using System;
using UnityEngine;

namespace Novena.Components.AvProVideoPlayer {
	public class AvProVideoPlayer : MonoBehaviour {
		[SerializeField] private MediaPlayer _mediaPlayer;

		public void LoadVideo(string path)
		{
			MediaPath mediaPath = new MediaPath(path, MediaPathType.AbsolutePathOrURL);
			_mediaPlayer.OpenMedia(mediaPath, false);
		}

		private string GetVideoPath()
		{
			string output = string.Empty;
			var media = Data.Theme.GetMediaByName("Video");

			if (media != null)
			{
				output = media.FullPath;
			}

			return output;
		}

		private void OnEnable()
		{
			try
			{

				LoadVideo(GetVideoPath());
				PlayVideo();
			}
			catch (Exception e)
			{
				Debug.Log(e);
			}
		}
		private void OnDisable()
		{
			ResetPlayer();
		}

		public void PlayVideo()
		{
			_mediaPlayer.Play();
		}

		public void ResetPlayer()
		{
			_mediaPlayer.CloseMedia();
		}
	}
}