using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nandan
{
	public class RandomCameraMovement : MonoBehaviour
	{
		[SerializeField] private Vector2 min;
		[SerializeField] private Vector2 max;

		[SerializeField] private float lerpSpeed = 0.05f;

		private Vector3 newPosition;

		// Start is called before the first frame update
		void Start()
		{
			newPosition = transform.position;
		}

		// Update is called once per frame
		void Update()
		{
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * lerpSpeed);

			if (Vector3.Distance(transform.position, newPosition) < 1f)
			{
				GetNewPosition();
			}
		}

		private void GetNewPosition()
		{
			var xPos = Random.Range(min.x, max.x);
			var yPos = Random.Range(min.y, max.y);
			newPosition = new Vector3(xPos, yPos, -10);
		}
	}
}