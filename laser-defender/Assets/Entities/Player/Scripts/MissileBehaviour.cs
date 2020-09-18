using UnityEngine;
using System.Collections;

public class MissileBehaviour : MonoBehaviour {

	public float Speed;
		
	// Use this for initialization
	void Start () {
		//Invoke("FixCourse", 1f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		this.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * Speed, ForceMode2D.Force);
	}
	void FixCourse()
	{
		this.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 100f, ForceMode2D.Force);
	}
}
