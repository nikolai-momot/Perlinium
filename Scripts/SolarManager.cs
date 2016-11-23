using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class SolarManager : MonoBehaviour
{
    /// <summary>
    /// Center of the solar system
    /// </summary>
    public GameObject sun;

    /// <summary>
    /// Is the solar system in motion
    /// </summary>
    //public bool solarOrbit;

    /// <summary>
    /// planets, moons, etc
    /// </summary>
    public List<SolarBody> solarBodies;

    /// <summary>
    /// Generates the map that forms the solarbody texture
    /// </summary>
    MapGenerator mapGenerator;

    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();

        transform.GetComponentInChildren<CameraController>().target = sun.transform;
    }

    /// <summary>
    /// Updates variables on inspector validation
    /// </summary>
    void OnValidate()
    {
        updateSolarBodies();
    }

    /// <summary>
    /// Loops through solar bodies within the system and updates their respective properties
    /// </summary>
    public void updateSolarBodies()
    {
        foreach (SolarBody solarBody in solarBodies)
            updateSolarBody(solarBody);
    }

    /// <summary>
    /// Takes the provided solarBody and updates their properties and sattelites
    /// </summary>
    /// <param name="solarBody"></param>
    void updateSolarBody(SolarBody solarBody)
    {
        if (solarBody == null)
            return;

        //solarBody.inOrbit = solarOrbit;
        solarBody.transform.localScale = new Vector3(solarBody.mass, solarBody.mass, solarBody.mass);
        solarBody.transform.position = new Vector3(0, 0, solarBody.adjustedDistance());

        foreach (SolarBody satellite in solarBody.satellites)
            updateSolarBody(satellite);

    }

    /// <summary>
    /// Takes the planet farthest from the sun and returns their position
    /// </summary>
    /// <returns></returns>
    public Vector3 getLastPlanetPosition()
    {
        return (solarBodies.Count == 0) ? new Vector3(0, 0, 200) : solarBodies[solarBodies.Count - 1].transform.localPosition;
    }

    /// <summary>
    /// Removes the each sattelite orbiting the provided solar body
    /// </summary>
    /// <param name="solarBody"></param>
    void RemoveSatellites(SolarBody solarBody)
    {
        foreach (SolarBody body in solarBody.satellites)
            RemoveSolarBody(body);
    }

    /// <summary>
    /// Removes the game object from each list of planets, removes their sattelites and destroys the gameobject
    /// </summary>
    /// <param name="solarBody"></param>
    public void RemoveSolarBody(SolarBody solarBody)
    {
        if (solarBody == null)
            return;

        solarBodies.Remove(solarBody);
        RemoveSatellites(solarBody);
        DestroyImmediate(solarBody.gameObject);
    }

    /// <summary>
    /// Adds a sattellite to the provided planet
    /// Create gameobject, sets up the orbit and generates texture
    /// </summary>
    /// <param name="parent"></param>
    public void AddSatellite(SolarBody parent)
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(sun.transform);

        SolarBody newSolar = newBody.GetComponent<SolarBody>();

        newSolar.distanceFromAxis += 50;

        newSolar.setupSatellite(parent.transform, parent.orbitSpeed * 2);
        parent.satellites.Add(newSolar);

        string newName = "Satellite " + parent.satellites.Count + " of " + parent.name;
        newBody.name = newName;
        newSolar.name = newName;

        if (mapGenerator == null)
            mapGenerator = GetComponent<MapGenerator>();

        mapGenerator.GenerateMap(newSolar);
    }

    /// <summary>
    /// Adds a planet to the solar system
    /// Create gameobject, sets up the orbit and generates texture
    /// </summary>
    public void AddSolarBody()
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(sun.transform, false);

        SolarBody newSolar = newBody.GetComponent<SolarBody>();

        newSolar.distanceFromAxis += Mathf.RoundToInt(getLastPlanetPosition().z);

        newSolar.setup(sun.transform, Mathf.RoundToInt(getLastPlanetPosition().z) * 2, BodyType.Earth, 5);
        solarBodies.Add(newSolar);

        string newName = "Planet " + solarBodies.Count;
        newBody.name = newName;
        newSolar.name = newName;


        if (mapGenerator == null)
            mapGenerator = GetComponent<MapGenerator>();

        mapGenerator.GenerateMap(newSolar);
    }
}
