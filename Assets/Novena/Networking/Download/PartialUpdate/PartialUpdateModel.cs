using System;

namespace Novena.Networking.Download.PartialUpdate
{
  [Serializable]
  public class PartialUpdateData
  {
    public PartialUpdateModel[] partialUpdateData;
  }
  
  [Serializable]
  public class PartialUpdateModel
  {
    public string Path;
    public string Timestamp;
  }
}