using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private int _player;
    private RectTransform _canvasTransform;
    private RectTransform _crosshairTransform;
    void Start()
    {
        _canvasTransform = transform.parent.GetComponent<RectTransform>();
        _crosshairTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //human crosshair positioning
        if (_player == 0)
            _crosshairTransform.localPosition = new Vector3(-_canvasTransform.rect.width / 4, _crosshairTransform.localPosition.y, _crosshairTransform.localPosition.z);

        //spirit crosshair positioning
        if (_player == 1)
            _crosshairTransform.localPosition = new Vector3(_canvasTransform.rect.width / 4, _crosshairTransform.localPosition.y, _crosshairTransform.localPosition.z);
    }
}
