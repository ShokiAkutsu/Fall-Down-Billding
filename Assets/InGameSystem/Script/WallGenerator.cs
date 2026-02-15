using UnityEngine;

public class WallGenerator : MonoBehaviour
{
	[Header("設定")]
	[SerializeField] GameObject _wallPrefab; // 壁にするCubeのPrefab
	[SerializeField] float _wallHeight = 2.0f; // 壁の高さ
	[SerializeField] bool _hideFrontWall = true; // 手前を透明にするか
	[SerializeField] int _wallLayers = 3;

	/// <summary>
	/// ステージを囲む壁を生成する
	/// </summary>
	/// <param name="width">マスの横数</param>
	/// <param name="height">マスの縦数</param>
	/// <param name="offset">WorldToCoordで使用しているオフセット（例: 4f）</param>
	public void GenerateOuterWalls(int width, int height, Transform floorParent)
	{
		for (int x = -1; x <= width; x++)
		{
			for (int z = -1; z <= height; z++)
			{
				bool isOuterEdge = (x == -1 || x == width || z == -1 || z == height);

				if (isOuterEdge)
				{
					// ここで段数の分だけループを回す
					for (int layer = 0; layer < _wallLayers; layer++)
					{
						// 段数に応じてY座標をずらす (壁の高さが2なら、0, 2, 4...となる)
						float wallY = layer * _wallHeight;
						SpawnWall(x, z, width, height, wallY, floorParent, z == -1);
					}
				}
			}
		}
	}

	void SpawnWall(int x, int z, int width, int height, float yOffset, Transform parent, bool isFront)
	{
		// GridUtilsに「このマスの座標をちょうだい」と頼むだけ
		// yOffset（段数 * 高さ）に、壁の半分の高さを足して中心を合わせる
		float finalY = yOffset + (_wallHeight / 2f);

		// 座標計算はここ1箇所に集約！
		Vector3 localPos = GridUtils.ToWorld(x, z, width, finalY);

		GameObject wall = Instantiate(_wallPrefab, Vector3.zero, Quaternion.identity, parent);

		// 親(floorParent)の子として、計算したローカル座標をセット
		wall.transform.localPosition = localPos;

		// 横幅も GridUtils の定数に合わせるとさらに安全
		wall.transform.localScale = new Vector3(GridUtils.CellSize, _wallHeight, GridUtils.CellSize);

		if (isFront && _hideFrontWall)
		{
			var mr = wall.GetComponent<MeshRenderer>();
			if (mr) mr.enabled = false;
		}
	}
}