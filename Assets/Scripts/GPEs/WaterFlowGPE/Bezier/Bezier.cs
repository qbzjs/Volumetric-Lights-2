using UnityEngine;

namespace WaterFlowGPE.Bezier
{
    public static class Bezier 
    {
        /// <summary>
        /// The GetPoint method give the local position of a point on a Bezier curve
        /// </summary>
        /// <param name="p0"> start point of the bezier curve </param>
        /// <param name="p1"> control point of p0 </param>
        /// <param name="p2"> control point of p3 </param>
        /// <param name="p3"> end point of the bezier curve </param>
        /// <param name="t"> the point on the curve to return the position, between 0 and 1 </param>
        /// <returns> the local position of t on the curve </returns>
        public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                Mathf.Pow(oneMinusT,3) * p0 +
                3f * Mathf.Pow(oneMinusT,2) * t * p1 +
                3f * oneMinusT * Mathf.Pow(t,2) * p2 +
                Mathf.Pow(t,3) * p3;
        }
	
        public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }
    }
}