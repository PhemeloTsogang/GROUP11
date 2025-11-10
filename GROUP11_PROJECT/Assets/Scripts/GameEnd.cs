using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.InputSystem;

public class GameEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject text;
    public GameObject stamina, healthbar,memories, memoryCount;
    public PlayerInput playerInput;
    public void PlayCutscene()
    {
        if (videoPlayer == null)
        {
            return;
        }

        stamina.SetActive(false);
        text.SetActive(false);
        healthbar.SetActive(false);
        memories.SetActive(false);
        memoryCount.SetActive(false);

        playerInput.enabled = false;

        if (Time.timeScale != 0)
        {
            Time.timeScale = 0f;
        }


        foreach (var source in FindObjectsOfType<AudioSource>())
        {
            source.Stop();
        }

        videoPlayer.Play();

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnCutsceneEnd;
        }
    }

    private void OnCutsceneEnd(VideoPlayer vp)
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnCutsceneEnd;
        }

        playerInput.enabled = true;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        if (vp != null && vp.isPlaying)
        {
            vp.Stop();
        }

        StartCoroutine(LoadMainMenuNextFrame());
    }

    private IEnumerator LoadMainMenuNextFrame()
    {
        yield return null;
        SceneManager.LoadScene("MainMenu");
    }
}
