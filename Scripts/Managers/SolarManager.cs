using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MapManager))]
public class SolarManager : MonoBehaviour
{
    /// <summary>
    /// planets, moons, etc
    /// </summary>
    public List<SolarBody> solarBodies;

    /// <summary>
    /// Generates the map that forms the solarbody texture
    /// </summary>
    MapManager mapGenerator;

    void Start()
    {
        mapGenerator = GetComponent<MapManager>();
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

        solarBody.ScaleBodyMass();

        updateSatellites(solarBody);
    }

    /// <summary>
    /// Loops through the satellites of the provided solarbody
    /// </summary>
    /// <param name="solarBody"></param>
    void updateSatellites(SolarBody solarBody)
    {
        foreach (SolarBody satellite in solarBody.satellites)
            updateSolarBody(satellite);
    }

    /// <summary>
    /// Takes the planet farthest from the sun and returns their position
    /// If there isn't one, it return a position close to the sun
    /// </summary>
    /// <returns></returns>
    public Vector3 getLastPlanetPosition()
    {
        return (solarBodies.Count == 0) ? new Vector3(0,0,200) : getLastPlanet().transform.localPosition;
    }
    
    /// <summary>
    /// Gets the SolarBody farthest from the sun
    /// </summary>
    /// <returns></returns>
    SolarBody getLastPlanet() {
        return solarBodies[solarBodies.Count - 1];
    }

    /// <summary>
    /// Adds a sattellite to the provided planet
    /// Creates a gameobject, sets up the orbit and generates texture
    /// </summary>
    /// <param name="parent"></param>
    public void AddSatellite(SolarBody parent)
    {
        GameObject newBody = CreateSolarObject();

        SolarBody newSolar = newBody.GetComponent<SolarBody>();
        
        newSolar.SetupSatellite(parent.transform, parent.GetOrbitSpeed() * 2);

        parent.AddSatellite(newSolar);

        SetSatelliteName(newBody, newSolar, parent);

        GenerateMap(newSolar);
    }

    void SetSatelliteName(GameObject newBody, SolarBody newSolar, SolarBody parent)
    {
        string newName = "Satellite " + parent.satellites.Count + " of " + parent.name;
        newBody.name = newName;
        newSolar.name = newName;
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
    /// Adds a planet to the solar system
    /// Create gameobject, sets up the orbit and generates texture
    /// </summary>
    public void AddSolarBody()
    {
        GameObject newBody = CreateSolarObject();

        SolarBody newSolar = newBody.GetComponent<SolarBody>();
        
        newSolar.Setup(transform, Mathf.RoundToInt(getLastPlanetPosition().z) * 2, BodyType.Earth, 5);

        solarBodies.Add(newSolar);

        SetPlanetName(newBody, newSolar);

        GenerateMap(newSolar);
    }

    /// <summary>
    /// Sets the name of the planet based on it's place in the solar system
    /// </summary>
    /// <param name="newBody"></param>
    /// <param name="newSolar"></param>
    void SetPlanetName(GameObject newBody, SolarBody newSolar)
    {
        string newName = "Planet " + solarBodies.Count;
        newBody.name = newName;
        newSolar.name = newName;
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
    /// Instatiates a SolarBody Prefab and sets it's parent
    /// </summary>
    /// <returns></returns>
    GameObject CreateSolarObject()
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;

        newBody.transform.SetParent(transform);

        return newBody;
    } 
    /// <summary>
    /// Fetches the MapManager if it hasn't been already and
    /// calls the method to generate texture maps for the solar system
    /// </summary>
    /// <param name="solarBody"></param>
    void GenerateMap(SolarBody solarBody)
    {
        if (mapGenerator == null)
            mapGenerator = GetComponent<MapManager>();

        mapGenerator.GenerateMap(solarBody);
    }
}
