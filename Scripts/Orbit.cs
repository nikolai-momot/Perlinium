using UnityEngine;

public class Orbit : MonoBehaviour
{
    /// <summary>
    /// The solarbody that this object will orbit around
    /// </summary>
    public Transform axis;

    /// <summary>
    /// The speed at which this object orbits about the axis
    /// </summary>
    public float orbitSpeed = 7;
    
    /// <summary>
    /// The speed at which this object rotates about itself
    /// </summary>
    public float rotationRate = 2;

    /// <summary>
    /// The distance between this object and it's axis
    /// </summary>
    public float radius;

    /// <summary>
    /// 
    /// </summary>
    float satelliteOffset;

    /// <summary>
    /// The current angle of this object relative to the "North" of it's axis
    /// </summary>
    float angle = 0f;
    
    void Start()
    {
        SetRandomStartingAngle();
    }

    void Update()
    {
        MoveSolarBody();
        RotateSolarBody();
    }

    /*void OnValidate()
    {
        if (axis != null)
            transform.localPosition = new Vector3(0, 0, radius);
    }*/

    /// <summary>
    /// Adjusts orbit variables to match the solarBody
    /// </summary>
    /// <param name="orbitAround"></param>
    /// <param name="orbitSpeed"></param>
    /// <param name="radius"></param>
    public void Setup(float distanceFromSun, Transform orbitAround, bool isSatelitte)
    {
        axis = orbitAround;

        orbitSpeed = (isSatelitte)? orbitSpeed * 2.5f: orbitSpeed;
        
        radius += orbitAround.localScale.x;

        float adjustedDistance = (isSatelitte)? distanceFromSun + radius + 50 : distanceFromSun + radius + 400;
        
        radius = (isSatelitte) ? radius: adjustedDistance;

        transform.localPosition = new Vector3(0, 0, adjustedDistance);
    }

    /// <summary>
    /// Sets the solarbody's starting position at a random point relative to the object its orbiting
    /// </summary>
    void SetRandomStartingAngle()
    {
        angle = Random.Range(0, 360);
    }
    
    /// <summary>
    /// Calculates the new orbit position and moves the solarBody accordingly
    /// </summary>
    public void MoveSolarBody()
    {
        if (axis == null)
            return;

        UpdateAngle();

        float circleX = GetXCoordinate();
        float circleY = GetYCoordinate();

        UpdatePosition(circleX, circleY);
    }

    /// <summary>
    /// Update angle according to orbit speed 
    /// </summary>
    void UpdateAngle()
    {
        angle += (orbitSpeed * Time.deltaTime) / 10;
    }

    /// <summary>
    /// Uses the Sin function to calculate the y coordinate
    /// </summary>
    /// <returns></returns>
    float GetYCoordinate()
    {
        return ( axis.localPosition.z + ( radius * Mathf.Sin( angle ) ) );
    }

    /// <summary>
    /// Uses the cos function to calculate the x coordinate 
    /// </summary>
    /// <returns></returns>
    float GetXCoordinate()
    {
        return ( axis.localPosition.x + ( radius * Mathf.Cos( angle ) ) );
    }

    /// <summary>
    /// Updates the solabody's position
    /// </summary>
    /// <param name="circleX"></param>
    /// <param name="circleY"></param>
    void UpdatePosition(float circleX, float circleY)
    {
        transform.localPosition = new Vector3(circleX, axis.localPosition.y, circleY);
    }

    /// <summary>
    /// Rotates the object about itself
    /// </summary>
    void RotateSolarBody()
    {
        transform.Rotate(new Vector3(0, -rotationRate, 0));
    }
}