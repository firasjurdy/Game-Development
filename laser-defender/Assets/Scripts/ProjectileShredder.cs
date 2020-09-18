using UnityEngine;
using System.Collections;

public class ProjectileShredder : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D enteringCollider)
	{
		Destroy(enteringCollider.gameObject);
	}
	
}
