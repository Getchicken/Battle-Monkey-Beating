using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour
{
    public GameObject Kunai;

    public void DeactivateKunai()
    {
        Kunai.SetActive(false);
    }

    public void ActivateKunai()
    {
        Kunai.SetActive(true);
    }
}
