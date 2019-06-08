using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
  public class StartMenu : MonoBehaviour
  {
    public Button PlayButton;
    public Button CharacterButton;
    public Button ExitButton;

    private void OnEnable()
    {
      PlayButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
      PlayButton.onClick.AddListener(() => SceneManager.LoadScene("CharacterScene"));
      PlayButton.onClick.AddListener(() => Application.Quit());
    }

    private void OnDisable()
    {
      PlayButton.onClick.RemoveAllListeners();
      CharacterButton.onClick.RemoveAllListeners();
      ExitButton.onClick.RemoveAllListeners();
    }
  }
}