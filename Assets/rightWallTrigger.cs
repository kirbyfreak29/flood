using UnityEngine;
using System.Collections;

public class rightWallTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Wall") {
//			Debug.Log ("Player's right side started touching " + other.gameObject.name);
			GetComponentInParent<characterController>().touchingRightWall = true;
			GetComponentInParent<characterController>().startWallHitTimer();
		}
	}
	
	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "Wall") {
//			Debug.Log ("Player's right side is currently touching " + other.gameObject.name);
		}
	}
	
	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Wall") {
//			Debug.Log ("Player's right side stopped touching " + other.gameObject.name);
			GetComponentInParent<characterController>().touchingRightWall = false;
		}
	}
}
