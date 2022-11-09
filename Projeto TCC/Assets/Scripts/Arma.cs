using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Arma : MonoBehaviour
{
    //Arma m16 = new Arma("M16", 1.0f, 30);
    bool noChao;
    bool estaEquipada;
    [SerializeField]int municaoAtualInicial;
    [SerializeField]int municaoRestanteInicial;
    public int municaoAtual;
    public int municaoRestante;
    public Transform emissorTiro;
    public GameObject objetoTiro;
    float taxaTiro;
    bool podeAtirar;
    int qtdeRecarga;
    [SerializeField] float taxaInicial;
    [SerializeField] VisualEffect Muzzle;
    [SerializeField] Animator armaAnim;
    [SerializeField] bool armaAutomatica;
    [SerializeField] float tempoRecarga;

    

    void Start()
    {
        noChao = false;
        taxaTiro = 0.2f;
        estaEquipada = true;
        taxaTiro = 0;
        Debug.Log(transform.parent.name);
        armaAnim.SetBool("Parou de atirar", true);
        municaoAtual = municaoAtualInicial;
        municaoRestante = municaoRestanteInicial;
        podeAtirar = true;
    }

    void OnEnable(){
        armaAnim.SetBool("Parou de atirar", true);
    }

    void Update()
    {
        if(!noChao){
        }
        if(estaEquipada){
            if(podeAtirar){
                if(municaoAtual>=1){
                    if(armaAutomatica){
                        if(Input.GetMouseButton(0)){
                            if (taxaTiro <= 0){              
                                if(Atirar()){
                                    armaAnim.SetBool("Parou de atirar", false);
                                    Debug.Log("A");
                                    taxaTiro = taxaInicial;
                                }
                            }
                            else{
                                taxaTiro -= Time.deltaTime;
                            }        
                        }
                        if(Input.GetMouseButtonUp(0)){
                            taxaTiro = 0f;
                            armaAnim.SetBool("Parou de atirar", true);
                        }   
                    }
                    else{
                        if(Input.GetMouseButtonDown(0)){
                            if (taxaTiro <= 0){
                            
                                if(Atirar()){
                                    armaAnim.SetBool("Parou de atirar", false);
                                    Debug.Log("A");
                                    taxaTiro = taxaInicial;
                                    armaAnim.SetBool("Parou de atirar", true);
                                }
                            }
                            else{
                                taxaTiro -= Time.deltaTime;
                            }        
                        }
                    }
                }
                if(municaoAtual == 0){
                    armaAnim.SetBool("Parou de atirar", true);
                }     
            }

        }
    }
    void FixedUpdate(){
        if(municaoRestante > 0 && municaoAtual == 0){
            StartCoroutine(Recarga());
        }
        if(municaoAtual <= 0 ){
            municaoAtual = 0;
        }
    }


    bool Atirar(){
        armaAnim.SetTrigger("Atirando");
        Muzzle.Play();
        GameObject Tiro = Instantiate(objetoTiro, emissorTiro.position, emissorTiro.rotation);
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Camera cam = transform.parent.GetComponent<Camera>();
             
        var ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
        
        Tiro.GetComponent<Rigidbody>().velocity = ray.direction * 150;
        Destroy(Tiro, 0.5f);
        municaoAtual -= 1;

        return true;
    }

    public void OnCollisionEnter(Collision collision){

    }

    IEnumerator Recarga(){
        yield return new WaitForSeconds(tempoRecarga);
        qtdeRecarga = municaoAtualInicial - municaoAtual;
        if(municaoRestante >= municaoAtualInicial){
            municaoAtual = municaoAtualInicial;
            municaoRestante -= qtdeRecarga;
        }
        if(municaoRestante < municaoAtualInicial){
            municaoAtual = municaoRestante;
            municaoRestante -= qtdeRecarga;
        }
        if(municaoRestante - qtdeRecarga <= 0){
            municaoAtual = municaoRestante;
            municaoRestante = 0;
        }
        podeAtirar = true;
        qtdeRecarga = 0;
    }
}
