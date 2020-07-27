using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSounds : MonoBehaviour {

    private AudioSource source;

    public AudioClip BUTTON_SELECT, BUTTON_BACK;
    public AudioClip ITEM_BREAK, ITEM_USAGE_NOTIFICATION;

    /// <summary>
    /// Get audio source component
    /// </summary>
    void Awake() => source = GetComponent<AudioSource>();

    /// <summary>
    /// Play provided sound once
    /// </summary>
    /// <param name="clip">sound to play</param>
    public void PlaySound(AudioClip clip) => source.PlayOneShot(clip);
}
