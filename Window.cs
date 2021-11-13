using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using OpenTK.Mathematics;
using Vector3 = System.Numerics.Vector3;
using Vector2 = System.Numerics.Vector2;

namespace Lakitu
{
    public class Window : GameWindow
    {
        ImGuiController _controller;
        Mesh mesh = new Mesh();
        Shader shader;

        public Window() : base(GameWindowSettings.Default, new NativeWindowSettings(){ Size = new Vector2i(1600, 900), APIVersion = new Version(4, 5) })
        { }

        protected override void OnLoad()
        {
            base.OnLoad();

            Title = "Lakitu";

            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            shader = new Shader("Shader", File.ReadAllText("Engine/Shader.vs"), File.ReadAllText("Engine/Shader.fs"));
            mesh.Textures.Add(new Texture("Tex", new Bitmap("Gota.png"), false, false));
            mesh.Vertices.Add(new Vertex() {
                Position = new Vector3(0.5f,  0.5f, 0.0f),
                VertexColor = new Vector3(1, 1, 1),
                UV = new Vector2(1, 0)
            });
            mesh.Vertices.Add(new Vertex() {
                Position = new Vector3(0.5f, -0.5f, 0.0f),
                VertexColor = new Vector3(1, 1, 1),
                UV = new Vector2(1, 1)
            });
            mesh.Vertices.Add(new Vertex() {
                Position = new Vector3(-0.5f, -0.5f, 0.0f),
                VertexColor = new Vector3(1, 1, 1),
                UV = new Vector2(0, 1)
            });
            mesh.Vertices.Add(new Vertex() {
                Position = new Vector3(-0.5f,  0.5f, 0.0f),
                VertexColor = new Vector3(1, 1, 1),
                UV = new Vector2(0, 0)
            });
            mesh.Indices.AddRange(new uint[] { 0, 1, 3, 1, 2, 3 });
            mesh.Reload();
        }
        
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            // Update the opengl viewport
            GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);

            // Tell ImGui of the new size
            _controller.WindowResized(ClientSize.X, ClientSize.Y);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _controller.Update(this, (float)e.Time);

            GL.ClearColor(new Color4(0, 32, 48, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            shader.UseShader();
            mesh.Draw(shader);

            ImGui.ShowDemoWindow();
            _controller.Render();

            Util.CheckGLError("End of frame");

            SwapBuffers();

        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            
            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            
            _controller.MouseScroll(e.Offset);
        }
    }
}
