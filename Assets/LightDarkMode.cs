using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightDarkMode : MonoBehaviour
{
    private static Color DARK_COLOR = Color.grey;
    private static Color LIGHT_COLOR = Color.white;

    private TextMeshProUGUI subtitleText;

    // Start is called before the first frame update
    void Start()
    {
        this.subtitleText = this.GetComponent<TextMeshProUGUI>();    
    }

    public void changeColor()
    {
        if (subtitleText.color == LIGHT_COLOR)
            subtitleText.color = DARK_COLOR;
        else
            subtitleText.color = LIGHT_COLOR;
    }
}
