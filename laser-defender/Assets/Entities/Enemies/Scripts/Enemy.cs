using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	[Header("Stats")]
    [SerializeField] private float _health = 200f;
    [SerializeField] private int _scoreValue = 150;

    [Header("Weapon")]
	[SerializeField] private GameObject _shotProjectilePrefab = null;
    [SerializeField] private float _initialWeaponDelay = 3f;
    [SerializeField] private float _baseLaserSpeed = 3f;
    [SerializeField] private float _firingIntervalMin = 0.8f;
    [SerializeField] private float _firingIntervalMax = 1.4f;
    [SerializeField] private bool _shootsInBursts = false;
    
	[Header("Prefabs")]
	[SerializeField] private GameObject _score3DTextPrefab = null;
	[SerializeField] private GameObject _deathExplosionPrefab = null;
    
	[Header("Sounds")]
	[SerializeField] private AudioClip[] arrivalSounds = null;
	[SerializeField] private AudioClip[] laserSounds = null;
	[SerializeField] private AudioClip[] gettingHitSounds = null;
	[SerializeField] private AudioClip _deathSound = null;
	
	[Header("Colors")]
	[SerializeField] private Color _normalColor = Color.white;
	[SerializeField] private Color _gotHitColor = Color.white;

	private Transform _bodyTransform;
	private SpriteRenderer _spriteRenderer;
	private AudioSource _audioSource;	
	private ParticleSystem _gotHitParticles;
    
	private void Start ()
	{
		_spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
		_spriteRenderer.color = _normalColor;
		
		_audioSource = this.GetComponent<AudioSource>();
		_audioSource.PlayOneShot(arrivalSounds[Random.Range(0,arrivalSounds.Length)], 2f);
        
        var hitCollider = this.GetComponentInChildren<HitCollider>();
        hitCollider.E_ColliderHit += OnColliderHit;

        _bodyTransform = hitCollider.transform;
		
        _gotHitParticles = this.GetComponent<ParticleSystem>();

        StartCoroutine(CoR_ShootWeapons(_initialWeaponDelay));
	}

	private void OnColliderHit(float damageValue)
	{
		_health -= damageValue;
		_gotHitParticles.Emit(10);
		_audioSource.PlayOneShot(gettingHitSounds[Random.Range(0,gettingHitSounds.Length)], 1f);
		_spriteRenderer.color = _gotHitColor;
		
		if (_health <= 0) 
		{
			Die();
		}
		
		if (_resetColorCoroutine != null) StopCoroutine(_resetColorCoroutine);
		_resetColorCoroutine = CoR_ResetColor();
		StartCoroutine(_resetColorCoroutine);
	}

	private IEnumerator _resetColorCoroutine;
	private IEnumerator CoR_ResetColor()
	{
		yield return new WaitForSeconds(0.1f);
		_spriteRenderer.color = _normalColor;
		_resetColorCoroutine = null;
	}

	private IEnumerator CoR_ShootWeapons(float initialDelay)
	{
		yield return new WaitForSeconds(initialDelay);
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(_firingIntervalMin, _firingIntervalMax));
			if (_shootsInBursts)
			{
				StartCoroutine(CoR_ShootProjectileBurst());
			}
			else
			{
				ShootSingleProjectile();
			}
		}
	}
	
	private void ShootSingleProjectile(Vector3 angle = default)
	{
		GameObject projectile = Instantiate (_shotProjectilePrefab, this.transform.position, Quaternion.Euler(angle));
		projectile.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * -_baseLaserSpeed);
		
        AudioSource.PlayClipAtPoint(laserSounds[Random.Range(0,laserSounds.Length)], _bodyTransform.position, 0.8f);
	}

	private IEnumerator CoR_ShootProjectileBurst()
	{
		ShootSingleProjectile(Vector3.zero);
		yield return new WaitForSeconds(0.2f);
		ShootSingleProjectile(new Vector3(0f, 0f, 30f));
		yield return new WaitForSeconds(0.4f);
		ShootSingleProjectile(new Vector3(0f, 0f, -30f));
	}
	
	private void Die()
	{
		GameObject scoreText = Instantiate(_score3DTextPrefab, this.transform.position, Quaternion.identity) as GameObject;
		scoreText.GetComponent<KillScoreInfo>().ShowScore(_scoreValue.ToString());
        Instantiate(_deathExplosionPrefab, _bodyTransform.position, Quaternion.identity);
		AudioSource.PlayClipAtPoint(gettingHitSounds[Random.Range(0,gettingHitSounds.Length)], _bodyTransform.position, 1f);
		AudioSource.PlayClipAtPoint(_deathSound, _bodyTransform.position, 5f);
		GameScore.UpdateScore(_scoreValue);
		Destroy(this.gameObject);
	}

}
