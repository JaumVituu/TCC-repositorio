using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
    //Arma m16 = new Arma("M16", 1.0f, 30);
    bool noChao;
    bool estaEquipada;
    float taxaTiro;
    public Transform emissorTiro;
    public GameObject objetoTiro;
    float recargaTiro;
    [SerializeField] float recargaInicial;
    

    void Start()
    {
        noChao = false;
        taxaTiro = 0.2f;
        estaEquipada = true;
        recargaTiro = 0;
        Debug.Log(transform.parent.name);
    }

    void Update()
    {
        if(!noChao){
        }
        if(estaEquipada){
            if(Input.GetMouseButton(0)){
                if (recargaTiro <= 0){
                    if(Atirar()){
                        Debug.Log("A");
                        recargaTiro = recargaInicial;
                    }
                }
                else{
                    recargaTiro -= Time.deltaTime;
                }        
            }
            if(Input.GetMouseButtonUp(0)){
                recargaTiro = 0f;
            }     
        }
    }

    bool Atirar(){
        GameObject Tiro = Instantiate(objetoTiro, emissorTiro.position, emissorTiro.rotation);
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Camera cam = transform.parent.GetComponent<Camera>();
             
        var ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
        
        Tiro.GetComponent<Rigidbody>().velocity = ray.direction * 150;
        Destroy(Tiro, 0.5f);
        return true;
    }

    public void OnCollisionEnter(Collision collision){

    }
}
