using System;
using System.Collections;
using System.Net;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Scripts.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Scripts.Utility
{
  public class AssetsFileLoader : MonoBehaviour
  {
    public static IEnumerator LoadSprite(string path, Image image)
    {
      UnityWebRequest webRequest;
      string filepath = "";
      Texture2D texture;

      if (Application.platform == RuntimePlatform.WindowsEditor)
      {
        filepath = "file:///" + path;
      }
      if (Application.platform == RuntimePlatform.Android)
      {
        filepath = "file://" + path;
      }
      if (Application.platform == RuntimePlatform.WindowsPlayer)
      {
        filepath = "file:///" + path;
      }
      Debug.Log("<color=yellow>ImageLoader LoadSprite Image path:</color> " + filepath);

      using (webRequest = UnityWebRequestTexture.GetTexture(filepath))
      {
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
          Debug.Log(webRequest.error);
        }
        else
        {
          Debug.Log("Loaded");
          texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
          texture.filterMode = FilterMode.Bilinear;
          texture.Compress(false);
          yield return new WaitForEndOfFrame();
          float aspectRatio = (float)texture.width / (float)texture.height;
          yield return new WaitForEndOfFrame();
          try
          {
            image.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
          }
          catch
          {
            Debug.Log("<color=yellow>No Aspect Ratio Component</color>");
          }

          image.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0f, 0f));
          
          yield return new WaitForEndOfFrame();
          webRequest.Dispose();
        }
      }
    }
    
    public static IEnumerator LoadAudio(string path, Action<AudioClip> onAudioLoaded)
    {
      UnityWebRequest webRequest;
      string filepath = "file:///" + path;
      
      using (webRequest = UnityWebRequestMultimedia.GetAudioClip(filepath, AudioType.OGGVORBIS))
      {
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
          Debug.Log(webRequest.error);
        }
        else
        {
          AudioClip audioclip = DownloadHandlerAudioClip.GetContent(webRequest);
          onAudioLoaded(audioclip);
        }
      }
    }
    
    public static void LoadTexture2D(string texturePath, RawImage rawImage, FilterMode filterMode = FilterMode.Bilinear)
    {
      Uri fileUri = new Uri(texturePath);
      WebClient client = new WebClient();
      byte[] raw = client.DownloadData(fileUri);
      Texture2D tx = new Texture2D(1, 1);
      tx.filterMode = filterMode;
      tx.LoadImage(raw);
      float aspectRatio = (float)tx.width / (float)tx.height;
      try
      {
        rawImage.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
      }
      catch
      {
        Debug.Log("LoadTexture2D: <color=yellow>No Aspect Ratio Component</color>");
      }
      rawImage.texture = tx;
    }

    public static async UniTask LoadTextureAsync(string texturePath, RawImage rawImage, FilterMode filterMode = FilterMode.Bilinear)
    {
      var uwr = new UnityWebRequest(texturePath);

      uwr.downloadHandler = new DownloadHandlerBuffer();

      var response = await uwr.SendWebRequest();

      if (response.result != UnityWebRequest.Result.Success)
      {
        Debug.LogError("ImageLoader GetTexture: " + response.error);
      }

      if (response.isDone)
      {
        if (response.result == UnityWebRequest.Result.Success)
        {         
          //Create texture and return it
          Texture2D tex = new Texture2D(1, 1);
          
          tex.LoadImage(response.downloadHandler.data, true);
          SetAspectRatio(rawImage, tex.width, tex.height);

          rawImage.texture = tex;
        }
      }
    } 

    private static void SetAspectRatio(RawImage rawImage, float width, float height)
		{
      float aspectRatio = (float)width / (float)height;
      try
      {
        rawImage.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
      }
      catch
      {
        Debug.Log("LoadTexture2D: <color=yellow>No Aspect Ratio Component</color>");
      }
    }
  }
}