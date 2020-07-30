using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    // Make class static and destroy if script already exists
    private static PlayerSounds _instance; // **<- reference link to the class
    public static PlayerSounds Instance { get { return _instance; } }

    private void Awake() {
        // If instance not yet created
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private enum SpeechType { None, HealthLow, Grunt }
    private SpeechType curSpeechType;

    public static readonly int[] HIT_SPEECH_PERCENTAGES = new int[] { 75 };

    [SerializeField]
    private AudioClip[] hitSpeechAudios;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip[] takeHitGrunts;

    public AudioClip WEAPON_PICKUP;

    public void PlayRandomTakeHitGrunt() {
        // Don't play grunt sounds if player is saying something
        if (source.isPlaying)
            return;

        curSpeechType = SpeechType.Grunt;
        AudioClip sound = takeHitGrunts[Random.Range(0, takeHitGrunts.Length)];
        PlayAudio(sound);
    }

    public void PlayHealthLowAudio(int index) {
        // If player is already saying some health low speech, don't cut it out
        if (source.isPlaying && curSpeechType.Equals(SpeechType.HealthLow))
            return;

        curSpeechType = SpeechType.HealthLow;
        PlayAudio(hitSpeechAudios[index]);
    }

    private void PlayAudio(AudioClip clip) {
        source.clip = clip;
        source.Play();
    }
}
