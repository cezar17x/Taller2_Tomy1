using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarBoss : MonoBehaviour
{
    public GameObject jefe;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jefe.GetComponent<Jefe>().enabled = true;
        }
    }
}