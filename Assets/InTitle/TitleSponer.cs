using UnityEngine;

public class TitleSponer : MonoBehaviour
{
	[SerializeField] GameObject _prefab; // ~‚ç‚¹‚½‚¢ƒvƒŒƒnƒu
	[SerializeField] float _rangeX = 5f;
	[SerializeField] float _rangeZ = 5f;
	[SerializeField] SceneFader _fader;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			for(int i  = 0; i < 100;  i++)
				Spawn();
	}

	void Spawn()
	{
		float randomX = Random.Range(-_rangeX, _rangeX);
		float randomZ = Random.Range(-_rangeZ, _rangeZ);

		Vector3 spawnPoint = new Vector3(randomX, transform.position.y, randomZ);

		Vector3 targetPoint = new Vector3(0, transform.position.y, 0);

		Vector3 directionToCenter = targetPoint - spawnPoint;

		if (directionToCenter != Vector3.zero && _prefab)
		{
			Quaternion lookRotation = Quaternion.LookRotation(directionToCenter, Vector3.up);
			Quaternion flippedRotation = lookRotation * Quaternion.Euler(0, 180f, 0);
			Instantiate(_prefab, spawnPoint, flippedRotation);
		}

		_fader.FadeAndLoad("InGame");
	}
}
