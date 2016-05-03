using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using System.Collections.Generic;

namespace TrabalhoCG3 {
    public class Game:GameWindow {
        public Matrix4 ProjectionMatrix;

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
            DrawMouseSelection();
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
            KeyDown += (object sender, KeyboardKeyEventArgs e) => {
                switch(e.Key){
                    case KeyMapping.ChangePrimitiveType:
                        if(States.SelectedGraphicObject!=null){
                            for (int i = 0; i < GraphicObject.PrimitiveTypes.Length; i++) {
                                if(States.SelectedGraphicObject.PrimitiveType == GraphicObject.PrimitiveTypes[i]){
                                    if(i+1 >= GraphicObject.PrimitiveTypes.Length){
                                        States.SelectedGraphicObject.PrimitiveType = GraphicObject.PrimitiveTypes[0];
                                        Console.WriteLine("Mudou a primitiva");
                                        break;
                                    } else {
                                        States.SelectedGraphicObject.PrimitiveType = GraphicObject.PrimitiveTypes[i+1];
                                        Console.WriteLine("Mudou a primitiva");
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case KeyMapping.Rotate:
                        ResetTransforms();
                        if(States.SelectedGraphicObject!=null){
                            States.LastMouseTransformingPosition = States.MousePosition;
                            States.IsTransforming = true;
                            States.IsRotating = true;
                            Console.WriteLine("Rotacionando objeto...");
                        }
                        break;
                    case KeyMapping.Scale:
                        ResetTransforms();
                        if(States.SelectedGraphicObject!=null){
                            States.LastMouseTransformingPosition = States.MousePosition;
                            States.IsTransforming = true;
                            States.IsScaling = true;
                            Console.WriteLine("Escalando objeto...");
                        }
                        break;
                    case KeyMapping.Translate:
                        ResetTransforms();
                        if(States.SelectedGraphicObject!=null){
                            States.LastMouseTransformingPosition = States.MousePosition;
                            States.IsTransforming = true;
                            States.IsTranslating = true;
                            Console.WriteLine("Transladando objeto...");
                        }

                        break;
                    case KeyMapping.DeleteObject:
                        if(States.SelectedGraphicObject != null){
                            Remove(States.SelectedGraphicObject, States.World.GraphicObjects);
                            Console.WriteLine("Objeto removido");
                            States.SelectedGraphicObject = null;
                        }
                        break;
                    case KeyMapping.CreateGraphicObject:
                        if(States.GraphicObjectCreating==null){
                            States.GraphicObjectCreating = new GraphicObject();
                            Console.WriteLine("Criando objeto...");
                        }
                        break;
                    case KeyMapping.NextObject:
                        if(States.SelectedGraphicObject != null){
                            int i = States.World.GraphicObjects.IndexOf(States.SelectedGraphicObject);
                            if(i+1 >= States.World.GraphicObjects.Count){
                                States.SelectedGraphicObject = States.World.GraphicObjects[0];
                            } else {
                                States.SelectedGraphicObject = States.World.GraphicObjects[i+1];
                            }
                        } else {
                            if(States.World.GraphicObjects.Count>0)
                                States.SelectedGraphicObject = States.World.GraphicObjects[0];
                        }
                        Console.WriteLine("Selecionado próximo objeto no mundo");
                        break;
                    case KeyMapping.PreviousObject:
                        if(States.SelectedGraphicObject != null){
                            int i = States.World.GraphicObjects.IndexOf(States.SelectedGraphicObject);
                            if(i-1 < 0)
                                States.SelectedGraphicObject = States.World.GraphicObjects[States.World.GraphicObjects.Count-1];
                            else
                                States.SelectedGraphicObject = States.World.GraphicObjects[i-1];
                        } else {
                            if(States.World.GraphicObjects.Count>0)
                                States.SelectedGraphicObject = States.World.GraphicObjects[0];
                        }
                        Console.WriteLine("Selecionado objeto anterior no mundo");
                        break;
                    case KeyMapping.Help:
                        Console.WriteLine("-------------- AJUDA --------------");

                        Console.WriteLine(String.Format("Tecla {0}: Esta ajuda", KeyMapping.Help));
                        Console.WriteLine(String.Format("Tecla {0}: Incrementa valor", KeyMapping.Increment));
                        Console.WriteLine(String.Format("Tecla {0}: Decrementa valor", KeyMapping.Decrement));

                        Console.WriteLine(String.Format("Tecla {0}: Cria um objeto gráfico", KeyMapping.CreateGraphicObject));
                        Console.WriteLine(String.Format("Tecla {0}: Deleta um objeto gráfico", KeyMapping.DeleteObject));

                        Console.WriteLine(String.Format("Tecla {0}: Menos zoom", KeyMapping.ZoomMinus));
                        Console.WriteLine(String.Format("Tecla {0}: Mais zoom", KeyMapping.ZoomPlus));
                        Console.WriteLine(String.Format("Botão do meio do mouse: Pan"));

                        Console.WriteLine(String.Format("Tecla {0}: Próximo objeto no mundo", KeyMapping.NextObject));
                        Console.WriteLine(String.Format("Tecla {0}: Objeto anterior no mundo", KeyMapping.PreviousObject));

                        Console.WriteLine(String.Format("Tecla {0}: Muda a primitiva", KeyMapping.ChangePrimitiveType));
                        Console.WriteLine(String.Format("Tecla {0}: Translada", KeyMapping.Translate));
                        Console.WriteLine(String.Format("Tecla {0}: Rotaciona", KeyMapping.Rotate));
                        Console.WriteLine(String.Format("Tecla {0}: Escala", KeyMapping.Scale));

                        Console.WriteLine("-----------------------------------");
                        break;
                    default:
                        Console.WriteLine("Tecla não mapeada pressionada: "+e.Key+", aperte F1 para ver a lista de todas as teclas.");
                        break;
                        
                }
            };
            MouseDown += (object sender, MouseButtonEventArgs e) => {
                States.LastMouseDownPosition = MouseToWorld(e.Position);

                if(e.Button == MouseButton.Middle){
                    States.IsPamming = true;
                    return;
                }
                if(States.GraphicObjectCreating == null)
                    States.IsSelecting = true;
                
                if(States.SelectedGraphicObject != null){
                    if(e.Button == MouseButton.Left){
                        States.SelectedGraphicObject.ReadyMatrix();
                        Console.WriteLine("Objeto transformado");
                    } else if(e.Button == MouseButton.Right){
                        States.SelectedGraphicObject.Transform.SetIdentity();
                        Console.WriteLine("Resetou a transformação");
                    }
                }
                if(States.GraphicObjectCreating != null){
                    if(e.Button == MouseButton.Left){
                        if(States.GraphicObjectCreating.Points.Count < 1){
                            States.GraphicObjectCreating.AddPoint(
                                new Point4D(States.LastMouseDownPosition.X, States.LastMouseDownPosition.Y)
                            );
                        }
                        States.GraphicObjectCreating.Points.RemoveAt(States.GraphicObjectCreating.Points.Count-1);
                        States.GraphicObjectCreating.AddPoint(
                            new Point4D(States.LastMouseDownPosition.X, States.LastMouseDownPosition.Y)
                        );
                        States.GraphicObjectCreating.AddPoint(
                            new Point4D(States.LastMouseDownPosition.X, States.LastMouseDownPosition.Y)
                        );
                        Console.WriteLine("Adicionou ponto ao objeto");
                    } else if(e.Button == MouseButton.Right){
                        if(States.GraphicObjectCreating.Points.Count < 1)
                            States.GraphicObjectCreating = null;
                        else{
                            States.GraphicObjectCreating.Points.RemoveAt(States.GraphicObjectCreating.Points.Count-1);
                            GraphicObject tmp = States.GraphicObjectCreating;
                            States.GraphicObjectCreating = null;
                            States.World.GraphicObjects.Add(tmp);
                        }
                        Console.WriteLine("Encerrou a criação do objeto");
                    }
                }

                States.SelectedPoints = null;
            };
            MouseUp += (object sender, MouseButtonEventArgs e) => {
                States.LastMouseUpPosition = MouseToWorld(e.Position);
                if(e.Button == MouseButton.Middle){
                    States.IsPamming = false;
                    return;
                }
                States.IsSelecting = false;




                ResetTransforms();


            };
            MouseMove += (object sender, MouseMoveEventArgs e) => {
                States.MousePosition = MouseToWorld(e.Position);
                if(States.IsPamming){
                    Point point = MouseToWorldDelta(new Point(e.XDelta, e.YDelta));
                    States.World.Camera.Center += new Point4D(-point.X, point.Y);

                    Matrix4.CreateOrthographicOffCenter(
                        (float) States.World.Camera.MinX_ortho, (float) States.World.Camera.MaxX_ortho, 
                        (float) States.World.Camera.MinY_ortho, (float) States.World.Camera.MaxY_ortho, 
                        0, 100, out ProjectionMatrix);
                    GL.MatrixMode (MatrixMode.Projection);
                    GL.LoadMatrix (ref ProjectionMatrix);
                    
                } else if(States.IsSelecting){

                } else if(States.IsTransforming){
                    Point4D difference = new Point4D(
                        States.MousePosition.X - States.LastMouseTransformingPosition.X,
                        States.MousePosition.Y - States.LastMouseTransformingPosition.Y
                    );
                    Point delta = new Point(
                        e.XDelta,
                        e.YDelta
                    );
                    delta = MouseToWorldDelta(delta);

                    if(States.IsTranslating){
                        if(States.SelectedGraphicObject != null){
                            States.SelectedGraphicObject.Transform.AddTranslation(delta.X, -delta.Y, 0);
                        } else if(States.SelectedPoints != null){

                        }
                    } else if(States.IsScaling){
                        if(States.SelectedGraphicObject != null){
                            States.SelectedGraphicObject.Transform.SetScale(difference.X/100f, -difference.Y/100f, 0);
                        } else if(States.SelectedPoints != null){

                        }
                    } else if (States.IsRotating){
                        if(States.SelectedGraphicObject != null){
                            States.SelectedGraphicObject.Transform.SetRotationZ(difference.X/360f);
                        } else if(States.SelectedPoints != null){

                        }
                    }
                } else if(States.GraphicObjectCreating != null){
                    if(States.GraphicObjectCreating.Points.Count > 0){
                        States.GraphicObjectCreating.Points[States.GraphicObjectCreating.Points.Count-1] = 
                            new Point4D(States.MousePosition.X, States.MousePosition.Y);
                    }
                }
            };



        }

        private void ResetTransforms(){
            States.IsTransforming = false;
            States.IsScaling = false;
            States.IsRotating = false;
            States.IsTranslating = false;
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
        private bool Remove(GraphicObject searching, List<GraphicObject> objects){
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
        public Point MouseToWorld(Point mousePosition) {
            Size cameraSize = States.World.Camera.Size;
            Size bounds = ClientSize;
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
        public Point MouseToWorldDelta(Point mouseDelta) {
            Size cameraSize = States.World.Camera.Size;
            Size bounds = ClientSize;
            Point res = new Point(
                mouseDelta.X * cameraSize.Width / bounds.Width,
                mouseDelta.Y * cameraSize.Height / bounds.Height);
            return res;
        }

    }
}

