using UnityEngine;
using System.Collections;

public class PlayerExplosion : MonoBehaviour {
    
    [SerializeField] private AudioClip _explosionSound = null;
    
	private void Start () 
    {
        var audioSource = this.GetComponent<AudioSource>();
        audioSource.PlayOneShot(_explosionSound, 1f);
        StartCoroutine(CoR_WaitAndDestroy());
    }

    private IEnumerator CoR_WaitAndDestroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
    

}
