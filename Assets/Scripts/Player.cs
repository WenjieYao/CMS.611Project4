using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/****************************************************/
// The Player script is used for defining player
// properties and player behaviors
/****************************************************/

public class Player : Singleton<Player>
{
    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
    // Player maximum health
    [SerializeField]
    private int maxHealth = 0;
    // Player health
    private int health = 0;
    // Player attack power
    [SerializeField]
    private int attackPower = 0;
    // Player fire rate
    // Number of bullets fired per second
    [SerializeField]
    private float fireRate = 0;
    // Player movement speed
    [SerializeField]
    private float speed = 0;
    
    // A temporary vector indicating the movement direction
    private Vector2 moveVector;
    // Current rigidbody2d object
    private Rigidbody2D rb2d;
    
    /****************************************************/
    // Public properties that corresponds to the private
    // properties above
    /****************************************************/
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            this.maxHealth = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            this.health = value;
        }
    }

    public int AttackPower
    {
        get
        {
            return attackPower;
        }
        set
        {
            this.attackPower = value;
        }
    }

    public float FireRate
    {
        get
        {
            return fireRate;
        }
        set
        {
            this.fireRate = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            this.speed = value;
        }
    }

    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        // Initialze player with full health
        health = maxHealth;
        // Get current rigidbody2d object
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update Position and Rotation of Player
        UpdatePosition();
    }



    // Move physical position upon update
    private void UpdatePosition()
    {
        moveVector = new Vector2(0, 0);

        if (Input.GetKey("w"))
            moveVector += Vector2.up;
        if (Input.GetKey("s"))
            moveVector += Vector2.down;
        if (Input.GetKey("a"))
            moveVector += Vector2.left;
        if (Input.GetKey("d"))
            moveVector += Vector2.right;

        moveVector.Normalize();
        rb2d.MovePosition(rb2d.position + speed * moveVector * Time.fixedDeltaTime);
    }

}