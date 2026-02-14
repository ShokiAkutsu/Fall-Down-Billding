using UnityEngine;

public class PlayerGotMode : MonoBehaviour
{
	[SerializeField] bool _gotMode = false;
	[SerializeField] float _gotTime = 3f;
	float _timer = 0;

    void Update()
    {
		if (_gotMode)
		{
			_timer += Time.deltaTime;

			if (_gotTime < _timer)
				_gotMode = false;
		}
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (!_gotMode && collision.gameObject.tag == "Enemy")
		{
			_timer = 0; // デバッグ用数値を初期化
			_gotMode = true;
			CameraEffectController.Instance.SetDamageLevel();
		}
	}
}
