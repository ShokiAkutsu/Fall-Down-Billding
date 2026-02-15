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
		// マス間の距離が2なので、中心からのオフセットは (マス数 - 1) になる
		// 5x5なら offset=4, 3x3なら offset=2
		float offsetX = (width - 1);
		float offsetZ = (height - 1);

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
						SpawnWall(x, z, offsetX, offsetZ, wallY, floorParent, z == -1);
					}
				}
			}
		}
	}

	void SpawnWall(int x, int z, float offX, float offZ, float y, Transform parent, bool isFront)
	{
		Vector3 localPos = new Vector3(x * 2f - offX, y + (_wallHeight / 2f), z * 2f - offZ);

		GameObject wall = Instantiate(_wallPrefab, Vector3.zero, Quaternion.identity, parent);
		wall.transform.localPosition = localPos;
		wall.transform.localScale = new Vector3(2f, _wallHeight, 2f);

		// 手前かつ透明設定ならMeshRendererをオフ
		if (isFront && _hideFrontWall)
		{
			var mr = wall.GetComponent<MeshRenderer>();

			if (mr)
				mr.enabled = false;
		}
	}
}