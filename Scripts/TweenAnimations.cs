using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nandan
{
	public class TweenAnimations : MonoBehaviour
	{

		public LeanTweenType easeType;

		public void OnHoverEnter()
		{

			LeanTween.scale(gameObject, new Vector3(0.95f, 0.95f, 0.95f), 0f).setEase(easeType);

		}

		public void OnHoverExit()
		{
			LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0f).setEase(easeType);
		}

		public void Random()
		{
			LeanTween.alpha(gameObject, 0, 1).setEase(easeType);
		}

	}
}
