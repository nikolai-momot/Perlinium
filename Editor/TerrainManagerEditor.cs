using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    TerrainManager terrainManager;

    void OnEnable()
    {
        terrainManager = (TerrainManager)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical("box");
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);

        DrawAddPaletteButton();

        GUILayout.Space(5);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.EndVertical();
        GUILayout.Space(5);

        foreach (TerrainTypes palettes in terrainManager.palettes)
        {
            GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
            palettes.bodyType = (BodyType)EditorGUILayout.EnumPopup("Solar Body Type:", palettes.bodyType);
            GUILayout.Width(50);
            if (GUILayout.Button("Remove Palette"))
            {
                RemovePalette(palettes);
                return;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Add Colour"))
            {
                AddColour(palettes);
                return;
            }
            GUILayout.Space(5);
            GUILayout.EndHorizontal();
            foreach (TerrainColour palette in palettes.palette)
            {
                GUILayout.BeginVertical();
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Space(5);
                palette.name = GUILayout.TextField(palette.name, GUILayout.Width(100));
                GUILayout.Space(5);
                palette.height = GUILayout.HorizontalSlider(palette.height, 0, 1, GUILayout.Width(100));
                GUILayout.Space(5);
                palette.colour = EditorGUILayout.ColorField(palette.colour);
                GUILayout.Space(5);
                if (GUILayout.Button("X"))
                {
                    RemoveColour(palettes, palette);
                    return;
                }
                GUILayout.Space(5);
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
    }

    private void DrawAddPaletteButton()
    {
        GUILayout.Label("Total Terrain Palettes: " + terrainManager.palettes.Count);
        if (GUILayout.Button("Add Palette"))
            AddPalette();
    }

    void AddColour(TerrainTypes palettes)
    {
        palettes.palette.Add(new TerrainColour());
    }

    void RemoveColour(TerrainTypes palettes, TerrainColour palette)
    {
        palettes.palette.Remove(palette);
    }

    void AddPalette()
    {
        terrainManager.palettes.Add(new TerrainTypes());
    }

    void RemovePalette(TerrainTypes palettes)
    {
        terrainManager.palettes.Remove(palettes);
    }
}
