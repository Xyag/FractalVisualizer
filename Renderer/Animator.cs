using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FractalVisualizer
{
    public static class Animator
    {
        public static void AnimatePowerThreaded(Fractal f, Complex LL, Complex TR, double startPower, double endPower, int maxIterations, int threadCount, int frames)
        {
            Parallel.For(0,threadCount, i => AnimatePower(f, LL, TR, startPower, endPower, maxIterations, i, threadCount, frames));
        }
        public static void Animate(Fractal f, Complex startLL, Complex startTR, Complex endLL, Complex endTR, int maxIterations, int frames)
        {
            Bitmap[] animation = new Bitmap[frames];
            Stopwatch timer = new Stopwatch();
            for (int i = 0; i < frames; i++)
            {
                timer.Restart();
                animation[i] = new Bitmap(360,360);
                Complex currLL = interpolate(startLL,endLL,Math.Log10(1+9*(double)i/(double)(frames-1)),20);
                Complex currTR = interpolate(startTR,endTR,Math.Log10(1+9*(double)i/(double)(frames-1)),20);
                Console.WriteLine(currLL.ToString(), ", " + currTR.ToString());
                f.Graph(currLL,currTR,maxIterations,ref animation[i]);
                animation[i].Save(@"C:\Users\Public\Pictures\Mandelbrot\animation\frame"+ i + ".png");
                timer.Stop();
                Console.WriteLine("Frame {0} done in {1} ms", i, timer.ElapsedMilliseconds);
                
            }
        }
        public static void AnimatePower(Fractal f, Complex LL, Complex TR, double startPower, double endPower, int maxIterations, int totalFrames)
        {
            AnimatePower(f, LL, TR, startPower, endPower, maxIterations, 0, totalFrames, totalFrames);
        }
        private static void AnimatePower (Fractal f, Complex LL, Complex TR, double startPower, double endPower, int maxIterations, int startFrame, int mod, int totalFrames)
        {
            Console.WriteLine("Starting on frame {0} mod {1}", startFrame, mod);
            int currFrameNum = startFrame;
            int iterations = 0;
            Bitmap currFrame = new Bitmap(480,480);
            while (currFrameNum < totalFrames)
            {
                double currPower = startPower+(endPower-startPower)*(double)currFrameNum/(double)(totalFrames-1);
                f.Graph(LL,TR,maxIterations, ref currFrame,currPower);
                currFrame.Save(@"C:\Users\Jim\Pictures\Fractals\"+ currFrameNum + ".png");
                Console.WriteLine("Finished frame {0}", currFrameNum);

                iterations++;
                currFrameNum = startFrame + mod*iterations;
            }
        }  
        private static Complex interpolate(Complex a, Complex b, double t, double steepness)
        {
            double exp = Math.Exp(steepness);
            double coeff = 1/steepness*Math.Log((exp-1)*(t-1/(1-exp)));
            return new Complex(a.real*(1-coeff)+b.real*coeff,a.imaginary*(1-coeff)+b.imaginary*coeff);
        }   
    }
}