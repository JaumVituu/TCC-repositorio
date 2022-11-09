using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odin : MonoBehaviour
{
    Jogador Personagem = new Jogador("Odin");
    bool temArma;
    [SerializeField] GameObject Arma;
    public Camera cameraJogador;
    public GameObject Corvo;
    public bool segurandoCorvo;
    public bool corvoUsado;
    public bool corvoDisponivel;
    GameObject habilidadeCorvo;
    Transform cameraCorvo;
    float tempoVoo;
    bool vooTerminou;
    float recargaCorvo;
    public Quaternion rotacaoCorvo;
    private CharacterController charController;
    public GameObject Sistema;
    bool usandoCorvo;
    public float velocidade;
    float MouseY, MouseX;
    int tipoVoo;
    Vector3 direcaoCorvo;
    public float velCorvo;
    bool armaVisivel;
    //public bool EquipeAtual;

    void Start()
    {
        segurandoCorvo = false;
        corvoUsado = false;    
        tempoVoo = 3f;
        recargaCorvo = 30f;
        charController = GetComponent<CharacterController>();
        usandoCorvo = false;
        corvoDisponivel = false;
        temArma = true;
        armaVisivel = true;
    }


    void Update()
    {
        Personagem.Cair(charController, gameObject);
        //if(Sistema.GetComponent<Sistema>().turnoDaEquipe == EquipeAtual)
            if(cameraJogador.GetComponent<Camera>().enabled == true){
                Personagem.Movimenta(gameObject,charController, velocidade);
                Personagem.MovimentaMouse(gameObject,cameraJogador,charController);
            }
        //}
        UsarCorvo();
    }
    

    void UsarCorvo(){
        if(vooTerminou && !armaVisivel){
            Personagem.MudarVisibilidadeArma(Arma);
            armaVisivel = true;
        }
        rotacaoCorvo = /*transform.rotation */ cameraJogador.transform.rotation;       
        if (corvoUsado){  
            if(tipoVoo == 0){
                if(tempoVoo >= 0f){
                    //direcaoCorvo = habilidadeCorvo.transform.forward + habilidadeCorvo.transform.right;
                    habilidadeCorvo.transform.Translate(0,0,5*Time.deltaTime,Space.Self);
                    //habilidadeCorvo.GetComponent<Rigidbody>().velocity = direcaoCorvo*velCorvo;
                    cameraCorvo = habilidadeCorvo.transform.GetChild(0);
                    cameraCorvo.GetComponent<Camera>().enabled = false;
                    tempoVoo -= Time.deltaTime;
                }
                else{
                    corvoDisponivel = true;
                }
            }          
            if(tipoVoo == 1){
                if(tempoVoo >= 0f){
                    armaVisivel = false;
                    vooTerminou = false;
                    habilidadeCorvo.transform.Translate(0,0,10*Time.deltaTime,Space.Self);
                    direcaoCorvo = habilidadeCorvo.transform.forward + habilidadeCorvo.transform.right;
                    //habilidadeCorvo.GetComponent<Rigidbody>().velocity = direcaoCorvo*velCorvo;
                    cameraCorvo = habilidadeCorvo.transform.GetChild(0);
                    cameraCorvo.GetComponent<Camera>().enabled = false;
                    tempoVoo -= Time.deltaTime;
                    /*if(MouseY <= 90f && MouseY >= -90f){
                        MouseY += Input.GetAxis("Mouse Y");        
                    }
                    if(MouseY >= 90f && Mathf.Abs(Input.GetAxis("Mouse Y"))/-Input.GetAxis("Mouse Y") == 1){
                        MouseY += Input.GetAxis("Mouse Y");
                    }
      
                    if(MouseY <= -90f && Mathf.Abs(Input.GetAxis("Mouse Y"))/-Input.GetAxis("Mouse Y") == -1){
                        MouseY += Input.GetAxis("Mouse Y");
                    }

                    MouseX += Input.GetAxis("Mouse X")*2;

                    habilidadeCorvo.transform.eulerAngles = new Vector3(-MouseY,MouseX,0);*/
                    habilidadeCorvo.transform.eulerAngles = cameraJogador.transform.rotation.eulerAngles;
                }
                else{
                    vooTerminou = true;
                    corvoDisponivel = true;
                }
            }
        }

        if(corvoDisponivel){
            if(Input.GetKeyDown(KeyCode.E)){
                    cameraCorvo.GetComponent<Camera>().enabled = !cameraCorvo.GetComponent<Camera>().enabled;
                    cameraJogador.GetComponent<Camera>().enabled = !cameraJogador.GetComponent<Camera>().enabled;
                    recargaCorvo -= Time.deltaTime;
                    usandoCorvo = !usandoCorvo;
                    
                }
        }


        if(corvoUsado == false){
            if(segurandoCorvo == false){
                if(Input.GetKeyDown(KeyCode.E)){
                    segurandoCorvo = true;
                    Debug.Log("pegou corvo");
                    if(temArma){
                        Personagem.MudarVisibilidadeArma(Arma);
                    }
                }
            }
            else{
                if(Input.GetKeyDown(KeyCode.E)){
                    segurandoCorvo = false;
                    Debug.Log("soltou corvo");
                    if(temArma){
                        Personagem.MudarVisibilidadeArma(Arma);
                    }
                }
                if(Input.GetMouseButtonDown(0)){
                    tipoVoo = 0;
                    habilidadeCorvo = Instantiate(Corvo, transform.position + new Vector3(1,1,0), rotacaoCorvo );

                    corvoUsado = true;
                    try{
                        Corvo.transform.Rotate(0,0,0);
                        Debug.Log("Ok");
                    }
                    catch(Exception e){
                        Debug.Log("Erro: " + e);
                    }
            	}

                if(Input.GetMouseButtonDown(1)){
                    tipoVoo = 1;
                    habilidadeCorvo = Instantiate(Corvo, transform.position + new Vector3(1,1,0), rotacaoCorvo );

                    corvoUsado = true;
                    try{
                        Corvo.transform.Rotate(0,0,0);
                        Debug.Log("Ok");
                    }
                    catch(Exception e){
                        Debug.Log("Erro: " + e);
                    }
            	}             
            } 
        }
        
        if(usandoCorvo){
            try{
                if(MouseY <= 90f && MouseY >= -90f){
                    MouseY += Input.GetAxis("Mouse Y");        
                }
                if(MouseY >= 90f && Mathf.Abs(Input.GetAxis("Mouse Y"))/-Input.GetAxis("Mouse Y") == 1){
                    MouseY += Input.GetAxis("Mouse Y");
                }
      
                if(MouseY <= -90f && Mathf.Abs(Input.GetAxis("Mouse Y"))/-Input.GetAxis("Mouse Y") == -1){
                    MouseY += Input.GetAxis("Mouse Y");
                }

                MouseX += Input.GetAxis("Mouse X")*2;

                habilidadeCorvo.transform.eulerAngles = new Vector3(-MouseY,MouseX,0);
            }
            catch(Exception e){
                Debug.Log("Corvo não encontrado, código do erro: " + e);
            }
        }              
    }    
}
