using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Tags of champion attack prefabs that the enemy takes damage from.
    public List<GameObject> vulnerableAttackPrefabs;
    public List<string> vulnerableAttackTags;
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If enemy hits player, inflict damage on champion
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<Champion>().DealChampionDamage(attackPower);
            Destroy(gameObject);
        }

        if (vulnerableAttackTags.Contains(collision.gameObject.tag))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                TakeDamage(1);
                StartCoroutine(
                    Knockback(10 * projectile.Direction, 0.25f)
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
