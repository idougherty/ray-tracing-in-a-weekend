using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace ray_tracing_in_a_weekend
{
    struct RenderParams {
        public Camera cam;
        public Bitmap bmp;
        public List<Hittable> objects;
        public double envBrightness;
        public int samples;
        public int maxDepth;
        public int imageWidth;
        public int imageHeight;

        public RenderParams(Camera cam, Bitmap bmp, List<Hittable> objects, double envBrightness, int samples, int maxDepth, int imageWidth, int imageHeight) {
            this.cam = cam;
            this.bmp = bmp;
            this.objects = objects;
            this.envBrightness = envBrightness;
            this.samples = samples;
            this.maxDepth = maxDepth;
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
        }
    }
    
    class Program
    {

        static void Main(string[] args)
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Console.WriteLine("\nThe required image library is only supported on Windows. :(\n");
                return;
            }

            // Image
            const double aspectRatio = 16.0 / 9;
            const int imageWidth = 900;
            const int imageHeight = (int) (imageWidth / aspectRatio);
            const int samples_per_pixel = 12;
            const int maxDepth = 10;
            const int numThreads = 12;

            // Camera & Environment
            (List<Hittable> objects, Camera cam, double envBrightness) = Environments.Build4Ball(aspectRatio);
            // (List<Hittable> objects, Camera cam, double envBrightness) = Environments.BuildRandom(aspectRatio);
            // (List<Hittable> objects, Camera cam, double envBrightness) = Environments.BuildLights(aspectRatio);

            // Rendering
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine("\nStarting ray tracer!\n");
            Console.WriteLine("Samples per pixel:\t{0}", samples_per_pixel);
            Console.WriteLine("Max bounce depth:\t{0}", maxDepth);
            Console.WriteLine("Image resolution:\t{0} x {1}\n", imageWidth, imageHeight);

            Bitmap bmp = new Bitmap(imageWidth, imageHeight);
            Stack<(int, int)> jobPool = new Stack<(int, int)>();
            Task<List<(int, int, Vec3)>>[] tasks = new Task<List<(int, int, Vec3)>>[numThreads];
            RenderParams parameters = new RenderParams(cam, bmp, objects, envBrightness, samples_per_pixel, maxDepth, imageWidth, imageHeight);

            for(int y = 0; y < imageHeight; y++) {
                for(int x = 0; x < imageWidth; x++) {
                    jobPool.Push((x, y));
                }
            }

            for(int i = 0; i < numThreads; i++) {
                tasks[i] = Task<List<(int, int, Vec3)>>.Factory.StartNew(() => {
                    return ThreadRenderer(parameters, jobPool);
                });
            }

            foreach(Task<List<(int, int, Vec3)>> task in tasks) {
                foreach((int x, int y, Vec3 color) in task.Result) {
                    WriteColor(bmp, x, y, color, samples_per_pixel);
                }
            }

            bmp.Save($"{imageWidth}x{imageHeight}_render.png");
            bmp.Dispose();

            stopwatch.Stop();
            Console.WriteLine("\n\nDone!\nExecution took {0:N2} seconds", (double)stopwatch.ElapsedMilliseconds / 1000);
        }

        static List<(int, int, Vec3)> ThreadRenderer(RenderParams args, Stack<(int, int)> jobs) {
            List<(int, int, Vec3)> solutions = new List<(int, int, Vec3)>();

            while(jobs.Count > 0) {

                (int x, int y) = jobs.Pop();

                int size = (args.imageHeight * args.imageWidth);

                if(x == 0) {
                    double completion = (double)(size - jobs.Count) / size * 100;
                    LogProgress(completion);
                }

                Vec3 color = new Vec3(0, 0, 0);

                for(int s = 0; s < args.samples; s++) {
                    double u = (x + Util.RandomDouble()) / (args.imageWidth - 1);
                    double v = (y + Util.RandomDouble()) / (args.imageHeight - 1);

                    Ray r = args.cam.GetRay(u, v);
                    color += r.Trace(args.objects, args.envBrightness, args.maxDepth);
                }

                solutions.Add((x, y, color));
            }

            return solutions;
        }

        static void WriteColor(Bitmap bmp, int x, int y, Vec3 color, int samples) {
            int r = (int) (Math.Clamp(Math.Sqrt(color.x / samples) * 256, 0, 255));
            int g = (int) (Math.Clamp(Math.Sqrt(color.y / samples) * 256, 0, 255));
            int b = (int) (Math.Clamp(Math.Sqrt(color.z / samples) * 256, 0, 255));

            Color c = Color.FromArgb(255, r, g, b);
            bmp.SetPixel(x, y, c);
        }

        static void LogProgress(double completion) {
            int intComp = (int) completion;
            String loadingBar = String.Concat(Enumerable.Repeat("=", intComp/2)) + ">" + String.Concat(Enumerable.Repeat("-", 50 - intComp/2));
            Console.Write("\rProcessing: {0:N2}%\t[{1}]", completion, loadingBar);
        }
    }
}