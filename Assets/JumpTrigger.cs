using UnityEngine;
using System.Collections;

public class JumpTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Jumpable") {
//			Debug.Log ("Player started standing on " + other.gameObject.name);
			GetComponentInParent<characterController>().canJump = true;
			GetComponentInParent<characterController>().resetJumps();
			Debug.Log ("Can Jump");
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "Jumpable") {
//			Debug.Log ("Player is currently standing on " + other.gameObject.name);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Jumpable") {
//			Debug.Log ("Player stopped standing on " + other.gameObject.name);
			GetComponentInParent<characterController>().canJump = false;
		}
	}
}
