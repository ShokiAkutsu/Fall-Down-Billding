using UnityEngine;
using System.Collections;

public abstract class VisualSlide : EnemyBase
{
	protected override IEnumerator MoveVisualRoutine(Vector2Int next)
	{
		Vector3 startPos = transform.localPosition;
		Vector3 targetPos = GetLocalPos(next, 1.5f);

		float distance = Vector3.Distance(startPos, targetPos);

		float duration = distance / Mathf.Max(_speed, 0.1f); // “ž’…“_‚Ü‚Å‚Ì‘¬‚³‚ÌŒvŽZ

		if (distance < 0.01f) yield break;

		float elapsed = 0;
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float percent = elapsed / duration;
			transform.localPosition = Vector3.Lerp(startPos, targetPos, percent);
			yield return null;
		}

		transform.localPosition = targetPos;
	}
}