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
			if (target.x >= 0 && target.x < _floorSize && target.y >= 0 && target.y < _floorSize)
			{
				if (target != _lastCoord || _lastCoord == _currentCoord)
					candidates.Add(target);
			}
		}

		Vector2Int next = candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : _lastCoord;
		_lastCoord = _currentCoord; // Ÿ‚Ìƒ^[ƒ“‚Ì‚½‚ß‚É¡‚ÌêŠ‚ğ‹L‰¯
		return next;
	}
}
