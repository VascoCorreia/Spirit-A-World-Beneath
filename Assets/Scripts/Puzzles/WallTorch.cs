using UnityEngine;

public class WallTorch : MonoBehaviour
{
    [SerializeField] public GameObject _Torch;
    [SerializeField] public GameObject _WallTorch;
    [SerializeField] public GameObject _Bridge;
    [SerializeField] public GameObject _Wall1;
    [SerializeField] public GameObject _Wall2;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player Torch")
        {
            _Wall1.SetActive(false);
            _Wall2.SetActive(false);
            _Torch.SetActive(false);
            _WallTorch.SetActive(true);
            _Bridge.SetActive(true);
        }
    }
}
