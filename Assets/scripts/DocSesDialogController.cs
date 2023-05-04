using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class DocSesDialogController : MonoBehaviour
{
    [SerializeField] GameObject DialogDocSes, Panel, LanguagePanel;

    //Enables hand panel
    //Disables recsesdialog box
    public void OnRecSesYesClicked()
    {        
        Panel.SetActive(true);
        DialogDocSes.SetActive(false);
        LanguagePanel.SetActive(true);
    }

    //Sets a boolean in SpeechRecognition.cs to false if user selects no
    //Enables hand panel
    //Disables recsesdialog box
    public void OnRecSesNoClicked()
    {
        SpeechRecognition.recordSession = false;
        Panel.SetActive(true);
        DialogDocSes.SetActive(false);
        LanguagePanel.SetActive(true);
    }
}
