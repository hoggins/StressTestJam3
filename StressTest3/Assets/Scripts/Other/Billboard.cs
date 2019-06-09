using System;
using Controllers;
using UnityEngine;

namespace Other
{
  public class Billboard : MonoBehaviour
  {
    private void Update()
    {
      var cam = CameraController.Instance.GetComponent<Camera>();
transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            Vector3.up);

    }
  }
}