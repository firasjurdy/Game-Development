using UnityEngine;
using System.Collections;

public class Playspace : MonoBehaviour {

	[SerializeField] private float _width = 24f;
	public float Width { get { return this._width; } }
	
	[SerializeField] private float _height = 10f;
	public float Height { get { return this._height; } }	
		
}
