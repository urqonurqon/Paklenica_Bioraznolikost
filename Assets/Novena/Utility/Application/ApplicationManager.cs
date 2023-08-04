namespace Novena.Utility.Application
{
  public static class ApplicationManager
  {
    public static void RestartApplication()
    {
      System.Diagnostics.Process.Start(UnityEngine.Application.dataPath.Replace("_Data", ".exe")); //new program
      UnityEngine.Application.Quit();
    }
  }
}
