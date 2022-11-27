using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Jogador
{
    int vida = 100;
    public string name;
    public float MouseY, MouseX;
    public float speed;
    Camera camera;
    private CharacterController charCont;
    Transform transf;
    float gravidade;
    GameObject ArmaPersonagem;
    Vector2 recuo;

    

    public Jogador(string objectName){
        name = objectName;
    }

    void Start(){
        gravidade = 7.5f;
    }
    
    public void Movimenta(GameObject player, CharacterController controller, float velocidade){
        Cursor.lockState = CursorLockMode.Locked;
        speed = velocidade;     
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = player.transform.forward * horizontalInput + player.transform.right * -verticalInput;


        movementDirection.Normalize();
        controller.Move(movementDirection*speed/*magnitude*/);
        controller.Move(new Vector3(0,0,0));
    }

    public void Cair(CharacterController controller, GameObject player){
        if(controller.isGrounded == false){
            gravidade += 20f*Time.deltaTime; 
            controller.Move(-player.transform.up*gravidade*Time.deltaTime);
        }
        if(controller.isGrounded){
            gravidade = 7.5f;
        }
    }
    
    public void MudarVisibilidadeArma(GameObject Arma){
        Arma.gameObject.SetActive(!Arma.gameObject.activeSelf);
        if(Arma.gameObject.activeSelf == true){
            Arma.GetComponent<Animator>().SetTrigger("Pegou Arma");
            Arma.GetComponent<Animator>().SetBool("Recarga Terminou", true);
            Arma.GetComponent<Animator>().SetFloat("VelocidadeTiro",0.165f/Arma.GetComponent<Arma>().taxaInicial);
        }
    }

    public void MovimentaMouse(GameObject player, Camera playerCam, CharacterController controller, Vector2 recuo){
        camera = playerCam;
        charCont = controller;
        transf = player.transform;
        if(MouseY <= 90f && MouseY >= -90f){
            MouseY += Input.GetAxis("Mouse Y");        
        }
        if(MouseY >= 90f && Mathf.Abs(Input.GetAxis("Mouse Y"))/-Input.GetAxis("Mouse Y") == 1){
            //Debug.Log("Sinal invertido do angulo = " + Input.GetAxis("Mouse Y")/-Input.GetAxis("Mouse Y"));
            MouseY += Input.GetAxis("Mouse Y");
        }
      
        if(MouseY <= -90f && Mathf.Abs(Input.GetAxis("Mouse Y"))/-Input.GetAxis("Mouse Y") == -1){
           // Debug.Log("Sinal invertido do angulo = " + Input.GetAxis("Mouse Y")/-Input.GetAxis("Mouse Y"));
            MouseY += Input.GetAxis("Mouse Y");
        }

        MouseX += Input.GetAxis("Mouse X")*2;
        //Debug.Log("X = "+MouseY+", Y = "+MouseX);
        charCont.transform.eulerAngles = new Vector3(0f ,MouseX + recuo.y,0f);
        camera.transform.eulerAngles = player.transform.eulerAngles + new Vector3(-MouseY + -recuo.x,-90f,0f);
    }

    public int ReceberDano(int dano, Color opacidadeSangue, GameObject Sangue){
        opacidadeSangue.a += 0.2f;
        Sangue.GetComponent<Image>().color = opacidadeSangue;
        opacidadeSangue.a = 0;
        return dano;
        Debug.Log("Dano recebido");
    }

    /*public void Morrer(Animator anim, Canvas GameOver){
        anim.SetTrigger("Morreu");
    }*/
}
