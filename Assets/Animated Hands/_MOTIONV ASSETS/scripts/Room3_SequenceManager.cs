using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room3_SequenceManager : MonoBehaviour
{
    [Header("Sequence Settings")]
    public List<string> correctSequence = new List<string> { "Pink", "Orange", "Yellow", "Blue" };

    [Header("Buttons")]
    public List<Room3_Button> allButtons;

    [Header("Events")]
    public UnityEvent onSequenceComplete;
    public UnityEvent onSequenceFailed;

    private int currentIndex;

    public void ButtonPressed(Room3_Button pressedButton)
    {
        if (currentIndex >= correctSequence.Count) return;

        if (pressedButton.buttonID != correctSequence[currentIndex])
        {
            onSequenceFailed.Invoke();
            ResetSequence();
            return;
        }

        pressedButton.isLockedDown = true;

        if (++currentIndex == correctSequence.Count)
        {
            onSequenceComplete.Invoke();
        }
    }

    public void ResetSequence()
    {
        currentIndex = 0;
        allButtons.ForEach(button => button.ResetButton());
    }
}