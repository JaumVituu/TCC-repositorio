using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : MonoBehaviour
{
    public Transform FireArea;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision explosao){
        ContactPoint contact = explosao.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        if(explosao.gameObject.tag == "Ground"){
            Instantiate(FireArea, position, rotation);
            Destroy(gameObject);
        }
    }
}
