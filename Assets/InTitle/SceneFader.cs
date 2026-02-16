using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
	public Image fadeImage;
	public GameObject loadingText;
	public float fadeSpeed = 1.0f;

	// シーンが始まったら勝手にフェードイン
	void Start()
	{
		loadingText.SetActive(false); // ロード中テキストを消す
		StartCoroutine(FadeIn());
	}

	public void FadeAndLoad(string sceneName)
	{
		StartCoroutine(AsyncLoadFlow(sceneName));
	}

	IEnumerator AsyncLoadFlow(string sceneName)
	{
		// 1. グレーにする
		float alpha = 0;
		while (alpha < 1)
		{
			alpha += Time.deltaTime * fadeSpeed;
			fadeImage.color = new Color(0.5f, 0.5f, 0.5f, alpha);
			yield return null;
		}

		// 2. ロード中表示
		loadingText.SetActive(true);

		// 3. シーン読み込み（ここが完了すると、このキャンバスごと破棄されます）
		SceneManager.LoadScene(sceneName);
	}

	IEnumerator FadeIn()
	{
		// 最初からグレーにしておき、透明にしていく
		float alpha = 1;
		while (alpha > 0)
		{
			alpha -= Time.deltaTime * fadeSpeed;
			fadeImage.color = new Color(0.5f, 0.5f, 0.5f, alpha);
			yield return null;
		}
	}
}