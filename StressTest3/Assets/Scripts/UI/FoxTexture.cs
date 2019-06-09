using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class FoxTexture : MonoBehaviour
  {
    public Camera Camera;
    public RenderTexture Texture;
    public RawImage Image;

    private void Awake()
    {
      Texture = new RenderTexture(512,512, 1);
      Camera.targetTexture = Texture;
      Image.texture = Texture;
    }
  }
}