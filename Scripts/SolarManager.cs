using UnityEngine;
using System.Collections;

public class SolarManager : MonoBehaviour {
	public GameObject sun, earth;
	public bool inMovement;

	[Range(0,100)]
	public float rotationSpeed;

	[Range(0, 360)]
	public float rotationAngle;
	
	[Range(500,5000)]
	public float radius;

	void Start(){
		earth.transform.position = modifyPosition();
	}

	void Update() {
		if (inMovement) {
			earth.transform.Rotate (new Vector3 (0, -1, 0));
			earth.transform.RotateAround (sun.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
			Vector3 desiredPosition = modifyPosition ();
			earth.transform.position = Vector3.MoveTowards (earth.transform.position, desiredPosition, Time.deltaTime * rotationSpeed);
		}
	}

	void OnValidate() {
		earth.transform.position = modifyPosition();
	}

	Vector3 modifyPosition(){
		return (earth.transform.position - sun.transform.position).normalized * radius + sun.transform.position;
	}
}
