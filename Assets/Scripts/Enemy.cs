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
	[SerializeField]
	private GameObject target = null;
    public AIPath aiPath;
	// Current rigidbody2d object
    private Rigidbody2D rigidbody2D;
    //Enemy movement speed
    [SerializeField]
    private float speed = 1.0f;
    private int health;
    [SerializeField]
    private int maxHealth = 5;
    [SerializeField]
    private int attackPower = 1;
    private bool sensedTarget = false;

	public GameObject Target
	{
		get{
			return target;
		}
		set
		{
			this.target = value;
		}
	}

    public float Speed{
        get
        {
            return speed;
        }
        set
        {
            this.speed = value;
        }
    }

    public int Health{
        get
        {
            return health;
        }
        set
        {
            this.health = value;
        }
    }

    public int MaxHealth{
        get
        {
            return maxHealth;
        }
        set
        {
            this.maxHealth = value;
        }
    }


    public int AttackPower{
        get
        {
            return attackPower;
        }
        set
        {
            this.attackPower = value;
        }
    }


    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
            //Once target has been sensed, move to target. This happens even if the character moves out of range again.
            if(sensedTarget)
            {
                aiPath.canSearch = true;
                //Vector2 moveDirection = (Vector2)(target.transform.position - transform.position);
                //rigidbody2D.MovePosition(rigidbody2D.position + moveDirection * speed * Time.fixedDeltaTime);
                //sensedTarget = false;
            }

    }


    void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collisionGameObj = collision.gameObject;

        //if enemy hits player, inflict damage on champion
        if(collisionGameObj.GetComponent<Champion>() != null)
        {
            collisionGameObj.GetComponent<Champion>().DealChampionDamage(attackPower);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        GameObject collisionGameObj = other.gameObject;


        //Handles circle collider behavior. If enemy is in range of target, enable moving to target
        if(collisionGameObj == target)
        {
            sensedTarget = true;
        }
    }
}
