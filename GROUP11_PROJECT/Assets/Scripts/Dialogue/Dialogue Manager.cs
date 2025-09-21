using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Rendering.DebugUI.Table;


public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Animator animator;

    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue( Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void NextPage(CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;
    }
    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
/*
Title: How to make a Dialogue System in Unity
Author: Brackeys
Date: 15 August 2025
Code version: 1
Availability: https://www.youtube.com/watch?v=_nRzoTzeyxU&t=564s
*/