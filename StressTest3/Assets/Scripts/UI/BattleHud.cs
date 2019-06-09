using System;
using System.Collections;
using System.Linq;
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

    public CanvasGroup BossGroup;
    public Text BossText;

    public Button RetryButton;
    public Button LeaveButton;


    private bool _lost = false;
    private bool _boss = false;
    public static BattleHud Instance;

    void Awake()
    {
      Instance = this;
      Input.simulateMouseWithTouches = true;
      BossGroup.gameObject.SetActive(false);
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

    public void Boss()
    {
      if (_boss)
        return;
      _boss = true;
      BossGroup.alpha = 0f;
      BossGroup.gameObject.SetActive(true);
      BossText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 177  );

      StartCoroutine(BossCoroutine());
    }


    public AnimationCurve BossCurve = AnimationCurve.EaseInOut(0,0, 1, 1);

    private IEnumerator BossCoroutine()
    {
      var t = 0f;
      var appearDur = 3f;
      var initPosition = BossText.rectTransform.anchoredPosition;

      while (t <= 1f)
      {
        BossGroup.alpha = Mathf.Lerp(BossGroup.alpha, 1f, Time.deltaTime*5);
        BossText.rectTransform.anchoredPosition =
          Vector3.Lerp(initPosition, new Vector3(0, initPosition.y, 0), BossCurve.Evaluate(t));
        t += Time.deltaTime / appearDur;
        yield return null;
      }

      t = 0f;
      while (t <= 1f)
      {
        BossGroup.alpha = Mathf.Lerp(BossGroup.alpha, 0f, Time.deltaTime*5);
        t += Time.deltaTime;
        yield return null;
      }

      BossGroup.gameObject.SetActive(false);

      _boss = false;
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