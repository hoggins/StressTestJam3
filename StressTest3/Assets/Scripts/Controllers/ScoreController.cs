using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
  public class ScoreController : MonoBehaviour
  {
    public static ScoreController Instance { get; private set; }
    
    public double WalkDist { get; private set; }
    public int LettersUsed { get; private set; }
    public int WordsUsed { get; private set; }
    public int LettersWaseted { get; private set; }

    public Text DistLabel;
    public Text LettersUsedLabel;
    public Text LettersWasetedLabel;

    private void Awake()
    {
      Instance = this;
    }

    private void OnDestroy()
    {
      Instance = null;
    }

    public void TrackWalk(float meters)
    {
      WalkDist += meters;
      DistLabel.text = WalkDist.ToString("#");
    }

    public void TrackWord(string word)
    {
      ++WordsUsed;
      LettersUsed += word.Length;
      LettersUsedLabel.text = LettersUsed.ToString();
    }

    public void TrackWastedLetters(string word)
    {
      LettersWaseted += word.Length;
      LettersWasetedLabel.text = LettersWaseted.ToString();
    }
  }
}