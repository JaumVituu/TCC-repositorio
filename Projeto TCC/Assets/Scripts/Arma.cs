using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Arma : MonoBehaviour
{
    bool noChao;
    bool estaEquipada;
    public int municaoInicial;
    public int municaoAtual;
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
    bool recarregou;
    [SerializeField]float coiceCoeficiente;
    int spray;
    Transform cameraAtribuida;

    

    void Start()
    {
        noChao = false;
        estaEquipada = true;
        taxaTiro = 0;
        armaAnim.SetBool("Parou de atirar", true);
        municaoAtual = municaoInicial;
        podeAtirar = true;
        recarregou = true;

    }

    void OnEnable(){
        armaAnim.SetBool("Parou de atirar", true);
    }

    void Update(){
        cameraAtribuida = transform.parent;
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
                            spray = 0;
                        }   
                    }
                    else{
                        if(Input.GetMouseButtonDown(0)){
                            if (taxaTiro <= 0){
                            
                                if(Atirar()){
                                    armaAnim.SetBool("Parou de atirar", false);
                                    taxaTiro = taxaInicial;
                                    armaAnim.SetBool("Parou de atirar", true);
                                    spray = 0;
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
        if(municaoInicial > 0 && municaoAtual == 0 && recarregou){
            StartCoroutine(Recarregar());
        }
        if(municaoAtual <= 0 ){
            municaoAtual = 0;
        }
        if(Input.GetKey(KeyCode.R)){
            StartCoroutine(Recarregar());
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
        if(spray > 0 && spray < 13){
            cameraAtribuida.eulerAngles += new Vector3(0, spray*coiceCoeficiente, 0);
            ray.direction += new Vector3(0,spray*coiceCoeficiente,0);
        }
        
        Tiro.GetComponent<Rigidbody>().velocity = ray.direction * 150;
        Destroy(Tiro, 2f);
        municaoAtual -= 1;
        spray += 1;

        return true;
    }

    void OnCollisionEnter(Collision collision){

    }

    IEnumerator Recarregar(){
        armaAnim.SetBool("Parou de atirar", true);
        podeAtirar = false;
        recarregou = false;
        yield return new WaitForSeconds(tempoRecarga);
        municaoAtual = municaoInicial;
        recarregou = true;
        podeAtirar = true;
        spray = 0;
    }
}
