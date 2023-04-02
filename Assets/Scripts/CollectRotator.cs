using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRotator : MonoBehaviour
{
    [SerializeField] float RotateX = 15;
    [SerializeField] float RotateY = 30;
    [SerializeField] float RotateZ = 45;

    void Update()
    {
        transform.Rotate(new Vector3 (RotateX, RotateY, RotateZ) * Time.deltaTime);
    }
}
