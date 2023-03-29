
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class TutorialDialogController : MonoBehaviour
{
    [SerializeField] GameObject DialogTutorial, DialogDocSes;
    private int count = 0;
    //Deactivates tutorial dialog box
    //Activates record session dialog box
    public void OnTutorialOKBtnClick()
    {
        DialogTutorial.SetActive(false);
        if (count == 0)
        {
            DialogDocSes.SetActive(true);
            count++;
        }

    }

    public void openTutorial()
    {
        DialogTutorial.SetActive(true);
    }
}
