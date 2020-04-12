using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    /****************************************************/
    /***************** Basic Properties *****************/
    /****************************************************/
	[SerializeField]
	private float velocity = 2.0f;
	// A normalized vector representing the projectile's trajectory
	private Vector2 trajectory;


    /****************************************************/
    // Public properties that corresponds to the private
    // properties above
    /****************************************************/
	public float Velocity
	{
		get{
			return velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

    public Vector2 Trajectory
    {
        get{
            return trajectory;
        }
        set
        {
            this.trajectory = value;
        }
    }

    /****************************************************/

    /****************************************************/
    /***************** Basic Functions ******************/
    /****************************************************/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
    	MoveProjectile();
    }

    /*Set the trajectory of the projectile*/
    public void SetTrajectory(Vector2 trajectory){
    	this.trajectory = trajectory.normalized;
    }

    /*Move the projectile according to its velocity and trajectory*/
    public void MoveProjectile(){
    	transform.Translate(trajectory * velocity * Time.fixedDeltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
    	//if projectile hits the wall, destroy projectile
    	if(collision.gameObject.tag.Equals("Wall")){
    		Destroy(this.gameObject);
    	}

    }
}
