using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    public Text ammunationText;
    public Text magText;

    public static AmmoCount occurence;

    private void Awake()
    {
        occurence = this;
    }

    public void AmmoText(int presentAmmunation)
    {
        ammunationText.text = "Ammo. " + presentAmmunation;
    }

    public void MagText(int presentmag)
    {
        magText.text = "Magzine. " + presentmag;
    }
}
