using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableBtn : MonoBehaviour
{
    public GameObject gameobj;
    public bool isEnabled = true;

    public void ButtonClicked()
    {
        isEnabled = !isEnabled;
        gameobj.SetActive(isEnabled);
    }
}
