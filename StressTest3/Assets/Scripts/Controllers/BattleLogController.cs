using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
  public class BattleLogController : MonoBehaviour
  {
    public static BattleLogController Instance { get; private set; }
    
    public static int MessagesLimit = 1;
    public Text Text;
    private LinkedList<string> Messages = new LinkedList<string>();

    public bool IsActive => Text.gameObject.activeSelf;

    public void PushMessage(string msg)
    {
      Messages.AddLast(msg);
      
      if (Messages.Count > MessagesLimit)
        Messages.RemoveFirst();

      Text.text = string.Join(Environment.NewLine, Messages);
    }
    
    private void Awake()
    {
      Instance = this;
    }

    private void OnDestroy()
    {
      Instance = null;
    }

    public void ToggleText()
    {
      Text.gameObject.SetActive(!Text.gameObject.activeSelf);
    }
  }
}