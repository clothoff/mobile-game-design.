﻿using UnityEngine;
using System.Collections;

public class appliedForce : MonoBehaviour {
	private Vector3 force;
	private Rigidbody rb;
	public float speed;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		speed = GameObject.Find ("Player").GetComponent<PlayerController>().speed;
	}
	
	// Update is called once per frame
	void Update () {
	}

	//detect colission
	void OnCollisionExit (Collision col)
	{
		
		rb.velocity = Vector3.zero;

	}
}