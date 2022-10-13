using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    public enum SoundType { Typing, EndEdit, CubeSpawn, CubeDespawn }
    [Serializable]
    private class SoundEffect
    {
        public SoundType soundType;
        public AudioClip clip;
    }
    [SerializeField] SoundEffect[] _soundEffects;
    private AudioSource _audio;
    private static SFXPlayer _instance;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _instance = this;
    }

    public static void Play(SoundType soundType)
    {
        var clip = _instance._soundEffects.Single(s => s.soundType == soundType).clip;
        _instance._audio.PlayOneShot(clip);
    }
}
