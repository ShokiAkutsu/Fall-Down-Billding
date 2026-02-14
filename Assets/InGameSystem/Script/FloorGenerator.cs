using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FloorGenerator : MonoBehaviour
{
	[Header("設定")]
	[SerializeField] List<FloorStateSO> _floorStates; // 階層の情報
	[SerializeField] List<GameObject> _floor;   // ブロックのPrefab
	[SerializeField] GameObject _floorParent;	// 階層ごとの床の管理をする空の親オブジェクト
	[SerializeField] float _floorHeight = 10f;  // 階層ごとの高さ（下の階をどこに作るか）

	GameObject currentFloorObj; // 今いる階層の親
	GameObject nextFloorObj;    // 次の（下に控えている）階層の親

	void Start()
	{
		for (int i = 1; i <= _floorStates.Count; i++)
			SpawnFloor(i);      // Y座標を階層ごとにずらしながら生成
	}

	void SpawnFloor(int floorNum) // 指定したY座標に、階層に応じたサイズの床を作る
	{
		FloorStateSO state = _floorStates[floorNum - 1];
		int size = state.FloorSize;
		int switchCount = state.TotalSwitches;

		GameObject floorRoot = Instantiate(_floorParent);
		floorRoot.name = $"Floor_{floorNum}";
		floorRoot.transform.position = new Vector3(0, -_floorHeight * (floorNum - 1), 0);
		FloorManager manager = floorRoot.GetComponent<FloorManager>();

		List<Vector2Int> allCoords = new List<Vector2Int>(); // スイッチ座標の抽選箱
		for (int ix = 0; ix < size; ix++)
		{
			for (int iz = 0; iz < size; iz++)
			{
				allCoords.Add(new Vector2Int(ix, iz));
			}
		}
		
		List<Vector2Int> selectedSwitchCoords = allCoords // ランダムにスイッチの数だけ座標を抽出
			.OrderBy(c => System.Guid.NewGuid())
			.Take(switchCount)
			.ToList();

		
		manager.SetSwitchSequence(selectedSwitchCoords); // スイッチの順番を教えておく

		Vector2Int slideTargetCoord = allCoords
		.OrderBy(c => System.Guid.NewGuid())
		.First();

		List<Vector2Int> enemyCandidates = allCoords // 敵の出現場所を計算
			.Except(selectedSwitchCoords)
			.Where(c => c != slideTargetCoord)
			.ToList();

		int enemyCount = state.EnemyList.Count;

		List<Vector2Int> enemyCoords = enemyCandidates
			.OrderBy(c => System.Guid.NewGuid())
			.Take(enemyCount)
			.ToList();

		float offset = (size - 1) * -1; // 全体の大きさから幅を計算

		for (int x = 0; x < size; x++)
		{
			for (int z = 0; z < size; z++)
			{
				Vector2Int currentCoord = new Vector2Int(x, z);

				int index = selectedSwitchCoords.Contains(currentCoord) ? 1 : 0;

				Vector3 localPos = new Vector3(offset + x * 2, 0, offset + z * 2);
				GameObject block = Instantiate(_floor[index], Vector3.zero, Quaternion.identity);
				block.transform.localPosition = localPos; // ここで床の場所を確定
				
				if (enemyCoords.Contains(currentCoord)) // もしここが敵の座標なら
				{
					GameObject enemy = Instantiate(state.EnemyList[0], floorRoot.transform);
					enemy.transform.localPosition = localPos + Vector3.up * 1.5f; // 床と被らないように高さを揃える

					EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
					if (enemyScript != null)
					{
						enemyScript.Setup(currentCoord, size, manager); // 現在の座標(currentCoord)とステージサイズ(size)を渡す
					}
				}

				block.transform.SetParent(floorRoot.transform);
				block.transform.localPosition = localPos;
				
				BlockSprite sprite = block.GetComponent<BlockSprite>(); // マネージャーに登録
				manager.RegisterBlock(currentCoord, sprite);
			}
		}

		// --- クリア対象のセット ---
		manager.SetupClearFloor(slideTargetCoord);

		if (floorNum == 1) currentFloorObj = floorRoot;
		else nextFloorObj = floorRoot;
	}
}