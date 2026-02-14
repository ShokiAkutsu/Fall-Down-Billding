using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 今回のプロジェクトでは不必要になった
/// </summary>
public class RandomWalk : MonoBehaviour
{
	[SerializeField] float _speed = 3f;
	[SerializeField] float _stepInterval = 0.5f; // 次のマスへ動くまでの待ち時間

	Vector2Int _currentCoord;
	Vector2Int _lastCoord;
	int _floorSize;

	// 生成時にGeneratorから呼んでもらう初期化関数
	public void Setup(Vector2Int startCoord, int size)
	{
		_currentCoord = startCoord;
		_lastCoord = startCoord; // 最初は「戻らない場所」がないので自分自身にする
		_floorSize = size;

		StartCoroutine(WalkRoutine());
	}

	IEnumerator WalkRoutine()
	{
		while (true)
		{
			Vector2Int next = ChooseNextCoord();

			// 2. 移動演出（ワールド座標に変換）
			// Generatorと同じ計算式で目標地点を出す（offsetはSetupで渡してもOK）
			float offset = (_floorSize - 1) * -1f;
			Vector3 targetLocalPos = new Vector3(offset + next.x * 2f, 1.2f, offset + next.y * 2f);

			yield return StartCoroutine(MoveTo(targetLocalPos));

			// 3. 座標の更新
			_lastCoord = _currentCoord;
			_currentCoord = next;

			yield return new WaitForSeconds(_stepInterval);
		}
	}

	Vector2Int ChooseNextCoord()
	{
		// 上下左右の4方向
		Vector2Int[] directions = {
			Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
		};

		List<Vector2Int> candidates = new List<Vector2Int>();

		foreach (var dir in directions)
		{
			Vector2Int target = _currentCoord + dir;

			// 枠内であること、かつ「直前にいた場所」ではないこと
			if (target.x >= 0 && target.x < _floorSize &&
				target.y >= 0 && target.y < _floorSize &&
				target != _lastCoord)
			{
				candidates.Add(target);
			}
		}

		// もし行き止まり（1x1など）なら、仕方ないので戻ることを許可する
		if (candidates.Count == 0) return _lastCoord;

		// 候補からランダムに1つ選ぶ
		return candidates[Random.Range(0, candidates.Count)];
	}

	IEnumerator MoveTo(Vector3 target)
	{
		Vector3 startPos = transform.localPosition;
		Quaternion startRot = transform.localRotation;

		// 1. 移動方向ベクトル（ワールド空間）
		Vector3 moveDir = (target - startPos).normalized;

		// 2. 回転軸の計算（ワールド空間の軸を出す）
		// 進む方向 moveDir と「上」の外積をとれば、転がるための「横軸」が常に正しく出ます
		Vector3 worldRotationAxis = Vector3.Cross(Vector3.up, moveDir);

		float elapsed = 0;
		float duration = 0.4f;
		float archHeight = 0.4f;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float percent = elapsed / duration;

			// --- 位置の移動（放物線） ---
			Vector3 currentPos = Vector3.Lerp(startPos, target, percent);
			float yOffset = Mathf.Sin(percent * Mathf.PI) * archHeight;
			currentPos.y = 1.5f + yOffset;
			transform.localPosition = currentPos;

			// --- 回転の計算（ここが重要！） ---
			// 「ワールドの軸」で90度回転するクォータニオンを作る
			Quaternion worldRotation = Quaternion.AngleAxis(90 * percent, worldRotationAxis);

			// 元の回転（startRot）の「左側」から掛けることで、
			// 今のサイコロがどんな向きでも、世界基準の方向にパタンと倒れます
			transform.localRotation = worldRotation * startRot;

			yield return null;
		}

		// --- 移動完了後の補正 ---
		target.y = 1.5f;
		transform.localPosition = target;

		// 90度単位でピタッと止める
		Vector3 rot = transform.localRotation.eulerAngles;
		rot.x = Mathf.Round(rot.x / 90) * 90;
		rot.y = Mathf.Round(rot.y / 90) * 90;
		rot.z = Mathf.Round(rot.z / 90) * 90;
		transform.localRotation = Quaternion.Euler(rot);
	}
}
