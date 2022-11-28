using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    [SerializeField]GameObject canvasPortal;
    [SerializeField]Slider[] sliderPortal = new Slider[2];
    [SerializeField]float cargaObjetivoInicial;
    [SerializeField]float cargaObjetivoAtual;
    bool estaColidindo;
    bool estaCarregando;
    bool terminouCarga;

    void Start()
    {
        estaColidindo = false;
        estaCarregando = false;
        terminouCarga = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        sliderPortal[0].maxValue = sliderPortal[1].minValue = cargaObjetivoInicial/2;
        sliderPortal[1].maxValue = cargaObjetivoInicial;
        sliderPortal[0].value = sliderPortal[1].value = cargaObjetivoAtual;
        if(estaColidindo){
            canvasPortal.SetActive(true);
            if(Input.GetKey(KeyCode.Q) && cargaObjetivoAtual <= cargaObjetivoInicial){
                cargaObjetivoAtual += Time.deltaTime;
        
            }
            if(Input.GetKeyUp(KeyCode.Q)){
                estaCarregando = false;
                if(cargaObjetivoAtual >= cargaObjetivoInicial/2 && cargaObjetivoAtual <= cargaObjetivoInicial){
                    cargaObjetivoAtual = cargaObjetivoInicial/2;
                }
                if(cargaObjetivoAtual < cargaObjetivoInicial/2){
                    cargaObjetivoAtual = 0;
                }
            } 
        }
        else{
            canvasPortal.SetActive(false);
        }

        if( cargaObjetivoAtual >= cargaObjetivoInicial){
            terminouCarga = true;
        }
    }

    void OnTriggerStay(Collider colisao){        
        if(colisao.gameObject.tag == "Player"){
            estaColidindo = true;
            if(Input.GetKey(KeyCode.Q) && cargaObjetivoAtual <= cargaObjetivoInicial){
                colisao.gameObject.SendMessage("EstarOcioso", false);
                estaCarregando = true;
        
            }
            if(!estaCarregando){
                colisao.gameObject.SendMessage("EstarOcioso", true);
            }
            if(terminouCarga){
                colisao.gameObject.SendMessage("ComecarFade","Vitória");
            }
        }
    }

    void OnTriggerExit(Collider colisao){
        if(colisao.gameObject.tag == "Player"){
            estaColidindo = false;
        }
    }
}
