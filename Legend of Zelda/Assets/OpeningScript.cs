using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningScript : MonoBehaviour {

    [SerializeField]
    private GameObject openingUI;
    [SerializeField]
    private RectTransform openingRight;
    [SerializeField]
    private RectTransform openingLeft;

    private int i = 0;

    [SerializeField]
    private Vector2 openingPos = Vector2.zero;

    public void Opening()
    {
        if (i < 16)
        {
            i++;
            StartCoroutine(openingTimer());
        }
        else
        {
            openingUI.SetActive(false);
            GameManager.instance.AfterOpening();
        }
    }

    private IEnumerator openingTimer()
    {
        openingPos += new Vector2(32f, 0f);
        openingRight.anchoredPosition = openingPos;
        openingLeft.anchoredPosition = -openingPos;
        yield return new WaitForSeconds(0.0667f);
        Opening();
    }
}
