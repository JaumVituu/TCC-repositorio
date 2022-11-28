using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Odin : MonoBehaviour
{
    public int vida;
    Jogador Personagem = new Jogador("Odin");
    bool temArma;
    public Transform Arma;
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
    public Vector2 recuoArma;
    [SerializeField]GameObject sangue;
    [SerializeField]Color corSangue;
    [SerializeField]Animator animatorOdin;
    [SerializeField]Image fadeOut;
    Color fadeOutColor;
    bool telaEscureceu;
    [SerializeField]bool isDebuging;
    public bool podeAgir;
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
        telaEscureceu = false;
        podeAgir = true;
    }

    public void EstarOcioso(bool variavel){
        podeAgir = variavel;
        Debug.Log("Ok");
    }


    void Update()
    {
        Arma.gameObject.GetComponent<Arma>().personagemPodeAgir = podeAgir;
        if(isDebuging)DebugCode();
        
        if(vida <= 0){
            Personagem.Morrer(animatorOdin);
            vida = 0;
            StartCoroutine(MostrarGameOver("GameOver"));
        }
        else{
            if(podeAgir){
                UsarCorvo();
            }
            sangue.GetComponent<Image>().color = Color.Lerp(new Color (sangue.GetComponent<Image>().color.r,sangue.GetComponent<Image>().color.g,sangue.GetComponent<Image>().color.b,0), sangue.GetComponent<Image>().color, 0.9975f);
        }

        corSangue = sangue.GetComponent<Image>().color;
        Personagem.Cair(charController, gameObject);
        //if(Sistema.GetComponent<Sistema>().turnoDaEquipe == EquipeAtual)
            if(cameraJogador.GetComponent<Camera>().enabled == true && vida > 0 && podeAgir){
                Personagem.Movimenta(gameObject,charController, velocidade);
                Personagem.MovimentaMouse(gameObject,cameraJogador,charController,recuoArma);
            }
        //}
        Arma = cameraJogador.transform.GetChild(0);
        if(Arma.gameObject.tag == "Arma"){
            recuoArma = Arma.gameObject.GetComponent<Arma>().recuo;
        }
    }

    void DebugCode(){
        if((Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift)) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space))){
            Personagem.Morrer(animatorOdin);
            StartCoroutine(MostrarGameOver("GameOver"));
        }
        if(Input.GetKeyDown(KeyCode.P)){
            Personagem.ReceberDano(25,corSangue, sangue);
        }
    }

    public void PerderVida(int vidaPerdida){
        vida -= vidaPerdida;
    }
    

    void UsarCorvo(){
        if(vooTerminou && !armaVisivel){
            Personagem.MudarVisibilidadeArma(Arma.gameObject);
            armaVisivel = true;
        }
        rotacaoCorvo = cameraJogador.transform.rotation;       
        if (corvoUsado){
            if(tempoVoo >= 0f && Input.GetKey(KeyCode.E)){
                tempoVoo = 0;
            }  
            if(tipoVoo == 0){
                if(tempoVoo >= 0f){
                    armaVisivel = false;
                    vooTerminou = false;
                    habilidadeCorvo.transform.Translate(0,0,10*Time.deltaTime,Space.Self);
                    cameraCorvo = habilidadeCorvo.transform.GetChild(0);
                    cameraCorvo.GetComponent<Camera>().enabled = false;
                    tempoVoo -= Time.deltaTime;
                }
                else{
                    corvoDisponivel = true;
                    vooTerminou = true;
                }
            }          
            if(tipoVoo == 1){
                if(tempoVoo >= 0f){
                    armaVisivel = false;
                    vooTerminou = false;
                    habilidadeCorvo.transform.Translate(0,0,20*Time.deltaTime,Space.Self);
                    direcaoCorvo = habilidadeCorvo.transform.forward + habilidadeCorvo.transform.right;
                    cameraCorvo = habilidadeCorvo.transform.GetChild(0);
                    cameraCorvo.GetComponent<Camera>().enabled = false;
                    tempoVoo -= Time.deltaTime;
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
                        Personagem.MudarVisibilidadeArma(Arma.gameObject);
                    }
                }
            }
            else{
                if(Input.GetKeyDown(KeyCode.E)){
                    segurandoCorvo = false;
                    Debug.Log("soltou corvo");
                    if(temArma){
                        Personagem.MudarVisibilidadeArma(Arma.gameObject);
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
            cameraJogador.GetComponent<Camera>().enabled = false;
            cameraCorvo.gameObject.SetActive(true);
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
        else if(corvoDisponivel){
            cameraJogador.gameObject.SetActive(true);
            try{
                cameraCorvo.gameObject.SetActive(false);
            }
            catch(Exception e){
                Debug.Log("Erro: " + e);
            }
        }              
    }

    void OnTriggerEnter(Collider colisao){
        if(colisao.gameObject.name == "Kick hitbox"){
            vida -= Personagem.ReceberDano(50, corSangue, sangue);
        }
    }
    
    public void ComecarFade(string nomeDaCena){
        StartCoroutine(MostrarGameOver(nomeDaCena));
    }

    IEnumerator MostrarGameOver(string nomeCena){
        if(!telaEscureceu){
            StartCoroutine(FadeOut());
            telaEscureceu = true;
        }
        yield return new WaitForSeconds(3);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(nomeCena);
    }

    IEnumerator FadeOut(){
        do{
            fadeOutColor = fadeOut.GetComponent<Image>().color;
            fadeOutColor = new Color(fadeOutColor.r,fadeOutColor.g,fadeOutColor.b,fadeOutColor.a + 0.0125f);
            fadeOut.GetComponent<Image>().color = fadeOutColor;
            yield return new WaitForSeconds(0.025f);
            Debug.Log(fadeOutColor.a);
        }
        while(fadeOutColor.a > 0);
    }    
}
