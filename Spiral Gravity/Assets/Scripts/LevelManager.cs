/*
 * LevelManager is responsible for sending the player to the next level when they complete it
 * and move the camera to the next level
 */

using UnityEngine;

/*
 * Author:          Alex McRandal
 * Contact:         Email - mcrandalalex@gmail.com
 * Project:         Spiral Gravity
 * Last-Modified:   July 23, 2022
 */

public class LevelManager : MonoBehaviour
{
    [Tooltip("Reference to the GravityManager script")]
    public GravityManager gManager;

    [Tooltip("Reference to the Transform component on the background sprite.")]
    public Transform backgroundTransform;

    [Tooltip("Reference to the level loader script in the main scene")]
    public LevelLoader levelLoader;

    //Index used to keep track of current level (offset by 1)
    private int levelIndex;

    //Collection of every level in order
    private GameObject[] levels;

    //Reference to the player game object
    private GameObject player;

    //Reference to the main camera object
    private GameObject mainCamera;

    private void Awake()
    {
        levels = GameObject.FindGameObjectsWithTag("Level");
        levelIndex = 0;
        player = GameObject.FindWithTag("Player");
        mainCamera = GameObject.FindWithTag("MainCamera");

        SortLevels();

        TimerManager.instance.StartTimer();
    }

    /// <summary>
    /// Tell the game that player has reached the next level
    /// </summary>
    public void NextLevel()
    {
        levelIndex++;
        if(levelIndex >= levels.Length)
        {
            TimerManager.instance.StopTimer();
            levelLoader.LoadLevel(2);
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        gManager.ResetGravity();
        player.transform.position = levels[levelIndex].transform.GetChild(0).transform.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        mainCamera.transform.position = 
            new Vector3(levels[levelIndex].transform.position.x, levels[levelIndex].transform.position.y, -10f);
        mainCamera.transform.rotation = Quaternion.identity;
        backgroundTransform.position = 
            new Vector3(levels[levelIndex].transform.position.x, levels[levelIndex].transform.position.y, 5f);
    }

    /// <summary>
    /// Restart from the current level. Typically in a situation where the player dies
    /// </summary>
    public void ResetLevel()
    {
        gManager.ResetGravity();
        player.transform.position = levels[levelIndex].transform.GetChild(0).transform.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        mainCamera.transform.position = 
            new Vector3(levels[levelIndex].transform.position.x, levels[levelIndex].transform.position.y, -10f);
        mainCamera.transform.rotation = Quaternion.identity;
        backgroundTransform.position = 
            new Vector3(levels[levelIndex].transform.position.x, levels[levelIndex].transform.position.y, 5f);
    }

    /// <summary>
    /// Sort the levels in the scene in ascending order based on the level number.
    /// Smart bubble sort is used in this method
    /// </summary>
    public void SortLevels()
    {
        GameObject sortTemp;
        bool smartControl;
        for(int i = levels.Length - 1; i > 0; i--)
        {
            smartControl = true;
            for(int j = 0; j < i ; j++)
            {
                if(levels[j].name[2..].CompareTo(levels[j+1].name[2..]) > 0)
                {
                    sortTemp = levels[j];
                    levels[j] = levels[j + 1];
                    levels[j + 1] = sortTemp;
                    smartControl = false;
                }
            }

            if (smartControl)
            {
                break;
            }
        }
    }
}
