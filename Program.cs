using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace FractalVisualizer
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Color[] graphColors = {Color.Black, Color.FromArgb(0,0,100), Color.FromArgb(0,50,200), Color.Red, Color.Yellow, Color.White};
            //Color[] graphColors = {Color.Black, Color.DarkRed, Color.Purple, Color.FromArgb(196,0,52)};
            
            double xMin = -3;//Convert.ToDouble(args[0]);
            double yMin = -3;//Convert.ToDouble(args[1]);
            double xMax = 3;//Convert.ToDouble(args[2]);
            double yMax = 3;//Convert.ToDouble(args[3]);
            string imagePath = @"C:\Users\Public\Pictures\Mandelbrot\debug\";//args[4];

            Bitmap b = new Bitmap(720,720);
            int maxIterations = 100;

            Mandelbrot man = new Mandelbrot(graphColors);
            //man.Graph(new Complex(xMin,yMin), new Complex(xMax,yMax), maxIterations, ref b);

            //Mandelbrot.Graph(new Complex(xMin,yMin), new Complex(xMax,yMax), maxIterations, graphColors, b);
            //b.Save(imagePath + getTimedName() + ".png");

            //Animator.Animate(new Complex(-1,-0.5), new Complex(1,0.5), new Complex(1,-0.0005), new Complex(1.0005,0.0005), 100, graphColors, 50);
            Animator.AnimatePowerThreaded(man, new Complex(-2,-2), new Complex(2,2), 0, 5, 50, 4, 100);
        }

        static string getTimedName()
        {
            return System.DateTime.Now.Hour + "-" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second + "-" + System.DateTime.Now.Millisecond;
        }

    }

    public struct Complex
    {
        public double real;
        public double imaginary;

        public Complex(double real, double imaginary) {this.real = real; this.imaginary = imaginary;}
        public double SqrMagnitude()
        {
            return real*real+imaginary*imaginary;
        }
        public override string ToString()
        {
            return String.Format("{0} + {1}i",real,imaginary);
        }
        public static Complex Exp(Complex num)
        {
            double expReal = Math.Exp(num.real);
            return new Complex(expReal*Math.Cos(num.imaginary),expReal*Math.Sin(num.imaginary));
        }
        public static Complex Log(Complex num)
        {
            double theta = Math.Atan2(num.imaginary,num.real);
            double r = Math.Sqrt(num.SqrMagnitude());
            return new Complex(Math.Log(r),theta);
        }
        public static Complex operator +(Complex lhs, Complex rhs)
        {
            return new Complex(lhs.real+rhs.real, lhs.imaginary+rhs.imaginary);
        }
        public static Complex operator -(Complex lhs, Complex rhs)
        {
            return new Complex(lhs.real-rhs.real, lhs.imaginary-rhs.imaginary);
        }
        public static Complex operator *(Complex lhs, Complex rhs)
        {
            return new Complex(lhs.real*rhs.real-lhs.imaginary*rhs.imaginary,lhs.real*rhs.imaginary+lhs.imaginary*rhs.real);
        }
        public static Complex operator *(double a, Complex v)
        {
            return new Complex(a*v.real,a*v.imaginary);
        }
        public static Complex operator /(Complex num, Complex denom)
        {
            double scale = denom.imaginary*denom.imaginary+denom.real*denom.real;
            return new Complex ((num.real*denom.real+num.imaginary*denom.imaginary)/scale,(num.imaginary*denom.real-num.real*denom.imaginary)/scale);
        }
    }
}
