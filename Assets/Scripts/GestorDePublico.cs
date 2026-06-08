using System.Collections.Generic;
using UnityEngine;

public class GestorDePublico : MonoBehaviour
{
    public GameObject prefabPelotita;
    public Transform[] puntosDeSpawn;
    public float radioDeSpawn = 5f;
    public float fuerzaSalto = 10f;
    public float fuerzaSaltoChico = 3f;
    public float probabilidadSaltoChico = 0.02f;

    public static int publicoTotalAcumulado = 0;

    private List<GameObject> publico = new List<GameObject>();

    private void Start()
    {
        if (publicoTotalAcumulado > 0)
        {
            GenerarPublico(publicoTotalAcumulado);
        }
    }

    public void GenerarPublico(int cantidad)
    {
        if (puntosDeSpawn.Length == 0) return;

        Random.State estadoAnterior = Random.state;
        Random.InitState(42);

        for (int i = 0; i < cantidad; i++)
        {
            Transform puntoElegido = puntosDeSpawn[Random.Range(0, puntosDeSpawn.Length)];
            Vector3 posicionSpawn = puntoElegido.position + Random.insideUnitSphere * radioDeSpawn;
            
            GameObject nuevaPelotita = Instantiate(prefabPelotita, posicionSpawn, Quaternion.identity);
            
            Rigidbody rb = nuevaPelotita.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }

            Renderer renderer = nuevaPelotita.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
            
            publico.Add(nuevaPelotita);
        }

        Random.state = estadoAnterior;
    }

    private void FixedUpdate()
    {
        foreach (GameObject pelotita in publico)
        {
            if (pelotita != null)
            {
                Rigidbody rb = pelotita.GetComponent<Rigidbody>();
                if (rb != null && Mathf.Abs(rb.velocity.y) < 0.1f)
                {
                    if (Random.value < probabilidadSaltoChico)
                    {
                        rb.AddForce(Vector3.up * fuerzaSaltoChico, ForceMode.Impulse);
                    }
                }
            }
        }
    }

    public void CelebrarGol()
    {
        foreach (GameObject pelotita in publico)
        {
            if (pelotita != null)
            {
                Rigidbody rb = pelotita.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
                }
            }
        }
    }
}
