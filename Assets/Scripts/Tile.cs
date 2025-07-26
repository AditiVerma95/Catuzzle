using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    public int boardIndexX;
    public int boardIndexY;
    
    public int value;
    public TextMeshProUGUI valueTMPro;
    public Image image;

    public void SetValue(int value) {
        this.value = value;
        valueTMPro.text = "" + value;
    }
}