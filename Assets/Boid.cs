using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

	float friendRadius = 20;
	float speed = 0.2f;
	float cohesionRadius = 20;
	int thinkTimer = 0;

	Vector3 moveTo;
	List<Boid> friends = new List <Boid>();
	List<Wall> walls = new List <Wall>();

	// Use this for initialization
	void Start () {
		thinkTimer = Random.Range (0, 50);
		getWalls ();
	}
	
	// Update is called once per frame
	void Update () {
		increment ();

//		if (thinkTimer == 0) {
			getFriends ();
//		}

		flock ();
	}

	void flock () {
		Vector2 align = getAverageDirection ();
		Vector2 avoidDirection = getAvoidDirection ();
		Vector2 avoidObjectsDirection = getAvoidObjectsDirection ();
		Vector2 noise = new Vector2 ((float) Random.Range (0.0f, 2.0f), (float) Random.Range (0.0f, 2.0f));
		Vector2 cohesion = getCohesion ();

		avoidObjectsDirection = avoidObjectsDirection * 3;

		moveTo = align + avoidDirection + avoidObjectsDirection + noise + cohesion;
		moveTo = moveTo / 25;
		moveTo = Vector2.ClampMagnitude (moveTo, speed);
		transform.position = transform.position + moveTo;

		transform.up = (moveTo * 10) - transform.position;

		Debug.DrawLine (transform.position, moveTo * 100);
	}

	Vector2 getAverageDirection() {
		Vector2 averageDirection = new Vector2 (0.0f, 0.0f);

		for (int i = 0; i < friends.Count; i++) {

			Boid friend = (Boid)friends [i];
			float distance = Vector2.Distance (transform.position, friend.transform.position);

			if ((distance > 0) && (distance > friendRadius)) {
				Vector2 copy = friend.transform.position;
				copy.Normalize ();
				copy = copy / distance;
				averageDirection = averageDirection + copy;
			}
		}

		return averageDirection;
	}

	Vector2 getAvoidDirection() {
		Vector2 avoidDirection = new Vector2 (0.0f, 0.0f);

		for (int i = 0; i < friends.Count; i++) {

			Boid friend = (Boid)friends [i];
			float distance = Vector2.Distance (transform.position, friend.transform.position);

			if ((distance > 0) && (distance > friendRadius)) {
				Vector2 copy = transform.position - friend.transform.position;
				copy.Normalize ();
				copy = copy / distance;
				avoidDirection = avoidDirection + copy;
			}
		}

		return avoidDirection;
	}

	Vector2 getAvoidObjectsDirection() {
		Vector2 avoidObjectsDirection = new Vector2 (0.0f, 0.01f);

		for (int i = 0; i < walls.Count; i++) {

			Wall wall = (Wall)walls [i];
			float distance = Vector2.Distance (transform.position, wall.transform.position);

			if ((distance > 0) && (distance > friendRadius)) {
				Vector2 copy = transform.position - wall.transform.position;
				copy.Normalize ();
				copy = copy / distance;
				avoidObjectsDirection = avoidObjectsDirection + copy;
			}
		}

		return avoidObjectsDirection;
	}

	Vector2 getCohesion() {
		float neighbourDistance = 50;
		int count = 0;

		Vector2 cohesion = new Vector2 (0.0f, 0.0f);
		for (int i = 0; i < friends.Count; i++) {
			Boid friend = friends [i];
			float distance = Vector2.Distance (transform.position, friend.transform.position);
			if ((distance > 0) && (distance < cohesionRadius)) {
				cohesion = (Vector3) cohesion + friend.transform.position;
				count++;
			}

			if (count > 0) {
				cohesion = cohesion / count;
				Vector2 desired = (Vector3) cohesion - transform.position;
				return Vector2.ClampMagnitude(desired, 0.05f);
			} else {
				return new Vector2(0, 0);
			}
		}

		return cohesion;
	}

	void getFriends() {
		List<Boid> nearby = new List<Boid> ();
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 100);
		print ("Collider length: " + colliders.Length);

		for (int i = 0; i < colliders.Length; i++)
		{
			Boid test = (Boid)colliders [i].gameObject.GetComponent<Boid>();
			if (test == this) {
				continue;
			}

			if (Mathf.Abs(test.transform.position.x - transform.position.x) < friendRadius && 
				Mathf.Abs(test.transform.position.y - transform.position.y) < friendRadius) {
				nearby.Add (test);
			}
		}

		friends = nearby;
	}

	void getWalls() {
		Object[] wallsArray = Object.FindObjectsOfType (typeof(Wall));
		walls = new List<Wall>();

		for (int i = 0; i < wallsArray.Length; i++) {
			Wall wall = (Wall)wallsArray [i];
			walls.Add (wall);
		}
	}

	void increment () {
		thinkTimer = (thinkTimer + 1) % 5;
	}
}