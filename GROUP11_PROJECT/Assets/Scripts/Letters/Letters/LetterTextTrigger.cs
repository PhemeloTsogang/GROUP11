using UnityEngine;

public class LetterTextTrigger : MonoBehaviour
{
    public LetterText letterText;
    public LetterTextManager manager;

     public void TriggerLetter()
    {
        FindFirstObjectByType<LetterTextManager>().StartLetter(letterText);
    }
}
