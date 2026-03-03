using UnityEngine;
using UnityEngine.EventSystems;

public class RaycstTest : MonoBehaviour
{
    void Update()
    {
        if (EventSystem.current == null)
            return;

        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        if (results.Count > 0)
        {
            Debug.Log("Raycast UI -> " + results[0].gameObject.name);
        }
        else
        {
            Debug.Log("Raycast UI -> NADA");
        }
    }
}