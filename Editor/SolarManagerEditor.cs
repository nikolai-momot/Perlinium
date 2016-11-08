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
        base.OnInspectorGUI();
        GUILayout.Space(5);

        GUILayout.BeginVertical("box");
            GUILayout.Space(5);
            if (GUILayout.Button("Add Solar Body"))
            {
                AddSolarBody();
                return;
            }
            GUILayout.Space(5);
        GUILayout.EndVertical();

        GUILayout.Space(5);
        GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
                solarManager.solarOrbit = EditorGUILayout.Toggle("Solar Orbit", solarManager.solarOrbit);
                solarManager.sun = (GameObject)EditorGUILayout.ObjectField("Sun", solarManager.sun, typeof(GameObject), true);
                solarManager.updateVariables();
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.Space(5);
        GUILayout.BeginVertical("box");
        //GUILayout.BeginHorizontal();
        foreach (SolarBody solarBody in solarManager.solarBodies)
            {
                DrawSolarBody(solarBody);
        }
        //GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(5);
    }

    void DrawSolarBody(SolarBody solarBody) {
        GUILayout.BeginVertical("box");
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                    GUILayout.Space(5);
                        solarBody.name = GUILayout.TextField(solarBody.name, GUILayout.Width(100));
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                        solarBody.bodyType = (SolarBody.BodyType)EditorGUILayout.EnumPopup("Solar Body Type:", solarBody.bodyType);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Remove Solar Body"))
                        {
                            RemoveSolarBody(solarBody);
                            return;
                        }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add Satellite"))
                        {
                            AddSolarBody(solarBody);
                            return;
                        }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                        GUILayout.Space(5); GUILayout.Label("Mass");
                            solarBody.mass = (int)Mathf.RoundToInt(GUILayout.HorizontalSlider(solarBody.mass, 100, 1000, GUILayout.Width(100)));
                            solarBody.transform.localScale = new Vector3(solarBody.mass, solarBody.mass, solarBody.mass);
                        GUILayout.Space(5);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                        GUILayout.Space(5);
                        GUILayout.Label("Distance from Sun");
                            solarBody.distanceFromSun = Mathf.RoundToInt(GUILayout.HorizontalSlider(solarBody.distanceFromSun, 675, 5000, GUILayout.Width(100)));
                            solarBody.transform.localPosition = new Vector3(0, 0, solarBody.distanceFromSun);
                        GUILayout.Space(5);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                        GUILayout.Space(5);
                            solarBody.inOrbit = GUILayout.Toggle(solarBody.inOrbit, "inOrbit");
                        GUILayout.Space(5);
                            solarBody.orbitSpeed = Mathf.RoundToInt(GUILayout.HorizontalSlider(solarBody.orbitSpeed, 1, 50, GUILayout.Width(100)));
                        GUILayout.Space(5);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                        GUILayout.Space(5);
                            solarBody.inRotation = GUILayout.Toggle(solarBody.inRotation, "inRotation");
                        GUILayout.Space(5);
                            solarBody.rotationSpeed = GUILayout.HorizontalSlider(solarBody.rotationSpeed, 1, 3, GUILayout.Width(100));
                        GUILayout.Space(5);
                    GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        GUILayout.Space(5);
        GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                    DrawSatellites(solarBody);
                GUILayout.Space(5);
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(5);
        GUILayout.EndVertical();
    }

    void DrawSatellites(SolarBody parent)
    {
        foreach (SolarBody satellite in parent.satellites)
            DrawSolarBody(satellite);
    }

    void RemoveSolarBody(SolarBody solarBody)
    {
        solarManager.solarBodies.Remove(solarBody);
        DestroyImmediate(solarBody.gameObject);
    }

    void AddSolarBody(SolarBody solarBody)
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(solarBody.transform);
        
        SolarBody newSolar = newBody.GetComponent<SolarBody>();

        solarBody.satellites.Add(newSolar);

        string newName = "New Planet " + solarManager.solarBodies.Count;
        newBody.name = newName;
        newSolar.name = newName;

        newSolar.mass /= 20;
        newSolar.distanceFromSun /= 20;
        newSolar.axisOfOrbit = solarBody.transform;
        newBody.transform.localPosition = new Vector3(0,0,newSolar.distanceFromSun);
    }

    void AddSolarBody()
    {
        GameObject newBody = Instantiate(Resources.Load("Prefab/SolarBody", typeof(GameObject))) as GameObject;
        newBody.transform.SetParent(solarManager.sun.transform);

        SolarBody newSolar = newBody.GetComponent<SolarBody>();
        
        solarManager.solarBodies.Add(newSolar);

        string newName = "New Planet " + solarManager.solarBodies.Count;
        newBody.name = newName;
        newSolar.name = newName;

        newSolar.mass /= 4;
        newSolar.axisOfOrbit = solarManager.sun.transform;
        newBody.transform.position = new Vector3(0, 0, newSolar.distanceFromSun);
    }
}
