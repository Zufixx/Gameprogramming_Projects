using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class PaletteSelector : MonoBehaviour {
    
    [SerializeField]
    private int palette;

    private new Renderer renderer;

    private MaterialPropertyBlock block;
    
    private void Start()
    {
        renderer = GetComponent<Renderer>();

        block = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(block);

        Texture tex = renderer.sharedMaterial.GetTexture("_PaletteTex");
        
        if (tex != null)
        {
            block.SetFloat("_PaletteSize", tex.height);
        }

        renderer.SetPropertyBlock(block);
    }

#if UNITY_EDITOR

    void Update () {

        if (block == null)
        {
            block = new MaterialPropertyBlock();
        }

        if (renderer == null)
        {
            renderer = GetComponent<Renderer>();
        }

        if (renderer.sharedMaterial.HasProperty("_PaletteTex") == false)
        {
            Debug.LogWarning("Material does not have property; _PaletteTex! This is likley an unsuported shader!");

            return;
        }

        renderer.GetPropertyBlock(block);
        
        Texture tex = renderer.sharedMaterial.GetTexture("_PaletteTex");

        if (tex != null)
        {
            block.SetFloat("_PaletteSize", tex.height);
        }
        
        block.SetFloat("_Palette", palette);

        renderer.SetPropertyBlock(block);
    }

    private void OnValidate()
    {
        if (renderer == null)
        {
            return;
        }

        if (renderer.sharedMaterial != null)
        {
            if (renderer.sharedMaterial.HasProperty("_PaletteTex") == false ||
            renderer.sharedMaterial.HasProperty("_PaletteSize") == false ||
            renderer.sharedMaterial.HasProperty("_Palette") == false)
            {
                Debug.LogWarning("The material assigned does not have all the required properties. Likley unsuported shader. There might be unexpected results!");
            }
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
