using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts
{
	public class PlacementHelperBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject testSphere;

        private List<Vector3> placementPoints;

        public void CreatePlacementPoints()
        {
            Bounds bounds = GetCombinedBounds(transform);
            float minY = bounds.min.y;
            float maxY = bounds.max.y;
            float y = minY + (maxY - minY) * 0.001f;


            var crossSection = GetCrossSectionAtY(y, bounds, transform);
            Debug.Log(transform.gameObject.name + " NumPoints: " + crossSection.Count);
            placementPoints = crossSection;
            foreach (var point in crossSection)
            {

                var tt = Instantiate(testSphere);
                tt.name = transform.gameObject.name + "RaycastHit";
                tt.transform.position = point + transform.position;
            }
        }

        private Bounds GetCombinedBounds(Transform rootTransform)
        {
            var combinedBounds = new Bounds();

            var mrs = rootTransform.GetComponentsInChildren<Renderer>();
            foreach (var mr in mrs)
            {
                combinedBounds.Encapsulate(mr.bounds);
            }

            return combinedBounds;
        }

        private List<Vector3> GetCrossSectionAtY(float yValue, Bounds bounds, Transform templateTransform, bool localValues = true)
        {
            int temporaryLayer = 31;
            Dictionary<Transform, int> originalLayers = new Dictionary<Transform, int>();
            SetLayerRecursively(templateTransform, temporaryLayer, originalLayers);

            List<Vector3> intersectionPoints = GetCrossSectionAtY(yValue, bounds, 1 << temporaryLayer);

            RestoreOriginalLayers(originalLayers);

            if (localValues)
            {
                intersectionPoints = GetLocalValues(intersectionPoints, templateTransform);
            }

            return intersectionPoints;
        }

        private List<Vector3> GetLocalValues(List<Vector3> intersectionPoints, Transform templateTransform)
        {
            var localVectors = new List<Vector3>();
            var center = templateTransform.position;
            foreach (var point in intersectionPoints)
            {

                localVectors.Add(point - center);
            }
            return localVectors;
        }

        private List<Vector3> GetCrossSectionAtY(float yValue, Bounds bounds, int layerMask)
        {
            List<Vector3> intersectionPoints = new List<Vector3>();
            float lengthZ = bounds.size.z;
            for (float x = bounds.min.x; x <= bounds.max.x; x += 0.05f) // Adjust step size as needed
            {
                var origin = new Vector3(x, yValue, bounds.min.z);
                Ray ray = new Ray(origin, Vector3.forward);

                //Debug.DrawRay(origin, Vector3.forward, UnityEngine.Color.red);


                if (Physics.Raycast(ray, out var hit, lengthZ, layerMask))
                {
                    intersectionPoints.Add(hit.point);
                }

                origin = new Vector3(x, yValue, bounds.max.z);
                ray = new Ray(origin, Vector3.back);
                //Debug.DrawRay(origin, Vector3.back, UnityEngine.Color.blue);

                if (Physics.Raycast(ray, out hit, lengthZ, layerMask))
                {
                    intersectionPoints.Add(hit.point);
                }
            }
            float lengthX = bounds.size.z;
            for (float z = bounds.min.z; z < bounds.max.z; z += 0.05f)
            {

                var origin = new Vector3(bounds.min.x, yValue, z);
                var direction = Vector3.right;
                Ray ray = new Ray(origin, direction);
                if (Physics.Raycast(ray, out var hit, lengthX, layerMask))
                {
                    intersectionPoints.Add(hit.point);
                }

                origin = new Vector3(bounds.max.x, yValue, z);
                direction = Vector3.left;
                ray = new Ray(origin, direction);
                if (Physics.Raycast(ray, out hit, lengthX, layerMask))
                {
                    intersectionPoints.Add(hit.point);
                }
            }

            return intersectionPoints;
        }

        private void SetLayerRecursively(Transform transform, int newLayer, Dictionary<Transform, int> originalLayers)
        {
            // Store the original layer and set the new one
            originalLayers[transform] = transform.gameObject.layer;
            transform.gameObject.layer = newLayer;

            // Recursively apply to children
            foreach (Transform child in transform)
            {
                SetLayerRecursively(child, newLayer, originalLayers);
            }
        }

        private void RestoreOriginalLayers(Dictionary<Transform, int> originalLayers)
        {
            foreach (var kvp in originalLayers)
            {
                kvp.Key.gameObject.layer = kvp.Value;
            }
        }

    }
}
