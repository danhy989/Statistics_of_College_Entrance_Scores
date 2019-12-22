using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Common
{
    public class RegressionHelper
    {
        public static double LinearRegression(
            double[] xVals,
            double[] yVals,
            int year)
        {
			
            if (xVals.Length != yVals.Length)
            {
                throw new Exception("Input values should be with the same length.");
            }
			
			Tuple<double, double> p = Fit.Line(xVals, yVals);
			double intercept = p.Item1;
			double slope = p.Item2;

			var predictedValue = (slope * year) + intercept;
			return predictedValue;
        }

        public static double[] PolynomialRegression(double[] xVals,
            double[] yVals,int year)
        {
            double x1, x2;
            double[] p = Fit.Polynomial(xVals, yVals, 2);
            if (xVals.Length != yVals.Length)
            {
                throw new Exception("Input values should be with the same length.");
            }

            // p2X^2 + p1X + p0 = year 
            // X => score

            double p0 = p[0] - year;
            double p1 = p[1];
            double p2 = p[2];
            double denta = p1 * p1 - 4 * p2 * p0;

            if (denta < 0)
            {
                return new double[]{ };
            }

            if(denta == 0)
            {
                x1 = -p1 / (2*p2);
                return new double[] { x1 };
            }

            double sqrtDenta = System.Math.Sqrt(denta);
            double denominator = 2 * p2;
            x1 = (-p1 + sqrtDenta) / denominator;
            x2 = (-p1 - sqrtDenta) / denominator;
            
            return new double[]{ x1, x2 };
        }
    }
}
