using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x <= 7.5f){
            transform.localScale += new Vector3(7.5f* Time.deltaTime,0.25f* Time.deltaTime,7.5f* Time.deltaTime);
        }
        Destroy(gameObject,5);
    }
}
