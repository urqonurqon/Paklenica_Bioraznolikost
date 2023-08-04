using System.Collections.Generic;
using System.IO;
using Doozy.Engine.UI;
using PDollarGestureRecognizer;
using UnityEngine;

namespace Novena.Admin
{
  public class AdminGestureLogin : MonoBehaviour
  {
    public bool EnableGestureDraw = false;
    public Transform gestureOnScreenPrefab;

    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();
    private int strokeId = -1;

    private Vector3 virtualKeyPosition = Vector2.zero;
    private Rect drawArea;

    private RuntimePlatform platform;
    private int vertexCount = 0;

    private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
    private LineRenderer currentGestureLineRenderer;

    private bool recognized;

    private void Start()
    {
      //Load user custom gestures
      string[] filePaths = Directory.GetFiles(Application.streamingAssetsPath + "/Gestures", "*.xml");
      foreach (string filePath in filePaths)
        trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
    }

    void Update()
    {
      if (EnableGestureDraw == false) return;
      
      platform = Application.platform;
      drawArea = new Rect(0, 200, Screen.width, Screen.height-200);

      if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
      {
        if (Input.touchCount > 0)
        {
          virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        }
      }
      else
      {
        if (Input.GetMouseButton(0))
        {
          virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        }
      }

      if (drawArea.Contains(virtualKeyPosition))
      {
        if (Input.GetMouseButtonDown(0))
        {
          if (recognized)
          {
            ClearLines();
          }

          ++strokeId;

          Transform tmpGesture =
            Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
          currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

          gestureLinesRenderer.Add(currentGestureLineRenderer);

          vertexCount = 0;
        }

        if (Input.GetMouseButton(0))
        {
          points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

          //currentGestureLineRenderer.SetVertexCount(++vertexCount);
          if (currentGestureLineRenderer != null)
          {
            currentGestureLineRenderer.positionCount = ++vertexCount;
            
            currentGestureLineRenderer.SetPosition(vertexCount - 1,
              Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));

          }
          
        }
      }
    }

    public void OnLoginButtonClick()
    {
      recognized = true;

      Gesture candidate = new Gesture(points.ToArray());
      Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
      
      Debug.LogError("Gesture score: " + gestureResult.Score);

      if (gestureResult.Score >= 0.80f)
      {
        gameObject.GetComponent<UIView>().Hide();
        
        points.Clear();

        foreach (LineRenderer lineRenderer in gestureLinesRenderer)
        {
          lineRenderer.positionCount = 0;
          Destroy(lineRenderer.gameObject);
        }

        gestureLinesRenderer.Clear();
      }
    }

    public void ClearLines()
    {
      recognized = false;
      strokeId = -1;

      points.Clear();

      foreach (LineRenderer lineRenderer in gestureLinesRenderer)
      {
        lineRenderer.positionCount = 0;
        Destroy(lineRenderer.gameObject);
      }

      gestureLinesRenderer.Clear();
    }
  }
}