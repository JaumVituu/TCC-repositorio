using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sistema : MonoBehaviour
{
    bool turnoDaEquipe;
    public float tempoTurno;
    public float parOuImpar;

    void Start()
    {
        parOuImpar = Mathf.Round(Random.value);
        if(parOuImpar == 1){
            turnoDaEquipe = true;
        }
        else{
            turnoDaEquipe = false;
        }
        tempoTurno = 60f;
    }

    void Update()
    {
        //Debug.Log(turnoDaEquipe);
        //Debug.Log("Tempo do turno:" + tempoTurno);
        if(tempoTurno <= 0f){
            turnoDaEquipe = !turnoDaEquipe;
            tempoTurno = 30f;
        }
        else{
            tempoTurno -= Time.deltaTime;
        }

    }
}
