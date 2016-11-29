using UnityEngine;

public class CameraManager : MonoBehaviour {
	
	public Transform target;

	public float distance = 5.0f;

	[Range(100,150)]
	public float sensitivity;
	Transform cameraTransform;

    Camera cam;

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

    public float zoomSensitivity = 15.0f;
    public float zoomSpeed = 5.0f;
    public float zoomMin = 5.0f;
    public float zoomMax = 80.0f;

    private float zoom;     
    //Vector3 offset;

    void Awake() {
        cam = transform.GetComponent<Camera>();
    }

	void Start() {
        Vector3 angles = transform.eulerAngles;

		rotationYAxis = angles.y;
		rotationXAxis = angles.x;

        //offset = transform.position - target.position;

        zoom = cam.fieldOfView;
    }

	void Update(){
        int cameraDistance = Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel")*sensitivity);
		transform.position += new Vector3 (0,0,cameraDistance);

        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
    }

    void LateUpdate() {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);

        if (target)
		{
            if (Input.GetMouseButton(0))
			{
				velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
				velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
			}

            rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
		
			
			transform.rotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);

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