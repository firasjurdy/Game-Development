using UnityEngine;
using System.Collections;

public class EnemyProjeticle : MonoBehaviour {

	[SerializeField] private float _projectileDamage = 50f;
	public float ProjectileDamage { get { return this._projectileDamage; } }
	
	public void DeSpawn()
	{
		Destroy(gameObject);
	}	
}
