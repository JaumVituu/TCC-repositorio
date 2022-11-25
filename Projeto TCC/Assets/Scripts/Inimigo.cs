using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{
    [SerializeField]Slider BarraVida;
    [SerializeField]GameObject CanvasBarraVida; 
    //[SerializeField]NavMeshAgent agent;
    [SerializeField]GameObject ObjetoTextoDano;
    [SerializeField]Text textoDano;
 
    int vida;
    [SerializeField]int vidaInicial;
    void Start()
    {
        vida = vidaInicial;
        BarraVida.maxValue = vidaInicial;
    }

    void Update()
    {
        BarraVida.value = vida;
        Camera cameraAtiva = (Camera)FindObjectOfType(typeof(Camera));
        CanvasBarraVida.transform.LookAt(cameraAtiva.transform);
    }

    public void PerderVida(TipoDano danoRecebido){
        int dano = danoRecebido.quantidadeDano;
        int vidaPerdida;
        Collider colisorAtingido = danoRecebido.lugarDano;
        if(colisorAtingido.name == "Head hurtbox"){
            vidaPerdida = dano*3;
            vida -= vidaPerdida;
            StartCoroutine(MostrarTextoDano(vidaPerdida));
        }
        if(colisorAtingido.name == "Body hurtbox"){
            vidaPerdida = dano;
            vida -= vidaPerdida;
            StartCoroutine(MostrarTextoDano(vidaPerdida));
        }       
        if (vida <= 0){
            Destroy(gameObject);
        }
    }

    IEnumerator MostrarTextoDano(int danoTexto){
        GameObject textoDano = Instantiate(ObjetoTextoDano, transform.position + new Vector3(2,0,0),Quaternion.identity);    
        Text texto = textoDano.transform.GetChild(0).GetComponent<Text>();
        texto.text = danoTexto.ToString();
        textoDano.transform.Translate(0,1*Time.deltaTime,0);
        yield return new WaitForSeconds(3);
        Destroy(textoDano);
    }
}
