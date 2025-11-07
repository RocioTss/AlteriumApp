using UnityEngine;
 
public class Animación : MonoBehaviour

{
    [Header("Movimiento (flotación)")]

    public float floatAmplitude = 0.5f;  // Distancia máxima que sube/baja

    public float floatSpeed = 2f;        // Velocidad de subida y bajada
 
    [Header("Rotación")]

    public float rotationSpeed = 50f;    // Grados por segundo alrededor del eje X
 
    private Vector3 startPos;
 
    void Start()

    {

        startPos = transform.position;

    }
 
    void Update()

    {

        // Movimiento tipo flotación sobre eje Z

        float newZ = startPos.z + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        transform.position = new Vector3(startPos.x, startPos.y, newZ);
 
        // Rotación hacia su derecha (eje X local)

        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);

    }
 
    

}

 