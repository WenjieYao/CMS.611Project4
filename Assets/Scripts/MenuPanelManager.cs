using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelManager : MonoBehaviour
{
    #region Inspector Variables

    public GameObject BlackScreen;
    public GameObject HelpPopup;
    public GameObject CreditsPopup;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BlackScreen.SetActive(true);
        HelpPopup.SetActive(false);
        CreditsPopup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel(int idx)
    {
        switch (idx)
        {
            case 1:
                HelpPopup.SetActive(true);
                break;
            case 2:
                CreditsPopup.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ClosePanel(int idx)
    {
        switch (idx)
        {
            case 1:
                HelpPopup.SetActive(false);
                break;
            case 2:
                CreditsPopup.SetActive(false);
                break;
            default:
                break;
        }
    }

}
