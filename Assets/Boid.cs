using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

	float speed = 0.2f;
	int thinkTimer = 0;

	Vector3 moveTo;
	Boid[] friends;

	// Use this for initialization
	void Start () {
		thinkTimer = Random.Range (0, 50);
	}
	
	// Update is called once per frame
	void Update () {
		increment ();

		if (thinkTimer == 0) {
			getFriends ();
		}

		flock ();
	}

	void flock () {
		Vector2 align = getAverageDirection ();
		Vector2 avoidDirection = getAvoidDirection ();
		Vector2 avoidObjectsDirection = getAvoidObjectsDirection ();
		Vector2 noise = new Vector2 ((float) Random.Range (0, 0), (float) Random.Range (0, 0));
		Vector2 cohesion = getCohesion ();

		moveTo = align + avoidDirection + avoidObjectsDirection + noise + cohesion;
		moveTo = Vector2.ClampMagnitude (moveTo, speed);
		transform.position = transform.position + moveTo;
	}

	Vector2 getAverageDirection() {
		Vector2 averageDirection = new Vector2 (0.0f, 0.01f);

		return averageDirection;
	}

	Vector2 getAvoidDirection() {
		Vector2 avoidDirection = new Vector2 (0.01f, 0.0f);

		return avoidDirection;
	}

	Vector2 getAvoidObjectsDirection() {
		Vector2 avoidObjectsDirection = new Vector2 (0.0f, 0.01f);

		return avoidObjectsDirection;
	}

	Vector2 getCohesion() {
		Vector2 cohesion = new Vector2 (0.0f, 0.0f);

		return cohesion;	
	}

	void getFriends() {
		print ("GETTING FRIENDS!");
	}

	void increment () {
		thinkTimer = (thinkTimer + 1) % 5;
	}
}
