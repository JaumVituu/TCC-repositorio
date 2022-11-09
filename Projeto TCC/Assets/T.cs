using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T : MonoBehaviour
{
    CharacterController cont;
    public float velocidade;
    float inputX, inputZ;
    void Start()
    {
        cont = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        Vector3 movementDirection = transform.forward * inputX + transform.right * inputZ;
        cont.SimpleMove(movementDirection * velocidade);
        if(Input.GetMouseButton(0)){
            cont.transform.Rotate(0,2,0);
        }
    }
}
