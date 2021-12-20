
namespace ray_tracing_in_a_weekend {
    abstract class Hittable {
        public abstract bool Hit(Ray r, double t_min, double t_max, ref HitRecord rec);
    }
}