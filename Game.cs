using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using System.Collections.Generic;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe da tela do jogo
    /// </summary>
    public class Game:GameWindow {
        public static Matrix4 ProjectionMatrix;

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
            Matrix4.CreateOrthographicOffCenter(
                (float) States.World.Camera.MinX_ortho, (float) States.World.Camera.MaxX_ortho, 
                (float) States.World.Camera.MinY_ortho, (float) States.World.Camera.MaxY_ortho, 
                0, 100, out ProjectionMatrix);
            GL.MatrixMode (MatrixMode.Projection);
            GL.LoadMatrix (ref ProjectionMatrix);
            Console.WriteLine("Janela redimensionada");
        }
        /// <summary>
        /// Função executada a cada atualização de frame (não o Draw).
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnUpdateFrame (FrameEventArgs e){
            var state = OpenTK.Input.Keyboard.GetState();
            if (state [Key.Escape]) {
                Console.WriteLine("Saindo do jogo...");
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
            //DrawMouseSelection();
            GL.Color3 (Color.Blue);
            foreach(GraphicObject obj in States.World.GraphicObjects) {
                obj.Draw();
            }
            if(States.GraphicObjectCreating != null){
                States.GraphicObjectCreating.Draw();
            }
            DrawSelectedObject();
            GL.PopMatrix ();
            GL.Finish ();
            SwapBuffers ();

        }
        public Game() {
            States.Game = this;
            KeyDown += (object sender, KeyboardKeyEventArgs e) => {
                switch(e.Key){
                    case KeyMapping.ChangePrimitiveType:
                        Events.KeyDownChangePrimitiveType(sender,e);
                        break;
                    case KeyMapping.Rotate:
                        Events.KeyDownRotate(sender,e);
                        break;
                    case KeyMapping.Scale:
                        Events.KeyDownScale(sender,e);
                        break;
                    case KeyMapping.Translate:
                        Events.KeyDownTranslate(sender,e);
                        break;
                    case KeyMapping.DeleteObject:
                        Events.KeyDownDeleteObject(sender,e);
                        break;
                    case KeyMapping.CreateGraphicObject:
                        Events.KeyDownCreateGraphicObject(sender,e);
                        break;
                    case KeyMapping.NextObject:
                        Events.KeyDownNextObject(sender,e);
                        break;
                    case KeyMapping.PreviousObject:
                        Events.KeyDownPreviousObject(sender,e);
                        break;
                    case KeyMapping.Help:
                        Events.KeyDownHelp(sender,e);
                        break;
                    case KeyMapping.Deselect:
                        Events.KeyDownDeselect(sender,e);
                        break;
                    case KeyMapping.ChangeColor:
                        Events.KeyDownChangeColor(sender,e);
                        break;
                    default:
                        Events.KeyDownOther(sender,e);
                        break;
                        
                }
            };
            MouseDown += (object sender, MouseButtonEventArgs e) => {
                States.LastMouseDownPosition = MouseToWorld(e.Position);

                if(e.Button == MouseButton.Middle){
                    if(Events.MouseDownIsMiddle(sender,e)) return;
                }
                if(States.GraphicObjectCreating == null){
                    if(Events.MouseDownIsGraphicObjectCreatingNull(sender,e)) return;
                }
                
                if(States.SelectedGraphicObject != null){
                    if(Events.MouseDownIsSelectedGraphicObjectNotNull(sender,e)) return;
                }
                if(States.GraphicObjectCreating != null){
                    if(Events.MouseDownIsGraphicObjectCreatingNotNull(sender,e)) return;
                }

                States.SelectedPoints = null;
            };
            MouseUp += (object sender, MouseButtonEventArgs e) => {
                States.LastMouseUpPosition = MouseToWorld(e.Position);
                if(e.Button == MouseButton.Middle){
                    if(Events.MouseUpIsMiddle(sender,e)) return;
                }
                States.IsSelecting = false;
                Events.ResetTransforms();
            };
            MouseMove += (object sender, MouseMoveEventArgs e) => {
                States.MousePosition = MouseToWorld(e.Position);
                if(States.IsPamming){
                    if(Events.MouseMoveIsPamming(sender,e)) return;
                } else if(States.IsSelecting){
                    if(Events.MouseMoveIsSelecting(sender,e)) return;
                } else if(States.IsTransforming){
                    if(Events.MouseMoveIsTransforming(sender,e)) return;
                    if(States.IsTranslating){
                        if(Events.MouseMoveIsTranslating(sender,e)) return;
                    } else if(States.IsScaling){
                        if(Events.MouseMoveIsScaling(sender,e)) return;
                    } else if (States.IsRotating){
                        if(Events.MouseMoveIsRotating(sender,e)) return;
                    }
                } else if(States.GraphicObjectCreating != null){
                    if(Events.MouseMoveIsGraphicObjectCreatingNotNull(sender,e)) return;
                }
            };
            MouseWheel += (object sender, MouseWheelEventArgs e) => {
                Size s = States.World.Camera.Size;
                s.Width -= e.Delta*50;
                s.Height -= e.Delta*50;
                if(s.Width < 0)
                    s.Width = 1;
                if(s.Height < 0)
                    s.Height = 1;
                States.World.Camera.Size = s;

                Matrix4.CreateOrthographicOffCenter(
                    (float) States.World.Camera.MinX_ortho, (float) States.World.Camera.MaxX_ortho, 
                    (float) States.World.Camera.MinY_ortho, (float) States.World.Camera.MaxY_ortho, 
                    0, 100, out Game.ProjectionMatrix);
                GL.MatrixMode (MatrixMode.Projection);
                GL.LoadMatrix (ref Game.ProjectionMatrix);
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
        /// <summary>
        /// Desenha as seleções que foram feitas
        /// </summary>
        private void DrawSelections(){
            if(States.SelectedPoints != null && States.SelectedPoints.Length > 0) {

            } else if(States.SelectedGraphicObject != null) {

            }
        }

        /// <summary>
        /// Função que remove o objeto
        /// </summary>
        /// <param name="searching">Objeto a ser removido</param>
        /// <param name="objects">Lista pai do objeto (normalmente é passado a lista do world, e esta vai entrando recursivamente nos filhos).</param>
        public static bool Remove(GraphicObject searching, List<GraphicObject> objects){
            GraphicObject found = null;
            foreach(GraphicObject obj in objects) {
                if(searching == obj) {
                    found = obj;
                    break;
                } else {
                    if(Remove(searching, obj.Sons))
                        return true;
                }
            }
            if(found != null) {
                objects.Remove(found);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Função que desenha a bounding box do objeto selecionado
        /// </summary>
        private void DrawSelectedObject(){
            if(States.SelectedGraphicObject != null) {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(Color.Green);
                GL.LineWidth(1);
                GL.Vertex2(States.SelectedGraphicObject.BBox.MinX, States.SelectedGraphicObject.BBox.MinY);
                GL.Vertex2(States.SelectedGraphicObject.BBox.MinX, States.SelectedGraphicObject.BBox.MaxY);
                GL.Vertex2(States.SelectedGraphicObject.BBox.MaxX, States.SelectedGraphicObject.BBox.MaxY);
                GL.Vertex2(States.SelectedGraphicObject.BBox.MaxX, States.SelectedGraphicObject.BBox.MinY);
                GL.End();
            }
        }
        /// <summary>
        /// Desenha a seleção do mouse
        /// </summary>
        public void DrawMouseSelection(){
            if(States.IsSelecting) {
                GL.LineWidth(1);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(Color.Orange);


                GL.Vertex2(States.LastMouseDownPosition.X, States.LastMouseDownPosition.Y);
                GL.Vertex2(States.LastMouseDownPosition.X, States.MousePosition.Y);
                GL.Vertex2(States.MousePosition.X, States.MousePosition.Y);
                GL.Vertex2(States.MousePosition.X, States.LastMouseDownPosition.Y);

                GL.End();
            }
        }
        /// <summary>
        /// Função que traduz a posição do mouse na janela para as coordenadas do mundo
        /// </summary>
        /// <returns>As coordenadas do mouse</returns>
        /// <param name="mousePosition">Posição do mouse na tela</param>
        public static Point MouseToWorld(Point mousePosition) {
            Size cameraSize = States.World.Camera.Size;
            Size bounds = States.Game.ClientSize;
            Point res = new Point(
                              mousePosition.X * cameraSize.Width / bounds.Width - cameraSize.Width / 2,
                              mousePosition.Y * cameraSize.Height / bounds.Height - cameraSize.Height / 2);
            res.Y = -res.Y;
            res.X += (int)States.World.Camera.Center.X;
            res.Y += (int)States.World.Camera.Center.Y;

            return res;
        }
        /// <summary>
        /// Função que traduz a posição do mouse na janela para as coordenadas do mundo (delta)
        /// </summary>
        /// <returns>As coordenadas do mouse</returns>
        /// <param name="mouseDelta">Delta do mouse</param>
        public static Point MouseToWorldDelta(Point mouseDelta) {
            Size cameraSize = States.World.Camera.Size;
            Size bounds = States.Game.ClientSize;
            Point res = new Point(
                mouseDelta.X * cameraSize.Width / bounds.Width,
                mouseDelta.Y * cameraSize.Height / bounds.Height);
            return res;
        }

    }
}

