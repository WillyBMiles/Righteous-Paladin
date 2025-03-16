using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRandomizer : MonoBehaviour
{
    public bool playOnAwake = true;
    public List<AudioClip> randomClips;

    private void Awake()
    {
        if (playOnAwake)
            Play();
    }

    public void Play()
    {
        var source = GetComponent<AudioSource>();
        source.clip = randomClips[Random.Range(0, randomClips.Count)];
        source.Play();
    }
}
