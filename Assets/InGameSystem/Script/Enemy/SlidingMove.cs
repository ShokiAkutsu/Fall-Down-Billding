using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlidingMove : VisualSlide
{
	private Vector2Int _moveDir = Vector2Int.up; // 初期方向

	[Header("警告のObject")]
	public GameObject _indicatorPrefab;

	protected override Vector2Int DecideNextCoord()
	{
		Vector2Int target = _currentCoord;
		Vector2Int checkPos = _currentCoord + _moveDir;

		// 最初の1歩目が「穴」または「他の敵」なら、その方向は諦めて方向転換
		if (!IsInsideFloor(checkPos) || !IsSafe(checkPos) || IsOccupiedByOtherEnemy(checkPos))
		{
			_moveDir = GetRandomDirection();
			return _currentCoord;
		}

		// 滑り出し開始
		while (IsInsideFloor(checkPos))
		{
			// もし「進む先」が穴、または「他の敵」がいたら、その手前でストップ！
			if (!IsSafe(checkPos) || IsOccupiedByOtherEnemy(checkPos))
			{
				break; // whileループを抜ける
			}

			target = checkPos;
			checkPos += _moveDir;
		}

		return target;
	}

	protected override IEnumerator MoveVisualRoutine(Vector2Int next)
	{
		if (next == _currentCoord) yield break;

		if (_indicatorPrefab != null)
		{
			List<GameObject> indicators = new List<GameObject>();
			Vector2Int tempCoord = _currentCoord + _moveDir;

			while (true)
			{
				GameObject ind = Instantiate(_indicatorPrefab, transform.parent);
				ind.transform.localPosition = GetLocalPos(tempCoord, 1.05f);
				indicators.Add(ind);// 滑る前に警告を出す

				if (tempCoord == next) break;
				tempCoord += _moveDir;
			}

			yield return new WaitForSeconds(0.8f); // 警告を出して置く時間

			foreach (var ind in indicators) Destroy(ind); // 出していた警告を消す
		}

		yield return StartCoroutine(base.MoveVisualRoutine(next)); // 移動する
	}

	private bool IsInsideFloor(Vector2Int coord)
	{
		return coord.x >= 0 && coord.x < _floorSize && coord.y >= 0 && coord.y < _floorSize;
	}

	private Vector2Int GetRandomDirection()
	{
		Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
		return dirs[Random.Range(0, dirs.Length)];
	}
}