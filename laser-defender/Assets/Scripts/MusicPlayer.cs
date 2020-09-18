using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    [SerializeField] private AudioClip _musicToPlay = null;
	
    private void Start()
    {
	    var musicSource = GetComponent<AudioSource>();
	    musicSource.clip = _musicToPlay;
		musicSource.loop = true;
		musicSource.Play();
    }
    
}
