
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class TutorialDialogController : MonoBehaviour
{
    [SerializeField] GameObject DialogTutorial, DialogDocSes;

    //Deactivates tutorial dialog box
    //Activates record session dialog box
    public void OnTutorialOKBtnClick()
    {
        DialogTutorial.SetActive(false);
        DialogDocSes.SetActive(true);
    }
}
