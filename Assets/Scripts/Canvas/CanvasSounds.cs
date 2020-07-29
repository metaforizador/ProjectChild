using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for playing UI canvas sounds using a single audio source.
/// 
/// This way 2 sounds will never play at the same time. It will
/// stop the previous sound when a new sound begins playing.
/// </summary>
public class CanvasSounds : MonoBehaviour {

    private AudioSource source;

    public AudioClip BUTTON_ENTER;
    public AudioClip BUTTON_SELECT, BUTTON_BACK;
    public AudioClip ITEM_BREAK, ITEM_USAGE_NOTIFICATION;

    /// <summary>
    /// Gets audio source component.
    /// </summary>
    void Awake() => source = GetComponent<AudioSource>();

    /// <summary>
    /// Plays provided sound once.
    /// </summary>
    /// <param name="clip">sound to play</param>
    public void PlaySound(AudioClip clip) => source.PlayOneShot(clip);
}
