using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour {

    [SerializeField]
    private Sprite[] palettes;

    private SpriteRenderer sr;

    public void Initialize(int index)
    {
        Debug.Log("Index:" + index.ToString());
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = palettes[index];
    }
}
