using UnityEngine;

public class LangHandler : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    
    public void openCanvas()
    {
        canvas.SetActive(true);
    }

    public void closeCanvas()
    {
        canvas.SetActive(false);
    }
}
