using UnityEngine;
using System.Collections;

public abstract class VisualRoll : EnemyBase
{
	public float archHeight = 0.4f;

	protected override IEnumerator MoveVisualRoutine(Vector2Int next)
	{
		Vector3 startPos = transform.localPosition;
		Vector3 targetPos = GetLocalPos(next, 1.5f);
		Quaternion startRot = transform.localRotation;

		Vector3 moveDir = (targetPos - startPos).normalized;
		Vector3 worldRotationAxis = Vector3.Cross(Vector3.up, moveDir);

		float elapsed = 0;
		float duration = 1f / _speed;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float percent = elapsed / duration;

			Vector3 currentPos = Vector3.Lerp(startPos, targetPos, percent); // •Ó‚ÌŠp‚ª—§‚Â‚æ‚¤‚É’²®
			currentPos.y = 1.5f + Mathf.Sin(percent * Mathf.PI) * archHeight;
			transform.localPosition = currentPos;

			transform.localRotation = Quaternion.AngleAxis(90 * percent, worldRotationAxis) * startRot;

			yield return null;
		}

		transform.localPosition = targetPos; // ’…’n‚µ‚½ó‘Ô‚É–ß‚·
		ApplyRotationSnap();
	}

	/// <summary>
	/// ‚Ü‚í‚·‚×‚«‰ñ“]²‚ğŒvZ‚µ‚ÄA‰ñ“]‚·‚éŠÖ”
	/// </summary>
	private void ApplyRotationSnap()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler(
			Mathf.Round(rot.x / 90) * 90,
			Mathf.Round(rot.y / 90) * 90,
			Mathf.Round(rot.z / 90) * 90
		);
	}
}