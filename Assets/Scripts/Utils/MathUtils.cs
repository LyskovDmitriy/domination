using System;
using UnityEngine;


namespace Utils
{
    public static class MathUtils
    {
        public static float[] SolveQuadraticEquasion(float a, float b, float c)
        {
            float d = Mathf.Pow(b, 2.0f) - 4 * a * c;

            if (d < 0)
            {
                throw new Exception("Negative discriminant");
            }

            if (Mathf.Approximately(d, 0.0f))
            {
                return new float[]
                {
                    (-b) / (2 * a)
                };
            }

            float sqrtD = Mathf.Sqrt(d);

            return new float[]
            {
                (-b + sqrtD) / (2 * a),
                (-b - sqrtD) / (2 * a)
            };
        }
    }
}
