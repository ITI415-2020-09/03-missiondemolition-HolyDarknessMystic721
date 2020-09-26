using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
	static private Slingshot S;
	// fields set in the Unity Inspector pane
	[Header("Set in Inspector")]
	public GameObject	prefabProjectile;
	public float 	velocityMult = 8f;

	// fields set dynamically 
	[Header("Set Dynamically")]
	public GameObject 		launchPoint;
	public Vector3		launchPos;
	public GameObject 		projectile;
	public bool 		aimingMode;
	public Rigidbody 		projectileRigidbody;

	static public Vector3 LAUNCH_POS {
		get {
			if (S == null) return Vector3.zero;
			return S.launchPos;
		}
	}
	
	void Awake() {
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
	}

	void OnMouseEnter() {
		// print("Slingshot:OnMouseEnter()");
		launchPoint.SetActive(true);
	}

	void OnMouseExit() {
		// print("Slingshot:OnMouseExit()");
		launchPoint.SetActive(false);
	}

	void OnMouseDown() {
		// the player has pressed the mouse button while over slingshot
		aimingMode = true;
		// instantiate a Projectile
		projectile = Instantiate(prefabProjectile) as GameObject;
		// start it at launchPoint 
		projectile.transform.position = launchPos;
		// set it to isKinematic for now 
		projectile.GetComponent<Rigidbody>().isKinematic = true;
		// set it to isKinematic for now
		projectileRigidbody = projectile.GetComponent<Rigidbody>();
		projectileRigidbody.isKinematic = true;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if Slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;

        // get the current mouse position in 2D screen coordinates
       	Vector3 mousePos2D = Input.mousePosition;
       	mousePos2D.z = -Camera.main.transform.position.z;
       	Vector3 mousePad3D = Camera.main.ScreenToWorldPoint(mousePos2D);

       	// find the delta from the LaunchPos to the mousePad3D
       	Vector3 mouseDelta = mousePad3D - launchPos;
       	// limit mouseDelta to the radius of the Slingshot Sphere Collider
       	float maxMagnitude = this.GetComponent<SphereCollider>().radius;
       	if (mouseDelta.magnitude > maxMagnitude) {
       		mouseDelta.Normalize();
       		mouseDelta *= maxMagnitude;
       	}

       	// move the projectile to this new position 
       	Vector3 projPos = launchPos + mouseDelta;
       	projectile.transform.position = projPos;

       	if (Input.GetMouseButtonUp(0)) {
       		// the mouse has been released
       		aimingMode = false;
       		projectileRigidbody.isKinematic = false;
       		projectileRigidbody.velocity = - mouseDelta * velocityMult;
       		FollowCam.POI = projectile;
       		projectile = null;
       	}
    }
}
