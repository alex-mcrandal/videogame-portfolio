/*
 * CameraManager is responsible for rotating the camera and changing the position
 * of the camera as the game goes on.
 */

using UnityEngine;

/*
 * Author:          Alex McRandal
 * Contact:         Email - mcrandalalex@gmail.com
 * Project:         Spiral Gravity
 * Last-Modified:   May 6, 2022
 */

public class CameraManager : MonoBehaviour
{
    [Tooltip("How fast the camera rotates.")]
     public float turnAcceleration = 5f;

    //private variables used to calculate and perform the camera's rotation
    float turnDistance = 0f;
    float initTime;
    bool turningClockwise = true;

    // Start is called before the first frame update
    void Start()
    {
        //initTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(turnDistance > 0f)
        {
            float deltaTurn = Mathf.Pow(turnAcceleration, -(Time.time - initTime) + 1);
            turnDistance -= deltaTurn;
            if (turningClockwise)
            {
                transform.Rotate(0f, 0f, -deltaTurn);
            }
            else
            {
                transform.Rotate(0f, 0f, deltaTurn);
            }
        }
        else
        {
            RoundCameraRotation();
        }
    }

    /// <summary>
    /// Initialize the turning conditions so the update method can apply the transformation
    /// </summary>
    /// <param name="_clockwise">If the camera should turn clockwise or counter-clockwise</param>
    public void Rotate(bool _clockwise)
    {
        turningClockwise = _clockwise;
        initTime = Time.time;
        turnDistance = 90f;
    }

    /// <summary>
    /// Set the rotation of the camera to the nearest
    /// </summary>
    private void RoundCameraRotation()
    {
        float _cameraRotation = transform.rotation.eulerAngles.z;

        _cameraRotation = Mathf.Floor(_cameraRotation * 0.1f + 0.5f) / 0.1f;

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _cameraRotation));
    }
}
