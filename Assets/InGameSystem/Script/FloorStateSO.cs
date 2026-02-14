using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FloorStateSO", menuName = "Scriptable Objects/FloorStateSO")]
public class FloorStateSO : ScriptableObject
{
    [SerializeField] int _floorSize = 5;        // 階層の広さ（正方形）
    [SerializeField] int _totalSwitches = 3;    // スイッチギミックの数
    [SerializeField] List<GameObject> _enemyList; // 出現する敵のList
    [SerializeField] bool _startHole = false;   // 穴を初めから開けておくか

	public int FloorSize => _floorSize;
    public int TotalSwitches => _totalSwitches;
    public List<GameObject> EnemyList => _enemyList;
    public bool StartHole => _startHole;
}
