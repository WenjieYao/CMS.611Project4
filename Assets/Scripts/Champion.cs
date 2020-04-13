using System.Collections;
using UnityEngine;

/****************************************************/
// The Player script is used for defining player
// properties and player behaviors
/****************************************************/
public class Champion : MonoBehaviour
{
    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
    public bool playerCanControl; // can be controlled by player

    public int maxHealth;
    public int health; // current health
    public int attackPower;
    public float fireRate; // Number of bullets fired per second
    public float speed = 1; // movement speed

    public GameObject projectilePrefab;
    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        // Initialze player with full health
        health = maxHealth;

        StartCoroutine(ShootProjectile());
    }

    void FixedUpdate()
    {
        // Only handle player input if playerCanControl
        if (playerCanControl)
        {
            HandlePlayerControls();
        }
    }

    /*
     * Experiment to see if projectiles are destroyed upon hitting wall
     * Player shoots a projectile upwards every {shootCooldown} seconds
     */
    private IEnumerator ShootProjectile()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            GameObject projectile = Instantiate(projectilePrefab, (Vector2)this.transform.position + Vector2.up, Quaternion.identity) as GameObject;
            projectile.GetComponent<Projectile>().SetTrajectory(Vector2.up);
            print("Spawned projectile");
        }
    }

    // Move physical position based on player controls
    private void HandlePlayerControls()
    {
        Vector2 moveVector = new Vector2(0, 0);
        if (Input.GetKey("w")) moveVector += Vector2.up;
        if (Input.GetKey("s")) moveVector += Vector2.down;
        if (Input.GetKey("a")) moveVector += Vector2.left;
        if (Input.GetKey("d")) moveVector += Vector2.right;
        moveVector.Normalize();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + speed * moveVector * Time.fixedDeltaTime);
    }
}
