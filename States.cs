using System;
using System.Drawing;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável por armazenar as variáveis estáticas
    /// </summary>
    public static class States {
        #region Fields
        public static bool IsSelecting {get; set;}
        public static GraphicObject SelectedGraphicObject { get; set;}
        public static bool IsSonsSelected { get; set;}
        public static Point4D[] SelectedPoints { get; set;}
        public static Point MousePosition { get; set;}
        public static Point LastMouseUpPosition { get; set;}
        public static Point LastMouseDownPosition { get; set;}
        public static bool IsTransforming { get; set;}
        public static bool IsScaling { get; set;}
        public static bool IsTranslating { get; set;}
        public static bool IsRotating { get; set;}
        public static bool IsPamming { get; set; }
        public static Point LastMouseTransformingPosition { get; set;}
        public static Game Game {get; set;}
        public static int IndiceSelectedVertice{ get; set;}
        public static World World {get; set;}
        public static GraphicObject GraphicObjectCreating{get; set;}

        #endregion
    }
}

