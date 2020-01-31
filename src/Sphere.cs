using System.Numerics;

namespace Raytracer
{
    public class Sphere : Shape
    {
        public Vector3 Origin;
        public float Radius;

        public Sphere(Vector3 origin, float radius, Vector3 color)
        {
            Origin = origin;
            Radius = radius;
            Color = color;
        }

        public override (bool, Vector3, Vector3) Intersect(Ray ray)
        {
            Vector3 point = Vector3.Zero;
            Vector3 normal = Vector3.Zero;

            float t0;
            float t1;
            
            float a = 1;
            float b = 2 * Vector3.Dot(ray.Direction, ray.Origin - Origin);
            float c = (ray.Origin - Origin).LengthSquared() - Radius * Radius;
            
            (t0, t1) = Functions.QuadraticSolver(a, b, c);

            if (t0 > t1)
            {
                float temp = t0;
                t0 = t1;
                t1 = temp;
            }
            
            if (t0 >= 0 && t1 >= 0)
            {
                point = ray.Origin + ray.Direction * t0;
                normal = Vector3.Normalize(point - Origin);
                return (true, point, normal);
            }
            
            return (false, point, normal);
        }
    }
}
