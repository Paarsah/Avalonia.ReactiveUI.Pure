using Avalonia.Controls;
using Avalonia.OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Avalonia.ReactiveUI.Pure.Views.UserControls
{
    public partial class ChartControl : UserControl
    {
        public ChartControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            GLControl.MakeCurrent();
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f); // Set a clear color
            GL.Clear(ClearBufferMask.ColorBufferBit); // Clear the screen
            GLControl.SwapBuffers();
        }
    }
}
