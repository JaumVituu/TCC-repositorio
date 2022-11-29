using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sistema : MonoBehaviour
{
    Scene cena;
    bool turnoDaEquipe;
    public float tempoTurno;
    public float parOuImpar;
    [SerializeField] Text Municao;
    [SerializeField] GameObject Arma;
    [SerializeField] Text Vida;
    [SerializeField] GameObject Jogador;
    [SerializeField] float tempoRestante;
    [SerializeField] Text Timer;
    [SerializeField]GameObject[] portalAtivo = new GameObject[3];

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
        cena = SceneManager.GetActiveScene();
        if(cena.name == "Mapa Usina (Sutur)" || cena.name == "Mapa Usina (Odin)"){
            portalAtivo[ (int)Mathf.Round(Random.Range(0,2))].SetActive(true);
        } 
    }

    void Update()
    {
        if(cena.name == "Mapa Usina (Sutur)" || cena.name == "Mapa Usina (Odin)"){
            tempoRestante -= Time.deltaTime;
        }
        if(tempoRestante <= 0){
            tempoRestante = 0;
            Jogador.SendMessage("ComecarFade","GameOver");
            
        }

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
            Vida.text = "Vida: " + Jogador.GetComponent<Odin>().vida.ToString();
        }
        if(Jogador.name == "Sutur(com Arma)"){
            Vida.text = "Vida: " + Jogador.GetComponent<Sutur>().vida.ToString();
        }
        if(cena.name == "Mapa Usina (Sutur)" || cena.name == "Mapa Usina (Odin)"){
            Timer.text = "Tempo restante: " + TimeFormat(tempoRestante);
        }
    }

    public string TimeFormat(float tempo){
        int segundos = (int) tempo%60;
        int minutos = (int) tempo/60;
        return string.Format("{0:00}:{1:00}", minutos,segundos);
    }
}
