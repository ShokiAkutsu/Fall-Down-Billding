using UnityEngine;

public class SwitchFloorSystem : BlockSprite
{
    bool _isActive = false;
    
    void Start()
    {
		_floorManager = GetComponentInParent<FloorManager>();
        _renderer.material.color = Color.red; // 普通の床と擬態
	}

    public void ActiveFloor()
    {
        _isActive = true;
		_renderer.material.color = Color.yellow; // 今押すスイッチが分かりやすくなるように
	}

	public override void ResetColor()
	{
		if (_isActive)
			_renderer.material.color = Color.yellow;
		else
			_renderer.material.color = Color.red;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player" && _isActive)
        {
            _isActive = false;
            _floorManager.SwitchOn(true);
            _renderer.material.color = Color.red; // スイッチの色を元に戻す
        }
	}
}
