﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject Player;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = new Vector3(0, 8, -5);
		transform.position = Player.transform.position + offset;
		offset = transform.position - Player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = Player.transform.position + offset;
	}
}
