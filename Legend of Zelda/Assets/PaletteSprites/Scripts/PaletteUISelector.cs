using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public class PaletteUISelector : MonoBehaviour
{

    public int palette;

    private new CanvasRenderer renderer;

    private Material mat;

    private void Start()
    {
        renderer = GetComponent<CanvasRenderer>();

        mat = renderer.GetMaterial();

        Texture tex = mat.GetTexture("_PaletteTex");

        if (tex != null)
        {
            mat.SetFloat("_PaletteSize", tex.height);
        }
    }

#if UNITY_EDITOR

    void Update()
    {

        if (renderer == null)
        {
            renderer = GetComponent<CanvasRenderer>();
        }

        mat = renderer.GetMaterial();

        if (mat == null)
        {
            return;
        }

        Texture tex = mat.GetTexture("_PaletteTex");

        if (tex != null)
        {
            mat.SetFloat("_PaletteSize", tex.height);
        }

        mat.SetFloat("_Palette", palette);
    }

    private void OnValidate()
    {
        if (renderer == null)
        {
            renderer = GetComponent<CanvasRenderer>();
        }

        if (renderer == null)
        {
            Debug.LogError("Could not find component " + typeof(CanvasRenderer));
        }

        if (mat == null)
        {
            mat = renderer.GetMaterial();
        }

        if (mat == null)
        {
            //Debug.LogError("Could not get material!");
            return;
        }
        
        if (mat.HasProperty("_PaletteTex") == false ||
            mat.HasProperty("_PaletteSize") == false ||
            mat.HasProperty("_Palette") == false)
        {
            Debug.LogWarning("The material assigned does not have all the required properties. Likley unsuported shader. There might be unexpected results!");
        }
    }

#endif

    public void SetPalette(int p)
    {
        palette = p;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        Renderer rend = GetComponent<Renderer>();

        rend.GetPropertyBlock(mpb);

        mpb.SetFloat("_Palette", palette);

        rend.SetPropertyBlock(mpb);
    }

    public int GetPalette()
    {
        return palette;
    }
}
