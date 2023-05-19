using UnityEngine;

public class WallTorch : MonoBehaviour
{
    [SerializeField] public GameObject _Torch;
    [SerializeField] public GameObject _WallTorch;
    [SerializeField] public GameObject _Bridge;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Torch")
        {
            _Torch.SetActive(false);
            _WallTorch.SetActive(true);
            _Bridge.SetActive(true);
        }
    }
}
