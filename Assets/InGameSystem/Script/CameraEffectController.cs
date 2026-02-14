using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class CameraEffectController : MonoBehaviour
{
	public static CameraEffectController Instance { get; private set; }

	[SerializeField] Image _endPanel;
	[SerializeField] Volume _volume;
	ChromaticAberration _chromatic;
	FilmGrain _grain;
	int _hitCount = 0;
	[SerializeField] int _maxHit = 4;
	bool _isFading = false;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	void Start()
	{
		_volume.profile.TryGet(out _chromatic);
		_volume.profile.TryGet(out _grain);
	}

	// 敵に当たった時に呼ぶ
	public void SetDamageLevel()
	{
		_hitCount++;

		float t = (float)_hitCount / _maxHit; // 調整 (0.0 ～ 1.0)

		if (_chromatic)
		{
			_chromatic.intensity.overrideState = true;
			_chromatic.intensity.value = t;
		}

		if (_grain)
		{
			_grain.intensity.overrideState = true;
			_grain.intensity.value = t;
		}

		if (_hitCount >= _maxHit + 1 && !_isFading)
			StartCoroutine(FadeOutRoutine());
	}

	IEnumerator FadeOutRoutine()
	{
		_isFading = true;
		float elapsed = 0f;
		float duration = 2f; // 暗くなる時間

		Color c = _endPanel.color;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			// Mathf.Lerp で Alphaを 0 から 1 へ
			c.a = Mathf.Lerp(0, 1, elapsed / duration);
			_endPanel.color = c;
			yield return null;
		}

		// 完全に暗くなった後の処理（リロードなど）
		Debug.Log("完全暗転。リトライ待ち...");
	}
}