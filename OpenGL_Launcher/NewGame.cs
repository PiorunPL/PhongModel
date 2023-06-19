using System;
using System.Collections.Generic;
using System.Text;
using MainProject.Controller;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using SkiaSharp;

namespace OpenGL_Launcher
{
    public class NewGame : GameWindow
    {
	    GRGlInterface grgInterface;
        GRContext grContext;
	    SKSurface surface;
	    SKCanvas canvas;
        GRBackendRenderTarget renderTarget;

        Controller Controller;
        InputController InputController;
        // private KeyboardState input = new KeyboardState();
        

        public NewGame(int width, int height, string title) : base(new GameWindowSettings {
            IsMultiThreaded = true,
            RenderFrequency = 100.0
        },
        new NativeWindowSettings {
            Title = title,
            Flags = ContextFlags.ForwardCompatible | ContextFlags.Debug,
            Profile = ContextProfile.Core,
            StartFocused = true,
            WindowBorder = WindowBorder.Fixed,
            Size = new Vector2i(width, height)
        })
        {
            VSync = VSyncMode.Off;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            grgInterface = GRGlInterface.Create();
	        grContext = GRContext.CreateGl(grgInterface);
            renderTarget = new GRBackendRenderTarget(ClientSize.X, ClientSize.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
            surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;
            
            Controller = new Controller(canvas);
            InputController = new InputController(Controller);
            
        }

        protected override void OnUnload()
        {
            surface.Dispose();
            renderTarget.Dispose();
            grContext.Dispose();
            grgInterface.Dispose();
            base.OnUnload();
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            canvas.Clear();
            var keyboardState = KeyboardState.GetSnapshot();
            Controller.CreatePhoto();            
            InputController.HandleKeyboardInput(keyboardState);
            
            canvas.Flush();
            SwapBuffers();
        }
    }
}
