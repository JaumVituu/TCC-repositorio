using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador
{
    int vida = 100;
    public string name;
    float MouseY, MouseX;
    public float speed;
    Camera camera;
    private CharacterController charCont;
    Transform transf;
    float gravidade;
    

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
            Debug.Log("Não tá no chão");
            gravidade += 20f*Time.deltaTime; 
            controller.Move(-player.transform.up*gravidade*Time.deltaTime);
        }
        if(controller.isGrounded){
            Debug.Log("Está no chão");
            gravidade = 7.5f;
        }
    }
    
    public void MudarVisibilidadeArma(GameObject Arma){
        Arma.gameObject.SetActive(!Arma.gameObject.activeSelf);
    }

    public void MovimentaMouse(GameObject player, Camera playerCam, CharacterController controller){
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
        charCont.transform.eulerAngles = new Vector3(0f,MouseX,0f);
        camera.transform.eulerAngles = player.transform.eulerAngles + new Vector3(-MouseY,-90f,0f);
    }
}
