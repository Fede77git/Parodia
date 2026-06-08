using System.Collections.Generic;
using UnityEngine;

public class GestorDePublico : MonoBehaviour
{
    public GameObject prefabPelotita;
    public Transform[] puntosDeSpawn;
    public float radioDeSpawn = 5f;
    public float fuerzaSalto = 10f;

    private List<GameObject> publico = new List<GameObject>();

    public void GenerarPublico(int cantidad)
    {
        if (puntosDeSpawn.Length == 0) return;

        for (int i = 0; i < cantidad; i++)
        {
            Transform puntoElegido = puntosDeSpawn[Random.Range(0, puntosDeSpawn.Length)];
            Vector3 posicionSpawn = puntoElegido.position + Random.insideUnitSphere * radioDeSpawn;
            
            GameObject nuevaPelotita = Instantiate(prefabPelotita, posicionSpawn, Quaternion.identity);
            
            Renderer renderer = nuevaPelotita.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
            
            publico.Add(nuevaPelotita);
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
