using System.Collections;
using UnityEngine;

namespace Novena.Helpers
{
  public class CoroutineWithDataHelper
  {
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;
    public CoroutineWithDataHelper(MonoBehaviour owner, IEnumerator target) {
      this.target = target;
      this.coroutine = owner.StartCoroutine(Run());
    }
 
    private IEnumerator Run() {
      while(target.MoveNext()) {
        result = target.Current;
        yield return result;
      }
    }
  }
}