using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using System;

namespace Mag3DView.Views.UserControls
{
    public class OpenGLControl : OpenGlControlBase
    {
        private int _vertexArray;
        private int _vertexBuffer;
        private int _shaderProgram;

        // Vertex data for a simple triangle
        private readonly float[] _vertices = {
            0.0f,  0.5f, 0.0f,
           -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f
        };

        // Vertex Shader source
        private const string VertexShaderSource = @"
            #version 330 core
            layout(location = 0) in vec3 aPosition;
            void main()
            {
                gl_Position = vec4(aPosition, 1.0);
            }";

        // Fragment Shader source
        private const string FragmentShaderSource = @"
            #version 330 core
            out vec4 FragColor;
            void main()
            {
                FragColor = vec4(1.0, 0.0, 0.0, 1.0); // Red color
            }";

        protected override void OnOpenGlInit(GlInterface gl)
        {
            // Load OpenGL bindings with the provided gl parameter
            GL.LoadBindings(new AvaloniaNativeBindingsContext(gl));

            // Initialize OpenGL resources
            InitOpenGL();
        }

        protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
            // Clear the screen with black color
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Use the shader program
            GL.UseProgram(_shaderProgram);

            // Bind the Vertex Array Object (VAO)
            GL.BindVertexArray(_vertexArray);

            // Draw the triangle
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            // Unbind the VAO
            GL.BindVertexArray(0);
        }

        private void InitOpenGL()
        {
            // Create Vertex Array Object (VAO)
            _vertexArray = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArray);

            // Create Vertex Buffer Object (VBO)
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * _vertices.Length, _vertices, BufferUsageHint.StaticDraw);

            // Compile shaders and create shader program
            _shaderProgram = CreateShaderProgram();

            // Specify the layout of the vertex data
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Unbind VAO and VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        private int CreateShaderProgram()
        {
            // Create and compile the vertex shader
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, VertexShaderSource);
            GL.CompileShader(vertexShader);
            CheckShaderCompileStatus(vertexShader);

            // Create and compile the fragment shader
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, FragmentShaderSource);
            GL.CompileShader(fragmentShader);
            CheckShaderCompileStatus(fragmentShader);

            // Link shaders into a shader program
            int shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);
            GL.LinkProgram(shaderProgram);
            CheckProgramLinkStatus(shaderProgram);

            // Clean up shaders (they are no longer needed once linked into the program)
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return shaderProgram;
        }

        private void CheckShaderCompileStatus(int shader)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error compiling shader: {infoLog}");
            }
        }

        private void CheckProgramLinkStatus(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Error linking shader program: {infoLog}");
            }
        }
    }

    // Native bindings context
    public class AvaloniaNativeBindingsContext : IBindingsContext
    {
        private readonly GlInterface _glInterface;

        public AvaloniaNativeBindingsContext(GlInterface glInterface)
        {
            _glInterface = glInterface;
        }

        public IntPtr GetProcAddress(string procName)
        {
            return _glInterface.GetProcAddress(procName);
        }
    }
}
