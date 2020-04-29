using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

/****************************************************/
// The Enemy script is used for defining enemy
// properties and enemy behaviors
/****************************************************/
public class Enemy : MonoBehaviour
{
    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
    // Enemy's attacking target
    public GameObject target = null;
    private bool sensedTarget = false;

    public int maxHealth = 5;
    public int health;

    public int attackPower = 1;
    public float knockbackPower = 3f;

    // Tags of champion attack prefabs that the enemy takes damage from.
    public List<GameObject> vulnerableAttackPrefabs;
    public List<string> vulnerableAttackTags;

    public AudioSource bulletBounceAudioPrefab;
    public AudioSource takeDamageAudioPrefab;
    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        foreach (var attack in vulnerableAttackPrefabs)
        {
            vulnerableAttackTags.Add(attack.tag);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Once target has been sensed, move to target.
        // This happens even if the character moves out of range again.
        if (sensedTarget)
        {
            GetComponent<AIDestinationSetter>().target = target.transform;
            GetComponent<AIPath>().canSearch = true;
        }

        // Die when go over chasm.
        Tilemap chasmMap = GameObject.FindGameObjectWithTag("Chasms").GetComponent<Tilemap>();
        if (chasmMap.HasTile(chasmMap.WorldToCell(this.transform.position)))
        {
            TakeDamage(10000);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Champion champ = collision.gameObject.GetComponent<Champion>();
        if (champ != null) // Enemy hits player
        {
            champ.DealChampionDamage(attackPower); // deal damage to champion
            champ.GetComponent<Rigidbody2D>().AddForce( // knockback
                knockbackPower * GetComponent<Rigidbody2D>().velocity.normalized,
                mode: ForceMode2D.Impulse
            );
            Destroy(gameObject);
        }

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (vulnerableAttackTags.Contains(projectile.tag))
            {
                TakeDamage(1);
                StartCoroutine(Knockback(projectile.velocity, 0.2f));
            }
            else
            {
                StartCoroutine(Knockback(projectile.velocity, 0.2f));
                GameManager.Instance.globalAudioSource.PlayOneShot(
                    bulletBounceAudioPrefab.clip,
                    bulletBounceAudioPrefab.volume
                );
            }
        }

        // This is broken :(
        if (collision.gameObject.tag.Equals("sword") && this.tag.Equals("enemygreen"))
        {
            Debug.Log("Enemy attacked by sword");
            TakeDamage(3);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // If enemy is in range of target, enable moving to target
        if (other.gameObject.tag.Equals("Player"))
        {
            target = other.gameObject;
            sensedTarget = true;
        }
    }

    void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        GameManager.Instance.globalAudioSource.PlayOneShot(
            takeDamageAudioPrefab.clip,
            takeDamageAudioPrefab.volume
        );
        if (health <= 0) { Destroy(gameObject); }
    }

    IEnumerator Knockback(Vector2 velocity, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            Vector2 curVelocity = Vector2.Lerp(velocity, Vector2.zero, elapsed / duration);
            GetComponent<AIPath>().Move(curVelocity * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
