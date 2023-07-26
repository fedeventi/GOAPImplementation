using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxQuery : MonoBehaviour, IQuery //IA2-P2
{
    public SpatialGrid targetGrid;

    public float width;
    public float heigth;
    

    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    public void Start()
    {
        targetGrid = transform.parent.parent.parent.GetComponent<SpatialGrid>();
    }

    public IEnumerable<IGridEntity> Query()
    {
        var w = width ;
        var h = heigth;
        //var a = high * 0.5f;

       return targetGrid.Query(
                                transform.position + transform.forward + new Vector3(-w, 0, -h),
                                transform.position + transform.forward + new Vector3(w, 0, h),                                
                                x => true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward , new Vector3(width, 1, heigth));
       
    }


}
