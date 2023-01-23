/*
 * BallMovement is responsible for moving the ball and updating its data
 */

using UnityEngine;

/*
 *  File:           BallMovement.cs
 *  Author:         Alex McRandal
 *  Email:          amcranda@heidelberg.edu
 *  Project:        GDM IV, Pong
 */

public class BallMovement : MonoBehaviour
{
    /// <summary>
    /// Data about the ball
    /// </summary>
    private BallData data;

    /// <summary>
    /// Variable to help move the ball
    /// </summary>
    private Vector3 calcMovement = Vector3.zero;

    private void Awake()
    {
        data = new BallData(_size: 0.5f);

        transform.localScale = new Vector3(data.GetSize(), data.GetSize(), data.GetSize());
    }

    /// <summary>
    /// Set the direction in which the ball is moving
    /// </summary>
    /// <param name="_movement">Direction of new ball movement</param>
    public void SetBallInMotion(Vector2 _movement)
    {
        data.SetMovement(_movement);
    }

    private void FixedUpdate()
    {
        calcMovement.x = data.GetSpeed() * Time.fixedDeltaTime * data.GetMovement().x;
        calcMovement.y = data.GetSpeed() * Time.fixedDeltaTime * data.GetMovement().y;

        transform.position += calcMovement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        data.SetMovement(Vector2.Reflect(data.GetMovement(), collision.contacts[0].normal));

        if (collision.gameObject.tag[..^1] == "Player")
        {
            data.SetSpeed(data.GetSpeed() + 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("P1Goal"))
        {
            GameManager.instance.AddPoint(2);
        }
        else
        {
            GameManager.instance.AddPoint(1);
        }

        Destroy(this.gameObject);
    }
}
