using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
	[SerializeField] float _moveSpeed = 3f;
	[SerializeField] float _rotateSpeed = 12f;
	Rigidbody _rb;
	Animator _animator;
	Vector3 _movement;

	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>();
	}

	private void Update()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		_movement = new Vector3(horizontal, 0, vertical).normalized; // 斜め移動の加速を制限

		if (_animator)
			_animator.SetFloat("Speed", _movement.magnitude, 0.1f, Time.fixedDeltaTime);
	}

	private void FixedUpdate()
	{
		_rb.linearVelocity = new Vector3(_movement.x * _moveSpeed, _rb.linearVelocity.y, _movement.z * _moveSpeed);

		if (_movement.magnitude > 0.1f)
		{
			Quaternion targetRotation = Quaternion.LookRotation(_movement); // 方向計算

			transform.rotation =
				Quaternion.Slerp(transform.rotation, targetRotation, _rotateSpeed * Time.fixedDeltaTime); // 回転

			if (_rb)
				_rb.angularVelocity = Vector3.zero; // 物理的な回転慣性をリセット
		}
	}
}