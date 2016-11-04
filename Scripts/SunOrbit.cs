using UnityEngine;
using System.Collections;

public class SunOrbit : MonoBehaviour {
	[Range(20,100)]
	public float rotationSpeed;

	void Update() {
		transform.RotateAround(Vector3.zero, Vector3.up, rotationSpeed * Time.deltaTime);
	}
}