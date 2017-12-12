using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class PaletteManager : MonoBehaviour {

    public const int colorCount = 20;

    [Serializable]
    public class Palette
    {
        public Color[] colors = new Color[colorCount];
    }

#if UNITY_EDITOR
    public bool liveEditMode;
#endif
    
    public Material[] paletteMaterials;
    
    public Palette[] palettes;
    
    public Texture2D tex;

#if UNITY_EDITOR
    private int lastPaletteCount;
#endif

    // TODO: Generate palette texture from inspector data

    private void Awake()
    {
        if (palettes == null)
        {
            Debug.LogWarning("No palette data found on awake!");

            return;
        }

        if (palettes.Length == 0)
        {
            return;
        }
        
        ValidatePalettes();

        tex = GenerateTexture();
        
        Color[] colors = GenerateImageColorArray();

        tex.SetPixels(colors);
        
        tex.Apply(false);

        foreach (var paletteMat in paletteMaterials)
        {
            if (paletteMat != null)
            {
                paletteMat.SetTexture("_PaletteTex", tex);
            }
        }
    }
    
    public void UpdateColors()
    {
        Color[] colors = GenerateImageColorArray();

        tex.SetPixels(colors);

        tex.Apply(false);

        foreach (var paletteMat in paletteMaterials)
        {
            if (paletteMat != null)
            {
                paletteMat.SetTexture("_PaletteTex", tex);
            }
        }
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (liveEditMode == false) return;

        if (lastPaletteCount != palettes.Length && palettes.Length != 0)
        {
            ValidatePalettes();

            if (tex == null || lastPaletteCount != palettes.Length)
            {
                tex = GenerateTexture();
            }

            UpdateColors();

            /*
            foreach (Material mat in paletteMaterials)
            {
                if (UnityEditor.AssetDatabase.Contains(mat))
                {
                    UnityEditor.EditorUtility.SetDirty(mat);
                }
                
                string path = UnityEditor.AssetDatabase.GetAssetPath(mat);

                UnityEditor.AssetDatabase.CreateAsset(mat, path);

                UnityEditor.AssetDatabase.SaveAssets();
                UnityEditor.AssetDatabase.Refresh();
            }
            */
        }

        lastPaletteCount = palettes.Length;
    }

    private void OnValidate()
    {
        ValidatePalettes();

        if (tex != null)
        {
            foreach (var paletteMat in paletteMaterials)
            {
                if (paletteMat != null)
                {
                    paletteMat.SetTexture("_PaletteTex", tex);
                }
            }
        }
    }

#endif

    private Color[] GenerateImageColorArray()
    {
        Color[] colors = new Color[colorCount * palettes.Length];

        for (int i = 0; i < palettes.Length; i++)
        {
            Array.Copy(palettes[i].colors, 0, colors, i * colorCount, colorCount);
        }

        return colors;
    }

    private Texture2D GenerateTexture()
    {
        Texture2D tex = new Texture2D(colorCount, palettes.Length, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Point,
            anisoLevel = 0
        };

        tex.name = "Palette Texture";
        tex.filterMode = FilterMode.Point;
        tex.anisoLevel = 0;
        
        return tex;
    }

    public void UpdatePalettes()
    {
        if (tex == null)
        {
            palettes = new Palette[0];

            return;
        }

        palettes = new Palette[tex.height];

        Color[] colors = tex.GetPixels();

        for (int i = 0; i < palettes.Length; i++)
        {
            palettes[i] = new Palette();

            palettes[i].colors = new Color[colorCount];

            Array.Copy(colors, i * colorCount, palettes[i].colors, 0, colorCount);
        }
    }

    private void ValidatePalettes()
    {
        if (palettes == null)
        {
            palettes = new Palette[0];
            return;
        }

        for (int i = 0; i < palettes.Length; i++)
        {
            if (palettes[i].colors.Length != colorCount)
            {
                palettes[i].colors = new Color[colorCount];
            }
        }
    }
}
