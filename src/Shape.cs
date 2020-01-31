using System.Numerics;

namespace Raytracer
{
    public abstract class Shape
    {
        public Vector3 Color;
        public abstract (bool, Vector3, Vector3) Intersect(Ray ray);
    }
}
