using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sutur : MonoBehaviour
{
    public int vida;
    Jogador Personagem = new Jogador("Sutur");
    bool temArma;
    public Transform Arma;
    public Camera cameraJogador;
    private CharacterController charController;
    public GameObject Sistema;
    public float velocidade;
    float MouseY, MouseX;
    bool armaVisivel;
    public Vector2 recuoArma;
    [SerializeField]GameObject sangue;
    [SerializeField]Color corSangue;
    [SerializeField]Animator animatorSutur;
    [SerializeField]Image fadeOut;
    Color fadeOutColor;
    bool telaEscureceu;
    [SerializeField]bool isDebuging;
    public bool podeAgir;
    public Transform attackPoint;
    public GameObject Projetil;
    int totalThrows;
    public float Recargadearremesso;
    [SerializeField]float forca;
    bool ProntoArremessar;

    private void Start()
    {
        ProntoArremessar = true;
        totalThrows = 1000;
        charController = GetComponent<CharacterController>();
        temArma = true;
        armaVisivel = true;
        telaEscureceu = false;
        podeAgir = true;
    }

    public void EstarOcioso(bool variavel){
        podeAgir = variavel;
        Debug.Log("Ok");
    }

    private void Update()
    {
        Arma.gameObject.GetComponent<Arma>().personagemPodeAgir = podeAgir;      
        if(vida <= 0){
            Personagem.Morrer(animatorSutur);
            vida = 0;
            StartCoroutine(MostrarGameOver("GameOver"));
        }
        else{
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
        if(Input.GetKeyDown(KeyCode.E) && ProntoArremessar && totalThrows > 0 && podeAgir && vida > 0)
        {
            Arremessar();
        }
    }

    public void PerderVida(int vidaPerdida){
        vida -= vidaPerdida;
    }

    private void Arremessar()
    {
        ProntoArremessar = false;

        GameObject projectile = Instantiate(Projetil, attackPoint.position, cameraJogador.transform.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cameraJogador.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cameraJogador.transform.position, cameraJogador.transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * forca + transform.up;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), 0/*Recargadearremesso*/);
    }

    private void ResetThrow()
    {
        ProntoArremessar = true;
    }

    void OnTriggerEnter(Collider colisao){
        if(colisao.gameObject.name == "Kick hitbox"){
            vida -= Personagem.ReceberDano(25, corSangue, sangue);
        }
        
    }
    void OnTriggerStay(Collider colisao){
        if(colisao.gameObject.tag == "Fogo"){
            PerderVida(1);
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
