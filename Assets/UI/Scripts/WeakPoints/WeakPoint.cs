using UnityEngine;
using UnityEngine.UI;

public class WeakPoint : MonoBehaviour
{
    [SerializeField] private Image image;

    private RectTransform parentRecTransform;
    private Camera _camera;
    private Collider point;

    [SerializeField] private Vector3 initScale = Vector3.one;
    [SerializeField] private float scaleDownVal = 0.0f;
    private Vector3 scale = new();
    private bool isScaleDown = false;
    private bool isActive = false;

    public void Initialize(RectTransform parentRectTransform, Camera camera, Collider collider)
    {
        parentRecTransform = parentRectTransform;
        _camera = camera;
        point = collider;

        point.enabled = false;

        image.enabled = false;

        scale = initScale;
    }

    private void Update()
    {
        if (!isActive) { return; }

        ScaleDown();

        BillboardByCamera();
    }

    private void ScaleDown()
    {
        if (!isScaleDown) { return; }

        scale *= scaleDownVal;

        if(scale.sqrMagnitude <= Vector3.one.sqrMagnitude)
        {
            scale = Vector3.one;

            isScaleDown = false;
        }

        image.gameObject.transform.localScale = scale;
    }

    private void BillboardByCamera()
    {
        var screenPoint = _camera.WorldToScreenPoint(point.transform.position);

        var cameraDir = _camera.transform.forward;

        var targetDir = point.transform.position - _camera.transform.position;

        var isFront = Vector3.Dot(targetDir, cameraDir) > 0;

        image.enabled = isFront;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRecTransform, screenPoint, null, out Vector2 localPoint);

        image.gameObject.transform.localPosition = localPoint;
    }

    public void OnActive()
    {
        isActive = true;

        image.enabled = true;

        scale = initScale;

        isScaleDown = true;
    }

    public void OnActiveFinished()
    {
        isActive = false;

        image.enabled = false;

        isScaleDown = false;
    }
}
