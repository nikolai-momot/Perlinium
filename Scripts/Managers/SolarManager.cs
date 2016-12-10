using UnityEngine;
using System.Collections.Generic;
using System;

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
    MapManager mapManager;

    SolarBody sun;

    void Awake()
    {
        sun = GameObject.Find("Sun").GetComponent<SolarBody>();

        mapManager = GetComponent<MapManager>();

        mapManager.GenerateMaps();
    }

    void Update() {
        mapManager.MoveSunMap(sun);
    }

    /// <summary>
    /// Updates variables on inspector validation
    /// </summary>
    void OnValidate()
    {
        updateSolarBodies();
    }

    /// <summary>
    /// Checks if the solarBody is null and removes it from the list if it is
    /// </summary>
    /// <param name="solarBody"></param>
    void CheckSolarBody(SolarBody solarBody)
    {
        if (solarBody == null)
            solarBodies.Remove(solarBody);
    }

    /// <summary>
    /// Loops through solar bodies within the system and updates their respective properties
    /// </summary>
    public void updateSolarBodies()
    {
        foreach (SolarBody solarBody in solarBodies)
        {
            CheckSolarBody(solarBody);

            updateSolarBody(solarBody);
        }
    }

    /// <summary>
    /// Takes the provided solarBody and updates their properties and sattelites
    /// </summary>
    /// <param name="solarBody"></param>
    void updateSolarBody(SolarBody solarBody)
    {
        CheckSolarBody(solarBody);

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
        {
            if (satellite == null)
            {
                solarBody.satellites.Remove(satellite);
                return;
            }

            updateSolarBody(satellite);
        }
    }

    /// <summary>
    /// Takes the planet farthest from the sun and returns their position
    /// If there isn't one, it return a position close to the sun
    /// </summary>
    /// <returns></returns>
    float getLastPlanetDistance()
    {
        if (solarBodies.Count == 0)
        {
            return 200f;
        }

        float lastPlanetDistance = getLastPlanet().transform.localPosition.z;

        lastPlanetDistance += getSatelliteDistance(getLastPlanet());

        return lastPlanetDistance;
    }

    /// <summary>
    /// Gets the SolarBody farthest from the sun
    /// </summary>
    /// <returns></returns>
    SolarBody getLastPlanet() {
        Debug.Log("+ "+solarBodies[solarBodies.Count - 1].gameObject.name);
        return solarBodies[solarBodies.Count - 1];
    }

    /// <summary>
    /// Adds a sattellite to the provided planet
    /// Creates a gameobject, sets up the orbit and generates texture
    /// </summary>
    /// <param name="parent"></param>
    public void AddSatellite(SolarBody parent)
    {
        GameObject newBody = CreateSolarBodyObject();

        SolarBody newSolar = newBody.GetComponent<SolarBody>();

        float distanceFromSun = getLastPlanetDistance();
        distanceFromSun += getSatelliteDistance( parent );

        newSolar.Setup(distanceFromSun, parent.transform, BodyType.Moon, true);

        parent.AddSatellite(newSolar);

        SetSatelliteName(newBody, newSolar, parent);

        ApplyTexture(newSolar);
    }

    float getSatelliteDistance(SolarBody parent)
    {
        float satelliteDistance = 0f;

        foreach (SolarBody satellite in parent.satellites)
            satelliteDistance += satellite.mass + satellite.GetRadius() + 50;

        return satelliteDistance;
    }

    /// <summary>
    /// Sets the name of the satellite GameObject and SolarBody
    /// </summary>
    /// <param name="newBody"></param>
    /// <param name="newSolar"></param>
    /// <param name="parent"></param>
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
        GameObject newBody = CreateSolarBodyObject();

        SolarBody newSolar = newBody.GetComponent<SolarBody>();

        float distanceFromSun = getLastPlanetDistance();

        newSolar.Setup(distanceFromSun, transform, BodyType.Earth, false);

        solarBodies.Add(newSolar);

        SetPlanetName(newBody, newSolar);

        ApplyTexture(newSolar);
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
    /// Gets the GameObject from the Sun solarbody
    /// </summary>
    /// <returns></returns>
    public GameObject getSun()
    {
        return (sun == null) ? GameObject.Find("Sun") : sun.gameObject;
    }

    /// <summary>
    /// Returns a list of planets in orbit
    /// </summary>
    /// <returns></returns>
    public List<SolarBody> GetSolarBodies() {
        return (solarBodies == null)? new List<SolarBody>() : solarBodies;
    }

    /// <summary>
    /// Instatiates a SolarBody Prefab and sets it's parent
    /// </summary>
    /// <returns></returns>
    GameObject CreateSolarBodyObject()
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
    void ApplyTexture(SolarBody solarBody)
    {
        if (mapManager == null)
            mapManager = GetComponent<MapManager>();

        mapManager.GenerateMap(solarBody);
    }
}
