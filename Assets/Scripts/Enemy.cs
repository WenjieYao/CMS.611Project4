using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    	Vector2 moveDirection = (Vector2)(target.transform.position - transform.position);
    	rigidbody2D.MovePosition(rigidbody2D.position + moveDirection * speed * Time.fixedDeltaTime);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collisionGameObj = collision.gameObject;
        //Debug.Log(collisionGameObj);

        //if enemy hits player, inflict damage on champion
        if(collisionGameObj.GetComponent<Champion>() != null){
            collisionGameObj.GetComponent<Champion>().DealChampionDamage(attackPower);
        }

    }
}
