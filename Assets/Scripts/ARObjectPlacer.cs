using UnityEngine;
using UnityEngine.EventSystems;

public class ARObjectPlacer : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject arObjectPrefab;
    private GameObject currentObject;

    void Update()
    {
        Debug.Log("Update jalan");

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Klik mouse terdeteksi");

    // sementara kita skip pengecekan UI
    Vector3 mousePos = Input.mousePosition;
    mousePos.z = 2f;
    Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
    Debug.Log("World Pos: " + worldPos);

    if (currentObject == null)
        currentObject = Instantiate(arObjectPrefab, worldPos, Quaternion.identity);
    else
        currentObject.transform.position = worldPos;
            // if (!EventSystem.current.IsPointerOverGameObject())
            // {
            //     Debug.Log("Klik bukan di UI");
            //     Vector3 mousePos = Input.mousePosition;
            //     mousePos.z = 2f;
            //     Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            //     Debug.Log("World Pos: " + worldPos);

            //     if (currentObject == null)
            //         currentObject = Instantiate(arObjectPrefab, worldPos, Quaternion.identity);
            //     else
            //         currentObject.transform.position = worldPos;
            // }
        }
    }
}
