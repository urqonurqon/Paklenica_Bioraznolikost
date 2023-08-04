using System.Collections.Generic;
using System.Linq;

namespace Novena.Utility.Map
{
  public static class Maps
  {
    public static List<Novena.DAL.Model.Guide.Map> MapList;

    public static Novena.DAL.Model.Guide.Map GetMapById(int id)
    {
      return MapList.FirstOrDefault(map => map.Id == id);
    }
    
    public static Novena.DAL.Model.Guide.Map GetMapByName(string name)
    {
      return MapList.FirstOrDefault(map => map.Name == name);
    }
  }
}