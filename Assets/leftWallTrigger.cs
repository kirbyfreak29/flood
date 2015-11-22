using UnityEngine;
using System.Collections;

public class leftWallTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Wall") {
//			Debug.Log ("Player's left side started touching " + other.gameObject.name);
			GetComponentInParent<characterController>().touchingLeftWall = true;
			GetComponentInParent<characterController>().startWallHitTimer();
		}
	}
	
	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "Wall") {
//			Debug.Log ("Player's left side is currently touching " + other.gameObject.name);
		}
	}
	
	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Wall") {
//			Debug.Log ("Player's left side stopped touching " + other.gameObject.name);
			GetComponentInParent<characterController>().touchingLeftWall = false;
		}
	}
}
