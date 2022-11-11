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
    public Animator armaAnim;
    [SerializeField] bool armaAutomatica;
    [SerializeField] float tempoRecarga;
    bool recarregou;
    [SerializeField]float coiceCoeficiente;
    [SerializeField]int multCoice;
    Vector2 spray;
    Transform cameraAtribuida;
    public Vector2 recuo;
    [SerializeField][Range(0,1)]float atrasoRecuo;
    [SerializeField][Range(0,1)]float recuoAleatorio;
    [SerializeField] Light luzTiro;


    

    void Start()
    {
        armaAnim.SetFloat("VelocidadeTiro",0.165f/taxaInicial);
        noChao = false;
        estaEquipada = true;
        taxaTiro = 0;
        armaAnim.SetBool("Parou de atirar", true);
        municaoAtual = municaoInicial;
        podeAtirar = true;
        recarregou = true;
        coiceCoeficiente *= multCoice;
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
                            spray = Vector2.zero;
                        }   
                    }
                    else{
                        if(Input.GetMouseButtonDown(0)){
                            if (taxaTiro <= 0){
                            
                                if(Atirar()){
                                    armaAnim.SetBool("Parou de atirar", false);
                                    taxaTiro = taxaInicial;
                                    armaAnim.SetBool("Parou de atirar", true);
                                    spray = Vector2.zero;
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
        luzTiro.intensity = Mathf.Lerp(luzTiro.intensity,0,0.75f);
        recuo.x = Mathf.Lerp(recuo.x, 0, atrasoRecuo);
        if(Mathf.Round(recuo.x*5) == 0){
            recuo.x = 0;
        }
        if(municaoInicial > 0 && municaoAtual == 0 && recarregou){
            StartCoroutine(Recarregar());
        }
        if(municaoAtual <= 0 ){
            municaoAtual = 0;
        }
        if(Input.GetKey(KeyCode.R)&&municaoAtual != municaoInicial){
            StartCoroutine(Recarregar());
        }
    }


    bool Atirar(){
        luzTiro.intensity = 6.0f;
        armaAnim.SetTrigger("Atirando");
        Muzzle.Play();
        GameObject Tiro = Instantiate(objetoTiro, emissorTiro.position, emissorTiro.rotation);
        float x = Screen.width / 2;
        float y = Screen.height / 2;

        Camera cam = transform.parent.GetComponent<Camera>();
             
        var ray = cam.ScreenPointToRay(new Vector3(x, y, 0));       
        Tiro.GetComponent<Rigidbody>().velocity = ray.direction * 150;
        Destroy(Tiro, 2f);
        municaoAtual -= 1;
        if(spray.x <= municaoInicial/3 + (int) Random.Range(-2,2)){
            recuo += new Vector2(coiceCoeficiente+Random.Range(recuoAleatorio,-recuoAleatorio),Random.Range(recuoAleatorio,-recuoAleatorio));
            spray.x += 1;
            
        }
        else if(spray.y <= municaoInicial/6 + (int) Random.Range(-2,2)){
            spray.y += 1;
            recuo += new Vector2(recuo.x*atrasoRecuo*5+Random.Range(recuoAleatorio,-recuoAleatorio), coiceCoeficiente/2+Random.Range(recuoAleatorio,-recuoAleatorio));
        }
        else if(spray.y <= 5*municaoInicial/6){
            spray.y += 2;
            recuo += new Vector2(recuo.x*atrasoRecuo*5+Random.Range(recuoAleatorio,-recuoAleatorio), -coiceCoeficiente/2+Random.Range(recuoAleatorio,-recuoAleatorio));
        }
        else{
            spray.y += 1;
            recuo += new Vector2(recuo.x*atrasoRecuo*5+Random.Range(recuoAleatorio,-recuoAleatorio), -coiceCoeficiente/2+Random.Range(recuoAleatorio,-recuoAleatorio));
        }
        return true;
    }

    void OnCollisionEnter(Collision collision){

    }

    IEnumerator Recarregar(){
        armaAnim.SetBool("Recarregando",true);
        armaAnim.SetBool("Parou de atirar", true);
        podeAtirar = false;
        recarregou = false;
        yield return new WaitForSeconds(tempoRecarga);
        armaAnim.SetBool("Recarga Terminou",false);
        yield return new WaitForSeconds(0.1f);
        armaAnim.SetBool("Recarga Terminou",false);
        armaAnim.SetBool("Recarregando",true);
        municaoAtual = municaoInicial;
        podeAtirar = true;
        spray = Vector2.zero;
        recarregou = true;
    }
}
