﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	public Text candycount;
	public Text ExitWarning;
	public Canvas ending_screen;
	public Button pause_button;
	public int candytotal;
	public Vector3 original_pos;
	public int required_candynum;
	private int candynum;

	public bool keys;

	//for shadow detection
	private ShadowDetector sd;

	//For movement
	public GameObject tap;
	private Vector3 tapLocation;
	private Ray ray; 
	private Rigidbody rb;
	public float speed;
    public bool deathSet;

	//for movable objects
	GameObject[] movedObjects;
	Vector3[] movedOjectsPosition;

	// Use this for initialization
	void Start () {
		//store original position
		original_pos = transform.position;

		//shadow detector
		sd = this.gameObject.GetComponent<ShadowDetector>();

		//for movement
		tapLocation = transform.position;
		rb = GetComponent<Rigidbody> ();
		rb.freezeRotation = true;

		//store all movable object positions
		movedObjects = GameObject.FindGameObjectsWithTag("movableObject");
		movedOjectsPosition = new Vector3[movedObjects.Length];
		for (int j = 0; j < movedObjects.Length; j++) {
			movedOjectsPosition [j] = movedObjects [j].transform.position;
		}
			
		ExitWarning.enabled = false;

		if (ending_screen.gameObject.activeInHierarchy == true) {
			ending_screen.gameObject.SetActive (false);
		}

		//set candy
		SetCountText ();
		candynum = 0;

        deathSet = false;

	}

	void Update(){
		//hit light
		if (!sd.isShaded) {
			//Debug.Log ("die");

            playDeathAnimation(true);
            print("dead");
            StartCoroutine(respawn());
            

        } else {
            //Debug.Log ("live");
            if (deathSet)
            {
                playDeathAnimation(false);
                deathSet = false;
                print("alive");
            }
        }
	}


	void FixedUpdate () {
		
		if (keys) {
			if ((Input.GetMouseButton (0) || Input.touchCount == 1)) {
				if (Input.GetMouseButton (0)) {
					ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				} else {
					ray = Camera.main.ScreenPointToRay (Input.touches [0].position);
				}
				//identify hit, find the correct hit
				RaycastHit[] hits;
				hits = Physics.RaycastAll (ray);
				for (int i = 0; i < hits.Length; i++) {
					RaycastHit hit = hits [i];
					if (hit.collider.name == "floor") {
						indicateTap (hit);
						break;
					}
				}
			}

			//get to that location
			if ((Mathf.Abs (transform.position.x - tapLocation.x) > 0.1f) || (Mathf.Abs (transform.position.z - tapLocation.z) > 0.1f)) {
				rb.velocity = (tapLocation - transform.position) * speed;
			} else {
				tap.SetActive (false);
			}
		}
	}


	void OnTriggerEnter(Collider other){
		//Candy collector
		if (other.gameObject.CompareTag ("Candy")) {
			other.gameObject.SetActive (false);
			candynum += 1;
			SetCountText ();
		}

		//Exit Level
		else if (other.gameObject.CompareTag ("Exittrigger")) {
			
			if (candynum >= required_candynum) {
				Time.timeScale = 0;
				if (ending_screen.gameObject.activeInHierarchy == false) {
					ending_screen.gameObject.SetActive (true);
					pause_button.gameObject.SetActive (false);
				}

			} else {
				StartCoroutine (ShowMessage (ExitWarning, "Need More Candies For Gate Opening", 3));
			}
		}

		else if (other.gameObject.CompareTag ("Button_trigger_level1")) {
			other.GetComponent<level1Switch>().Triggered();
		}
		else if (other.gameObject.CompareTag ("Button_trigger")) {
			keys = false;
			tapLocation = transform.position;
			other.GetComponent<Trigger_Controller>().Triggered();
		}

	}

	void SetCountText(){
		candycount.text = "Candy " + candynum.ToString () + "/" + candytotal;
	}



	//indicate successful tap/click, store and face to that location
	void indicateTap(RaycastHit hit){
		tapLocation = hit.point;
		tapLocation.y = transform.position.y;
		tap.GetComponent <tapEffect> ().active = true;
		tap.SetActive (true);
		tap.transform.position = tapLocation;
		transform.LookAt (tapLocation);
	}

	IEnumerator ShowMessage (Text guiText, string message, float delay) {
		guiText.text = message;
		guiText.enabled = true;
		yield return new WaitForSeconds(delay);
		guiText.enabled = false;
	}


    //check of GameObject g in array

    //pop up warning, pumpkin stops and resqpawns, movable objects respawn
    IEnumerator respawn()
    {
        //StartCoroutine(ShowMessage(ExitWarning, "You Shall Not Embrace the Light", 4));
        print(Time.time);
        yield return new WaitForSeconds(3);
        transform.position = original_pos;
        tapLocation = original_pos;
        rb.velocity = Vector3.zero;
        for (int i = 0; i < movedObjects.Length; i++)
        {
            movedObjects[i].transform.position = movedOjectsPosition[i];
        }
        print(Time.time);
    }

    void playDeathAnimation(bool setValue)
    {
        GameObject pumpkin = GameObject.FindGameObjectWithTag("pumpkin");
        Animator anim = pumpkin.GetComponent<Animator>();
        anim.SetBool("isDead", setValue);
        if (!deathSet)
        {
            deathSet = true;
        }
    }


}