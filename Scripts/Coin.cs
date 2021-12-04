using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nandan {
	public class Coin : MonoBehaviour
	{

		[SerializeField] private AudioSource audio;
		[SerializeField] private AudioClip hitClip;

		private void Awake(){
			audio = GameObject.Find("one shot sfx").GetComponent<AudioSource>();
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.tag.Equals("enemy")) {
				Destroy(collision.gameObject);
				GameObject.Find("GameManager").GetComponent<GameManager>().numEnemies--;
				audio.PlayOneShot(hitClip, 0.5f);
			}

			if(!collision.gameObject.tag.Equals("player")){
				Destroy(gameObject);
			}
		}
	}
}