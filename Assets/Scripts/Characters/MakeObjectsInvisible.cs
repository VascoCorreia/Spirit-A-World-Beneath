using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeObjectsInvisible : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private List<Material[]> _materialsInTheWay = new List<Material[]>();
    private List<Material[]> _materialsAlreadyTransparent = new List<Material[]>();

    private void Update()
    {
        _materialsInTheWay = AllObjectsInTheWay(ref _materialsInTheWay);

        MakeObjectsSolid();
        MakeObjectsTransparent();
    }

    private void MakeObjectsTransparent()
    {
        _materialsInTheWay.ForEach((materialArray) =>
        {
            _materialsAlreadyTransparent.Add(materialArray);
            foreach (Material mat in materialArray)
            {
                MaterialExtensions.ToTransparentMode(mat);
            }
        });
    }

    private void MakeObjectsSolid()
    {
        List<Material[]> temp = _materialsAlreadyTransparent.ToList();

        _materialsAlreadyTransparent.ForEach((materialArray) =>
        {
            if (!_materialsInTheWay.Contains(materialArray))
            {
                foreach (Material mat in materialArray)
                {
                    MaterialExtensions.ToOpaqueMode(mat);
                }
                temp.Remove(materialArray);
            }
        });
        _materialsAlreadyTransparent = temp;
    }

    List<Material[]> AllObjectsInTheWay(ref List<Material[]> materials)
    {
        materials.Clear();
        RaycastHit[] hits;
        hits = Physics.RaycastAll(_camera.transform.position, transform.position - _camera.transform.position, Vector3.Distance(transform.position, _camera.transform.position));

        foreach (RaycastHit hit in hits)
        {
            if (!materials.Contains(hit.collider.GetComponent<MeshRenderer>().materials))
            {
                materials.Add(hit.collider.GetComponent<MeshRenderer>().materials);
            }
        }

        return materials;

    }
}
