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
    
    public static StartMenu Instance { get; private set; }

    private void Awake()
    {
      Instance = this;
    }

    private void OnDestroy()
    {
      Instance = null;
    }
    
    private void OnEnable()
    {
      GameBalance.Init();
      
      AudioController.Instance?.PlayMusicMenu();
      PlayButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
      CharacterButton?.onClick?.AddListener(() => ToggleSkillUi());
      ExitButton?.onClick?.AddListener(() => Application.Quit());
    }

    public void ToggleSkillUi()
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