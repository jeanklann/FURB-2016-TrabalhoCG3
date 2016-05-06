using System;
using OpenTK.Input;

namespace TrabalhoCG3 {
    /// <summary>
    /// Classe responsável por armazenar todo o mapeamento das teclas
    /// </summary>
    public static class KeyMapping {
        public const Key Help = Key.F1;

        public const Key ChangePrimitiveType = Key.Space;
        public const Key Translate = Key.G;
        public const Key Scale = Key.S;
        public const Key Rotate = Key.R;
        public const Key SelectGraphicObjectWithSons = Key.ControlLeft;
        public const Key Increment = Key.Plus;
        public const Key Decrement = Key.Minus;

        public const Key Deselect = Key.A;
        public const Key NextObject = Key.PageUp;
        public const Key PreviousObject = Key.PageDown;

        public const Key DeleteObject = Key.Delete;
        public const Key CreateGraphicObject = Key.C;

        public const Key ZoomPlus = Key.Plus;
        public const Key ZoomMinus = Key.Minus;

        public const Key ChangeColor = Key.O;



    }
}

