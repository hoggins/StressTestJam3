using System;
using Characters;
using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
  public class BattleHud : MonoBehaviour
  {
    public Image PlayerHp;
    public GameObject GameLost;
    public CanvasGroup GameLostCanvasGroup;

    public Button RetryButton;
    public Button LeaveButton;


    private bool _lost = false;

    void Awake()
    {
      Input.simulateMouseWithTouches = true;
      RetryButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene"));
      LeaveButton.onClick.AddListener(() =>
      {
        AudioController.Instance.StopMusic();
        SceneManager.LoadScene("StartMenu");
      });
    }

    private void Update()
    {
      PlayerHp.fillAmount = Player.Instance.Hp/ Player.Instance.MaxHp;


      if(Player.Instance.Hp <= 0f)
        Lose();

      if (_lost)
      {
        GameLostCanvasGroup.alpha = Mathf.Lerp(GameLostCanvasGroup.alpha, 1f, Time.deltaTime);
        if (GameLostCanvasGroup.alpha > 0.8f)
        {
          if (Input.GetMouseButtonDown(0))
          {
            SceneManager.LoadScene("MainScene");
          }
        }
      }
    }


    private void Lose()
    {
      if(_lost)
        return;

      _lost = true;
      GameLostCanvasGroup.alpha = 0f;

      AudioController.Instance.PlayDeathHero();
      GameLost.gameObject.SetActive(true);
    }

  }
}