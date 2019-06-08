using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Letters
{
  public class LetterCore
  {
    public static readonly List<char> BaseLetters = new List<char>
    {
      'э', 'щ', 'ф', 'ы', 'ш', 'ч', 'г', 'б', 'у', 'д', 'в', 'с', 'т', 'р', 'е', 'а', 'о', 'и', 'н', 'к', 'л', 'п', 'м',
      'ь', 'з', 'я', 'ц', 'ж', 'х', 'й', 'ю', 'ъ'
    };

    private static readonly Random _random = new Random(1);

    private static List<string> _words;

    public static void Init()
    {
      if (_words != null)
        return;
      _words = LoadDb();
    }
    
    public static List<string> LoadDb()
    {
      var dat = (TextAsset) Resources.Load("db/zdb-win");
      return dat.text.Split('\n').Select(w => w.Trim()).ToList();
    }

    public static List<char> PickLetters(int targetLetters)
    {
      var res = new List<char>();
      for (var index = 0; index < BaseLetters.Count && res.Count < targetLetters; index++)
      {
        var letter = BaseLetters[index];
        var chance = (index + 1d) / (BaseLetters.Count - res.Count);
        if (_random.NextDouble() < chance)
        {
          res.Add(letter.ToString().ToUpper()[0]);
        }
      }

      return res;
    }

    public static int GetWordLength(string letters)
    {
      var isWord = _words.Any(w => string.Compare(w, letters, StringComparison.InvariantCultureIgnoreCase) == 0);
      return isWord ? letters.Length : 0;
    }
  }
}