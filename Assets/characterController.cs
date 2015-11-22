using UnityEngine;
using System.Collections;

public class characterController : MonoBehaviour {

	/* Variables */
	public Rigidbody2D target;
	public Camera mainCamera;
	public int moveSpeed = 30;
	public double moveSpeedIncrease = .1;
	public bool canJump, touchingLeftWall, touchingRightWall, startJumping, isKicking, controlsFlipped = false;
	public bool canKickOffWall = false;
	public int wallHitTimer = 0, wallHitTimerLength = 10;
	public int jumpHeight = 50;
	public int totalJumps = 3, jumpsLeft = 3, jumpDelayTimer = 0, jumpDelay = 5;
	public int jumpHorizBoost = 5;
	public int kickingTimer = 0;
	public string arrowKeyBeingPressed = "None";

	public bool carryingSomething = false;
	public string currentFacing = "Right";
	public bool pickupableObjectInFront = false;
	public GameObject pickupableObject, carriedObject;
	public Vector2 objectHoldPoint = new Vector2(0, -1);
	public bool pickupObject, throwObject = false;
	public bool currentlyBeingHeld = false;

	private float latestNonZeroXVelocity;
	
	public Sprite spritePlayerNormal, spritePlayerKicking;
	private SpriteRenderer spriteRenderer;

