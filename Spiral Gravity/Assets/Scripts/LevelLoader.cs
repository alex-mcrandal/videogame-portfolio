/*
 * LevelLoader is a short script used to load from one scene to another and
 * display the progess as a loading bar (without text)
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Author:          Alex McRandal
 * Email:           mcrandalalex@gmail.com
 * Project:         Sprial Gravity
 * Last-Modified:   6-2-2022
 */

public class LevelLoader : MonoBehaviour
{
    [Tooltip("Reference to the panel that represents the loading screne")]
    public GameObject loadingScreen;

    [Tooltip("Reference to the slider/progress bar that displays the loading progress")]
    public Slider loadingBar;


    /// <summary>
    /// Method used by outside scripts to load the next level/scene based on
    /// the index of the next scene
    /// </summary>
    /// <param name="sceneIndex">Numeric index of the next level/scene</param>
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    /// <summary>
    /// Load the next level/scene asynchronously and continuously display the progress to the player
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <returns>One frame inbetween each progress check</returns>
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        float progress;
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBar.value = progress;

            yield return null;
        }
    }
}
