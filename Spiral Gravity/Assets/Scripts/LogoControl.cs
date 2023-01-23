/*
 * LogoControl will spin the logo as well as change the color of it
 */

using UnityEngine;

/*
 * Author:          Alex McRandal
 * Contact:         Email - mcrandalalex@gmail.com
 * Project:         Spiral Gravity
 * Last-Modified:   May 19, 2022
 */

public class LogoControl : MonoBehaviour
{
    [Tooltip("How fast the logo rotates")]
    public float rotateSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
    }
}
