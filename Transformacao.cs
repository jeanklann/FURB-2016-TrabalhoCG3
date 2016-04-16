using System;

namespace TrabalhoCG3 {
    public class Transformacao {
        public const double DEG_TO_RAD = 0.017453292519943295769236907684886;
        public Transformacao() {
            
        }
        private double[] matriz =
        {    1, 0, 0, 0 ,
             0, 1, 0, 0 ,
             0, 0, 1, 0 ,
             0, 0, 0, 1     };

        public double[] Matriz {
            set {
                for(int i = 0; i < matriz.Length; i++) {
                    matriz[i] = value[i];
                } }
            get { return matriz; }
        }
            
        public void setMatriz(double valor, int indice){
            matriz[indice] = valor;
        }

        public void AtribuirIdentidade() {
            for (int i=0; i<16; ++i) {
                matriz[i] = 0.0;
            }
            matriz[0] = matriz[5] = matriz[10] = matriz[15] = 1.0;
        }

        public void AtribuirTranslacao(double tx, double ty, double tz)
        {
            AtribuirIdentidade();
            matriz[12] = tx;
            matriz[13] = ty;
            matriz[14] = tz;
        }

        public void AtribuirEscala(double sX, double sY, double sZ)
        {
            AtribuirIdentidade();
            matriz[0] =  sX;
            matriz[5] =  sY;
            matriz[10] = sZ;
        }

        public void AtribuirRotacaoX(double radians)
        {
            AtribuirIdentidade();
            matriz[5] =   Math.Cos(radians);
            matriz[9] =  -Math.Sin(radians);
            matriz[6] =   Math.Sin(radians);
            matriz[10] =  Math.Cos(radians);
        }

        public void AtribuirRotacaoY(double radians)
        {
            AtribuirIdentidade();
            matriz[0] =   Math.Cos(radians);
            matriz[8] =   Math.Sin(radians);
            matriz[2] =  -Math.Sin(radians);
            matriz[10] =  Math.Cos(radians);
        }

        public void AtribuirRotacaoZ(double radians)
        {
            AtribuirIdentidade();
            matriz[0] =  Math.Cos(radians);
            matriz[4] = -Math.Sin(radians);
            matriz[1] =  Math.Sin(radians);
            matriz[5] =  Math.Cos(radians);
        }

        public Ponto4D TransformPoint(Ponto4D point) {
            Ponto4D pointResult = new Ponto4D(
                matriz[0]*point.X  + matriz[4]*point.Y + matriz[8]*point.Z + matriz[12]*point.W,
                matriz[1]*point.X  + matriz[5]*point.Y + matriz[9]*point.Z + matriz[13]*point.W,
                matriz[2]*point.X  + matriz[6]*point.Y + matriz[10]*point.Z + matriz[14]*point.W,
                matriz[3]*point.X  + matriz[7]*point.Y + matriz[11]*point.Z + matriz[15]*point.W
            );
            return pointResult;
        }

        public Transformacao TransformMatrix(Transformacao t) {
            Transformacao result = new Transformacao();
            for (int i=0; i < 16; ++i)
                result.matriz[i] =
                    matriz[i%4]*t.matriz[i/4*4]+matriz[(i%4)+4]*t.matriz[i/4*4+1]
                    + matriz[(i%4)+8]*t.matriz[i/4*4+2]+matriz[(i%4)+12]*t.matriz[i/4*4+3];
            return result;
        }

        public override string ToString() {
            string res = "";
            res += string.Format("|{0},{1},{2},{3}|", Matriz[0],Matriz[1],Matriz[2],Matriz[3]);
            res += string.Format("|{0},{1},{2},{3}|", Matriz[4],Matriz[5],Matriz[6],Matriz[7]);
            res += string.Format("|{0},{1},{2},{3}|", Matriz[8],Matriz[9],Matriz[10],Matriz[11]);
            res += string.Format("|{0},{1},{2},{3}|", Matriz[12],Matriz[13],Matriz[14],Matriz[15]);
            return res;

        }


        


        public void Translacao(){}
        public void Rotacao(){}
        public void Escala(){}

    }
}

