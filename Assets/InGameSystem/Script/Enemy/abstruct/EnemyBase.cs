using UnityEngine;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour
{
	[Header("基本設定")]
	[SerializeField] protected float _speed = 3f;
	[SerializeField] protected float _stepInterval = 0.5f;
	protected Vector2Int _currentCoord;
	protected int _floorSize;
	protected FloorManager _floorManager;

	/// <summary>
	/// 敵の生成時に呼ばれる
	/// </summary>
	/// <param name="startCoord"></param>
	/// <param name="size"></param>
	public void Setup(Vector2Int startCoord, int size, FloorManager floorManager)
	{
		_currentCoord = startCoord;
		_floorSize = size;
		_floorManager = floorManager;
		StartCoroutine(MainLoop());
	}

	/// <summary>
	/// 共通の行動ループ
	/// </summary>
	/// <returns></returns>
	private IEnumerator MainLoop()
	{
		while (true)
		{
			Debug.Log($"{gameObject.name} : 思考中...");
			Vector2Int next = DecideNextCoord();

			Debug.Log($"{gameObject.name} : 移動中... 目標:{next}");
			yield return StartCoroutine(MoveVisualRoutine(next));

			_currentCoord = next;
			yield return new WaitForSeconds(_stepInterval);
		}
	}
	protected abstract Vector2Int DecideNextCoord();         // アルゴリズム（思考層）用
	protected abstract IEnumerator MoveVisualRoutine(Vector2Int next); // 見た目の動き（演出層）用
	/// <summary>
	/// 座標計算を行う
	/// </summary>
	/// <param name="coord"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	protected Vector3 GetLocalPos(Vector2Int coord, float y)
	{
		float offset = (_floorSize - 1) * -1f;
		return new Vector3(offset + coord.x * 2f, y, offset + coord.y * 2f);
	}

	/// <summary>
	/// 指定した座標に動いてもいいか聞く関数
	/// </summary>
	/// <param name="coord"></param>
	/// <returns></returns>
	protected bool IsSafe(Vector2Int coord)
	{
		bool isInside = coord.x >= 0 && coord.x < _floorSize && coord.y >= 0 && coord.y < _floorSize;
		if (!isInside) return false;

		return !_floorManager.IsHole(coord); // 穴が開く場所なら避けるようにする
	}
}