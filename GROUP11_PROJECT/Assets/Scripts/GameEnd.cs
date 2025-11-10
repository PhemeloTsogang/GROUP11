using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class GameEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject text;
    public GameObject stamina, healthbar;

    public void PlayCutscene()
    {
        if (videoPlayer == null)
        {
            return;
        }

        stamina.SetActive(false);
        text.SetActive(false);
        healthbar.SetActive(false);

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
