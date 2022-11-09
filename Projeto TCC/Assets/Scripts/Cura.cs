using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cura : MonoBehaviour
{
    public Color[] cor;
    public string tag1;
    public string tag2;
    float tempodesativo;
    float tempoativo;
    // Start is called before the first frame update
    void Start(){
        tempodesativo = 3;
        tempoativo = 0.01f;

    }
    void Update(){
        if(gameObject.tag == "Desativa"){
            tempodesativo = tempodesativo - Time.deltaTime;
        }
        if(tempodesativo <= 0){
            gameObject.tag = tag1;
            tempodesativo = 3;
            Color c = cor[1];
            GetComponent<Renderer>().material.color = c;
        }
        if(tempoativo <= 0){
            Color c = cor[0];
            GetComponent<Renderer>().material.color = c;
            gameObject.tag = tag2;
            tempoativo = 0.01f;
        }
    }
    void OnTriggerEnter(Collider pessoa){
        if(pessoa.gameObject.tag == "Player"){
            tempoativo = tempoativo - Time.deltaTime;
        }
    }
}
