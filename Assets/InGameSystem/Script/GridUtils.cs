using UnityEngine;

public static class GridUtils
{
	// マスの間隔（2f）や高さの基準などを定数にしておく
	public const float CellSize = 2f;
	public const float DefaultY = 1f;

	/// <summary>
	/// グリッド座標を世界（または親のローカル）座標に変換する
	/// </summary>
	/// <param name="coord">マスのX, Z座標</param>
	/// <param name="gridSize">ステージの1辺の数</param>
	/// <param name="y">高さ（中心のY座標）</param>
	public static Vector3 ToWorld(int x, int z, int gridSize, float y = DefaultY)
	{
		// 5x5なら4, 3x3なら2
		float offset = gridSize - 1;

		return new Vector3(
			x * CellSize - offset,
			y,
			z * CellSize - offset
		);
	}

	// Vector2Intで渡したい時用のオーバーロード
	public static Vector3 ToWorld(Vector2Int coord, int gridSize, float y = DefaultY)
	{
		return ToWorld(coord.x, coord.y, gridSize, y);
	}

	/// <summary>
	/// 世界座標をグリッド座標（Vector2Int）に逆変換（レイキャスト用など）
	/// </summary>
	public static Vector2Int ToCoord(Vector3 worldPos, int gridSize)
	{
		float offset = gridSize - 1;
		int x = Mathf.RoundToInt((worldPos.x + offset) / CellSize);
		int z = Mathf.RoundToInt((worldPos.z + offset) / CellSize);
		return new Vector2Int(x, z);
	}
}