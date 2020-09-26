using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
	// the static point of interest
	static public GameObject POI;

	[Header("Set in Inspector")]
	public float 	easing = 0.05f;
	public Vector2 	minXY = Vector2.zero;

	[Header("Set Dynamically")]
	// the desired z position of the camera
	public float 	camZ;

	void Awake() {
		camZ = this.transform.position.z;
	}

	void FixedUpdate() {
		// if there is only one line following an if, it does not need braces
		// if (POI == null) return; // return if there is no POI

		// get the position of the POI
		//Vector3 destination = POI.transform.position;

		Vector3 destination;

		// if there is no is no POI, return to P:(0, 0, 0)
		if (POI == null) {
			destination = Vector3.zero;
		} else {
			// get the position of the POI
			destination = POI.transform.position;

			// if POI is a projectile, check to see if its at rest
			if (POI.tag == "Projectile") {
				// if it is sleeping ( that is, not moving )
				if (POI.GetComponent<Rigidbody>().IsSleeping()) {
					// return to default view
					POI = null;
					// in the next update
					return;
				}
			}
		}
		// limit the X and Y to minimum values
		destination.x = Mathf.Max(minXY.x, destination.x);
		destination.y = Mathf.Max(minXY.y, destination.y);
		// interpolate from the current Camera position towards destination
		destination = Vector3.Lerp(transform.position, destination, easing);
		// force destination.z to be camZ to keep the camera far enough away
		destination.z = camZ;
		// set the camera to the destination
		transform.position = destination;
		// set the orthographicSize of the Camera to keep Ground in view
		Camera.main.orthographicSize = destination.y + 10;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
