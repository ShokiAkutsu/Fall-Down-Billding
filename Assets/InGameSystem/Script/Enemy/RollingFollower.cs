using System.Collections.Generic;
using UnityEngine;
using System.Linq; // 便利機能を使うために追加

public class RollingFollower : VisualRoll
{
	protected override Vector2Int DecideNextCoord()
	{
		Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

		List<Vector2Int> candidates = new List<Vector2Int>(); // 移動できる場所をリスト化
		foreach (var dir in directions)
		{
			Vector2Int target = _currentCoord + dir;
			if (IsSafe(target))
			{
				candidates.Add(target);
			}
		}

		if (candidates.Count == 0) return _currentCoord; // 動けなかったら留まる

		Vector2Int playerPos = _floorManager.PlayerPos;
		
		Vector2Int bestMove = candidates[0]; // 候補地の中から「プレイヤーとの距離」が最小になるものを選ぶ
		float minDistance = float.MaxValue;

		foreach (var move in candidates)
		{
			float dist = Vector2.Distance(move, playerPos); // ベクトルの差の長さを計算
			if (dist < minDistance)
			{
				minDistance = dist;
				bestMove = move;
			}
		}

		return bestMove;
	}
}