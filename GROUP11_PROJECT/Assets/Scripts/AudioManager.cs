using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public AudioSource Play(string soundName, Transform parent)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);

        GameObject go = new GameObject("Audio_" + s.name);

        if (parent != null)
        {
            go.transform.SetParent(parent);
            go.transform.localPosition = Vector3.zero;
        }
        else
        {
            go.transform.position = Vector3.zero; 
        }

        AudioSource source = go.AddComponent<AudioSource>();
        SetupAudioSource(source, s);
        source.Play();

        if (!source.loop)
            Destroy(go, s.clip.length);

        return source;
    }

    private void SetupAudioSource(AudioSource source, Sound s)
    {
        source.clip = s.clip;
        source.volume = s.volume;
        source.pitch = s.pitch;
        source.spatialBlend = s.spatialBlend;
        source.loop = s.loop;
    }

    public void StopSound(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
            Destroy(source.gameObject);
        }
    }
}

