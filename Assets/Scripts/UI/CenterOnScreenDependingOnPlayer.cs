using UnityEngine;

public class CenterOnScreenDependingOnPlayer : MonoBehaviour
{
    private enum Player
    {
        Rory,
        Spirit
    }

    [SerializeField] private Player _player;
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
        if (_player == Player.Rory)
            _crosshairTransform.localPosition = new Vector3(-_canvasTransform.rect.width / 4, _crosshairTransform.localPosition.y, _crosshairTransform.localPosition.z);

        //spirit crosshair positioning
        if (_player == Player.Spirit)
            _crosshairTransform.localPosition = new Vector3(_canvasTransform.rect.width / 4, _crosshairTransform.localPosition.y, _crosshairTransform.localPosition.z);
    }
}
