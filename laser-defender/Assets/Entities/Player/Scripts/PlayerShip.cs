using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float _startingEnergy = 1000f;
    public float StartingEnergy { get { return this._startingEnergy; } }
    
    [SerializeField] private float _energyRegenRate = 0f;
    [SerializeField] private float _shipSpeed = 2000f;
    [SerializeField] private float _mainGunProjectileSpeed = 12f;
    [SerializeField] private float _firingRate = 10f;
    
    [SerializeField] private float _arrivalAnimationLength = 4.2f;
    
	[Header("Prefabs")]
	[SerializeField] public GameObject _greenLaserPrefab = null;
	[SerializeField] public GameObject _missilePrefab = null;
    [SerializeField] public GameObject _shipExplosion = null;
	
    [Header("Sounds")]
	[SerializeField] private AudioClip[] _mainWeaponSounds = null;
    [SerializeField] private AudioClip _gotHitSound = null;
    [SerializeField] private AudioClip _shipExplosionSound = null;
    [SerializeField] private AudioClip _energyChargeUpSound = null;
    
    public float CurrentEnergy { get; private set; }

    private Transform _shipTransform;
    private Animator _animator;
	private AudioSource _audioSource;
	private ParticleSystem _shieldParticles;
	private Rigidbody2D _rigidBody2D;
    
	private float _minPosX;
	private float _maxPosX;

	private float _mainGunFireInterval;
	
	public void SetupAndStart()
	{
		_shipTransform = this.GetComponent<Transform>();
		_animator = this.GetComponent<Animator>();
		_shieldParticles = this.GetComponentInChildren<ParticleSystem>();
		_audioSource = this.GetComponent<AudioSource>();
		_rigidBody2D = this.GetComponent<Rigidbody2D>();
		
		float playSpaceWidth = FindObjectOfType<Playspace>().Width;
		
		_minPosX = (playSpaceWidth / 2f - 0.5f) * -1f;
		_maxPosX = playSpaceWidth / 2f - 0.5f;

		_mainGunFireInterval = 1f / _firingRate;
		
		CurrentEnergy = StartingEnergy;

		StartCoroutine(CoR_Arrive());
	}

	
	private IEnumerator CoR_Arrive()
	{
		_animator.enabled = true;	
		yield return new WaitForSeconds(_arrivalAnimationLength);
		_animator.enabled = false;
	}
	
	private void FixedUpdate () 
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			_rigidBody2D.AddForce(new Vector2(-_shipSpeed * Time.deltaTime, 0f), ForceMode2D.Impulse);	
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			_rigidBody2D.AddForce(new Vector2(_shipSpeed * Time.deltaTime, 0f), ForceMode2D.Impulse);			
		}
	}

	private float _lastShotTime;
	private bool _energyRegenerationDisabled = false;
	private void Update()
	{
		// restrict player to the game space
		float newX = Mathf.Clamp(_shipTransform.position.x, _minPosX, _maxPosX);
		_shipTransform.position = new Vector2(newX, _shipTransform.position.y);
		
        if (!_energyRegenerationDisabled && CurrentEnergy < StartingEnergy) RegenerateEnergy();

        if (Input.GetKey(KeyCode.Space))
        {
	        if (Time.time - _lastShotTime > _mainGunFireInterval)
	        {
		        ShootMainLaser();
		        _lastShotTime = Time.time;
	        }
        }
        else
        {
	        _lastShotTime = 0f;
        }
	}
	
	private void OnTriggerEnter2D(Collider2D thisHitMe)
	{
		EnemyProjeticle enemyProjectile = thisHitMe.gameObject.GetComponent<EnemyProjeticle>();
		if (enemyProjectile != null)
		{
			CurrentEnergy -= enemyProjectile.ProjectileDamage;
			enemyProjectile.DeSpawn();
			_audioSource.PlayOneShot(_gotHitSound, 0.8f);
			_shieldParticles.Emit(10000);
									
			if (CurrentEnergy <= 0)
			{
				CurrentEnergy = 0;
				_energyRegenerationDisabled = true;
                AudioSource.PlayClipAtPoint(_shipExplosionSound, this.transform.position, 1f);
				Instantiate(_shipExplosion, _shipTransform.position, Quaternion.identity);
				
				FindObjectOfType<GameScene>().EndGame();
				
				Destroy(this.gameObject);
			} 
		}
	}
    
	private void ShootMainLaser()
	{
		GameObject greenLaser = Instantiate (_greenLaserPrefab, this.transform.position, Quaternion.identity);
		greenLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, _mainGunProjectileSpeed);
		_audioSource.PlayOneShot(_mainWeaponSounds[Random.Range(0,_mainWeaponSounds.Length)], 0.5f);
	}
	
	private void ShootMissile()
	{
		GameObject missile = Instantiate (_missilePrefab, this.transform.position, Quaternion.identity);
		missile.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 2, ForceMode2D.Impulse);
	}
	
	private void RegenerateEnergy()
	{
		CurrentEnergy += _energyRegenRate * Time.deltaTime;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0f, StartingEnergy);
	}
	
    public void RecoverEnergy()
    {
	    CurrentEnergy += 500f;
	    CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0f, StartingEnergy);
        _audioSource.PlayOneShot(_energyChargeUpSound);
    }
}
