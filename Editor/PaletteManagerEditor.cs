using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PaletteManager))]
public class PaletteManagerEditor : Editor
{
    /// <summary>
    /// If an item is removed then subsequent inspector calls will get a NullReference Error
    /// This is avoided by returning true ending of drawing the GUI early and redrawing it with the new items
    /// </summary>
    const bool REFRESH_EDITOR = true;

    /// <summary>
    /// The editor target
    /// </summary>
    PaletteManager paletteManager;

    /// <summary>
    /// Sets editor targer on editor enable
    /// </summary>
    void OnEnable()
    {
        paletteManager = (PaletteManager)target;
    }

    /// <summary>
    /// Draws the main box and the palettes
    /// </summary>
    public override void OnInspectorGUI()
    {
        if(DrawMainBox())
            return;
        
        if (DrawPalettes())
            return;
    }

    /// <summary>
    /// The main box contents
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Loops through the palettes and prints their respective interfaces
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Draws a palette inteface
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Loops through the palette colours and draws their respective interfaces
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
    bool DrawColours(Palette palette) {
        foreach (PaletteColour colour in palette.palette)
        {
            if (DrawColour(palette, colour))
                return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }
    
    /// <summary>
    /// Draws the PaletteColour's interface
    /// </summary>
    /// <param name="palette"></param>
    /// <param name="paletteColour"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Draws the Add Palette button
    /// </summary>
    /// <returns></returns>
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
    
    /// <summary>
    /// Draws the remove palette button
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
    bool DrawRemovePaletteButton(Palette palette) {
        if (GUILayout.Button("Remove Palette"))
        {
            RemovePalette(palette);
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }
    
    /// <summary>
    /// Draws the add PaletteColour button
    /// </summary>
    /// <param name="palette"></param>
    /// <returns></returns>
    bool DrawAddColourButton(Palette palette)
    {
        if (GUILayout.Button("Add Colour"))
        {
            AddColour(palette);
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws the remove Palette button
    /// </summary>
    /// <param name="palettes"></param>
    /// <param name="palette"></param>
    /// <returns></returns>
    bool DrawRemoveColourButton(Palette palettes, PaletteColour palette) {
        if (GUILayout.Button("X"))
        {
            RemoveColour(palettes, palette);
            return REFRESH_EDITOR;
        }

        return !REFRESH_EDITOR;
    }

    /// <summary>
    /// Draws the Add Palette button
    /// </summary>
    void AddPalette()
    {
        paletteManager.palettes.Add(new Palette());
    }

    /// <summary>
    /// Draws the remove Palette button
    /// </summary>
    /// <param name="palettes"></param>
    void RemovePalette(Palette palettes)
    {
        paletteManager.palettes.Remove(palettes);
    }

    /// <summary>
    /// Draws the remove PaletteColour button
    /// </summary>
    /// <param name="palettes"></param>
    void AddColour(Palette palettes)
    {
        palettes.palette.Add(new PaletteColour());
    }

    /// <summary>
    /// Draws the remove PaletteColour button
    /// </summary>
    /// <param name="palettes"></param>
    /// <param name="palette"></param>
    void RemoveColour(Palette palettes, PaletteColour palette)
    {
        palettes.palette.Remove(palette);
    }
}