	// Input variables
	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode throwKey = KeyCode.C;
	public KeyCode kickKey = KeyCode.F;
	public KeyCode leftKey = KeyCode.LeftArrow;
	public KeyCode rightKey = KeyCode.RightArrow;
	
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer.sprite == null)
			spriteRenderer.sprite = spritePlayerNormal;
	}

	void Update () {
		// Gets key input for jumping
		if (Input.GetKeyDown (jumpKey) && jumpsLeft != 0 && jumpDelayTimer == 0) {
			jumpsLeft -= 1;
			startJumping = true;
			jumpDelayTimer = jumpDelay;
		}

		// Gets key input for pickup/throwing objects
		if (Input.GetKeyDown (throwKey))
		{
			if (!carryingSomething)
				pickupObject = true;
			else if (carryingSomething)
				throwObject = true;
		}
	}
	
	void FixedUpdate () {

		// Store the latest horizontal velocity you've been traveling at that's not 0
		if (target.velocity.x != 0)
			latestNonZeroXVelocity = target.velocity.x;

		// Moving Left
		if ((Input.GetKey (leftKey) && !controlsFlipped) || (Input.GetKey (rightKey) && controlsFlipped && arrowKeyBeingPressed == "Right") && !currentlyBeingHeld) {

			currentFacing = "Left";

			if (!controlsFlipped)
				arrowKeyBeingPressed = "Left";

			if (touchingLeftWall)
			{
				if (isKicking && canKickOffWall)
				{
					target.velocity = new Vector2 (-latestNonZeroXVelocity, target.velocity.y);
					controlsFlipped = !controlsFlipped;
				}
				else
				{
					target.velocity = new Vector2 (0, target.velocity.y);
					controlsFlipped = false;
				}
			}
			else if (canJump) {
				if (target.velocity.x != 0 && target.velocity.x <= -moveSpeed + 2) {
					target.velocity = new Vector2 ((float)(target.velocity.x - (float)moveSpeedIncrease), target.velocity.y);
				}
				else {
					target.velocity = new Vector2 (-moveSpeed, target.velocity.y);
				}
			}
			else if (!canJump) {
				if (target.velocity.x < -moveSpeed) {
					target.velocity = new Vector2 (target.velocity.x, target.velocity.y);
				}
				else
					target.velocity = new Vector2 (-moveSpeed, target.velocity.y);
			}
			else
				target.velocity = new Vector2 (-moveSpeed, target.velocity.y);
		}

		// Moving Right
		else if ((Input.GetKey (rightKey) && !controlsFlipped) || (Input.GetKey (leftKey) && controlsFlipped && arrowKeyBeingPressed == "Left") && !currentlyBeingHeld) {

			currentFacing = "Right";

			if (!controlsFlipped)
				arrowKeyBeingPressed = "Right";

			if (touchingRightWall)
			{
				if (isKicking && canKickOffWall)
				{
					target.velocity = new Vector2 (-latestNonZeroXVelocity, target.velocity.y);
					controlsFlipped = !controlsFlipped;
				}
				else
				{
					target.velocity = new Vector2 (0, target.velocity.y);
					controlsFlipped = false;
				}
			}
			else if (canJump) {
				if ((target.velocity.x != 0) && (target.velocity.x >= moveSpeed - 5)) {
					Debug.Log ("Speeding up right");
					target.velocity = new Vector2 ((float)(target.velocity.x + (float)moveSpeedIncrease), target.velocity.y);
				}
				else {
					target.velocity = new Vector2 (moveSpeed, target.velocity.y);
					Debug.Log ("Staying constant right");
				}
			}
			else if (!canJump) {
				if (target.velocity.x > moveSpeed) {
					target.velocity = new Vector2 (target.velocity.x, target.velocity.y);
				}
				else
					target.velocity = new Vector2 (moveSpeed, target.velocity.y);
			}
			else
				target.velocity = new Vector2 (moveSpeed, target.velocity.y);
		}

		// Not moving (or moving purely on momentum with no user input)
		else {
			controlsFlipped = false;
			arrowKeyBeingPressed = "None";

			if (!canJump) {
//				if (Mathf.Abs(target.velocity.x) >= moveSpeed)
//					target.velocity = new Vector2 (target.velocity.x/(float)1.1, target.velocity.y);
//				else
//					target.velocity = new Vector2 (target.velocity.x/(float)1.05, target.velocity.y);
////				target.velocity = new Vector2 (target.velocity.x, target.velocity.y);
				if (touchingRightWall || touchingLeftWall)
				{
					if (isKicking && canKickOffWall)
					{
						target.velocity = new Vector2 (-latestNonZeroXVelocity, target.velocity.y);
						controlsFlipped = !controlsFlipped;
						if (currentFacing == "Right")
							currentFacing = "Left";
						else
							currentFacing = "Right";
					}
					else
						target.velocity = new Vector2 (0, target.velocity.y);
				}
			}
			else {
				target.velocity = new Vector2 (target.velocity.x/(float)1.1, target.velocity.y);	
			}
		}

		// Jumping
		if (startJumping) {
			if ((Input.GetKey (leftKey) && !controlsFlipped) || (Input.GetKey (rightKey) && controlsFlipped))
				target.velocity = new Vector2 (target.velocity.x, jumpHeight);
			else if ((Input.GetKey (rightKey) && !controlsFlipped) || (Input.GetKey (leftKey) && controlsFlipped))
				target.velocity = new Vector2 (target.velocity.x, jumpHeight);
			else
				target.velocity = new Vector2 (target.velocity.x, jumpHeight);
			startJumping = false;
		}

		// Picking Up
		if (pickupObject && pickupableObject != null)
		{
			carriedObject = pickupableObject;
			pickupableObject = null;
			gameObject.AddComponent<DistanceJoint2D>();
			gameObject.GetComponentInParent<DistanceJoint2D>().connectedBody = carriedObject.GetComponent<Rigidbody2D>();
			gameObject.GetComponentInParent<DistanceJoint2D>().distance = 0;
			gameObject.GetComponentInParent<DistanceJoint2D>().anchor = objectHoldPoint;
			gameObject.GetComponentInParent<DistanceJoint2D>().connectedAnchor = carriedObject.gameObject.GetComponent<pickupableObjectScript>().objectHoldPoint;
			carriedObject.gameObject.GetComponent<Transform>().rotation = Quaternion.identity;
			carriedObject.gameObject.GetComponent<Rigidbody2D>().fixedAngle = true;
			if (carriedObject.tag == "Player")
				carriedObject.GetComponent<characterController>().currentlyBeingHeldTrueSetter();

			carryingSomething = true;
			pickupObject = false;
			Debug.Log ("Picked up object");
		} else if (throwObject && carriedObject != null) {
			Destroy(gameObject.GetComponent<DistanceJoint2D>());
			if (currentFacing == "Right")
				carriedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(100, 100);
			else
				carriedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-100, 100);
			if (carriedObject.tag == "Player")
				carriedObject.GetComponent<characterController>().currentlyBeingHeldFalseSetter();
			else {
				//carriedObject.gameObject.GetComponent<Rigidbody2D>().fixedAngle = false;
			}

			pickupableObject = null;
			carriedObject = null;
			carryingSomething = false;
			throwObject = false;
			Debug.Log ("Threw object");
		} else {
			throwObject = pickupObject = false;
		}

		// Kicking
		if (Input.GetKey (kickKey) && !isKicking)
		{
			isKicking = true;
			kickingTimer = 60;
			spriteRenderer.sprite = spritePlayerKicking;
		}

		if (isKicking)
		{
			kickingTimer--;

			if (kickingTimer == 0)
			{
				isKicking = false;
				spriteRenderer.sprite = spritePlayerNormal;
			}
		}

		// Jumping Timer
		if (jumpDelayTimer > 0)
			jumpDelayTimer--;

		// Hit Wall Timer
		if (wallHitTimer > 0)
			wallHitTimer--;
		else
			canKickOffWall = false;

		// Movement Cap Speed
		if (target.velocity.x > 150)
			target.velocity = new Vector2 (150, target.velocity.y);
		if (target.velocity.x < -150)
			target.velocity = new Vector2 (-150, target.velocity.y);
	}

	void OnGUI () {
		GUI.Label (new Rect (mainCamera.WorldToScreenPoint(target.position).x, mainCamera.WorldToScreenPoint(-target.position).y + 70, 150, 50), "Current Facing: " + currentFacing);
		GUI.Label (new Rect (mainCamera.WorldToScreenPoint(target.position).x, mainCamera.WorldToScreenPoint(-target.position).y + 90, 120, 50), "Jumps Left: " + (((int)(jumpsLeft)).ToString()));
		GUI.Label (new Rect (mainCamera.WorldToScreenPoint(target.position).x, mainCamera.WorldToScreenPoint(-target.position).y + 110, 120, 50), "X Speed: " + (int)target.velocity.x);
		GUI.Label (new Rect (mainCamera.WorldToScreenPoint (target.position).x, mainCamera.WorldToScreenPoint (-target.position).y + 130, 120, 50), "Y Speed: " + (int)target.velocity.y);
		
	}

	// Functions
	
	public void resetJumps() {
		jumpsLeft = totalJumps;
	}

	public void startWallHitTimer() {
		wallHitTimer = wallHitTimerLength;
		canKickOffWall = true;
	}

	public void pickupableObjectInFrontSetter(bool boolean)
	{
		pickupableObjectInFront = boolean;
	}

	public void pickupableObjectSetter(GameObject obj)
	{
		pickupableObject = obj;
	}

	public void currentlyBeingHeldTrueSetter()
	{
		currentlyBeingHeld = true;
		GetComponent<Rigidbody2D>().mass = 0;
	}

	public void currentlyBeingHeldFalseSetter()
	{
		currentlyBeingHeld = false;
		GetComponent<Rigidbody2D>().mass = 5;
		jumpsLeft = totalJumps;
	}
}
