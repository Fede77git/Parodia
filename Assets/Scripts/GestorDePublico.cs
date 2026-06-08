using System.Collections.Generic;
using UnityEngine;

public class GestorDePublico : MonoBehaviour
{
    public GameObject prefabGatito;
    public Transform[] puntosDeSpawn;
    public float radioDeSpawn = 2.5f;
    public float fuerzaSalto = 10f;
    public float fuerzaSaltoChico = 3f;
    public float probabilidadSaltoChico = 0.02f;

    public static int publicoTotalAcumulado = 2;

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
            Vector3 posicionSpawn = Vector3.zero;
            Quaternion rotacionSpawn = Quaternion.identity;
            bool posicionValida = false;
            
            for (int intento = 0; intento < 15; intento++)
            {
                Transform puntoElegido = puntosDeSpawn[Random.Range(0, puntosDeSpawn.Length)];
                float offsetX = Random.Range(-radioDeSpawn, radioDeSpawn);
                
                posicionSpawn = puntoElegido.position + puntoElegido.right * offsetX;
                rotacionSpawn = puntoElegido.rotation;
                
                posicionValida = true;
                foreach (GameObject p in publico)
                {
                    if (p != null && Vector3.Distance(p.transform.position, posicionSpawn) < 1.2f)
                    {
                        posicionValida = false;
                        break;
                    }
                }
                
                if (posicionValida) break;
            }

            if (!posicionValida) continue;

            GameObject nuevoGatito = Instantiate(prefabGatito, posicionSpawn, rotacionSpawn);
            
            Rigidbody rb = nuevoGatito.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }

            Renderer[] renderers = nuevoGatito.GetComponentsInChildren<Renderer>();
            Color colorAleatorio = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            foreach (Renderer r in renderers)
            {
                r.material.color = colorAleatorio;
            }
            
            publico.Add(nuevoGatito);
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
