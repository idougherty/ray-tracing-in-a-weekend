using System;

namespace ray_tracing_in_a_weekend {
    class Sphere : Hittable {
        public Vec3 center;
        public double radius;
        public Material mat;

        public Sphere(Vec3 center, double radius, Material mat) {
            this.center = center;
            this.radius = radius;
            this.mat = mat;
        }

        public override bool Hit(Ray r, double t_min, double t_max, ref HitRecord rec) {
            Vec3 oc = r.origin - this.center;

            double a = r.dir.Dot(r.dir);
            double half_b = oc.Dot(r.dir);
            double c = oc.Dot(oc) - this.radius * this.radius;

            double disc = half_b * half_b - a * c;
            
            if(disc < 0) return false;
            
            double sqrtd = Math.Sqrt(disc);
            double root = (-half_b - sqrtd) / a;

            if(root < t_min || root > t_max) {
                root = (-half_b + sqrtd) / a;

                if(root < t_min || root > t_max)
                    return false;
            }

            rec.t = root;
            rec.p = r.At(root);
            Vec3 outNormal = (rec.p - this.center) / radius;
            rec.SetFaceNormal(r, outNormal);
            rec.mat = this.mat;

            return disc > 0;
        }
    }
}