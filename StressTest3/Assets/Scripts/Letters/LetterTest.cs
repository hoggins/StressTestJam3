using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Letters
{
  public class LetterTest : MonoBehaviour
  {
    public InputField Input;
    public Text Output;
    private List<string> _db;

    private void Awake()
    {
      _db = LetterCore.LoadDb();
    }

    public void OnSubmit()
    {
      var letters = Input.text;

      var lSet = new HashSet<char>(letters);
      var pick = _db.Where(w => lSet.IsSupersetOf(w.AsEnumerable())).ToList();
      var str = string.Join("\n", pick);
      Output.text = str;
    }
  }
}