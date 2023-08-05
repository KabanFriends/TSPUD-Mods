using System;
using UnityEngine;
using LibSM64;

public class ExampleCamera : MonoBehaviour
{
    [SerializeField] public GameObject target = null;

    [SerializeField] float radius = 2;
    [SerializeField] float elevation = 1.2f;

    void LateUpdate()
    {
        var m = target.transform.position;
        var n = transform.position;
        m.y = 0;
        n.y = 0;
        n = (n - m).normalized * radius;
        n = Quaternion.AngleAxis( Input.GetAxis("Mouse X"), Vector3.up ) * n;
        n = Quaternion.AngleAxis(Input.GetAxis("Mouse Y"), Vector3.left) * n;
        n += m;
        n.y = target.transform.position.y + elevation;

        transform.position = n;
        transform.LookAt( target.transform.position );
    }
}
