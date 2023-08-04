using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Profiling;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR

using UnityEditor;

#endif
#if UNITY_5
using UnityEngine.Profiling;
#endif

using PDollarGestureRecognizer;

//-----------------------------------------------------------------------------------------------------
public class StatsMan : MonoBehaviour
{
  public Color tx_Color = Color.white;
  private StringBuilder tx;
  public Text gui;

  private float updateInterval = 1.0f;
  private float lastInterval; // Last interval end time
  private float frames = 0; // Frames over current interval

  private float framesavtick = 0;
  private float framesav = 0.0f;

  //

  //public Transform gestureOnScreenPrefab;

  private List<Gesture> trainingSet = new List<Gesture>();

  private List<Point> points = new List<Point>();
  private int strokeId = -1;

  private Vector3 virtualKeyPosition = Vector2.zero;
  private Rect drawArea;

  private RuntimePlatform platform;

  private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
  private LineRenderer currentGestureLineRenderer;

  //GUI
  private string message;

  private bool recognized = true;

  private float gestureTime = 0.0f;

  // Use this for initialization
  private void Start()
  {
    gameObject.GetComponent<Canvas>().enabled = true;
    lastInterval = Time.realtimeSinceStartup;
    frames = 0;
    framesav = 0;
    tx = new StringBuilder();
    tx.Capacity = 200;
    Screen.sleepTimeout = SleepTimeout.NeverSleep;

    platform = Application.platform;
    drawArea = new Rect(0, 0, Screen.width, Screen.height);

    //Load pre-made gestures
    TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
    foreach (TextAsset gestureXml in gesturesXml)
      trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

    //Load user custom gestures
    string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
    foreach (string filePath in filePaths)
      trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
  }

  private void OnDisable()
  {
  }

  public void Recognize()
  {
    Debug.Log(points.Count);

    Gesture candidate = new Gesture(points.ToArray());

    Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());

    message = gestureResult.GestureClass + " " + gestureResult.Score;
    Debug.Log(message);

    if (gestureResult.GestureClass == "line" && gestureResult.Score > 0.5f)
    {
      Debug.Log("SHOW");
      if (gameObject.GetComponent<Canvas>().enabled)
      {
        gameObject.GetComponent<Canvas>().enabled = false;
      }
      else
      {
        gameObject.GetComponent<Canvas>().enabled = true;
      }
    }

    if (gestureResult.GestureClass == "null" && gestureResult.Score > 0.6f)
    {
      Application.Quit();
    }
  }

  // Update is called once per frame
  private void Update()
  {
    //------------------------------------------
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
        gestureTime = 0.0f;

        if (recognized)
        {
          recognized = false;
          strokeId = -1;

          points.Clear();
        }

        ++strokeId;

        //Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
        //currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

        //gestureLinesRenderer.Add(currentGestureLineRenderer);
      }

      if (Input.GetMouseButton(0))
      {
        points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));
      }

      if (Input.GetMouseButtonUp(0))
      {
        //Recognize();
      }
    }

    if (recognized == false)
    {
      gestureTime += Time.deltaTime;

      //Debug.Log("gestureTime:" + gestureTime);
      if (gestureTime > 1f)
      {
        gestureTime = 0;
        recognized = true;

        Debug.Log("points.Count:" + points.Count);

        if (points.Count > 5)
        {
          Recognize();
        }
        else
        {
          points.Clear();
        }
      }
    }

    //------------------------------------------

    ++frames;

    var timeNow = Time.realtimeSinceStartup;

    if (timeNow > lastInterval + updateInterval)
    {
      float fps = frames / (timeNow - lastInterval);
      float ms = 1000.0f / Mathf.Max(fps, 0.00001f);

      ++framesavtick;
      framesav += fps;
      float fpsav = framesav / framesavtick;

      tx.Length = 0;

      string screen = gameObject.GetComponent<RectTransform>().sizeDelta.x + " : " + gameObject.GetComponent<RectTransform>().sizeDelta.y + " : " + gameObject.GetComponent<RectTransform>().localScale.x;

      tx.AppendFormat("Time : {0} ms     Current FPS: {1}     AvgFPS: {2}\nGPU memory : {3}    Sys Memory : {4}\nScreen: {5}\n ", ms, fps, fpsav, SystemInfo.graphicsMemorySize, SystemInfo.systemMemorySize, screen)

      .AppendFormat("TotalAllocatedMemory : {0}mb\nTotalReservedMemory : {1}mb\nTotalUnusedReservedMemory : {2}mb",
      Profiler.GetTotalAllocatedMemoryLong() / 1048576,
      Profiler.GetTotalReservedMemoryLong() / 1048576,
      Profiler.GetTotalUnusedReservedMemoryLong() / 1048576
      );

#if UNITY_EDITOR
      tx.AppendFormat("\nDrawCalls : {0}\nUsed Texture Memory : {1}\nrenderedTextureCount : {2}", UnityStats.drawCalls, UnityStats.usedTextureMemorySize / 1048576, UnityStats.usedTextureCount);
#endif

      gui.text = tx.ToString();
      frames = 0;
      lastInterval = timeNow;
    }
  }
}