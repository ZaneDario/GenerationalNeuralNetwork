using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts.Resources
{
    class Maths
    {
        public static double Sigmoid(double x)
        {
            return 1f / (1f + Math.Pow(Math.E, -x));
        }

        public static double InverseSigmoid(double x)
        {
            return 1 / Sigmoid(x);
        }

        public static double LerpValue(double value, float min, float max)
        {
            return (value > max ? max : (value < min ? min : value));
        }
    }
}
