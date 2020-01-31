using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SDL2;

namespace Raytracer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int width = 640;
            const int height = 480;
            uint[] framebuffer = new uint[height*width];
            
            Matrix4x4 cameraToWorld = new Matrix4x4(
                0.945519f, 0.0f, -0.325569f, 0.0f,
                -0.179534f, 0.834209f, -0.521403f, 0.0f,
                0.271593f, 0.551447f, 0.78876f, 0.0f,
                4.208271f, 8.374532f, 17.932925f, 1.0f);
            const float fov = 51.52f;

            List<Shape> myShapes = new List<Shape>();
            
//            myShapes.Add(new Triangle(
//                new Vector3(-1,-1,-5),
//                new Vector3(1,-1,-5),
//                new Vector3(0,1,-5)));

            for (int i = 0; i < 32; i++)
            {
                Vector3 randPos = new Vector3(
                    (0.5f - (float) Functions.Rand.NextDouble()) * 10,
                    (0.5f - (float) Functions.Rand.NextDouble()) * 10,
                    (0.5f + (float) Functions.Rand.NextDouble()) * 10);
                float randRadius = (0.5f + (float) Functions.Rand.NextDouble() * 0.5f);
                Vector3 randColor = new Vector3(
                    (float) Functions.Rand.NextDouble(),
                    (float) Functions.Rand.NextDouble(),
                    (float) Functions.Rand.NextDouble());
                myShapes.Add(new Sphere(randPos, randRadius, randColor));
            }
            
            Functions.Render(ref framebuffer, width, height, cameraToWorld, fov, ref myShapes);

            SDL.SDL_Init(SDL.SDL_INIT_VIDEO);

            IntPtr window = SDL.SDL_CreateWindow(
                "",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                width,
                height,
                0
            );

            IntPtr renderer = SDL.SDL_CreateRenderer(window, -1, 0);

            IntPtr texture = SDL.SDL_CreateTexture(
                renderer, 
                SDL.SDL_PIXELFORMAT_ARGB8888,
                (int) SDL.SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
                width, 
                height
            );
            
            unsafe
            {
                fixed (uint* pArray = framebuffer)
                {
                    IntPtr pointer = new IntPtr(pArray);
                    SDL.SDL_UpdateTexture(texture, IntPtr.Zero, pointer, width*sizeof(int));
                }
            }
            

            SDL.SDL_Event e;
            bool quit = false;
            while (!quit)
            {
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;

                        case SDL.SDL_EventType.SDL_KEYDOWN:

                            switch (e.key.keysym.sym)
                            {
                                case SDL.SDL_Keycode.SDLK_q:
                                    quit = true;
                                    break;
                            }
                            break;
                    }
                }
                SDL.SDL_RenderClear(renderer);
                SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
                SDL.SDL_RenderPresent(renderer);
                SDL.SDL_Delay(100);
            }
            SDL.SDL_DestroyTexture(texture);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }
    }
}
