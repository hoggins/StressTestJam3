using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
  public class LoadingScene : MonoBehaviour
  {
    private IEnumerator Start()
    {
      yield return new WaitForSeconds(2);
      SceneManager.LoadScene("StartMenu");
    }
  }
}