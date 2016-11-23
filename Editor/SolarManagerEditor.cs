using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SolarManager))]
public class SolarManagerEditor : Editor
{
    SolarManager solarManager;

    bool displayBaseInspector;

    /// <summary>
    /// If an item is removed then subsequent inspector calls will get a NullReference Error
    /// This is avoided by returning true ending of drawing the GUI early and redrawing it with the new items
    /// </summary>
    const bool REFRESH_EDITOR = true;

    void OnEnable()
    {
        solarManager = (SolarManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (DrawMainControlBox())
            return;

        GUILayout.Space(5);

        if (DrawSolarBodies())
            return;
    }

    bool DrawMainControlBox()
    {
        GUILayout.BeginVertical("box");
        
            DrawSunObjectField();

            if (DrawAddSolarBodyButton())
                return REFRESH_EDITOR;

        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    bool DrawSolarBodies()
    {
        GUILayout.BeginVertical();

            foreach (SolarBody solarBody in solarManager.solarBodies)
                if (DrawSolarBody(solarBody))
                    return REFRESH_EDITOR;
            
        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    bool DrawSolarBody(SolarBody solarBody)
    {
        if (solarBody == null)
            return REFRESH_EDITOR;

        GUILayout.BeginHorizontal("box");

            GUILayout.BeginVertical();

                if (DrawRemoveSolarBodyButton(solarBody))
                    return REFRESH_EDITOR;

                if (DrawSolarBodyMain(solarBody))
                   return REFRESH_EDITOR;

                if (DrawAddSatelliteButton(solarBody))
                    return REFRESH_EDITOR;

                if (DrawSatellites(solarBody))
                    return REFRESH_EDITOR;

            GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    bool DrawSatellites(SolarBody parent)
    {
        GUILayout.BeginVertical();

            foreach (SolarBody satellite in parent.satellites)
                if (DrawSatellite(satellite))
                    return REFRESH_EDITOR;

        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    bool DrawSatellite(SolarBody satellite)
    {
        GUILayout.BeginHorizontal("box");

            GUILayout.BeginVertical();

                if (DrawRemoveSatelliteButton(satellite))
                        return REFRESH_EDITOR;

                if (DrawSolarBodyMain(satellite))
                    return REFRESH_EDITOR;

            GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    bool DrawSolarBodyMain(SolarBody solarBody)
    {
        GUILayout.BeginVertical();

            DrawNameField(solarBody);
            DrawBodyTypeField(solarBody);
            DrawMassField(solarBody);
            DrawDistanceFromAxisField(solarBody);
            DrawSeedOffsetField(solarBody);

        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    bool DrawAddSolarBodyButton()
    {
        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Solar Body"))
            {
                solarManager.AddSolarBody();
                return REFRESH_EDITOR;
            }

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    bool DrawRemoveSolarBodyButton(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Remove Solar Body"))
            {
                solarManager.RemoveSolarBody(solarBody);
                return REFRESH_EDITOR;
            }

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    bool DrawAddSatelliteButton(SolarBody solarBody)
    {
        if (solarBody.bodyType != BodyType.Moon)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Satellite"))
            {
                solarManager.AddSatellite(solarBody);
                return REFRESH_EDITOR;
            }
            GUILayout.EndHorizontal();
        }

        return !REFRESH_EDITOR;
    }

    bool DrawRemoveSatelliteButton(SolarBody satellite)
    {
        GUILayout.BeginHorizontal();

            if (GUILayout.Button("Remove Satellite"))
            {
                solarManager.RemoveSolarBody(satellite);
                return REFRESH_EDITOR;
            }

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }
    
    void DrawSunObjectField()
    {
        GUILayout.BeginHorizontal();

            string label = "Center of the solar system ";
            solarManager.sun = (GameObject)EditorGUILayout.ObjectField(label, solarManager.sun, typeof(GameObject), true);
            solarManager.updateSolarBodies();

        GUILayout.EndHorizontal();
    }

    void DrawSeedOffsetField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Space(5);

            GUILayout.Label("Seed Offset");

            solarBody.seedOffset = EditorGUILayout.IntField(solarBody.seedOffset);

            GUILayout.Space(5);

        GUILayout.EndHorizontal();
    }

    void DrawDistanceFromAxisField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Space(5);

            GUILayout.Label("Distance from Sun");

            solarBody.distanceFromAxis = EditorGUILayout.IntField(solarBody.distanceFromAxis);

            solarBody.transform.localPosition = new Vector3(0, 0, solarBody.distanceFromAxis);

            GUILayout.Space(5);

        GUILayout.EndHorizontal();
    }

    void DrawMassField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Space(5);

            GUILayout.Label("Mass");

            solarBody.mass = EditorGUILayout.IntField(solarBody.mass);

            solarBody.transform.localScale = new Vector3(solarBody.mass, solarBody.mass, solarBody.mass);

            GUILayout.Space(5);

        GUILayout.EndHorizontal();
    }

    void DrawBodyTypeField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            solarBody.bodyType = (BodyType)EditorGUILayout.EnumPopup("Solar Body Type:", solarBody.bodyType);

        GUILayout.EndHorizontal();
    }

    void DrawNameField(SolarBody solarBody)
    {
        GUILayout.BeginHorizontal();

            GUILayout.Label("Name");

            solarBody.name = GUILayout.TextField(solarBody.name, GUILayout.Width(135));

        GUILayout.EndHorizontal();
    }
}
