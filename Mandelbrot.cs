using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;


namespace FractalVisualizer
{
    public class Mandelbrot : Fractal
    {
        public Mandelbrot(Color[] ColorSet) : base(ColorSet)
        {
            sqrMaxDist = 4;
        }

        protected override Complex movePoint (Complex point, Complex initialPoint, params double[] args)
        {
            return Complex.Exp(args[0]*Complex.Log(point)) + initialPoint;
        }
    }
}