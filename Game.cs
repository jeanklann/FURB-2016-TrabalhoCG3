using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace TrabalhoCG3 {
    public class Game:GameWindow {

        /// <summary>
        /// Função executada no carregamento do OpenGL, é chamado automaticamente pela API do OpenTK.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnLoad (EventArgs e){
            base.OnLoad (e);
            GL.ClearColor (Color.White);
        }
        /// <summary>
        /// Função executada ao redimensionar a tela, é chamada automaticamente pela API do OpenTK.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnResize (EventArgs e){
            GL.Viewport(new Rectangle(ClientRectangle.Left,ClientRectangle.Top,ClientRectangle.Width,ClientRectangle.Height));
            Matrix4 prj;
            Matrix4.CreateOrthographicOffCenter(
                States.Camera.MinX_ortho, States.Camera.MaxX_ortho, 
                States.Camera.MinY_ortho, States.Camera.MaxY_ortho, 
                0, 100, out prj);
            GL.MatrixMode (MatrixMode.Projection);
            GL.LoadMatrix (ref prj);
        }
        /// <summary>
        /// Função executada a cada atualização de frame (não o Draw).
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnUpdateFrame (FrameEventArgs e){
            var state = OpenTK.Input.Keyboard.GetState();
            if (state [Key.Escape]) {
                Environment.Exit (0);
            }


        }
        /// <summary>
        /// Função executada para renderizar cada frame.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnRenderFrame (FrameEventArgs e){
            GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.PushMatrix ();
            DrawSRU ();
            DrawMouseSelection();
            GL.Color3 (Color.Blue);
            GL.PointSize (2);
            GL.Begin (PrimitiveType.Points);
            /////////////////// MAIN DRAW
            GL.End ();
            GL.PopMatrix ();
            GL.Finish ();
            SwapBuffers ();

        }
        public Game() {
            KeyDown += (object sender, KeyboardKeyEventArgs e) => {
                switch(e.Key){
                    case KeyMapping.ChangePrimitiveType:

                        break;
                    case KeyMapping.Rotate:

                        break;
                    case KeyMapping.Scale:

                        break;
                    case KeyMapping.Translate:

                        break;
                    default:
                        Console.WriteLine("Tecla não mapeada pressionada: "+e.Key);
                        break;
                        
                }
            };
            MouseDown += (object sender, MouseButtonEventArgs e) => {
                Console.WriteLine("Mouse button down: " + e.Button + " at: " + e.Position);
                States.IsSelecting = true;
                States.LastMouseDownPosition = MouseToWorld(e.Position);
            };
            MouseUp += (object sender, MouseButtonEventArgs e) => {
                Console.WriteLine("Mouse button up: " + e.Button + " at: " + e.Position);
                States.IsSelecting = false;
                States.LastMouseUpPosition = e.Position;

            };
            MouseMove += (object sender, MouseMoveEventArgs e) => {
                States.MousePosition = MouseToWorld(e.Position);
            };


        }

        /// <summary>
        /// Desenha o SRU
        /// </summary>
        private void DrawSRU()
        {
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex2(-200, 0);
            GL.Vertex2(200, 0);
            GL.Color3(Color.Green);
            GL.Vertex2(0, -200);
            GL.Vertex2(0, 200);
            GL.End();
        }

        public void DrawMouseSelection(){
            if(States.IsSelecting) {
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(Color.Orange);


                GL.Vertex2(States.LastMouseDownPosition.X, -States.LastMouseDownPosition.Y);
                GL.Vertex2(States.LastMouseDownPosition.X, -States.MousePosition.Y);
                GL.Vertex2(States.MousePosition.X, -States.MousePosition.Y);
                GL.Vertex2(States.MousePosition.X, -States.LastMouseDownPosition.Y);

                GL.End();
            }
        }

        public Point MouseToWorld(Point mousePosition) {
            Size cameraSize = States.Camera.Size;
            Size bounds = ClientSize;
            Point res = new Point(
                              mousePosition.X * cameraSize.Width / bounds.Width - cameraSize.Width / 2,
                              mousePosition.Y * cameraSize.Height / bounds.Height - cameraSize.Height / 2);
            return res;
        }

    }
}

