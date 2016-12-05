using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public CameraManager cameraManager;
    
    SolarManager solarManager;

    /// <summary>
    /// Navigable solarBodies
    /// </summary>
    List<GameObject> solarBodies;

    void Start() {
        solarManager = GetComponent<SolarManager>();
        FillSolarBodies();
    }

    /// <summary>
    /// Adds all the solar bodies in the system to a list of navigable gameobjects
    /// </summary>
    void FillSolarBodies() {
        solarBodies = new List<GameObject>();

        solarBodies.Add( solarManager.getSun() );

        foreach (SolarBody solarBody in solarManager.solarBodies)
        {
            solarBodies.Add(solarBody.gameObject);
            AddSatellites(solarBody);
        }
    }

    /// <summary>
    /// Adds a satellite to the solarBodies list
    /// </summary>
    /// <param name="solarBody"></param>
    void AddSatellites(SolarBody solarBody)
    {
        foreach (SolarBody satellite in solarBody.satellites) 
            solarBodies.Add(satellite.gameObject);
    }

    /// <summary>
    /// Exits the application
    /// </summary>
    public void ExitButtonPress()
    {
        Application.Quit();
    }

    /// <summary>
    /// Moves on to the next planet in the solar system
    /// </summary>
    public void NextButtonPress() {
        int index = solarBodies.IndexOf(cameraManager.GetTarget());

        cameraManager.SetTarget( (index+1 == solarBodies.Count) ? solarBodies[index] : solarBodies[index+1] );
        cameraManager.ResetPosition();
        cameraManager.SetOffset();
    }

    /// <summary>
    /// Moves on to the previous planet in the solar system
    /// </summary>
    public void PrevButtonPress() {
        int index = solarBodies.IndexOf(cameraManager.GetTarget());

        cameraManager.SetTarget((index - 1 < 0) ? solarBodies[0] : solarBodies[index-1]);
        cameraManager.ResetPosition();
        cameraManager.SetOffset();
    }
}
