using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 階層ごとにまとめる親ObjectにつけるScript
/// 各、床の管理をする
/// 出来れば、SwitchOn()は別クラスにしたいところ
/// </summary>
public class FloorManager : MonoBehaviour
{
	List<Vector2Int> _switchList = new List<Vector2Int>();
    GameObject _holeFloor;
	Dictionary<Vector2Int, BlockSprite> _allBlocks = new Dictionary<Vector2Int, BlockSprite>(); // ブロックの場所を覚えておく
	bool _isActive = false;
	[SerializeField] private Vector2Int _targetCoord = new Vector2Int(0, 0);
	List<Vector2Int> _switchOrder = new List<Vector2Int>();
	GameObject _clearTargetFloor; // スライドさせる対象

	public void SetupClearFloor(Vector2Int coord)
	{
		
		if (_allBlocks.ContainsKey(coord)) // 住所録から、指定された座標のブロックを探す
		{
			_clearTargetFloor = _allBlocks[coord].gameObject;// 見つかったら、そのGameObjectをクリア対象として保存

			Debug.Log($"座標 {coord} の床をクリア対象に設定しました。");
		}
		else
		{
			Debug.LogError($"座標 {coord} に床が見つかりません！住所録への登録順序を確認してください。");
		}
	}

	public void SetSwitchSequence(List<Vector2Int> coords)
	{
		_switchOrder = coords; // リスト数を自分の変数に保存

		Debug.Log($"スイッチの順序を{_switchOrder.Count}個受信しました。");
	}

	public void RegisterBlock(Vector2Int coord, BlockSprite block)
	{
		if (!_allBlocks.ContainsKey(coord))
		{
			_allBlocks.Add(coord, block);
			block.SetManager(this);
		}
	}

	/// <summary>
	/// 対応する床が踏まれたときに呼ばれる
	/// 次のスイッチがあるなら、アルゴリズムで辿って次のスイッチを光らせる
	/// 呼ばれた元で最後なら、対応の床を開ける
	/// </summary>
	public void SwitchOn(bool isRemove)
	{
		if (_switchOrder.Count == 0) return;

		Vector2Int start = _switchOrder[0]; // 今の場所を記憶

		if (isRemove)
			_switchOrder.RemoveAt(0);

		if (_switchOrder.Count > 0) // まだ次に踏むべきスイッチがあるなら、そこまで光を走らせる
		{
			Vector2Int end = _switchOrder[0];
			StartCoroutine(ShowPathRoutine(start, end));
		}
		else // 全て踏み終わり
		{
			StartCoroutine(FloorClearRoutine());
		}
	}

	IEnumerator FloorClearRoutine()
	{
		Debug.Log("Floor Clear!");
		yield return new WaitForSeconds(0.5f);

		if (_clearTargetFloor) // 床を動かす
		{
			yield return StartCoroutine(MoveSingleBlock(_clearTargetFloor));
		}
		else
		{
			Debug.LogWarning("動かす床が設定されていません！");
		}
	}

	IEnumerator MoveSingleBlock(GameObject block)
	{
		Vector3 currentPos = block.transform.position; // ブロックの現在位置からグリッド座標を割り出す想定です
		int x = Mathf.RoundToInt(currentPos.x);
		int z = Mathf.RoundToInt(currentPos.z);

		int maxIndex = 4;

		Vector3 slideDir = Vector3.zero; // 動かす方向

		if (x == maxIndex) slideDir = Vector3.left * 2; // 右端なら左へ
		else slideDir = Vector3.right * 2;             // そのほかは右へ

		yield return new WaitForSeconds(0.5f);

		Vector3 startPos = block.transform.position;
		Vector3 downPos = startPos + Vector3.down * 1.1f; // 床の厚み分下げる

		float elapsed = 0;
		while (elapsed < 0.8f)
		{
			elapsed += Time.deltaTime;
			block.transform.position = Vector3.Lerp(startPos, downPos, elapsed / 0.8f);
			yield return null;
		}

		Vector3 finalPos = downPos + slideDir;
		elapsed = 0;

		while (elapsed < 1.2f) // 横にスライドさせる
		{
			elapsed += Time.deltaTime;
			float t = Mathf.SmoothStep(0, 1, elapsed / 1.2f);
			block.transform.position = Vector3.Lerp(downPos, finalPos, t);
			yield return null;
		}
	}

	IEnumerator ShowPathRoutine(Vector2Int start, Vector2Int end)
	{
		Vector2Int current = start;

		while (current.x != end.x) // X座標の移動
		{
			current.x += (end.x > current.x) ? 1 : -1;
			FlashBlockAt(current);
			yield return new WaitForSeconds(0.1f); // 光る速さ
		}

		while (current.y != end.y) // Z座標の移動
		{
			current.y += (end.y > current.y) ? 1 : -1;
			FlashBlockAt(current);
			yield return new WaitForSeconds(0.1f);
		}

		if (_allBlocks.ContainsKey(end)) // 最後まで辿り着いたら踏めるようにする
		{
			var nextSwitch = _allBlocks[end] as SwitchFloorSystem;
			if (nextSwitch) nextSwitch.ActiveFloor();
		}
	}

	void FlashBlockAt(Vector2Int coord)
	{
		if (_allBlocks.ContainsKey(coord))
		{
			_allBlocks[coord].Flash(); // BlockScriptで定義した演出
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !_isActive)// Playerが始めてこの階層に降り立ったら処理を行う
		{
			_isActive = true;
			SwitchOn(false);
		}
	}
}
