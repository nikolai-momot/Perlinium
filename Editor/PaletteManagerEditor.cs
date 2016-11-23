using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PaletteManager))]
public class PaletteManagerEditor : Editor
{
    /// <summary>
    /// If an item is removed then subsequent inspector calls will get a NullReference Error
    /// This is avoided by returning true ending of drawing the GUI early and redrawing it with the new items
    /// </summary>
    const bool REFRESH_EDITOR = true;

    PaletteManager paletteManager;

    void OnEnable()
    {
        paletteManager = (PaletteManager)target;
    }

    public override void OnInspectorGUI()
    {
        if(DrawMainBox())
            return;
        
        if (DrawPalettes())
            return;
    }

    bool DrawMainBox() {
        GUILayout.BeginVertical("box");

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();

                GUILayout.Space(5);

                if(DrawAddPaletteButton())
                    return REFRESH_EDITOR;

                GUILayout.Space(5);

            GUILayout.EndHorizontal();

            GUILayout.Space(5);

        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    bool DrawPalettes()
    {
        foreach (Palette palette in paletteManager.palettes)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

                GUILayout.Space(5);

                palette.bodyType = (BodyType)EditorGUILayout.EnumPopup("Solar Body Type:", palette.bodyType);

                GUILayout.Space(5);

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();

                GUILayout.Space(5);
                    if (DrawRemovePaletteButton(palette))
                        return REFRESH_EDITOR;
                GUILayout.Space(5);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            
                    GUILayout.Space(5);
            
                    if (DrawPalette(palette))
                        return REFRESH_EDITOR;
                    
                    GUILayout.Space(5);

                GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        return !REFRESH_EDITOR;
    }

    bool DrawPalette(Palette palette)
    {
        GUILayout.BeginVertical();

            GUILayout.Space(5);
        
            if(DrawAddColourButton(palette))
                return REFRESH_EDITOR;

            if (DrawColours(palette))
                return REFRESH_EDITOR;

            GUILayout.Space(5);

        GUILayout.EndVertical();

        return !REFRESH_EDITOR;
    }

    bool DrawColours(Palette palette) {
        foreach (PaletteColour colour in palette.palette)
        {
            if (DrawColour(palette, colour))
                return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }

    bool DrawColour(Palette palette, PaletteColour paletteColour) {
        GUILayout.BeginHorizontal();
            GUILayout.Space(5);

            paletteColour.name = GUILayout.TextField(paletteColour.name, GUILayout.Width(100));

            GUILayout.Space(5);

            paletteColour.height = GUILayout.HorizontalSlider(paletteColour.height, 0, 1, GUILayout.Width(100));

            GUILayout.Space(5);

            paletteColour.colour = EditorGUILayout.ColorField(paletteColour.colour);

            GUILayout.Space(5);

            if (DrawRemoveColourButton(palette, paletteColour))
                return REFRESH_EDITOR;

            GUILayout.Space(5);

        GUILayout.EndHorizontal();

        return !REFRESH_EDITOR;
    }

    bool DrawAddPaletteButton()
    {
        GUILayout.Label("Total Terrain Palettes: " + paletteManager.palettes.Count);
        if (GUILayout.Button("Add Palette"))
        {
            AddPalette();
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }

    bool DrawRemovePaletteButton(Palette palette) {
        if (GUILayout.Button("Remove Palette"))
        {
            RemovePalette(palette);
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }

    bool DrawAddColourButton(Palette palette)
    {
        if (GUILayout.Button("Add Colour"))
        {
            AddColour(palette);
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }

    bool DrawRemoveColourButton(Palette palettes, PaletteColour palette) {
        if (GUILayout.Button("X"))
        {
            RemoveColour(palettes, palette);
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }

    void AddPalette()
    {
        paletteManager.palettes.Add(new Palette());
    }

    void RemovePalette(Palette palettes)
    {
        paletteManager.palettes.Remove(palettes);
    }

    void AddColour(Palette palettes)
    {
        palettes.palette.Add(new PaletteColour());
    }

    void RemoveColour(Palette palettes, PaletteColour palette)
    {
        palettes.palette.Remove(palette);
    }
}
