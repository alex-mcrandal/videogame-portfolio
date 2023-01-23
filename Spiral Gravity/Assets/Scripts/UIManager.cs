/*
 * UIManager handles most of the functionality of all the buttons 
 * in the main menu scene. Public functions are provided for the 
 * buttons to call on-click.
 */

using UnityEngine;
using UnityEngine.UI;

/*
 * Author:          Alex McRandal
 * Email:           mcrandalalex@gmail.com
 * Project:         Sprial Gravity
 * Last-Modified:   July 6, 2022
 */

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Reference to this script from anywhere in the scene
    /// </summary>
    [HideInInspector]
    public static UIManager instance;

    [Tooltip("Reference to every view on the main menu")]
    public GameObject[] screenPanels;

    [Tooltip("References to the sliders under Settings")]
    public Slider[] settingSliders;

    /// <summary>
    /// Method call to quit the application
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Displays the appropiate panel using the indecies of the array in the
    /// Unity Editor
    /// </summary>
    /// <param name="panelIndex">Index of the panel to be displayed. Defaults to "0" to show main menu</param>
    public void ShowPanel(int panelIndex = 0)
    {
        for(int i = 0; i < screenPanels.Length; i++)
        {
            if(screenPanels[i] != null)
            {
                screenPanels[i].SetActive(false);
            }
        }

        screenPanels[panelIndex].SetActive(true);
    }

    /// <summary>
    /// Hide all of the panels from view
    /// </summary>
    public void HidePanels()
    {
        foreach (GameObject _panel in screenPanels)
        {
            _panel.SetActive(false);
        }
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Instance of UIManager already exists! Destroying this object!");
            Destroy(this);
        }

        settingSliders[0].value = AudioData.instance.musicVolume;
        settingSliders[1].value = AudioData.instance.sfxVolume;
    }
}
