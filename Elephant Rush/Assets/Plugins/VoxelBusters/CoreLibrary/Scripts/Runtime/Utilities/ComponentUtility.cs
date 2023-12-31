﻿using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class ComponentUtility
    {
        #region Static methods
        
        public static T AddComponentIfNotFound<T>(this GameObject gameObject) where T : Component
        {
            var     component   = gameObject.GetComponent<T>();
            if (null == component)
            {
                component       = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static T GetComponentInPredecessor<T>(this MonoBehaviour monoBehaviour) where T : Component
        {
            var     parentTransform     = monoBehaviour.transform.parent;
            while (parentTransform)
            {
                var     targetComponent = parentTransform.GetComponent<T>();
                if (targetComponent)
                {
                    return targetComponent;
                }
                parentTransform         = parentTransform.parent;
            }

            return null;
        }

        public static T[] GetComponentsInChildren<T>(this Component component, bool includeParent, bool includeInactive) where T : Component
        {
            var     components  = component.GetComponentsInChildren<T>(includeInactive);
            if (includeParent)
            {
                return components;
            }
            else
            {
                var     childComponents = new List<T>();
                foreach (var item in components)
                {
                    if (item.transform != component.transform)
                    {
                        childComponents.Add(item);
                    }
                }

                return childComponents.ToArray();
            }
        }

        #endregion
    }
}