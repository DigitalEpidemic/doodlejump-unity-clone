using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    AudioSource audioSource;

    void Awake() {
        MakeSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    void MakeSingleton() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySoundEffect(AudioClip sound) {
        audioSource.PlayOneShot(sound);
    }

    public void PlayGameOverSound() {
        audioSource.Play();
    }
}
