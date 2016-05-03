using System;
using OpenTK.Input;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TrabalhoCG3 {
    public static class Events {
        private static Point delta;
        private static Point4D difference;

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
        public static bool KeyDownDeleteObject(object sender, KeyboardKeyEventArgs e){
            if(States.SelectedGraphicObject != null){
                Game.Remove(States.SelectedGraphicObject, States.World.GraphicObjects);
                Console.WriteLine("Objeto removido");
                States.SelectedGraphicObject = null;
            }
            return false;
        }
        public static bool KeyDownCreateGraphicObject(object sender, KeyboardKeyEventArgs e){
            if(States.GraphicObjectCreating==null){
                States.GraphicObjectCreating = new GraphicObject();
                Console.WriteLine("Criando objeto...");
            }
            return false;
        }
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

            Console.WriteLine(String.Format("Tecla {0}: Próximo objeto no mundo", KeyMapping.NextObject));
            Console.WriteLine(String.Format("Tecla {0}: Objeto anterior no mundo", KeyMapping.PreviousObject));

            Console.WriteLine(String.Format("Tecla {0}: Muda a primitiva", KeyMapping.ChangePrimitiveType));
            Console.WriteLine(String.Format("Tecla {0}: Translada", KeyMapping.Translate));
            Console.WriteLine(String.Format("Tecla {0}: Rotaciona", KeyMapping.Rotate));
            Console.WriteLine(String.Format("Tecla {0}: Escala", KeyMapping.Scale));

            Console.WriteLine("-----------------------------------");
            return false;
        }
        public static bool KeyDownDeselect(object sender, KeyboardKeyEventArgs e){
            States.SelectedGraphicObject = null;
            return false;
        }
        public static bool KeyDownOther(object sender, KeyboardKeyEventArgs e){
            Console.WriteLine("Tecla não mapeada pressionada: "+e.Key+", aperte F1 para ver a lista de todas as teclas.");
            return false;
        }


        public static bool MouseUpIsMiddle(object sender, MouseButtonEventArgs e){
            States.IsPamming = false;
            return false;
        }

        public static bool MouseDownIsMiddle(object sender, MouseButtonEventArgs e){
            States.IsPamming = true;
            return true;
        }
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
                    States.World.GraphicObjects.Add(tmp);
                }
                Console.WriteLine("Encerrou a criação do objeto");
            }
            return false;
        }
        public static bool MouseDownIsGraphicObjectCreatingNull(object sender, MouseButtonEventArgs e){
            States.IsSelecting = true;
            return false;
        }
        public static bool MouseDownIsSelectedGraphicObjectNotNull(object sender, MouseButtonEventArgs e){
            if(e.Button == MouseButton.Left){
                States.SelectedGraphicObject.ReadyMatrix();
                Console.WriteLine("Objeto transformado");
            } else if(e.Button == MouseButton.Right){
                States.SelectedGraphicObject.Transform.SetIdentity();
                Console.WriteLine("Resetou a transformação");
            }
            return false;
        }
        public static bool MouseUpIsPamming(object sender, MouseButtonEventArgs e){
            States.IsPamming = false;
            return true;
        }
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
        public static bool MouseMoveIsSelecting(object sender, MouseMoveEventArgs e){
            return false;
        }

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
        public static bool MouseMoveIsTranslating(object sender, MouseMoveEventArgs e){
            if(States.SelectedGraphicObject != null){
                States.SelectedGraphicObject.Transform.AddTranslation(delta.X, -delta.Y, 0);
            } else if(States.SelectedPoints != null){

            }
            return false;
        }
        public static bool MouseMoveIsScaling(object sender, MouseMoveEventArgs e){
            if(States.SelectedGraphicObject != null){
                States.SelectedGraphicObject.Transform.SetScale(difference.X/100f+1, difference.Y/100f+1, 0);
            } else if(States.SelectedPoints != null){

            }
            return false;
        }
        public static bool MouseMoveIsRotating(object sender, MouseMoveEventArgs e){
            if(States.SelectedGraphicObject != null){
                States.SelectedGraphicObject.Transform.SetRotationZ(difference.X/360f);
            } else if(States.SelectedPoints != null){

            }
            return false;
        }
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
    }
}

