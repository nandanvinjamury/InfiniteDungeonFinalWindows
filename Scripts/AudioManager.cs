using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nandan
{
	public class AudioManager : MonoBehaviour
	{
		void Start()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}