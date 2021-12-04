using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Nandan{ 
	public class EnemyController : MonoBehaviour
	{

		private Rigidbody2D rb2d;
		[SerializeField] private float moveSpeed;
		private Vector3 point;
		private NavMeshHit hit;

		public GameObject syringe;
		public float wanderRadius;
		public float wanderTimer;
		private Transform player;

		private NavMeshAgent agent;
		private float timer;

		private float shootTimer;

		void Start()
		{
			Application.targetFrameRate = 120;
			rb2d = GetComponent<Rigidbody2D>();
			agent = GetComponent<NavMeshAgent>();
			player = GameObject.FindGameObjectWithTag("player").transform;
			wanderTimer = 5;
			wanderRadius = 8;
			shootTimer = 4;
			agent.updateRotation = false;
			agent.updateUpAxis = false;
			timer = wanderTimer;
		}

		// Update is called once per frame
		void Update()
		{
			timer += Time.deltaTime;
			shootTimer -= Time.deltaTime;


			transform.rotation = Quaternion.Euler(Vector3.zero);


			if (timer >= wanderTimer || agent.remainingDistance <= 0.5f)
			{
				Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
				agent.SetDestination(newPos);
				timer = 0;
			}

			if(Vector3.Distance(transform.position, player.position) < 10f && shootTimer <= 0){

				ShootPlayer();
				shootTimer = 4;
			}
		}


		public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
		{
			Vector3 randDirection = Random.insideUnitCircle * dist;
			randDirection += origin;
			NavMeshHit navHit;
			NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
			return navHit.position;
		}

		private void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere(transform.position, wanderRadius);
		}

		public void ShootPlayer(){

			Instantiate(syringe, transform.position, Quaternion.identity);
		}

	}
}