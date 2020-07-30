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

    private const int LOW = 0, MEDIUM = 1, HIGH = 2;
    private int curSpeechPriority;

    public static readonly int[] HIT_SPEECH_PERCENTAGES = new int[] { 75 };

    [SerializeField]
    private AudioClip[] hitSpeechAudios;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip[] takeHitGrunts;

    [SerializeField]
    private AudioClip weaponPickup;

    public void PlayRandomTakeHitGrunt() {
        if (NotAbleToPlay(LOW))
            return;

        curSpeechPriority = LOW;
        AudioClip sound = takeHitGrunts[Random.Range(0, takeHitGrunts.Length)];
        PlayAudio(sound);
    }

    public void PlayHealthLowAudio(int index) {
        if (NotAbleToPlay(MEDIUM))
            return;

        PlayAudio(hitSpeechAudios[index]);
    }

    public void PlayWeaponPickup() {
        if (NotAbleToPlay(MEDIUM))
            return;

        PlayAudio(weaponPickup);
    }

    private bool NotAbleToPlay(int priority) {
        bool notAble = source.isPlaying && curSpeechPriority >= priority;

        if (!notAble)
            curSpeechPriority = priority;

        return notAble;
    }

    private void PlayAudio(AudioClip clip) {
        source.clip = clip;
        source.Play();
    }
}
