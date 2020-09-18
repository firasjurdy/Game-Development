using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	[SerializeField] private float _damage = 50f;
	public float Damage { get { return this._damage; } }
	
	public void Hit()
	{
		Destroy(gameObject);
	}	
}
