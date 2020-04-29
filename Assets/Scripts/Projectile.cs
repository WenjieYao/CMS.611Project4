using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
    public float timeToLive = 1f;
    public float speed = 8.0f;
    private Vector2 direction;
    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value.normalized; }
    }

    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start() {}

    void FixedUpdate()
    {
        // Move the projectile according to its velocity and trajectory
        transform.Translate(speed * direction * Time.fixedDeltaTime);

        if (timeToLive < 0) {
            Destroy(gameObject);
        }
        timeToLive -= Time.fixedDeltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if projectile hits something other than player,
        // destroy projectile
        if (!collision.gameObject.tag.Equals("Player"))
        {
            Destroy(gameObject);
        }
    }
}
