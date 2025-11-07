using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject[] objeto; //objetos a agregar
 
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the portal!");
            // Add portal functionality here

            for (int i = 0; i < objeto.Length; i++)
            {
                objeto[i].SetActive(true); // Activar los objetos al entrar en el portal
            }
            
            gameObject.SetActive(false);

        }
    }    

}
