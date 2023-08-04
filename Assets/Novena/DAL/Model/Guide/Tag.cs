namespace Novena.DAL.Model.Guide
{
  [System.Serializable]
  public class Tag
  {
    public int Id;
    public string Title;
    public int TagCategoryId;
  }
  
  [System.Serializable]
  public class TagCategorie
  {
    public int Id;
    public string Title;
    public Tag[] Tags;
  }
}