using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

/// <summary>
/// 生成した床全てにアタッチ、もしくは継承している
/// 床全体が行う可能性のある処理を記述
/// </summary>
public class BlockSprite : MonoBehaviour
{
	protected Renderer _renderer;
	protected FloorManager _floorManager;

	void Awake()
	{
		_renderer = GetComponent<Renderer>();
	}

	public void SetManager(FloorManager floorManager)
	{
		_floorManager = floorManager;
	}

	public virtual void Flash()
	{
		StartCoroutine(FlashRoutine());
	}

	IEnumerator FlashRoutine()
	{
		_renderer.material.color = Color.cyan; // 通り道の光の色
		yield return new WaitForSeconds(0.2f);
		ResetColor();
	}

	public virtual void ResetColor()
	{
		_renderer.material.color = Color.red;
	}
}
