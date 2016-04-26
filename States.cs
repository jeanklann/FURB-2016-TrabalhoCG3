using System;
using System.Drawing;

namespace TrabalhoCG3 {
    public static class States {
        public static bool IsSelecting {get; set;}
        public static GraphicObject SelectedGraphicObject { get; set;}
        public static bool IsSonsSelected { get; set;}
        public static Point4D[] SelectedPoints { get; set;}
        public static Point MousePosition { get; set;}
        public static Point LastMouseUpPosition { get; set;}
        public static Point LastMouseDownPosition { get; set;}

        public static Camera Camera;
    }
}

