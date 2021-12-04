using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nandan
{
	public class FollowPlayer : MonoBehaviour
	{
		[SerializeField] private Transform player;

		// Update is called once per frame
		void Update()
		{
			transform.position = new Vector3(player.position.x, player.position.y, 0);
		}
	}
}