using System;
using OpenTK.Input;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável por controlar os eventos do mouse e teclado
    /// </summary>
    public static class Events {
        #region Fields
        /// <summary>
        /// Variação da movimentação do mouse
        /// </summary>
        private static Point delta;
        /// <summary>
        /// A diferença entre o ultima clique e a posicao atual
        /// </summary>
        private static Point4D difference;
        /// <summary>
        /// A distância para detectar que clicou em um vértice
        /// </summary>
        private const double DISTANCIA_VERTICE = 20;
        #endregion
        #region Methods
        /// <summary>
        /// Muda a primitiva do objeto selecionado
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownChangePrimitiveType(object sender, KeyboardKeyEventArgs e){
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
            return false;
        }
        /// <summary>
        /// Rotaciona o objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownRotate(object sender, KeyboardKeyEventArgs e){
            ResetTransforms();
            if(States.SelectedGraphicObject!=null){
                States.LastMouseTransformingPosition = States.MousePosition;
                States.IsTransforming = true;
                States.IsRotating = true;
                Console.WriteLine("Rotacionando objeto...");
            }
            return false;
        }
        /// <summary>
        /// Escala o objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownScale(object sender, KeyboardKeyEventArgs e){
            ResetTransforms();
            if(States.SelectedGraphicObject!=null){
                States.LastMouseTransformingPosition = States.MousePosition;
                States.IsTransforming = true;
                States.IsScaling = true;
                Console.WriteLine("Escalando objeto...");
            }
            return false;
        }
        /// <summary>
        /// Translada o objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownTranslate(object sender, KeyboardKeyEventArgs e){
            ResetTransforms();
            if(States.SelectedGraphicObject!=null){
                States.LastMouseTransformingPosition = States.MousePosition;
                States.IsTransforming = true;
                States.IsTranslating = true;
                Console.WriteLine("Transladando objeto...");
            }
            return false;
        }
        /// <summary>
        /// Deleta o objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownDeleteObject(object sender, KeyboardKeyEventArgs e){
            if(States.SelectedGraphicObject != null){
                if(States.IndiceSelectedVertice != -1) {
                    States.SelectedGraphicObject.Points.RemoveAt(States.IndiceSelectedVertice);
                    States.IndiceSelectedVertice = -1;
                    Console.WriteLine("Ponto removido");
                    States.SelectedGraphicObject.ReadyMatrix();
                    if(States.SelectedGraphicObject.Points.Count < 1) {
                        Game.Remove(States.SelectedGraphicObject, States.World.GraphicObjects);
                        Console.WriteLine(", e o objeto também foi, pois acabaram os pontos.");
                    }
                } else {
                    Game.Remove(States.SelectedGraphicObject, States.World.GraphicObjects);
                    Console.WriteLine("Objeto removido");
                    States.SelectedGraphicObject = null;
                }
            }
            return false;
        }
        /// <summary>
        /// Cria um novo objeto gráfico
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownCreateGraphicObject(object sender, KeyboardKeyEventArgs e){
            if(States.GraphicObjectCreating==null){
                States.GraphicObjectCreating = new GraphicObject();
                Console.WriteLine("Criando objeto...");
            }
            return false;
        }
        /// <summary>
        /// Pega o próximo objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownNextObject(object sender, KeyboardKeyEventArgs e){
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
            return false;
        }
        /// <summary>
        /// Pega o objeto anterior
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownPreviousObject(object sender, KeyboardKeyEventArgs e){
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
            return false;
        }
        /// <summary>
        /// Pega o objeto anterior
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownChangeColor(object sender, KeyboardKeyEventArgs e){
            if(States.SelectedGraphicObject != null) {
                States.SelectedGraphicObject.ChangeColor();
                Console.WriteLine("Mudado a cor do objeto");
            } else {
                Console.WriteLine("Nenhum objeto selecionado para mudar a cor");
            }
            return false;
        }
        /// <summary>
        /// Mostra a ajuda no console
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownHelp(object sender, KeyboardKeyEventArgs e){
            Console.WriteLine("-------------- AJUDA --------------");

            Console.WriteLine(String.Format("Tecla {0}: Esta ajuda", KeyMapping.Help));
            Console.WriteLine(String.Format("Tecla {0}: Incrementa valor", KeyMapping.Increment));
            Console.WriteLine(String.Format("Tecla {0}: Decrementa valor", KeyMapping.Decrement));

            Console.WriteLine(String.Format("Tecla {0}: Cria um objeto gráfico", KeyMapping.CreateGraphicObject));
            Console.WriteLine(String.Format("Tecla {0}: Deleta um objeto gráfico", KeyMapping.DeleteObject));

            Console.WriteLine(String.Format("Tecla {0}: Menos zoom", KeyMapping.ZoomMinus));
            Console.WriteLine(String.Format("Tecla {0}: Mais zoom", KeyMapping.ZoomPlus));
            Console.WriteLine(String.Format("Botão do meio do mouse: Pan"));

            Console.WriteLine(String.Format("Tecla {0}: Deseleciona o objeto", KeyMapping.Deselect));
            Console.WriteLine(String.Format("Tecla {0}: Próximo objeto no mundo", KeyMapping.NextObject));
            Console.WriteLine(String.Format("Tecla {0}: Objeto anterior no mundo", KeyMapping.PreviousObject));

            Console.WriteLine(String.Format("Tecla {0}: Muda a cor", KeyMapping.ChangeColor));
            Console.WriteLine(String.Format("Tecla {0}: Muda a primitiva", KeyMapping.ChangePrimitiveType));
            Console.WriteLine(String.Format("Tecla {0}: Translada", KeyMapping.Translate));
            Console.WriteLine(String.Format("Tecla {0}: Rotaciona", KeyMapping.Rotate));
            Console.WriteLine(String.Format("Tecla {0}: Escala", KeyMapping.Scale));

            Console.WriteLine("-----------------------------------");
            return false;
        }
        /// <summary>
        /// Desseleciona o objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownDeselect(object sender, KeyboardKeyEventArgs e){
            States.SelectedGraphicObject = null;
            return false;
        }
        /// <summary>
        /// Caso pressiou outra tecla
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyboardKeyEventArgs</param>
        public static bool KeyDownOther(object sender, KeyboardKeyEventArgs e){
            Console.WriteLine("Tecla não mapeada pressionada: "+e.Key+", aperte F1 para ver a lista de todas as teclas.");
            return false;
        }

        /// <summary>
        /// Se o botão do mouse que soltou é o do meio
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseUpIsMiddle(object sender, MouseButtonEventArgs e){
            States.IsPamming = false;
            return false;
        }
        /// <summary>
        /// Se o botão do mouse que pressionou é o do meio
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseDownIsMiddle(object sender, MouseButtonEventArgs e){
            States.IsPamming = true;
            return true;
        }
        /// <summary>
        /// Se o botão do mouse foi pressionado e o objeto está sendo criado
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseDownIsGraphicObjectCreatingNotNull(object sender, MouseButtonEventArgs e){
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
                    if(States.SelectedGraphicObject != null)
                        States.SelectedGraphicObject.Sons.Add(tmp);
                    else
                        States.World.GraphicObjects.Add(tmp);
                }
                Console.WriteLine("Encerrou a criação do objeto");
            }
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi pressionado e o objeto não está sendo criado
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseDownIsGraphicObjectCreatingNull(object sender, MouseButtonEventArgs e){
            States.IsSelecting = true;

            VerificaSelecao(States.World.GraphicObjects);



            return false;
        }
        /// <summary>
        /// Faz a verificação se o ponto onde foi clicado está dentro do objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="objects">Lista de objeto a ser iterado (inicialmente deve ser chamado o States.World.GraphicObjects</param>
        private static bool VerificaSelecao(List<GraphicObject> objects){
            if(States.SelectedGraphicObject != null) {
                Point4D mouse = new Point4D(States.LastMouseDownPosition.X, States.LastMouseDownPosition.Y);
                List<Point4D> pontos = States.SelectedGraphicObject.Points;
                for(int i = 0; i < pontos.Count; i++) {
                    if(pontos[i].Distance(mouse) < DISTANCIA_VERTICE) {
                        States.IndiceSelectedVertice = i;
                            return false;
                    }
                }
            }
            States.IndiceSelectedVertice = -1;
            if(States.IsTransforming)
                return false;
            if(States.GraphicObjectCreating != null)
                return false;
            
            States.SelectedGraphicObject = null;
            foreach(GraphicObject obj in objects) {
                int n = 0;
                for(int i = 0; i < obj.Points.Count; i++) {
                    int j = i+1;
                    if(j >= obj.Points.Count)
                        j = 0;
                    
                    double t = (States.LastMouseDownPosition.Y -obj.Points[i].Y) / (obj.Points[j].Y-obj.Points[i].Y);
                    Point4D inter = new Point4D(
                        obj.Points[i].X + (obj.Points[j].X - obj.Points[i].X) * t,
                        obj.Points[i].Y + (obj.Points[j].Y - obj.Points[i].Y) * t
                    );

                    if(Math.Abs(obj.Points[i].Y - obj.Points[j].Y) > 0.1) {
                        if(Math.Abs(inter.X - States.LastMouseDownPosition.X) < 0.1)
                            break;
                        else {
                            if( inter.X > States.LastMouseDownPosition.X && 
                                inter.Y > Math.Min(obj.Points[i].Y, obj.Points[j].Y) &&
                                inter.Y <= Math.Max(obj.Points[i].Y, obj.Points[j].Y)
                            ) {
                                n++;
                            }
                        }
                    } else {
                        if (Math.Abs(States.LastMouseDownPosition.Y - obj.Points[i].Y) < 0.1 && 
                            States.LastMouseDownPosition.X >= Math.Min(obj.Points[i].X, obj.Points[j].X) && 
                            States.LastMouseDownPosition.X <= Math.Max(obj.Points[i].X, obj.Points[j].X)){
                            break;
                        }
                    }
                }

                if(n % 2 != 0) {
                    States.SelectedGraphicObject = obj;
                    break;
                }
                VerificaSelecao(obj.Sons);
            }

            return false;
        }

        /// <summary>
        /// Se o botão do mouse foi pressionado e há um objeto selecionado
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseDownIsSelectedGraphicObjectNotNull(object sender, MouseButtonEventArgs e){
            if(States.IsTransforming) {
                if(e.Button == MouseButton.Left) {
                    States.SelectedGraphicObject.ReadyMatrix();
                    Console.WriteLine("Objeto transformado");
                } else if(e.Button == MouseButton.Right) {
                    States.SelectedGraphicObject.Transform.SetIdentity();
                    Console.WriteLine("Resetou a transformação");
                }
            } else {
                VerificaSelecao(States.World.GraphicObjects);
            }
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi solto e está mexendo a tela
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseUpIsPamming(object sender, MouseButtonEventArgs e){
            States.IsPamming = false;
            return true;
        }
        /// <summary>
        /// Se o botão do mouse foi movido e está mexendo a tela
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseMoveIsPamming(object sender, MouseMoveEventArgs e){
            Point point = Game.MouseToWorldDelta(new Point(e.XDelta, e.YDelta));
            States.World.Camera.Center += new Point4D(-point.X, point.Y);

            Matrix4.CreateOrthographicOffCenter(
                (float) States.World.Camera.MinX_ortho, (float) States.World.Camera.MaxX_ortho, 
                (float) States.World.Camera.MinY_ortho, (float) States.World.Camera.MaxY_ortho, 
                0, 100, out Game.ProjectionMatrix);
            GL.MatrixMode (MatrixMode.Projection);
            GL.LoadMatrix (ref Game.ProjectionMatrix);
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi movido e selecionando
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseMoveIsSelecting(object sender, MouseMoveEventArgs e){
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi movido e está transformando um objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseMoveIsTransforming(object sender, MouseMoveEventArgs e){
            difference = new Point4D(
                States.MousePosition.X - States.LastMouseTransformingPosition.X,
                States.MousePosition.Y - States.LastMouseTransformingPosition.Y
            );
            delta = new Point(
                e.XDelta,
                e.YDelta
            );
            delta = Game.MouseToWorldDelta(delta);
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi movido e está transladando um objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseMoveIsTranslating(object sender, MouseMoveEventArgs e){
            if(States.SelectedGraphicObject != null){
                if(States.IndiceSelectedVertice != -1) {
                    States.SelectedGraphicObject.Points[States.IndiceSelectedVertice].X += delta.X;
                    States.SelectedGraphicObject.Points[States.IndiceSelectedVertice].Y -= delta.Y;
                } else {
                    States.SelectedGraphicObject.Transform.AddTranslation(delta.X, -delta.Y, 0);
                }
            } else if(States.SelectedPoints != null){

            }
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi movido e está escalando um objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseMoveIsScaling(object sender, MouseMoveEventArgs e){
            if(States.SelectedGraphicObject != null){
                States.SelectedGraphicObject.Transform.SetScale(difference.X/100f+1, difference.Y/100f+1, 0,
                    States.SelectedGraphicObject.BBox.Center);
            } else if(States.SelectedPoints != null){

            }
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi movido e está rotacionando um objeto
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseMoveIsRotating(object sender, MouseMoveEventArgs e){
            if(States.SelectedGraphicObject != null){
                States.SelectedGraphicObject.Transform.SetRotationZ(difference.X/100f,
                    States.SelectedGraphicObject.BBox.Center);
            } else if(States.SelectedPoints != null){

            }
            return false;
        }
        /// <summary>
        /// Se o botão do mouse foi movido e há um objeto sendo criado
        /// </summary>
        /// <returns>Retorna true se é necessário interromper para não verificar os próximos eventos.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">MouseButtonEventArgs</param>
        public static bool MouseMoveIsGraphicObjectCreatingNotNull(object sender, MouseMoveEventArgs e){
            if(States.GraphicObjectCreating.Points.Count > 0){
                States.GraphicObjectCreating.Points[States.GraphicObjectCreating.Points.Count-1] = 
                    new Point4D(States.MousePosition.X, States.MousePosition.Y);
            }
            return false;
        }

        /// <summary>
        /// Reseta a transformação
        /// </summary>
        public static void ResetTransforms(){
            States.IsTransforming = false;
            States.IsScaling = false;
            States.IsRotating = false;
            States.IsTranslating = false;
        }

        #endregion
    }
}

