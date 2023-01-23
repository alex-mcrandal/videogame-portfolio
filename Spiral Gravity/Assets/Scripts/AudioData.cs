/*
 * AudioData stores the volume for music and sfxs
 */

using UnityEngine;

/*
 * Author:              Alex McRandal
 * Email:               mcrandalalex@gmail.com
 * Project:             Spiral Gravity
 * Last-Modified:       July 12, 2022
 */

public class AudioData : MonoBehaviour
{
    /// <summary>
    /// Reference to the audio data from any script
    /// </summary>
    [HideInInspector]
    public static AudioData instance;

    [Tooltip("Volume level for the music")]
    public float musicVolume;

    [Tooltip("Volume level for the sfxs")]
    public float sfxVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Instance of AudioData already exists. Destroying this object!");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        musicVolume = 0.5f;
        sfxVolume = 0.5f;
    }
}
