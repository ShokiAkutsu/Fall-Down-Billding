using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] Transform _player; // 追跡対象
	[SerializeField] float _yOffset = 5f; // プレイヤーとの高さの距離

	void LateUpdate() // プレイヤーの移動が終わった後に実行
	{
		if (_player) // Y方向、高さのみ追従
			transform.position = new Vector3(transform.position.x, _player.position.y + _yOffset, transform.position.z);
	}
}