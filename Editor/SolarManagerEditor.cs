using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SolarManager))]
public class SolarManagerEditor : Editor
{
    SolarManager solarManager;

    void OnEnable()
    {
        solarManager = (SolarManager)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        
        GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
                solarManager.sun = (GameObject)EditorGUILayout.ObjectField("Center of the solar system ", solarManager.sun, typeof(GameObject), true);
                solarManager.updateVariables();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Solar Body"))
                {
                    AddSolarBody();
                    return;
                }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                solarManager.solarOrbit = EditorGUILayout.Toggle("Solar Orbit", solarManager.solarOrbit);
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.BeginVertical();
            foreach (SolarBody solarBody in solarManager.solarBodies)
            {
                if (DrawSolarBody(solarBody))
                    return;
            }
        GUILayout.EndVertical();
    }

    bool DrawSolarBody(SolarBody solarBody)
    {

        if (solarBody == null)
            return true;

        GUILayout.BeginVertical("box");
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
                DrawLeftColumn(solarBody);
                if (DrawRightColumn(solarBody))
                    return true;
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal(); GUILayout.BeginVertical();
                if (DrawSatellites(solarBody))
                    return true;    
            GUILayout.EndVertical();GUILayout.EndHorizontal(); 
            GUILayout.Space(5);
        GUILayout.EndVertical();

        return false;
    }

    void DrawLeftColumn(SolarBody solarBody)
    {
        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                GUILayout.Label("Name");
                solarBody.name = GUILayout.TextField(solarBody.name, GUILayout.Width(135));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                solarBody.bodyType = (SolarBody.BodyType)EditorGUILayout.EnumPopup("Solar Body Type:", solarBody.bodyType);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                    GUILayout.Label("Mass");
                    solarBody.mass = EditorGUILayout.IntField(solarBody.mass);
                    solarBody.transform.localScale = new Vector3(solarBody.mass, solarBody.mass, solarBody.mass);
                GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                    GUILayout.Label("Distance from Sun");
                    solarBody.distanceFromSun = EditorGUILayout.IntField(solarBody.distanceFromSun);
                    solarBody.transform.localPosition = new Vector3(0, 0, solarBody.distanceFromSun);
                GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                    GUILayout.Label("Seed Offset");
                    solarBody.seedOffset = EditorGUILayout.IntField(solarBody.seedOffset);
                GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        GUILayout.EndVertical();
    }

    bool DrawRightColumn(SolarBody solarBody)
    {
        GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                    solarBody.inOrbit = GUILayout.Toggle(solarBody.inOrbit, "Orbit");
                GUILayout.Space(5);
                    solarBody.orbitSpeed = Mathf.RoundToInt(GUILayout.HorizontalSlider(solarBody.orbitSpeed, 1, 50, GUILayout.Width(100)));
                GUILayout.Space(5);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                    solarBody.inRotation = GUILayout.Toggle(solarBody.inRotation, "Rotation");
                GUILayout.Space(5);
                    solarBody.rotationSpeed = GUILayout.HorizontalSlider(solarBody.rotationSpeed, 1, 3, GUILayout.Width(100));
                GUILayout.Space(5);
            GUILayout.EndHorizontal();
                if (solarBody.axisOfOrbit.Equals(solarManager.sun.transform))
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add Satellite"))
                    {
                        AddSolarBody(solarBody);
                        return true;
                    }
                    GUILayout.EndHorizontal();
                }
            GUILayout.BeginHorizontal();
                if (GUILayout.Button("Remove Solar Body"))
                {
                    RemoveSolarBody(solarBody);
                    return true;
                }
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        return false;
    }
    
    bool DrawSatellites(SolarBody parent)
    {
        foreach (SolarBody satellite in parent.satellites)
            if (DrawSolarBody(satellite))
                return true;

        return false;
    }

    void RemoveSatellites(SolarBody solarBody) {
        foreach (SolarBody body in solarBody.satellites)
            RemoveSolarBody(body);
    }

    void RemoveSolarBody(SolarBody solarBody)
    {
        if (solarBody == null)
            return;

        solarManager.solarBodies.Remove(solarBody);
        RemoveSatellites(solarBody);
        DestroyImmediate(solarBody.gameObject);
    }

    void AddSolarBody(SolarBody parent)
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(solarManager.sun.transform);

        SolarBody newSolar = newBody.GetComponent<SolarBody>();
        parent.satellites.Add(newSolar);

        string newName = "Satellite " + solarManager.solarBodies.Count+" of " + parent.name;
        newBody.name = newName;
        newSolar.name = newName;
        
        newSolar.mass /= 15;
        newSolar.axisOfOrbit = parent.transform;


        newSolar.distanceFromSun /= 20;
        newSolar.distanceFromSun += Mathf.RoundToInt(parent.transform.position.z);
        newBody.transform.localPosition = new Vector3(0,0, newSolar.distanceFromSun);



    }

    void AddSolarBody()
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(solarManager.sun.transform, false);

        SolarBody newSolar = newBody.GetComponent<SolarBody>();
        
        solarManager.solarBodies.Add(newSolar);

        string newName = "Planet " + solarManager.solarBodies.Count;
        newBody.name = newName;
        newSolar.name = newName;
        
        newSolar.axisOfOrbit = solarManager.sun.transform;
        
        newBody.transform.position = new Vector3(0, 0, newSolar.distanceFromSun);
    }
}
