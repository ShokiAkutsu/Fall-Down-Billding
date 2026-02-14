using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 出来たら、FloorManagerから役目を引き継げるとベスト
/// スイッチが押されたら、次の行動を指定して動かせるScriptにしたい
/// </summary>
public class SwitchManager : MonoBehaviour
{
	public int totalSwitches = 3;
	private int currentStep = 0;
	//public List<LogicalBlock> allBlocks; // 階層の全ブロック

	public void OnSwitchStep(int num, Vector3 pos)
	{
		if (num == currentStep + 1)
		{
			currentStep = num;

			if (currentStep == totalSwitches)
			{
				ActivateRandomFloor(); // 全部押されたら床を動かす
			}
		}
	}

	void ActivateRandomFloor()
	{
		// 全ブロックの中から、スイッチ以外のものをランダムに選んで動かす
		//int index = Random.Range(0, allBlocks.Count);
		//allBlocks[index].Activate(new Vector3(0, -10f, 0), 2f);
	}
}