using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering.Universal;

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
    public bool personagemPodeAgir;
    int qtdeRecarga;
    public float taxaInicial;
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
    [SerializeField] GameObject BuracoTiro;
    [SerializeField] float dano;
    [SerializeField] GameObject IndicaTiro;
    [SerializeField] GameObject ParticulasParede;
    public bool estaAndando;
    

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
                        if(Input.GetMouseButton(0) && personagemPodeAgir){
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
        if(municaoAtual == 0 && recarregou){
            StartCoroutine(Recarregar());
        }
        if(municaoAtual <= 0 ){
            municaoAtual = 0;
        }
        if(Input.GetKey(KeyCode.R) && municaoAtual != municaoInicial){
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

        Ray raycastRay = new Ray(new Vector3(x,y, emissorTiro.position.z), ray.direction);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)){
            //DestruirTiro(Tiro);
            if(hit.transform.gameObject.tag == "Player" || hit.transform.gameObject.tag == "Enemy"){
                Collider colisorAtingido = hit.collider;
                TipoDano danoCausado = new TipoDano((int)dano,colisorAtingido);
                hit.transform.parent.gameObject.SendMessage("PerderVida",danoCausado);
            }
            else if(hit.transform.gameObject.tag == "ObjetoTreino"){
                hit.transform.gameObject.SendMessage("PerderVida",dano);
            }

            else if(hit.transform.gameObject.tag == "Alvo"){
                GameObject Indicador = Instantiate(IndicaTiro, hit.point, Quaternion.LookRotation(-hit.normal));
                Destroy(Indicador,15f);

            }
            else{
                GameObject Particulas = Instantiate(ParticulasParede, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(Particulas);
                GameObject Buraco = Instantiate(BuracoTiro, hit.point, Quaternion.LookRotation(-hit.normal));
                Destroy(Buraco,15f);
                Buraco.transform.eulerAngles += new Vector3(0, 0, Random.Range(0,360));
                Buraco.transform.localScale += new Vector3(Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f));
            }
        }

        Tiro.GetComponent<Rigidbody>().velocity = ray.direction * 300;
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

    void DestruirTiro(GameObject objeto){
        Destroy(objeto);
    }

    IEnumerator Recarregar(){
        armaAnim.SetBool("Recarga Terminou", true);
        armaAnim.SetBool("Recarregando",true);
        armaAnim.SetBool("Parou de atirar", true);
        podeAtirar = false;
        recarregou = false;

        yield return new WaitForSeconds(tempoRecarga);
        armaAnim.SetBool("Recarga Terminou",false);

        yield return new WaitForSeconds(0.1f);
        armaAnim.SetBool("Recarga Terminou",true);
        armaAnim.SetBool("Recarregando",false);
        municaoAtual = municaoInicial;
        podeAtirar = true;
        spray = Vector2.zero;
        recarregou = true;
    }
}
