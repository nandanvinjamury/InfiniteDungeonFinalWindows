using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nandan
{
	public class PlayerController : MonoBehaviour
	{

		private Rigidbody2D rb2d;
		[SerializeField] private float moveSpeed;
		[SerializeField] private ParticleSystem particles;
		private float health;
		[SerializeField] private Image healthbar;
		[SerializeField] private AudioSource audio;
		[SerializeField] private AudioSource footsteps;
		[SerializeField] private AudioClip hitClip;
		[SerializeField] private AudioClip coinClip;
		private float shootTimer;
		[SerializeField] private GameObject coin;
		private Camera cam;

		// Start is called before the first frame update
		void Start()
		{
			Application.targetFrameRate = 120;
			rb2d = GetComponent<Rigidbody2D>();
			health = 100;
			shootTimer = 1f;
			cam = Camera.main;
		}

		// Update is called once per frame
		void Update()
		{
			shootTimer -= Time.deltaTime;

			transform.Rotate(0, 0, -Input.GetAxisRaw("Horizontal") * 5);
			Vector3 velocity = transform.TransformDirection(new Vector3(0, Input.GetAxisRaw("Vertical"), 0)) * moveSpeed;
			rb2d.velocity = velocity;
			if (Input.GetAxisRaw("Vertical") != 0)
			{
				footsteps.mute = false;
			}
			else{
				footsteps.mute = true;
			}


			if((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && shootTimer <= 0){
				Shoot();
				shootTimer = 0.5f;
			}
		}

		private void Shoot(){
			audio.PlayOneShot(coinClip, 0.5f);
			Vector3 mousePos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
			Vector2 direction = (Vector2)mousePos - (Vector2)transform.position;
			Quaternion rotation = new Quaternion();
			rotation.eulerAngles = Vector3.zero;
			GameObject projectile = Instantiate(coin, transform.position, Quaternion.identity);
			projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x * 10f, direction.y * 7f);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if((collision.gameObject.tag.Equals("enemy") || collision.gameObject.tag.Equals("missile")) && health > 0){
				particles.Play();
				audio.PlayOneShot(hitClip, 0.5f);
				health -= 16.67f;
				healthbar.fillAmount = (health / 100f);
				if (health <= 0)
				{
					FindObjectOfType<GameManager>().FadeOutWLaugh();
				}
			}
		}
	}
}