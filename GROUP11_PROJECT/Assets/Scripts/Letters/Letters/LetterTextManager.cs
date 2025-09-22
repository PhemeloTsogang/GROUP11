using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class LetterTextManager : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text letterTextText;
    public Animator animator;

    private Queue <string> pages;

    void Start()
    {
        pages = new Queue<string>();
    }

    public void StartLetter(LetterText letterText)
    {
        animator.SetBool("IsOpen", true);
        titleText.text = letterText.name;
        pages.Clear();

        foreach (string page in letterText.pages)
        {
            pages.Enqueue(page);
        }

        DisplayNextPage();
    }

    public void NextPage(CallbackContext context)
    {
        if (context.performed)
        {
            DisplayNextPage();
        }
    }

    public void DisplayNextPage()
    {
        if (pages.Count == 0)
        {
            EndLetterText();
            return;
        }

        string page = pages.Dequeue();
        letterTextText.text = page;
    }
    public void EndLetterText()
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
// Start is called once before the first execution of Update after the MonoBehaviour is created
