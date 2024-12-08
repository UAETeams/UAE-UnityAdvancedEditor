using UnityEngine;
using RTG;
using System.Collections.Generic;
using RuntimeInspectorNamespace;
using System;

namespace RTG
{
    public class HierarchyTransforms : MonoBehaviour
    {
        public RuntimeInspector Inspector;
        public GameObject targetObject;
        ObjectTransformGizmo objectTransformGizmo = new ObjectTransformGizmo();

        public void CreateMoveGizmo()
        {
            if(targetObject != null)
            {
                RTGizmosEngine.Get.RemoveGizmo(objectTransformGizmo.Gizmo);

                objectTransformGizmo = RTGizmosEngine.Get.CreateObjectMoveGizmo();
                objectTransformGizmo.SetTargetObject(targetObject);


                MoveGizmo moveGizmo = objectTransformGizmo.Gizmo.MoveGizmo;
                moveGizmo.SetVertexSnapTargetObjects(new List<GameObject> { targetObject });
            }
        }
        public void CreateRotateGizmo()
        {
            if(targetObject != null)
            {
                RTGizmosEngine.Get.RemoveGizmo(objectTransformGizmo.Gizmo);

                objectTransformGizmo = RTGizmosEngine.Get.CreateObjectRotationGizmo();
                objectTransformGizmo.SetTargetObject(targetObject);
            }
        }
        public void CreateReSizeGizmo()
        {
            if(targetObject != null)
            {
                RTGizmosEngine.Get.RemoveGizmo(objectTransformGizmo.Gizmo);

                objectTransformGizmo = RTGizmosEngine.Get.CreateObjectScaleGizmo();
                objectTransformGizmo.SetTargetObject(targetObject);
            }
        }
        public void RemoveGizmos()
        {
            RTGizmosEngine.Get.RemoveGizmo(objectTransformGizmo.Gizmo);

            targetObject = null;

            Inspect();
        }
        public void Inspect()
        {
            Inspector.ComponentFilter = (GameObject gameObject, List<Component> components) =>
            {
                components.Remove(gameObject.GetComponent<SceneExpand>());
            };
            Inspector.Inspect(targetObject);
        }
    }
}
