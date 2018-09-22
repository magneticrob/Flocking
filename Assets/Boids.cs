using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour {

	public Transform wall;
	public Transform boid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			CreateBoid ();
		}

		if (Input.GetButtonDown ("Fire2")) {
			CreateWall ();
		}
	}

	void CreateWall() {
		createAtMousePoint (wall);
	}

	void CreateBoid() {
		createAtMousePoint (boid);
	}

	void createAtMousePoint(Transform gameObject) {
		float mouseX = Camera.main.ScreenToWorldPoint (Input.mousePosition).x;
		float mouseY = Camera.main.ScreenToWorldPoint (Input.mousePosition).y;
		Vector2 position = new Vector2 (mouseX, mouseY);
		Instantiate (gameObject, position, Quaternion.identity);
	}
}
