using UnityEngine;
using UnityEngine.UI;

namespace UI
{
#if UNITY_EDITOR
  using UnityEditor;

  [CustomEditor(typeof(Touchable))]
  public class TouchableEditor : Editor
  {
    public override void OnInspectorGUI()
    { }
  }
#endif

  public sealed class Touchable
    : Graphic
      , ICanvasRaycastFilter
  {
#if UNITY_EDITOR
    public override void OnRebuildRequested()
    { }
#endif

    public override bool raycastTarget { get { return true; } set { } }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
      vh.Clear();
    }

    bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPoint, UnityEngine.Camera eventCamera)
    {
      Vector2 local;
      return RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);
    }
  }

}