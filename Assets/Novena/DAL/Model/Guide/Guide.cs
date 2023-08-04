namespace Novena.DAL.Model.Guide
{
  [System.Serializable]
  public class Guide
  {
    public int Id;
    public string Name;
    public TranslatedContent[] TranslatedContents;
    public Map[] Maps;
  }
  
  
  [System.Serializable]
  public struct Map
  {
    public int Id;
    public string Name;
    public string ImagePath;
    public string ImageTimestamp;
    public int ImageSize;
  }
}