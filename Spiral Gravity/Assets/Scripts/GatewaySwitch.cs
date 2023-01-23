/*
 * GatewaySwitch will activate specified gateway objects in the scene
 */

using UnityEngine;
using UnityEngine.Rendering.Universal;

/*
 * Author:              Alex McRandal
 * Email:               mcrandalalex@gmail.com
 * Project:             Spiral Gravity
 * Last-Modified:       Sept 4, 2022
 */

public class GatewaySwitch : MonoBehaviour
{
    [Tooltip("Light attached to switch (assigned in script)")]
    public Light2D switchLight; 

    [Tooltip("Gateways to be controlled by this specific switch")]
    public GatewayMovement[] gateways;

    private void Awake()
    {
        switchLight = this.GetComponent<Light2D>();

        if (switchLight == null)
        {
            Debug.LogError("Could not find component of type Light2D");
        }

        switchLight.intensity = 2f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (switchLight.intensity == 2f)
            {
                switchLight.intensity = 20f;
            }
            else
            {
                switchLight.intensity = 2f;
            }

            AudioManager.instance.PlaySFX(1);

            foreach (GatewayMovement _gateway in gateways)
            {
                _gateway.BeginToMove();
            }
        }
    }
}
