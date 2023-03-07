
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class disableCanvas : MonoBehaviour
{
    [SerializeField] GameObject DialogTutorial, DialogDocSes;

    public void OnOKBtnClick()
    {
        DialogTutorial.SetActive(false);
        DialogDocSes.SetActive(true);
    }
}
