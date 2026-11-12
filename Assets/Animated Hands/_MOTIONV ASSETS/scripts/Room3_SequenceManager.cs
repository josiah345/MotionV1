using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Room3_SequenceManager : MonoBehaviour
{
    [Header("Sequence Settings")]
    public List<string> correctSequence = new List<string> { "Pink", "Orange", "Yellow", "Blue" };

    [Header("All Buttons In Scene")]
    public List<Room3_Button> allButtons = new List<Room3_Button>();

    [Header("Events")]
    public UnityEvent onSequenceComplete;
    public UnityEvent onSequenceFailed;

    private int currentIndex = 0;

    public void ButtonPressed(Room3_Button pressedButton)
    {
        string buttonID = pressedButton.buttonID;

        if (buttonID == correctSequence[currentIndex])
        {
            pressedButton.isLockedDown = true;
            currentIndex++;
            Debug.Log($"Correct! Step {currentIndex}/{correctSequence.Count}");

            if (currentIndex >= correctSequence.Count)
            {
                Debug.Log("SEQUENCE COMPLETE! Opening door.");
                onSequenceComplete.Invoke();
            }
        }
        else
        {
            Debug.Log($"Wrong! Resetting sequence.");
            onSequenceFailed.Invoke();
            ResetSequence(); // Changed this to call our new function
        }
    }

    // --- NEW FUNCTION FOR THE DOOR TO CALL ---
    public void ResetSequence()
    {
        currentIndex = 0;
        foreach (Room3_Button btn in allButtons)
        {
            btn.ResetButton();
        }
        Debug.Log("Sequence Reset Complete.");
    }
}