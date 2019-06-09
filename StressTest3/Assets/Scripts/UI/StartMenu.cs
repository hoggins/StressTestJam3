using System;
using Controllers;
using Model;
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

    public GameObject MenuRoot;
    public GameObject SkillUiRoot;
    
    private void OnEnable()
    {
      GameBalance.Init();
      
      AudioController.Instance?.PlayMusicMenu();
      PlayButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
      CharacterButton?.onClick?.AddListener(() => ToggleSkillUi());
      ExitButton?.onClick?.AddListener(() => Application.Quit());
    }

    private void ToggleSkillUi()
    {
      MenuRoot.SetActive(!MenuRoot.activeSelf);
      SkillUiRoot.SetActive(!SkillUiRoot.activeSelf);
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