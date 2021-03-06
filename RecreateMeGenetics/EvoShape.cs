﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecreateMeUtils;
using System.Drawing;

namespace RecreateMeGenetics
{
    //Class of a single shape of the drawing
    class EvoShape
    {
        //Points for the shape
        private List<EvoPoint> shapePoints;
        //Color of the shape
        private EvoColor color;
        public EvoColor Color
        {
            get { return color; }
            private set { color = value; }
        }
        //Minimum points required for a shape
        private int minPoints;
        public int MinPoints
        {
            get { return minPoints; }
            private set { minPoints = value; }
        }
        //Maximum points required for a shape
        private int maxPoints;
        public int MaxPoints
        {
            get { return maxPoints; }
            private set { maxPoints = value; }
        }

        //Constructor creating a shape in memory
        public EvoShape(int minPoints, int maxPoints)
        {
            MinPoints = minPoints;
            MaxPoints = maxPoints;

            color = new EvoColor();

            shapePoints = new List<EvoPoint>();
            var center = new EvoPoint();

            for (int i=0; i<MinPoints; i++)
            {   
                int X = Math.Min(Math.Max(0, center.X + Numbers.GetRandom(-3, 3)), Numbers.MaxWidth);
                int Y = Math.Min(Math.Max(0, center.Y + Numbers.GetRandom(-3, 3)), Numbers.MaxHeight);
                shapePoints.Add(new EvoPoint(X, Y));
            }            
        }

        public EvoShape Clone()
        {
            return new EvoShape(shapePoints, Color, MinPoints, MaxPoints);
        }

        private EvoShape(List<EvoPoint> points, EvoColor color, int minPoints, int maxPoints)
        {

            shapePoints = new List<EvoPoint>();
            foreach (var point in points)
            {
                shapePoints.Add(point.Clone());
            }
            Color = color.Clone();
            MinPoints = minPoints;
            MaxPoints = maxPoints;
        }

        //Gets brush which is needed to paint 
        public SolidBrush getBrush()
        {
            return color.ToBrush();
        }

        //Gets points in form used by 
        public Point[] getPoints()
        {
            Point[] points = new Point[shapePoints.Count];
            int i = 0;

            foreach (var evoPoint in shapePoints)
            {
                points[i++] = new Point(evoPoint.X, evoPoint.Y);
            }
            return points;
        }

        //Mutating shape by changing number of points or changing their position
        //TODO change mutation rates to variables
        public void Mutate(EvoDrawing parent)
        {
            foreach (var point in shapePoints)
            {
                point.Mutate(parent);
            }

            //Add point to shape
            if (shapePoints.Count < MaxPoints && Numbers.ProbabilityFulfilled(Numbers.AddPointMutationProbability))
            {
                parent.NeedRepaint = true;

                EvoPoint additionalPoint = new EvoPoint();
                int i = Numbers.GetRandom(1, shapePoints.Count - 1);
                additionalPoint.X = Numbers.GetAverage(shapePoints[i - 1].X, shapePoints[i].X);
                additionalPoint.Y = Numbers.GetAverage(shapePoints[i - 1].Y, shapePoints[i].Y);
                shapePoints.Insert(i, additionalPoint);
            }
            //Delete point
            if(shapePoints.Count > MinPoints && Numbers.ProbabilityFulfilled(Numbers.RemovePointmutationProbability))
            {
                parent.NeedRepaint = true;
                shapePoints.Remove(
                    shapePoints.ElementAt<EvoPoint>(
                        Numbers.GetRandom(0, shapePoints.Count)));
            }

            color.Mutate(parent);
        }
    }
}
