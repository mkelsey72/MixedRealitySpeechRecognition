using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LightDarkMode : MonoBehaviour
{
    private static Color DARK_COLOR = Color.black; //new Color(35.00f/255.0f, 35.00f/255.00f, 35.00f/255.00f);
    private static Color LIGHT_COLOR = Color.white;

    private TextMeshProUGUI subtitleText;

    // Start is called before the first frame update
    void Start()
    {
        this.subtitleText = this.GetComponent<TextMeshProUGUI>();
        //this.subtitleText.color = Color.white;
    }

    public void changeColor()
    {
        if (subtitleText.color == LIGHT_COLOR)
        {
            subtitleText.faceColor = DARK_COLOR;
            subtitleText.color = DARK_COLOR;
            //subtitleText.alpha = 1;
        }
        else
        {
            subtitleText.faceColor = LIGHT_COLOR;
            subtitleText.color = LIGHT_COLOR;
        }
    }
}
