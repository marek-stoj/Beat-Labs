using UnityEngine;

namespace BeatLabs.Utils
{
  public static class ComponentExtensions
  {
    public static T GetComponentSafe<T>(this Component component)
    {
      T childComponent = component.GetComponent<T>();

      if (childComponent == null)
      {
        throw CommonUtils.CreateComponentNullException(typeof(T));
      }

      return childComponent;
    }

    public static T GetComponentInChildrenSafe<T>(this Component component)
    {
      T descendantComponent = component.GetComponentInChildren<T>();

      if (descendantComponent == null)
      {
        throw CommonUtils.CreateComponentNullException(typeof(T));
      }

      return descendantComponent;
    }

    public static T GetComponentInParentSafe<T>(this Component component)
    {
      T componentInParent = component.GetComponentInParent<T>();

      if (componentInParent == null)
      {
        throw CommonUtils.CreateComponentNullException(typeof(T));
      }

      return componentInParent;
    }

    public static T FindComponent<T>(this Component component)
    {
      return component.GetComponent<T>();
    }

    public static T FindComponentInChildren<T>(this Component component)
    {
      return component.GetComponentInChildren<T>();
    }

    public static T FindComponentInParent<T>(this Component component)
    {
      return component.GetComponentInParent<T>();
    }

    public static GameObject FindChildByNameRecursive(this Component component, string name)
    {
      return component.gameObject.FindChildByNameRecursive(name);
    }
  }
}
