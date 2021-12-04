using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

	private Transform player;
	private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.FindGameObjectWithTag("player").transform;
		rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector2 direction = (Vector2)player.position - rb.position;
		direction.Normalize();
		float rotateAmt = Vector3.Cross(direction, transform.up).z;

		rb.angularVelocity = -rotateAmt * 200f;
		rb.velocity = transform.up * 5f;
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag.Equals("player") || collision.gameObject.tag.Equals("wall")){
			Destroy(this.gameObject);
		}
	}
}
