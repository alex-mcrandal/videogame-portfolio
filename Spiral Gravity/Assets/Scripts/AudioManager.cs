/*
 * AudioManager will play music over the scene in a repetative pattern or on a loop.
 * In addition, this script will play sound effects when instructed by other scripts.
 */

using UnityEngine;

/*
 * Author:          Alex McRandal
 * Email:           mcrandalalex@gmail.com
 * Project:         Sprial Gravity
 * Last-Modified:   July 12, 2022
 */

public class AudioManager : MonoBehaviour
{
    [Tooltip("All of the sound effect clips used in a scene")]
    public AudioSource[] sfxs;

    [Tooltip("Soundtrack to play over-top of the game")]
    public AudioSource soundtrack;

    /// <summary>
    /// Reference to the AudioManager from other scripts
    /// </summary>
    public static AudioManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        foreach (AudioSource _sfx in sfxs)
        {
            _sfx.volume = AudioData.instance.sfxVolume;
        }

        if (soundtrack == null)
            return;

        soundtrack.volume = AudioData.instance.musicVolume;
        soundtrack.loop = true;
        soundtrack.Play();
    }

    /// <summary>
    /// Play a sfx based on an index position
    /// </summary>
    /// <param name="_sfxIndex">The index of the desired sfx</param>
    public void PlaySFX(int _sfxIndex)
    {
        if (_sfxIndex < 0)
            return;
        if (_sfxIndex > sfxs.Length)
            return;

        sfxs[_sfxIndex].Play();
    }

    /// <summary>
    /// Change the volume of the music
    /// </summary>
    public void SetMusicVolume()
    { 
        AudioData.instance.musicVolume = UIManager.instance.settingSliders[0].value;
        soundtrack.volume = AudioData.instance.musicVolume;
    }

    /// <summary>
    /// Change the volume of the sound effects
    /// </summary>
    public void SetSFXVolume()
    {
        AudioData.instance.sfxVolume = UIManager.instance.settingSliders[1].value;
        foreach (AudioSource _sfx in sfxs)
        {
            _sfx.volume = AudioData.instance.sfxVolume;
        }
    }
}
