using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Combinar_Objetos : MonoBehaviour
{
    bool touched1 = false;
    bool touched2 = false;
    bool actObjeto3 = false;

    [Header("Config")]
    public GameObject objeto1; // objetos a agregar
    public GameObject objeto2; // objetos a agregar
    public GameObject objeto3; // resultado (objeto a mostrar y además posición destino)

    public Camera cam;                     // Si queda vacío, usa Camera.main
    public float maxDistance = 200f;       // Alcance del raycast
    public LayerMask selectableLayers = ~0;// Filtra por capas (por defecto, todas)

    [Header("Movimiento hacia objeto3")]
    public float moveDuration = 1.0f;      // duración del movimiento (segundos)
    private float extraWaitSeconds = 0f;   // espera extra antes de activar objeto3 (opcional)

    // variable pública que indica si se activó el resultado


    private void Awake()
    {
        if (cam == null) cam = Camera.main;

        // Asegurar que el objeto3 esté oculto al inicio (si fue asignado)
        if (objeto3 != null)
            objeto3.SetActive(false);
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

        // condición de activación solicitada (sin importar el orden)
        if (touched1 && touched2 && !actObjeto3)
        {
            if (objeto1 != null) objeto1.SetActive(false);
            if (objeto2 != null) objeto2.SetActive(false);
            if (objeto3 != null) objeto3.SetActive(true);
            actObjeto3 = true;
        }
    }

    private void TryHit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, selectableLayers, QueryTriggerInteraction.Collide))
        {
            GameObject hitObj = hit.collider.gameObject;
            Debug.Log($"Tocaste: {hitObj.name}");

            if (!touched1 && objeto1 != null && IsSameOrChild(hitObj, objeto1))
            {
                //touched1 = true;
                // mover lentamente hacia la posición de objeto3 si está asignado
                if (objeto3 != null)
                {
                    StartCoroutine(MoveToTarget(objeto1.transform, objeto3.transform.position, moveDuration));
                    //delay?
                    touched1 = true;
                }
                else
                    Debug.LogWarning("objeto3 no asignado como destino para objeto1.");
                return;
            }

            if (!touched2 && objeto2 != null && IsSameOrChild(hitObj, objeto2))
            {
                //touched2 = true;
                // mover lentamente hacia la posición de objeto3 si está asignado
                if (objeto3 != null)
                {
                    StartCoroutine(MoveToTarget(objeto2.transform, objeto3.transform.position, moveDuration));
        
                    touched2 = true;
                }
                else
                    Debug.LogWarning("objeto3 no asignado como destino para objeto2.");
                return;
            }

            // acá poner lo que va a pasar al tocar otros objetos si lo necesitás
        }
    }

    // Coroutine que mueve suavemente desde la posición actual hasta targetPos en 'duration' segundos
    private IEnumerator MoveToTarget(Transform objTransform, Vector3 targetPos, float duration)
    {
        if (objTransform == null) yield break;
        Vector3 startPos = objTransform.position;
        float elapsed = 0f;

        // Si duration es 0 o negativo, hacer un teleport instantáneo
        if (duration <= 0f)
        {
            objTransform.position = targetPos;
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // suavizado: ease in/out (smoothstep)
            t = t * t * (3f - 2f * t);
            objTransform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        objTransform.position = targetPos;

        // opcional: esperar extra antes de activar el resultado (si querés usar extraWaitSeconds)
        if (extraWaitSeconds > 0f)
            yield return new WaitForSeconds(extraWaitSeconds);
    }

    private bool IsSameOrChild(GameObject hitObj, GameObject reference)
    {
        if (hitObj == reference) return true;
        return hitObj.transform.IsChildOf(reference.transform);
    }

    private bool IsPointerOverUI(bool touch = false)
    {
        if (!EventSystem.current) return false;

        if (!touch) // Mouse
            return EventSystem.current.IsPointerOverGameObject();

        // Touch (pointerId = fingerId del primer toque)
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }
}