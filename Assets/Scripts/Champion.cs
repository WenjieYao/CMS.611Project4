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
    private int shoot = 0;
    public int maxHealth;
    public int health; // current health
    public int attackPower;
    public float fireRate; // Number of bullets fired per second
    public float speed = 1; // movement speed
    public int direction = 0;
    private GameObject newsword;
    public GameObject projectilePrefab;
    public GameObject sword;
    private float dmgCooldownTimer;
    [SerializeField]
    private float maxDmgCooldown = 0.5f;

    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        // Initialze player with full health
        health = maxHealth;
        // Initialize cooldown timer to start at its max value
        dmgCooldownTimer = maxDmgCooldown;

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
            //GameObject projectile = Instantiate(projectilePrefab, (Vector2)this.transform.position + Vector2.up, Quaternion.identity) as GameObject;
            //projectile.GetComponent<Projectile>().SetTrajectory(Vector2.up);
            //print("Spawned projectile");
        }
    }

    // Move physical position based on player controls
    private void HandlePlayerControls()
    {
        shoot = shoot - 1;
        if (shoot <= 0) { Destroy(newsword); }
        Vector2 moveVector = new Vector2(0, 0);
        if (shoot <= 0 || attackPower == 1)
        {
            if (Input.GetKey("w"))
            {
                moveVector += Vector2.up; direction = 0;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (Input.GetKey("s"))
            {
                moveVector += Vector2.down; direction = 2;
                transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
            if (Input.GetKey("a"))
            {
                moveVector += Vector2.left; direction = 3;
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            if (Input.GetKey("d"))
            {
                moveVector += Vector2.right; direction = 1;
                transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            if (Input.GetKey("x") && shoot <= 0 && attackPower == 1)
            {
                if (direction == 0)
                {
                    GameObject projectile = Instantiate(projectilePrefab, (Vector2)this.transform.position + Vector2.up, Quaternion.identity) as GameObject;
                    projectile.GetComponent<Projectile>().SetTrajectory(Vector2.up);
                }
                if (direction == 1)
                {
                    GameObject projectile = Instantiate(projectilePrefab, (Vector2)this.transform.position + Vector2.right, Quaternion.identity) as GameObject;
                    projectile.GetComponent<Projectile>().SetTrajectory(Vector2.right);
                }
                if (direction == 2)
                {
                    GameObject projectile = Instantiate(projectilePrefab, (Vector2)this.transform.position + Vector2.down, Quaternion.identity) as GameObject;
                    projectile.GetComponent<Projectile>().SetTrajectory(Vector2.down);
                }
                if (direction == 3)
                {
                    GameObject projectile = Instantiate(projectilePrefab, (Vector2)this.transform.position + Vector2.left, Quaternion.identity) as GameObject;
                    projectile.GetComponent<Projectile>().SetTrajectory(Vector2.left);
                }
                print("Spawned projectile");
                shoot = 30;
            }
            if (Input.GetKey("x") && shoot <= 0 && attackPower != 1)
            {
                if (direction == 0)
                {
                    newsword = Instantiate(sword, (Vector2)this.transform.position + Vector2.up, Quaternion.identity) as GameObject;
                }
                if (direction == 1)
                {
                    newsword = Instantiate(sword, (Vector2)this.transform.position + Vector2.right, Quaternion.Euler(0, 0, 270)) as GameObject;
                }
                if (direction == 2)
                {
                    newsword = Instantiate(sword, (Vector2)this.transform.position + Vector2.down, Quaternion.Euler(0, 0, 180)) as GameObject;
                }
                if (direction == 3)
                {
                    newsword = Instantiate(sword, (Vector2)this.transform.position + Vector2.left, Quaternion.Euler(0, 0, 90)) as GameObject;
                }
                shoot = 20;
            }
        }
        moveVector.Normalize();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.MovePosition(rb.position + speed * moveVector * Time.fixedDeltaTime);
    }

    //Champion takes {damage} damage to health at most once per maxDmgCooldown
    public void DealChampionDamage(int damage){
        dmgCooldownTimer -= Time.fixedDeltaTime;
        if (dmgCooldownTimer <= 0){
            health -= damage;
            Debug.Log(health + "/" + maxHealth);
            if (health < 0){
                Die();
            }
            dmgCooldownTimer = maxDmgCooldown;
        }
    }

    //TODO: Trigger Game Over popup
    void Die(){
        health = 0;
        Debug.Log("Died");
    }
}
