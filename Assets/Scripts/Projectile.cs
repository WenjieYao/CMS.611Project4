using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
    public float timeToLive = 2f;
    public float spawnSpeed = 8.0f;
    public Vector2 velocity;
    /****************************************************/

    // Start is called before the first frame update
    void Start() { }

    // Sets velocity
    public void Fire(Rigidbody2D gun, Vector2 direction)
    {
        velocity = gun.velocity + direction * spawnSpeed;
    }

    void FixedUpdate()
    {
        // Move the projectile according to its velocity and trajectory
        transform.Translate(velocity * Time.fixedDeltaTime);

        if (timeToLive < 0) { Destroy(gameObject); }
        timeToLive -= Time.fixedDeltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if projectile hits something other than player,
        // destroy projectile
        if (!collision.gameObject.tag.Equals("Player") && !collision.gameObject.tag.Contains("bullet"))
        {
            Destroy(gameObject);
        }
    }
}
