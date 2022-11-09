using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sutur : MonoBehaviour
{
    public Transform cam;
    public Transform attackPoint;
    public GameObject Projetil;
    int totalThrows;
    public float Recargadearremesso;
    float forca;
    bool ProntoArremessar;

    private void Start()
    {
        ProntoArremessar = true;
        totalThrows = 1000;
        forca = 50;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && ProntoArremessar && totalThrows > 0)
        {
            Arremessar();
        }
    }

    private void Arremessar()
    {
        ProntoArremessar = false;

        GameObject projectile = Instantiate(Projetil, attackPoint.position, cam.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        Vector3 forceToAdd = forceDirection * forca + transform.up;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        Invoke(nameof(ResetThrow), Recargadearremesso);
    }

    private void ResetThrow()
    {
        ProntoArremessar = true;
    }
}
