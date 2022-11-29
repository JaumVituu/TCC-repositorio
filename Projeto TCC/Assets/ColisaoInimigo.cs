using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisaoInimigo : MonoBehaviour
{
    [SerializeField]GameObject Jotun;
    [SerializeField]float colldown;
    TipoDano dano;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        colldown -= Time.deltaTime;
    }
    
    void OnTriggerStay(Collider colisao){
        if(colisao.gameObject.tag == "Fogo" && colldown <= 0){
            dano.quantidadeDano = 100;
            colldown = 0.2f;
            Jotun.SendMessage("PerderVida",dano);
        }
    }




}
