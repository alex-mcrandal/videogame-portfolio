/*
 * IndividualLevel is a script provided to each level so that
 * it can send the player to the next level when they complete it.
 */

using UnityEngine;

/*
 * Author:          Alex McRandal
 * Contact:         Email - mcrandalalex@gmail.com
 * Project:         Spiral Gravity
 * Last-Modified:   May 6, 2022
 */

public class IndividualLevel : MonoBehaviour
{
    private LevelManager lManager;

    private void Awake()
    {
        lManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        lManager.NextLevel();
    }
}
