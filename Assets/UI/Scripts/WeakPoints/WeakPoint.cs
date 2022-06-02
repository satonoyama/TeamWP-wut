using UnityEngine;
using UnityEngine.UI;

public class WeakPoint : MonoBehaviour
{
    [SerializeField] private Image image;

    private RectTransform _parentRecTransform;
    private Camera _camera;
    private Collider point;

    [SerializeField] private Vector3 initScale = new();
    [SerializeField] private float scaleDownVal = 0.0f;
    private Vector3 scale = new();
    private bool isScaleDown = false;

    public void Initialize(RectTransform parentRectTransform, Camera camera, Collider collider)
    {
        _parentRecTransform = parentRectTransform;
        _camera = camera;
        point = collider;

        point.enabled = false;

        image.gameObject.SetActive(true);

        scale = initScale;
    }

    private void Update()
    {
        if (!image.gameObject.activeSelf) { return; }

        ScaleDown();

        BillboardByCamera();
    }

    private void ScaleDown()
    {
        if (!isScaleDown) { return; }

        scale *= scaleDownVal;

        if(scale.sqrMagnitude <= Vector3.one.sqrMagnitude)
        {
            Debug.Log(scale.sqrMagnitude);

            scale = Vector3.one;

            isScaleDown = false;
        }

        image.gameObject.transform.localScale = scale;
    }

    private void BillboardByCamera()
    {
        var screenPoint = _camera.WorldToScreenPoint(point.transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRecTransform, screenPoint, null, out Vector2 localPoint);

        image.gameObject.transform.localPosition = localPoint;
    }

    public void OnActive()
    {
        image.gameObject.SetActive(true);

        scale = initScale;

        isScaleDown = true;
    }

    public void OnActiveFinished()
    {
        image.gameObject.SetActive(false);

        isScaleDown = false;
    }
}
