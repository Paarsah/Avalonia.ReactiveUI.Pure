using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using OpenTK.Graphics.OpenGL;

namespace Mag3DView.Views.UserControls
{
    public class OpenGLControl : OpenGlControlBase
    {
        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            // Set the background color (black)
            gl.ClearColor(0f, 0f, 0f, 1f);
            gl.Clear((int)ClearBufferMask.ColorBufferBit | (int)ClearBufferMask.DepthBufferBit);

            // Add your OpenGL rendering code here

            // Example: render a simple triangle
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex2(0.0f, 1.0f);
            GL.Color3(0.0f, 1.0f, 0.0f); GL.Vertex2(-1.0f, -1.0f);
            GL.Color3(0.0f, 0.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
            GL.End();
        }
    }
}
