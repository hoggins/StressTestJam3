using System;
using Controllers;
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
      AudioController.Instance?.PlayMusicMenu();
      PlayButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
      CharacterButton?.onClick?.AddListener(() => SceneManager.LoadScene("CharacterScene"));
      ExitButton?.onClick?.AddListener(() => Application.Quit());
    }

    private void Start()
    {
      AudioController.Instance.PlayMusicMenu();
    }

    private void OnDisable()
    {
      AudioController.Instance.StopMusicMenu();
      PlayButton.onClick.RemoveAllListeners();
      CharacterButton?.onClick?.RemoveAllListeners();
      ExitButton?.onClick?.RemoveAllListeners();
    }
  }
}