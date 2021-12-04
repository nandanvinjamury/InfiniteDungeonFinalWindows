using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Nandan
{
	public class GameManager : MonoBehaviour
	{

		public bool isSplashScreen;
		public RectTransform blackImage;
		private AudioSource audio;
		public AudioClip clip;
		public float numEnemies;

		// Start is called before the first frame update
		void Start()
		{
			numEnemies = 10;
			audio = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioSource>();
			FadeIn();
			if (isSplashScreen)
			{
				Invoke("FadeOut", 5);
			}
		}

		public void Update()
		{
			if(numEnemies <= 0){
				FadeOut();
			}	
		}

		public void FadeIn()
		{
			LeanTween.alpha(blackImage, 0, 2f).setEase(LeanTweenType.easeOutSine);
			Invoke("DisableBlack", 2f);
		}

		public void DisableBlack(){
			blackImage.gameObject.SetActive(false);
		}

		public void FadeOut()
		{
			blackImage.gameObject.SetActive(true);
			LeanTween.alpha(blackImage, 1, 2f).setEase(LeanTweenType.easeInSine);

			if (SceneManager.GetActiveScene().buildIndex == 3)
			{
				Invoke("DungeonScene", 3f);
			}
			else
			{
				Invoke("NextScene", 3f);
			}
		}

		public void FadeOutWLaugh()
		{
			blackImage.gameObject.SetActive(true);
			audio.PlayOneShot(clip);
			LeanTween.alpha(blackImage, 1, 2f).setEase(LeanTweenType.easeInSine);
			if (SceneManager.GetActiveScene().buildIndex > 3)
			{
				Invoke("DungeonScene", 3f);
			} else {
				Invoke("NextScene", 3f);
			}
		}


		public void NextScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		public void DungeonScene()
		{
			SceneManager.LoadScene(3);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}