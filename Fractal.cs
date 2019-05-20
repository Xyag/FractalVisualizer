using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;


namespace FractalVisualizer
{
    public abstract class Fractal
    {
        public Color[] ColorSet;

        protected double sqrMaxDist;

        public Fractal(Color[] colorSet)
        {
            ColorSet = colorSet;
        }
        //using ref to avoid GC and allocation for high resolution images.
        public void Graph(Complex lowerLeft, Complex topRight, int maxIterations, ref Bitmap writeTo)
        {
            Graph(lowerLeft,topRight,maxIterations,ref writeTo,null);
        }
        public void Graph(Complex lowerLeft, Complex topRight, int maxIterations, ref Bitmap writeTo, params double[] args)
        {
            double dx = (topRight.real-lowerLeft.real)/(double)writeTo.Width;
            double dy = (topRight.imaginary-lowerLeft.imaginary)/(double)writeTo.Height;
            for (int y = 0; y < writeTo.Height; y++)
            {
                for (int x = 0; x < writeTo.Width; x++)
                {
                    int iter = diverge(lowerLeft + new Complex(dx*(double)x, dy*(double)y), maxIterations, sqrMaxDist, args);
                    writeTo.SetPixel(x,y,getColor(iter, maxIterations, ColorSet));
                }
            }
        }
        public virtual Color getColor(int iterations, int maxIterations, Color[] colors)
        {
            //lerps between colors
            double progress = (double)iterations/(double)maxIterations*((double)colors.Length-1);
            int lower = (int)Math.Floor(progress);
            int higher = (int)Math.Ceiling(progress);
            progress = progress - Math.Floor(progress);
            return Color.FromArgb((int)(colors[lower].R*(1-progress)+colors[higher].R*progress),
              (int)(colors[lower].G*(1-progress)+colors[higher].G*progress),
              (int)(colors[lower].B*(1-progress)+colors[higher].B*progress));
        }
        protected int diverge(Complex point, int maxIterations, double sqrMaxDist, params double[] args)
        {
            Complex newPoint = point;
            int iterations = 0;
            while (newPoint.SqrMagnitude() < sqrMaxDist && iterations<maxIterations)
            {
                newPoint = movePoint(newPoint, point, args);
                iterations++;
            }
            return iterations;
        }
        protected abstract Complex movePoint (Complex point, Complex initialPoint, params double[] args);
    }
}