using UnityEngine;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour, IScrollHandler
{
    private Vector3 initialScale;

    [SerializeField]
    private float _zoomSpeed = 0.1f;
    [SerializeField]
    private float _maxZoom = 10f;


    [SerializeField]
    private float _dragSpeed = 10f;
    private bool _isPinching;
    private Vector3 _dragOrigin;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (!_isPinching)
        {
            var delta = Vector3.one * (eventData.scrollDelta.y * _zoomSpeed);
            var desiredScale = transform.localScale + delta;
            desiredScale = ClampDesiredScale(desiredScale);
            transform.localScale = desiredScale;
        }
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(initialScale, desiredScale);
        desiredScale = Vector3.Min(initialScale * _maxZoom, desiredScale);
        return desiredScale;
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount == 0 && _isPinching == true)
        {
            _isPinching = false;
        }
        if (Input.touchCount == 2)
        {
            _isPinching = true;
            Debug.Log("Touching with two fingers");
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            var delta = Vector3.one * (difference * _zoomSpeed);
            var desiredScale = transform.localScale + delta;
            desiredScale = ClampDesiredScale(desiredScale);
            transform.localScale = desiredScale;

        }
      
#endif
    }
    
}
