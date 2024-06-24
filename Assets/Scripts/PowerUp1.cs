using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            BasketController basketController = other.gameObject.GetComponent<BasketController>();

            if (basketController != null)
            {
               
                basketController.MoveSpeed += 1f;
                
                gameObject.SetActive(false);
            }
        }
    }
}

