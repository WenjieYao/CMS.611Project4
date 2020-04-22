using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * The Player script is used for defining player
 * properties and player behaviors.
 */
public class Champion : MonoBehaviour
{
    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
    public bool playerCanControl; // can be controlled by player

    public float translateForce = 50;
    public float torqueForce = 5;

    public Tilemap iceMap;
    public float iceDrag = 1;
    private float originalDrag;

    public int maxHealth;
    public int curHealth; // current health

    private float dmgCooldownTimer;
    public float dmgCooldownSecs = 0.5f; // time invulnerable after taking damge

    public enum Weapon { Gun, Sword };
    public Weapon championWeapon;

    public GameObject bulletPrefab;
    public float gunCooldownSecs = 0.25f; // how long to wait per shot
    private float gunCooldownLeft = 0;

    public GameObject swordPrefab;
    public float swordExistSecs = 0.75f;
    public float swordCooldownSecs = 1f;
    private float swordCooldownLeft = 0;
    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        // Initialize player with full health
        curHealth = maxHealth;

        // Invulnerable when spawned.
        dmgCooldownTimer = dmgCooldownSecs;

        // Set originalDrag
        originalDrag = GetComponent<Rigidbody2D>().drag;
    }

    void FixedUpdate()
    {
        // Only handle player input if playerCanControl
        if (playerCanControl) { HandlePlayerControls(); }
    }

    private void HandlePlayerControls()
    {
        // Handle translational movement
        Vector2 translateDir = new Vector2(0, 0);
        if (Input.GetKey("w")) { translateDir += Vector2.up; }
        if (Input.GetKey("a")) { translateDir += Vector2.left; }
        if (Input.GetKey("s")) { translateDir += Vector2.down; }
        if (Input.GetKey("d")) { translateDir += Vector2.right; }
        // The 100 factor here should stay constant.
        // Change mass to change movement speed.
        translateDir.Normalize();

        if (iceMap.HasTile(iceMap.WorldToCell(this.transform.position)))
        {
            // reduce drag on ice
            GetComponent<Rigidbody2D>().drag = iceDrag;
            // reduce force proportionally so max-velocity is maintained.
            GetComponent<Rigidbody2D>().AddForce(
                (iceDrag / originalDrag) * translateForce * translateDir
            );
        }
        else
        {
            GetComponent<Rigidbody2D>().drag = originalDrag;
            GetComponent<Rigidbody2D>().AddForce(translateForce * translateDir);
        }

        // Handle rotational movement
        float torqueDir = 0;
        if (Input.GetKey("q")) { torqueDir += 1; }
        if (Input.GetKey("e")) { torqueDir -= 1; }
        GetComponent<Rigidbody2D>().AddTorque(torqueForce * torqueDir);

        // Handle attacks
        if (championWeapon == Weapon.Gun)
        {
            gunCooldownLeft -= Time.fixedDeltaTime;
            if (gunCooldownLeft <= 0) // able to fire gun
            {
                if (Input.GetKey("j"))
                {
                    // this.transform.up is the direction the player faces
                    Projectile bullet = Instantiate(
                        bulletPrefab,
                        this.transform.position + this.transform.up,
                        Quaternion.identity
                    ).GetComponent<Projectile>();
                    bullet.SetTrajectory(this.transform.up);
                    gunCooldownLeft = gunCooldownSecs;
                }
            }
        }
        else if (championWeapon == Weapon.Sword)
        {
            swordCooldownLeft -= Time.fixedDeltaTime;
            if (swordCooldownLeft <= 0) // able to attack with sword
            {
                if (Input.GetKey("j"))
                {
                    // this.transform.up is the direction the player faces
                    GameObject sword = Instantiate(
                        swordPrefab,
                        this.transform.position + this.transform.up,
                        this.transform.rotation
                    );
                    sword.transform.SetParent(this.transform); // lock relative position to champion
                    Destroy(sword, swordExistSecs); // destroy after swordExistSecs
                    swordCooldownLeft = swordCooldownSecs;
                }
            }
        }
    }

    // Champion takes {damage} damage to health at most once per maxDmgCooldown
    public void DealChampionDamage(int damage)
    {
        dmgCooldownTimer -= Time.fixedDeltaTime;
        if (dmgCooldownTimer <= 0)
        {
            curHealth -= damage;
            Debug.Log(curHealth + "/" + maxHealth);
            if (curHealth < 0)
            {
                Die();
            }
            dmgCooldownTimer = dmgCooldownSecs;
        }
    }

    void Die()
    {
        curHealth = 0;
        Debug.Log("Died");
        // TODO: Trigger Game Over popup
    }
}
