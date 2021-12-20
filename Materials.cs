namespace ray_tracing_in_a_weekend {
    abstract class Material {
        abstract public bool Scatter(Ray r, HitRecord rec, out Vec3 attenuation, out Ray scattered);
        
        public Vec3 Emitted(double u, double v, Vec3 p) {
            return new Vec3(0, 0, 0);
        }
    }

    class DiffuseLight : Material {
        public Vec3 albedo;

        public DiffuseLight(Vec3 color) {
            this.albedo = color;
        }

        public override bool Scatter(Ray r, HitRecord rec, out Vec3 attenuation, out Ray scattered) {
            scattered = new Ray(rec.p, new Vec3(0, 0, 0));
            attenuation = this.albedo;

            return false;
        }
        
    }

    class Lambertian : Material {
        public Vec3 albedo;

        public Lambertian(Vec3 color) {
            this.albedo = color;
        }

        public override bool Scatter(Ray r, HitRecord rec, out Vec3 attenuation, out Ray scattered) {
            Vec3 scatterDir = rec.normal + Vec3.RandomInHemisphere(rec.normal);

            if(scatterDir.NearZero())
                scatterDir = rec.normal;

            scattered = new Ray(rec.p, scatterDir);
            attenuation = this.albedo;

            return true;
        }
    }

    class Metal : Material {
        public Vec3 albedo;
        public double fuzz;

        public Metal(Vec3 color) {
            this.albedo = color;
            this.fuzz = 0;
        }

        public Metal(Vec3 color, double fuzz) {
            this.albedo = color;
            this.fuzz = fuzz;
        }

        public override bool Scatter(Ray r, HitRecord rec, out Vec3 attenuation, out Ray scattered) {
            Vec3 reflected = Vec3.Reflect(r.dir.Normalize(), rec.normal);
            
            if(fuzz != 0)
                reflected += fuzz * Vec3.RandomInHemisphere(rec.normal);
            
            scattered = new Ray(rec.p, reflected);
            attenuation = this.albedo;

            return true;
        }
    }
}