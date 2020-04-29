using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

    private int maxHealth = 5;
    private int curHealth; // current health

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

    public Slider slider;
    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        // Initialize player with full health
        curHealth = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = curHealth;
        // Invulnerable when spawned.
        dmgCooldownTimer = dmgCooldownSecs;

        // Set originalDrag
        originalDrag = GetComponent<Rigidbody2D>().drag;
    }

    void FixedUpdate()
    {
        // Only handle player input if playerCanControl
        if (playerCanControl) { HandlePlayerControls(); }

        // Die when go over chasm.
        Tilemap chasmMap = GameObject.FindGameObjectWithTag("Chasms").GetComponent<Tilemap>();
        if (chasmMap.HasTile(chasmMap.WorldToCell(this.transform.position)))
        {
            Die();
        }
    }

    private void HandlePlayerControls()
    {
        // Handle translational movement
        Vector2 translateDir = new Vector2(0, 0);
        if (Input.GetKey("w")) { translateDir += Vector2.up; }
        if (Input.GetKey("a")) { translateDir += Vector2.left; }
        if (Input.GetKey("s")) { translateDir += Vector2.down; }
        if (Input.GetKey("d")) { translateDir += Vector2.right; }
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

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        GetComponent<Rigidbody2D>().MoveRotation(
            Quaternion.LookRotation(
                forward: Vector3.forward,
                upwards: mousePos - transform.position
            )
        );

        // Handle attacks
        if (championWeapon == Weapon.Gun)
        {
            gunCooldownLeft -= Time.fixedDeltaTime;
            if (gunCooldownLeft <= 0) // able to fire gun
            {
                if (Input.GetMouseButton(0))
                {
                    // this.transform.up is the direction the player faces
                    Projectile bullet = Instantiate(
                        bulletPrefab,
                        this.transform.position + this.transform.up,
                        Quaternion.identity
                    ).GetComponent<Projectile>();
                    bullet.Direction = this.transform.up;
                    gunCooldownLeft = gunCooldownSecs;
                }
            }
        }
        else if (championWeapon == Weapon.Sword)
        {
            swordCooldownLeft -= Time.fixedDeltaTime;
            if (swordCooldownLeft <= 0) // able to attack with sword
            {
                if (Input.GetMouseButton(0))
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
        curHealth -= damage;
        slider.value = curHealth;
        if (curHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameManager.Instance.GameOver.SetActive(true);
        Destroy(gameObject);
        // TODO: Trigger Game Over popup
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ending"))
        {
            GameManager.Instance.Win.SetActive(true);
        }
    }
}
