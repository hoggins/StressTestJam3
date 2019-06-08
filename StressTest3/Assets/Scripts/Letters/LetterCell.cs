using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Letters
{
  public class LetterCell : MonoBehaviour
  {
    public Text Text;
    [NonSerialized] public Letter Letter;

    public int Index;

    private void Awake()
    {
      if (!Text)
        Text = gameObject.GetComponentInChildren<Text>();
    }

    public void SetLetter(Letter letter)
    {
      this.Letter = letter;
      if (this.Letter == null)
        Text.text = "";
      else if (this.Letter.Value.HasValue)
        Text.text = this.Letter.Value.Value.ToString();
    }
  }
}