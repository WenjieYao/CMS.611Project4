using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinichampController : MonoBehaviour
{

    public Camera MiniCamera;
    public GameObject Champion;

    private Transform minimap;
    private Vector3[] corners;

    // Start is called before the first frame update
    void Start()
    {
        minimap = transform.parent;
        corners = new Vector3[4];
        minimap.GetComponent<RectTransform>().GetWorldCorners(corners);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 normPos = MiniCamera.WorldToViewportPoint(Champion.transform.position);

        minimap.GetComponent<RectTransform>().GetWorldCorners(corners);
        Vector3 bl = corners[0];
        Vector3 tr = corners[2];

        Vector3 minichampPos = new Vector3(bl.x + (tr.x - bl.x) * normPos.x, bl.y + (tr.y - bl.y) * normPos.y, 0);

        transform.position = minichampPos;
    }
}
