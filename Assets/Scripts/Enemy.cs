using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    // Enemy's attacking target
	[SerializeField]
	private GameObject target = null;

	// Current rigidbody2d object
    private Rigidbody2D rigidbody2D;
    //Enemy movement speed
    [SerializeField]
    private float speed = 2;

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

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	Vector2 moveDirection = (Vector2)(target.transform.position - transform.position);
    	rigidbody2D.MovePosition(rigidbody2D.position + moveDirection * speed * Time.fixedDeltaTime);
    }
}
