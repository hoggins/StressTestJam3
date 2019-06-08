using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
  public class StartMenu : MonoBehaviour
  {
    public Button PlayButton;

    private void OnEnable()
    {
      PlayButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
    }

    private void OnDisable()
    {
      PlayButton.onClick.RemoveAllListeners();
    }
  }
}