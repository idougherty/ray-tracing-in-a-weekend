# C# Software Ray Tracer
A ray tracing project written in C# made by following the [_Ray Tracing in One Weekend_](https://raytracing.github.io/books/RayTracingInOneWeekend.html) book series.

## Features
* Different materials: (Matte, Metallic, & Diffuse Light)
* Multithreaded rendering
* Depth of field effects
* PNG output

## Sample Renders
![A render of 4 colored metallic balls with some roughness.](1280x720_render_4ball_scene.png)

![A render of a dark scene with one diffuse light illuminating 3 matte spheres .](1280x720_render_light_scene.png)

![A render of one large mirrored sphere with many small balls scattered around it.](1280x720_render_rand_scene.png)

## Running
> Currently only supported on Windows!
1. Create an environment in `Environments.cs`
2. Set rendering parameters in `Program.cs`
    - For renders using diffuse lights, get the best results using a high 'samples per pixel' setting.
3. Run from the console with `dotnet run`, your output will look like `[WIDTH]x[HEIGHT]\_render.png`
