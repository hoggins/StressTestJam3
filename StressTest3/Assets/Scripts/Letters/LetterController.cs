using System.Collections.Generic;
using System.Linq;

namespace Letters
{
  public class LetterController
  {
    public bool IsComplete => _nextCell == _letters.Count(l => l.gameObject.activeSelf);
    public static LetterController Instance;

    private List<LetterCell> _letters;

    private int _nextCell;

    public LetterController(List<LetterCell> letters)
    {
      _letters = letters.OrderBy(l=>l.Index).ToList();
      Instance = this;
    }

    public void InputNext(Letter letter)
    {
      _letters[_nextCell++].SetLetter(letter);
    }

    public List<Letter> GetValue()
    {
      return _letters.Where(l=>l.Letter != null).Select(l => l.Letter).ToList();
    }

    public void Reset()
    {
      _nextCell = 0;
      foreach (var letter in _letters)
      {
        letter.SetLetter(null);
      }
    }
  }
}