using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeObjectsInvisible : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private List<Material[]> _materialsInTheWay = new List<Material[]>();
    private List<Material[]> _materialsAlreadyTransparent = new List<Material[]>();
    private LayerMask _combinedLayerMask;
    private void Start()
    {
        CreateLayerMask();
    }

    private void Update()
    {
        _materialsInTheWay = AllObjectsInTheWay(ref _materialsInTheWay);

        MakeObjectsSolid();
        MakeObjectsTransparent();
    }

    //Creates a LayerMask with only the layers that we want to make transparent
    private void CreateLayerMask()
    {
        int _roryLayerMask, _spiritLayerMask, _dynamicPossessablesLayerMask, spiritBarrierLayerMask, humanBarrierLayerMask, levelLayout;

        _roryLayerMask = LayerMask.GetMask("Rory");
        _spiritLayerMask = LayerMask.GetMask("Spirit");
        _dynamicPossessablesLayerMask = LayerMask.GetMask("PossessableDynamic");
        humanBarrierLayerMask = LayerMask.GetMask("RoryBarrier");
        spiritBarrierLayerMask = LayerMask.GetMask("SpiritBarrier");
        levelLayout = LayerMask.GetMask("Default");

        _combinedLayerMask = ~(_roryLayerMask | _spiritLayerMask | _dynamicPossessablesLayerMask | humanBarrierLayerMask | spiritBarrierLayerMask | levelLayout);
    }

    //Puts all materials of an object that is between the character and the camera and belongs to the correct layermask in a List
    //We clear it in the beggining since this runs every frame
    List<Material[]> AllObjectsInTheWay(ref List<Material[]> materials)
    {
        materials.Clear();
        RaycastHit[] hits;
        hits = Physics.RaycastAll(_camera.transform.position, transform.position - _camera.transform.position, Vector3.Distance(transform.position, _camera.transform.position), _combinedLayerMask);

        foreach (RaycastHit hit in hits)
        {
            if(hit.collider.GetComponent<MeshRenderer>() == null && hit.collider.GetComponentsInChildren<MeshRenderer>().Length == 0)
            {
                continue;
            }

            else
            {
                MeshRenderer[] MeshRenderersInChildren = hit.collider.GetComponentsInChildren<MeshRenderer>();

                foreach(MeshRenderer meshRenderer in MeshRenderersInChildren)
                {
                    if (!materials.Contains(meshRenderer.materials))
                    {
                        materials.Add(meshRenderer.materials);
                    }
                }

                if (hit.collider.GetComponent<MeshRenderer>() != null && !materials.Contains(hit.collider.GetComponent<MeshRenderer>().materials))
                {
                    materials.Add(hit.collider.GetComponent<MeshRenderer>().materials);
                }
            }
        }

        return materials;

    }

    //Iterates through all the materials of an object that is between the camera and player and changes its materials to transparent
    //We add to a new list since we need to know which materials are currently invisible
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

    //Iterates through all the materials that are currently invisible, if the object is not in the way anymore we change its materials to their regular mode
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
}
