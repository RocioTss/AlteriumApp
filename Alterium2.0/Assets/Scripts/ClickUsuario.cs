using UnityEngine;
using UnityEngine.EventSystems;



public class ClickUsuario : MonoBehaviour
{
    [Header("Config")]

    public Camera cam;                     // Si queda vacío, usa Camera.main

    public float maxDistance = 200f;       // Alcance del raycast

    public LayerMask selectableLayers = ~0;// Filtra por capas (por defecto, todas)
 
    private void Awake()

    {

        if (cam == null) cam = Camera.main;

    }
 
    void Update()

    {

        // --- Mouse (PC) ---

        if (Input.GetMouseButtonDown(0))

        {

            if (IsPointerOverUI()) return; // Opcional: ignora si está sobre UI

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            TryHit(ray);

        }
 
        // --- Touch (Mobile) ---

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)

        {

            if (IsPointerOverUI(touch: true)) return; // Opcional

            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);

            TryHit(ray);

        }

    }
 
    private void TryHit(Ray ray)

    {

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, selectableLayers, QueryTriggerInteraction.Collide))

        {
                //aca poner lo que va a pasar al tocarlo
            
            // Imprime el nombre del objeto tocado

            Debug.Log($"Tocaste: {hit.collider.gameObject.name}");

            // Si preferís el root del prefab:

            // Debug.Log($"Tocaste: {hit.collider.transform.root.name}");

        }

    }
 
    // Ignora toques sobre UI (útil si tenés canvases interactivos)

    private bool IsPointerOverUI(bool touch = false)

    {

        if (!EventSystem.current) return false;
 
        if (!touch) // Mouse

            return EventSystem.current.IsPointerOverGameObject();
 
        // Touch (pointerId = fingerId del primer toque)

        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

    }
 
}
