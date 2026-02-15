using System.Collections.Generic;
using UnityEngine;

public class RollingRandom : VisualRoll
{
	private Vector2Int _lastCoord;

	protected override Vector2Int DecideNextCoord()
	{
		Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
		List<Vector2Int> candidates = new List<Vector2Int>();

		foreach (var dir in directions)
		{
			Vector2Int target = _currentCoord + dir;
			if (IsSafe(target) && !IsOccupiedByOtherEnemy(target))
			{
				if (target != _lastCoord || _lastCoord == _currentCoord)
					candidates.Add(target);
			}
		}

		Vector2Int next = candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : _lastCoord;

		if (candidates.Count > 0)
			next = candidates[Random.Range(0, candidates.Count)]; // 先が合ったら進む
		else
			next = IsSafe(_lastCoord) ? _lastCoord : _currentCoord; // 先がなかったら戻る

		_lastCoord = _currentCoord; // 次のターンのために今の場所を記憶
		return next;
	}
}
