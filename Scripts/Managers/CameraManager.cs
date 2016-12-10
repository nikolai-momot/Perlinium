using UnityEngine;

public class CameraManager : MonoBehaviour {
    /// <summary>
    /// The object that the camera focuses on
    /// </summary>
    GameObject target;       

    /// <summary>
    /// Offset distance between the target and camera
    /// </summary>
    Vector3 offset;

    void Start() {
        SetTarget( GameObject.Find("Sun") );
        SetOffset();
    }
    
    void LateUpdate()
    {
        OffsetCameraPosition();
    }

    /// <summary>
    /// Calculates and stores the offset value
    /// </summary>
    public void SetOffset()
    {
        offset = transform.position - target.transform.position;
    }

    /// <summary>
    /// Set the position of the camera's transform to be the same as the target with the offset added
    /// </summary>
    public void OffsetCameraPosition()
    {
        transform.position = target.transform.position + offset;
    }

    /// <summary>
    /// Adjusts the camera to an optimal view of the target
    /// </summary>
    public void SetPosition() {
        float x = target.transform.position.x;
        float y = 250;
        float z = target.transform.position.z-500;
        
        transform.position = new Vector3(x,y,z);
    }
    
    /// <summary>
    /// Sets the camera target and ajusts the camera position
    /// </summary>
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// Returns the current target gameobject
    /// </summary>
    /// <returns></returns>
    public GameObject GetTarget()
    {
        return target;
    }
}