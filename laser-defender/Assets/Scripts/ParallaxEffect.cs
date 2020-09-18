using UnityEngine;
using System.Collections;

public class ParallaxEffect : MonoBehaviour {

	[SerializeField] private Transform backgroundStars1 = null;
	[SerializeField] private Transform backgroundStars2 = null;
	[SerializeField] private Transform backgroundStars3 = null;
	[SerializeField] private Transform backgroundStars4 = null;
	[SerializeField] private Transform backgroundStars5 = null;
	[SerializeField] private Transform backgroundImg = null;
	
	[SerializeField] private float _cameraSpeedMultiplier = 1f;

	private Transform _gameCameraTransform;
	private float _leeway;
	private float _cameraXPosition;
	
	private Transform _playerShipTransform;
	private float _playerShipMaxX;
	private float _playerShipPositionPercentage;

	private bool _playerShipPresent = false;
		
	private void Start ()
	{
		var playerShip = FindObjectOfType<PlayerShip>();
		if (playerShip == null) return;
		
		_playerShipTransform = playerShip.GetComponent<Transform>();
		_playerShipPresent = true;
		
		float playSpaceWidth = FindObjectOfType<Playspace>().Width;

		var gameCamera = FindObjectOfType<Camera>();
		if (gameCamera != null)
		{
			float distanceToCamera = -gameCamera.transform.position.z;
			Vector3 rightBoundry = gameCamera.ViewportToWorldPoint(new Vector3(1f,0f,distanceToCamera));
			_leeway = playSpaceWidth / 2 - rightBoundry.x;
			_playerShipMaxX = playSpaceWidth / 2 - 0.5f;
			_gameCameraTransform = gameCamera.transform;
		}
	}
	
	private void Update () 
	{
		if (_playerShipTransform != null && _playerShipPresent)
		{
			_playerShipPositionPercentage = _playerShipTransform.transform.position.x / _playerShipMaxX;
			_cameraXPosition = Mathf.Clamp(_playerShipPositionPercentage * _leeway * _cameraSpeedMultiplier, -_leeway, _leeway);
			backgroundStars1.position = new Vector3 (_cameraXPosition * 0f, backgroundStars1.transform.position.y, backgroundStars1.position.z);
			backgroundStars2.position = new Vector3 (_cameraXPosition * 0.2f, backgroundStars2.transform.position.y, backgroundStars2.position.z);
			backgroundStars3.position = new Vector3 (_cameraXPosition * 0.4f, backgroundStars3.transform.position.y, backgroundStars3.position.z);
			backgroundStars4.position = new Vector3 (_cameraXPosition * 0.7f, backgroundStars4.transform.position.y, backgroundStars4.position.z);
			backgroundStars5.position = new Vector3 (_cameraXPosition * 1f, backgroundStars5.transform.position.y, backgroundStars5.position.z);
			backgroundImg.position = new Vector3 (_cameraXPosition * 0.9f, backgroundImg.transform.position.y, backgroundImg.position.z);
			_gameCameraTransform.position = new Vector3(_cameraXPosition, _gameCameraTransform.position.y, _gameCameraTransform.position.z);
		}
	}
}
