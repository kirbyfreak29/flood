using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class leftPickUpTrigger : MonoBehaviour {

	private List<Collider2D> TriggerList = new List<Collider2D>();
	
	void OnTriggerEnter2D (Collider2D other) {
		// If the object can be interacted with and is not already in the list, put it in, and tell the player what object it can pick up
		if (other.gameObject.tag == "Interactable" && !TriggerList.Contains (other)) {
			TriggerList.Add(other);
			GetComponentInParent<characterController>().pickupableObjectSetter(other.transform.parent.gameObject);
			GetComponentInParent<characterController>().pickupableObjectInFrontSetter(true);
			Debug.Log ( "Added object" );
		}
	}
	
	void OnTriggerExit2D (Collider2D other) {
		// If the object that leaves is interactable and in the list, take it out of the list
		if (other.gameObject.tag == "Interactable" && TriggerList.Contains (other)) {
			TriggerList.Remove(other);
			// If the list is empty, tell the player it can't pick anything up
			if (TriggerList.Count == 0)
			{
				GetComponentInParent<characterController>().pickupableObjectSetter(null);
				GetComponentInParent<characterController>().pickupableObjectInFrontSetter(false);
			}
			// If the list is not empty, tell the player what object it can pick up now
			else
			{
				GetComponentInParent<characterController>().pickupableObjectSetter(TriggerList[0].transform.parent.gameObject);
			}
			Debug.Log ( "Removed object" );
		}
	}
}
