using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cavalo : MonoBehaviour
{
    Vector3 mov;
    [SerializeField]
    private Text energytext;

    [SerializeField]
    private Text energytime;
    private float energytimer;
    private void Start() {
        energytext.enabled = false;
        energytime.enabled = false;
        mov.x = 0f;
        mov.y = 0f;
        mov.z = 0f;
        energytimer = 10;
    }
    private void Update(){
        energytime.text = (energytimer).ToString("0");
        if(Input.GetKey(KeyCode.W)){
            mov.z = 0.03f;
            movimenta(mov);
            mov.z = 0f;
        }

        if(Input.GetKey(KeyCode.S)){
            mov.z = -0.03f;
            movimenta(mov);
            mov.z = 0f;
        }
        if(Input.GetKey(KeyCode.A)){
            mov.x = -0.03f;
            movimenta(mov);
            mov.x = 0f;
        }
        if(Input.GetKey(KeyCode.D)){
            mov.x = 0.03f;
            movimenta(mov);
            mov.x = 0f;
        }
        if(Input.GetKey(KeyCode.Z)){
            energizar();
        }
        else{
            energytime.enabled = false;
            energytimer = 10;
        }
    }

void OnTriggerEnter(Collider colocarenergia){
    if(colocarenergia.gameObject.tag == "energyplace"){
        energytext.enabled = true;
    }
}

    public void movimenta(Vector3 mov){
        transform.Translate(mov);
    }
    public void energizar(){
        energytime.enabled = true;
        energytimer = energytimer - Time.deltaTime;

    }


}
