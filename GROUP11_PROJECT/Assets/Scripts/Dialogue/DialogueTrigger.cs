using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager manager;

    public void TriggerDialogue()
    {
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}

/*
Title: How to make a Dialogue System in Unity
Author: Brackeys
Date: 15 August 2025
Code version: 1
Availability: https://www.youtube.com/watch?v=_nRzoTzeyxU&t=564s
*/