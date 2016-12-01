using UnityEngine;

public class CameraManager : MonoBehaviour {
    /// <summary>
    /// The object that the camera focuses on
    /// </summary>
    public GameObject target;       

    /// <summary>
    /// Offset distance between the target and camera
    /// </summary>
    Vector3 offset;
    
    void Start()
    {
        SetOffset();        
    }
    
    void LateUpdate()
    {
        OffsetCameraPosition();
    }

    /// <summary>
    /// Calculates and stores the offset value
    /// </summary>
    void SetOffset()
    {
        offset = transform.position - target.transform.position;
    }

    /// <summary>
    /// Set the position of the camera's transform to be the same as the target with the offset added
    /// </summary>
    void OffsetCameraPosition()
    {
        transform.position = target.transform.position + offset;
    }

    /// <summary>
    /// Lowers the camera to be closer to the target planet
    /// </summary>
    void SetPlanetViewPosition()
    {
        SetOffset();

        offset = new Vector3(offset.x, 500, offset.z);
        
        OffsetCameraPosition();
    }

    /// <summary>
    /// Sets the camera high above the entire solar system
    /// </summary>
    void SetOverviewPosition()
    {
        transform.localPosition = new Vector3(0, 4000, 0);
    }
}