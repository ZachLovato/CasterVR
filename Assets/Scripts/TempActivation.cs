using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempActivation : MonoBehaviour
{
    [SerializeField] private GameObject obj1;
    [SerializeField] private GameObject obj2;

    public void switchActive()
    {
        obj1.SetActive(false);
        obj2.SetActive(true);
    }
}
