using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirRachaController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideAllRachas() {
        GameObject[] graves = GameObject.FindGameObjectsWithTag("Grave");
        foreach (var grave in graves) {
            grave.GetComponent<Grave>().HideRacha();
        }
    }
}
