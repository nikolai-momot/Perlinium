using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public Transform target;

	public float distance = 5.0f;
	[Range(100,150)]
	public float sensitivity;
	Transform cameraTransform;

	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float smoothTime = 2f;

	float rotationYAxis = 0.0f;
	float rotationXAxis = 0.0f;
	float velocityX = 0.0f;
	float velocityY = 0.0f;
    // Use this for initialization

    Vector3 offset; 

	void Start() {
        Vector3 angles = transform.eulerAngles;

		rotationYAxis = angles.y;
		rotationXAxis = angles.x;

        offset = transform.position - target.position;
    }

	void Update(){
        offset = transform.position - target.position;
        transform.position = target.transform.position + offset;

        int cameraDistance = Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel")*sensitivity);
		transform.position += new Vector3 (0,0,cameraDistance);
	}

    void LateUpdate() {
        if (target)
		{
			if (Input.GetMouseButton(0))
			{
				velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
				velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
			}
			rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
			rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
			Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
			Quaternion rotation = toRotation;
			
			transform.rotation = rotation;
			
			velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
			velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
		}
    }

    public static float ClampAngle(float angle, float min, float max) {
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}