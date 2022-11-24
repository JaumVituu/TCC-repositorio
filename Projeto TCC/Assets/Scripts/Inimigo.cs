using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inimigo : MonoBehaviour
{
    [SerializeField]GameObject BarraVida; 
    int vida;
    void Start()
    {
        vida = 100;
    }

    // Update is called once per frame
    void Update()
    {
        BarraVida.transform.LookAt(Camera.current.transform);
    }
}
