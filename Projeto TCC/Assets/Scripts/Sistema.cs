using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sistema : MonoBehaviour
{
    bool turnoDaEquipe;
    public float tempoTurno;
    public float parOuImpar;
    [SerializeField] Text Municao;
    [SerializeField] GameObject Arma;
    [SerializeField] Text Vida;
    [SerializeField] GameObject Jogador;

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

        Municao.text = "Munição: " + Arma.GetComponent<Arma>().municaoAtual.ToString() + "/" + Arma.GetComponent<Arma>().municaoInicial.ToString();
        if(Jogador.name == "Odin (com Arma)"){
            Vida.text = Jogador.GetComponent<Odin>().vida.ToString();
        }

    }
}
