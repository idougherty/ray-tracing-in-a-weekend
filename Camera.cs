using System;

namespace ray_tracing_in_a_weekend {
    class Camera {
        private Vec3 origin;
        private Vec3 topLeftCorner;
        private Vec3 horizontal, vertical;
        private Vec3 u, v, w;
        private double fov;
        private double aperture;

        public Camera(Vec3 origin, Vec3 lookAt, double aspectRatio, int fov, double aperture) {
            this.fov = fov / 180.0 * Math.PI;
            double viewPortWidth = 2 * Math.Tan(this.fov/2);
            double viewPortHeight = viewPortWidth / aspectRatio;

            this.w = (origin - lookAt).Normalize();
            this.u = (new Vec3(0, 1, 0).Cross(w)).Normalize();
            this.v = w.Cross(u);

            double focusDist = (origin - lookAt).Length();
            this.origin = origin;
            this.horizontal = focusDist * viewPortWidth * u;
            this.vertical = focusDist * viewPortHeight * v;
            this.topLeftCorner = this.origin - this.horizontal/2 + this.vertical/2 - focusDist * w;

            this.aperture = aperture;
        }

        public Ray GetRay(double s, double t) {
            Vec3 rd = this.aperture / 2 * Vec3.RandomInUnitSphere();
            Vec3 offset = u * rd.x + v * rd.y;

            return new Ray(origin + offset, topLeftCorner + s * horizontal - t * vertical - origin - offset);
        }
    }
}