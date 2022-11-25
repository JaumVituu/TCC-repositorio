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
    Text textoDano;
    [SerializeField]NavMeshAgent agente;
    Camera cameraAtiva;
    int vida;
    [SerializeField]int vidaInicial;
    List<GameObject> textos = new List<GameObject>();
    void Start()
    {
        vida = vidaInicial;
        BarraVida.maxValue = vidaInicial;
    }

    void Update()
    {
        BarraVida.value = vida;
        cameraAtiva = (Camera)FindObjectOfType(typeof(Camera));
        CanvasBarraVida.transform.LookAt(cameraAtiva.transform);
        agente.SetDestination(GameObject.FindWithTag("Player").transform.position);

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
        if(colisorAtingido.name == "Body hurtbox"){
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
        GameObject textoDano = Instantiate(ObjetoTextoDano, transform.position + new Vector3(0,3f,0),Quaternion.identity);
        textoDano.transform.LookAt(cameraAtiva.transform);    
        Text texto = textoDano.transform.GetChild(0).GetComponent<Text>();
        texto.text = danoTexto.ToString();
        textoDano.GetComponent<Rigidbody>().velocity = new Vector3(0,2,0);
        StartCoroutine(FadeText(texto.color,textoDano));
    }

    IEnumerator FadeText(Color textoFade, GameObject objetoTexto){
        float i = 1;
        while(i>=0){
            if(vida<=0){
                Destroy(textoDano,0.25f);
            }
            textoFade = new Color (textoFade.r,textoFade.g,textoFade.b,i);
            i -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        if(i<=0){
            Destroy(objetoTexto);
        }
    }

    /*void ChecarParedes(){
        if(Physics.SphereCast()){

        }
    }*/
}
