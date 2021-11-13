using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace Lakitu {

    public class Mesh {
        public List<Vertex> Vertices = new List<Vertex>();
        public List<uint> Indices = new List<uint>();
        public List<Texture> Textures = new List<Texture>();
        private Vertex[] LastVertices;
        private uint[] LastIndices;
        private Texture[] LastTextures;
        private int VertexArray;
        private int VertexBuffer;
        private int IndexBuffer;
        private bool Loaded;

        public unsafe void Reload() {

            // Unload if needed.
            if (Loaded) Unload();

            // Generate new buffers.
            VertexArray = GL.GenVertexArray();
            VertexBuffer = GL.GenBuffer();
            IndexBuffer = GL.GenBuffer();
            LastVertices = Vertices.ToArray();
            LastIndices = Indices.ToArray();
            LastTextures = Textures.ToArray();

            // Setup vertex array.
            GL.BindVertexArray(VertexArray);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, LastVertices.Length * sizeof(Vertex), LastVertices, BufferUsageHint.StaticDraw);
            
            // Setup index array.
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, LastIndices.Length * sizeof(uint), LastIndices, BufferUsageHint.StaticDraw);

            // Positions.
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), 0);
            GL.EnableVertexAttribArray(0);

            // Vertex Colors.
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(Vertex), sizeof(Vector3));
            GL.EnableVertexAttribArray(1);

            // UVs.
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), sizeof(Vector3) * 2);
            GL.EnableVertexAttribArray(2);

            // Finished loading.
            GL.BindVertexArray(0);
            Loaded = true;

        }

        public void Draw(Shader s) {
            /*for (int i = 0; i < LastTextures.Length; i++) {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                //s.SetFloat("tex1", i);
                GL.BindTexture(TextureTarget.Texture2D, LastTextures[i].GLTexture);
            }*/
            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, LastTextures[0].GLTexture);
            GL.BindVertexArray(VertexArray);
            GL.DrawElements(PrimitiveType.Triangles, LastIndices.Length, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public void Unload() {

            // Delete buffers.
            GL.DeleteVertexArray(VertexArray);
            GL.DeleteBuffer(VertexBuffer);
            GL.DeleteBuffer(IndexBuffer);

        }

    }

}