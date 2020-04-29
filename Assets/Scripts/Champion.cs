using System.Collections.Generic;
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

    public float translateForce = 50; // controls movement speed

    public Tilemap iceMap;
    public float iceDrag = 1;
    private float originalDrag;

    public int maxHealth = 5;
    public Slider healthBarSlider; // for healthbar
    public AudioSource takeDamageAudioPrefab;

    public float dmgCooldownSecs = 0.5f; // time invulnerable after taking damage
    private float dmgCooldownTimer;

    public enum Weapon { Gun, Sword };
    public Weapon championWeapon;

    public GameObject bulletPrefab;
    public AudioSource gunAudioPrefab;
    public float gunCooldownSecs = 0.25f; // how long to wait per shot
    private float gunCooldownLeft = 0;

    public GameObject swordPrefab;
    public float swordExistSecs = 0.75f;
    public float swordCooldownSecs = 1f;
    private float swordCooldownLeft = 0;
    /****************************************************/

    // Start is called before the first frame update
    void Start()
    {
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
        // Invulnerable when spawned.
        dmgCooldownTimer = dmgCooldownSecs;

        // Set originalDrag
        originalDrag = GetComponent<Rigidbody2D>().drag;

        InitGunAudio();
    }

    void FixedUpdate()
    {
        // Only handle player input if playerCanControl
        if (playerCanControl) { HandlePlayerControls(); }

        // Change drag based on terrain
        if (iceMap.HasTile(iceMap.WorldToCell(this.transform.position)))
        {
            GetComponent<Rigidbody2D>().drag = iceDrag;
        }
        else { GetComponent<Rigidbody2D>().drag = originalDrag; }

        // Die when go over chasm.
        Tilemap chasmMap = GameObject.FindGameObjectWithTag("Chasms").GetComponent<Tilemap>();
        if (chasmMap.HasTile(chasmMap.WorldToCell(this.transform.position)))
        {
            DealChampionDamage(1000);
        }

        FixedUpdateGunAudioState();
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
        GetComponent<Rigidbody2D>().AddForce(
            (GetComponent<Rigidbody2D>().drag / originalDrag)
            * translateForce * translateDir
        );

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
            gunCooldownLeft -= Time.deltaTime;
            if (gunCooldownLeft <= 0 && Input.GetMouseButton(0))
            {
                gunCooldownLeft = gunCooldownSecs;

                // this.transform.up is the direction the player faces
                Projectile bullet = Instantiate(
                    bulletPrefab,
                    this.transform.position + this.transform.up,
                    Quaternion.identity
                ).GetComponent<Projectile>();
                bullet.Fire(GetComponent<Rigidbody2D>(), this.transform.up);

                PlayGunAudio();
            }
        }
        else if (championWeapon == Weapon.Sword)
        {
            swordCooldownLeft -= Time.deltaTime;
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
        healthBarSlider.value -= damage; // TODO: Refactor so we don't break abstraction.
        GameManager.Instance.globalAudioSource.PlayOneShot(
            takeDamageAudioPrefab.clip,
            takeDamageAudioPrefab.volume
        );
        if (healthBarSlider.value <= 0) { Die(); }
    }

    void Die()
    {
        GameManager.Instance.GameOver.SetActive(true);
        GameObject.FindGameObjectWithTag("bg-music").GetComponent<AudioSource>().Stop();
        Destroy(gameObject);
        // TODO: Trigger Game Over popup
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ending"))
        {
            GameObject.FindGameObjectWithTag("bg-music").GetComponent<AudioSource>().Stop();
            healthBarSlider.value = healthBarSlider.maxValue = 1000000000;
            GameManager.Instance.Win.SetActive(true);
        }
    }

    /****************************************************/
    /********* Accurate gunshot audio stuff *************/
    /****************************************************/
    private List<AudioSource> gunAudioSources = new List<AudioSource>();
    private int curGunAudioIdx;
    private bool gunAudioIsScheduled = false;
    private double lastGunSoundTime = 0;

    void InitGunAudio()
    {
        if (gunAudioPrefab == null || gunAudioPrefab.clip == null) return;
        int numSourcesNeeded = 1 + Mathf.CeilToInt(gunAudioPrefab.clip.length / gunCooldownSecs);
        for (int i = 0; i < numSourcesNeeded; i++)
        {
            gunAudioSources.Add(Instantiate(gunAudioPrefab, parent: transform));
        }
        curGunAudioIdx = 0;
    }

    void FixedUpdateGunAudioState()
    {
        if (gunAudioPrefab == null) return;
        if (!Input.GetMouseButton(0))
        {
            gunAudioSources[curGunAudioIdx].Stop();
            gunAudioIsScheduled = false;
        }
    }

    void PlayGunAudio()
    {
        if (gunAudioPrefab == null) return;
        if (gunAudioIsScheduled)
        {
            lastGunSoundTime += gunCooldownSecs;
            curGunAudioIdx = (curGunAudioIdx + 1) % gunAudioSources.Count;
            gunAudioSources[curGunAudioIdx].PlayScheduled(
                lastGunSoundTime + gunCooldownSecs
            );
        }
        else
        {
            gunAudioSources[curGunAudioIdx].Play();
            lastGunSoundTime = AudioSettings.dspTime;

            curGunAudioIdx = (curGunAudioIdx + 1) % gunAudioSources.Count;
            gunAudioSources[curGunAudioIdx].PlayScheduled(
                lastGunSoundTime + gunCooldownSecs
            );
            gunAudioIsScheduled = true;
        }
    }
    /****************************************************/
}
