using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragZone : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private ThirdPersonCamera cam;
    [SerializeField] private float sensitivity = 1f;

    private int fingerId = -1;
    private Vector2 lastPos;

    public void OnPointerDown(PointerEventData eventData)
    {
        fingerId = eventData.pointerId;
        lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != fingerId) return;

        Vector2 delta = eventData.position - lastPos;
        lastPos = eventData.position;

        cam.AddRotation(delta * sensitivity);
    }
}