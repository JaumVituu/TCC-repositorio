using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Inimigo : MonoBehaviour
{
    [SerializeField]Slider BarraVida;
    [SerializeField]GameObject CanvasBarraVida; 
    [SerializeField]GameObject ObjetoTextoDano;
    Text textoDano;
    [SerializeField]NavMeshAgent agente;
    Camera cameraAtiva;
    int vida;
    [SerializeField]int vidaInicial;
    [SerializeField]Animator anim;
    [SerializeField]LayerMask camadaPlayer;
    [SerializeField]float velocidade;
    [SerializeField]bool estaAndando;
    public bool terminouDeBater;
    TipoDano danoFogo;

    void Start()
    {
        vida = vidaInicial;
        BarraVida.maxValue = vidaInicial;
        estaAndando = true;
    }

    void Update()
    {
        BarraVida.value = vida;
        cameraAtiva = (Camera)FindObjectOfType(typeof(Camera));
        if(estaAndando){
            SeguirJogador();
        }
        AtacarJogador();
        CanvasBarraVida.transform.LookAt(cameraAtiva.transform);

    }

    public void PerderVida(TipoDano danoRecebido){
        int dano = danoRecebido.quantidadeDano;
        int vidaPerdida;
        Collider colisorAtingido = danoRecebido.lugarDano;
        if(colisorAtingido.name == "Head hurtbox"){
            vidaPerdida = dano*3;
            vida -= vidaPerdida;
            if(vida > 0){
                MostrarTextoDano(vidaPerdida);
            }
        }
        if(colisorAtingido.name == "Body hurtbox" || colisorAtingido == null){
            vidaPerdida = dano;
            vida -= vidaPerdida;
            if(vida > 0){
                MostrarTextoDano(vidaPerdida);
            }
        } 

        if (vida <= 0){
            /*foreach(i in textos){
                Destroy(textos[i]);
            };*/
            Destroy(gameObject);
        }
    }

    void MostrarTextoDano(int danoTexto){
        GameObject textoDano = Instantiate(ObjetoTextoDano, transform.position + new Vector3(0,1.5f,0),Quaternion.identity);
        textoDano.transform.LookAt(cameraAtiva.transform);
        Text texto = textoDano.transform.GetChild(0).GetComponent<Text>();
        texto.text = danoTexto.ToString();
        textoDano.GetComponent<Rigidbody>().velocity = new Vector3(0,4,0);
        Destroy(textoDano, 0.5f);
    }

    /*IEnumerator FadeText (Action<Color> cor){
        yield return new WaitForSeconds(0.1f);
        Color corAlfa = new Color(cor.r, cor.g, cor.b, 0);
        cor(corAlfa);
    }*/

    void AtacarJogador(){
        if(Physics.CheckSphere(transform.position,1.5f,camadaPlayer)){
            terminouDeBater = false;
            Collider[] jogador = Physics.OverlapSphere(transform.position,2,camadaPlayer);
            //transform.LookAt(new Vector3 (jogador[0].gameObject.transform.position.x,transform.position.y,jogador[0].gameObject.transform.position.z));
            estaAndando = false;
            anim.SetBool("EstaAndando",false);
            agente.speed = 0;
            anim.SetBool("EstaAtacando",true);
        }
        if(!Physics.CheckSphere(transform.position,2,camadaPlayer) && terminouDeBater){
            //transform.LookAt(GameObject.FindWithTag("Player").transform.position);
            estaAndando = true;
            anim.SetBool("EstaAtacando",false);
        }
    }

    void SeguirJogador(){
        //transform.LookAt(GameObject.FindWithTag("Player").transform.position);
        agente.SetDestination(GameObject.FindWithTag("Player").transform.position);
        anim.SetBool("EstaAndando",true);
        agente.speed = velocidade;
    }
    
    void Morrer(){
        anim.SetTrigger("Morreu");
    }

    void OnTriggerStay(Collider colisao){
        if(colisao.gameObject.tag == "Fogo"){
            danoFogo.quantidadeDano = 1;
            PerderVida(danoFogo);
        }
    }
    
}
