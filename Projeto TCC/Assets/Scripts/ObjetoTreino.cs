using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetoTreino : MonoBehaviour
{
    public int vida;
    [SerializeField][Range(0,100)]float velocidade;
    Rigidbody rb;
    [SerializeField][Range(0,1)]float tamanho;
    int estaIndo;

    void Start()
    {
        vida = 100;
        rb = GetComponent<Rigidbody>();
        estaIndo = 1;    
    }

    void Update()
    {
        Movimentar();
        transform.localScale = new Vector3(tamanho,tamanho,tamanho);
    }

    void Movimentar(){
        rb.velocity = new Vector3(velocidade*estaIndo,0,0);
        if(transform.position.x >= 3){
            estaIndo = -1;
        }
        if(transform.position.x <= -3){
            estaIndo = 1;
        }
    }

    public void PerderVida(int dano){
        int vidaPerdida = dano;
        vida -= vidaPerdida;
        if(vida <= 0){
            StartCoroutine(Morrer());
        }
    }

    IEnumerator Morrer(){
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        vida = 100;
    }
}
