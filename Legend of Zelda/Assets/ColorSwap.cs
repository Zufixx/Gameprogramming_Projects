using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwap : MonoBehaviour {

    private Color32 oldBlack = new Color32(0, 0, 0, 255);
    private Color32 oldDark = new Color32(85, 85, 85, 255);
    private Color32 oldLight = new Color32(170, 170, 170, 255);
    private Color32 oldWhite = new Color32(255, 255, 255, 255);

    [SerializeField]
    private Color32 newBlack = new Color32(0, 0, 0, 255);
    [SerializeField]
    private Color32 newDark = new Color32(85, 85, 85, 255);
    [SerializeField]
    private Color32 newLight = new Color32(170, 170, 170, 255);
    [SerializeField]
    private Color32 newWhite = new Color32(255, 255, 255, 255);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
