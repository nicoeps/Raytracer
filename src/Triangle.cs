using System;
using  System.Numerics;

namespace Raytracer
{
    public class Triangle : Shape
    {
        public Vector3 V0, V1, V2;

        public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
        }

        public uint Vec2Col(Vector3 col)
        {
            byte r = (byte) (col.X * 255);
            byte g = (byte) (col.Y * 255);
            byte b = (byte) (col.Z * 255);
            return (uint) (255 << 24 | r << 16 | g << 8 | b);
        }
        
        public override (bool, Vector3, Vector3) Intersect(Ray ray)
        {
            Vector3[] cols = new[]
            {
                new Vector3(0.6f, 0.4f, 0.1f),
                new Vector3(0.1f, 0.5f, 0.3f),
                new Vector3(0.1f, 0.3f, 0.7f)
            };
            
            Vector3 normal = Vector3.Cross(V1-V0, V2-V0);
            float denom = Vector3.Dot(normal, normal);

            float normalDotRay = Vector3.Dot(normal, ray.Direction);
            if (Math.Abs(normalDotRay) < 1e-8)
            {
                return (false, Vector3.Zero, Vector3.Zero);
            }

            float d = Vector3.Dot(normal, V0);

            float t = (Vector3.Dot(normal, ray.Origin) + d) / normalDotRay;

            if (t < 0)
            {
                return (false, Vector3.Zero, Vector3.Zero);
            }

            Vector3 P = ray.Origin + t * ray.Direction;
            Vector3 C;

            Vector3 edgeg0 = V1 - V0;
            Vector3 vp0 = P - V0;
            C = Vector3.Cross(edgeg0, vp0);
            if (Vector3.Dot(normal, C) < 0)
            {
                return (false, Vector3.Zero, Vector3.Zero);
            }

            Vector3 edge1 = V2 - V1;
            Vector3 vp1 = P - V1;

            C = Vector3.Cross(edge1, vp1);
            float u = Vector3.Dot(normal, C);
            if (u < 0)
            {
                return (false, Vector3.Zero, Vector3.Zero);   
            }

            Vector3 edge2 = V0 - V2;
            Vector3 vp2 = P - V2;
            C = Vector3.Cross(edge2, vp2);
            float v = Vector3.Dot(normal, C);
            if (v < 0)
            {
                return (false, Vector3.Zero, Vector3.Zero);
            }

            u /= denom;
            v /= denom;
            
//            return (true, Vector3.Zero, u * cols[0] + v * cols[1] + (1 - u -v) * cols[2]);
            return (true, Vector3.Zero, new Vector3(u, v, (1 - u - v)));
        }
    }
}
