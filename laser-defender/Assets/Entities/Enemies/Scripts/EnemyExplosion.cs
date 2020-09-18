using UnityEngine;
using System.Collections;

public class EnemyExplosion : MonoBehaviour {

	[SerializeField] private AudioClip[] _explosionSounds = null;
    
	private void Start () 
	{
		var audioSource = this.GetComponent<AudioSource>();
        audioSource.PlayOneShot(_explosionSounds[Random.Range(0,_explosionSounds.Length)], 1.6f);
        StartCoroutine(CoR_WaitAndDestroy());
	}

	private IEnumerator CoR_WaitAndDestroy()
	{
		yield return new WaitForSeconds(4f);
        Destroy(gameObject);
	}
	
    
}
