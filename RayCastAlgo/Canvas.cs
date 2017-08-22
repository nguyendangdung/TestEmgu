using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RayCastAlgo
{
    public partial class Canvas : UserControl
    {
        List<PointF> polygon; //to hold points of the polygon
        bool isCompleated; //to know whether the drawing is completed
        bool isSelected; //to know whether the polygon is selected

        public Canvas()
        {
            InitializeComponent();
            polygon = new List<PointF>();
            isCompleated = false; //initially drawing is incomplete
            isSelected = false; //initially there is no polygon to select
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            // fetch the graphics object of the canvas
            Graphics g = e.Graphics;

            //draw polygon only if the number of points in the polygon is greater than zero
            if (polygon != null && polygon.Count > 0)
            {
                //If there are atleast two points and the polygon is not completed
                if (polygon.Count > 1 && !isCompleated)
                {
                    //draw outlines of the polygon while drawing
                    g.DrawLines(Pens.Black, polygon.ToArray());
                }
                else if (isCompleated) //if the drawing is completed
                {
                    //Draw the filled polygon
                    g.FillPolygon(Brushes.Black, polygon.ToArray());
                    //If the polygon is selected
                    if (isSelected)
                    {
                        //then draw a thick red outline around the polygon
                        Pen p = new Pen(Color.Red);
                        p.Width = 2;
                        g.DrawLines(p, polygon.ToArray());
                    }
                }
            }
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {

            PointF currentPoint = e.Location;//to store the mouse location
            //if the polygon is still drawing
            if (!isCompleated)
            {
                //if this is not the first point of the polygon
                if (polygon.Count > 0)
                {
                    PointF firstPoint = polygon[0]; //fetch the first point
                    //if the first point is closer to the current mouse location                    
                    if (Math.Abs(firstPoint.X - currentPoint.X) < 5 && 
                        Math.Abs(firstPoint.Y - currentPoint.Y) < 5)
                    {
                        //add the first point as the last point
                        polygon.Add(firstPoint);
                        //Drawing is completed
                        isCompleated = true;
                    }
                    else //if mouse location is not closer to the first point of the polygon
                    {
                        //add the mouse location as a point of the polygon
                        polygon.Add(currentPoint);
                    }
                }
                else //if this is the first point of the polygon
                {
                    //add the mouse location as the first point of the polygon
                    polygon.Add(currentPoint);
                }
            }
            else //if the drawing is completed then check whether it is selected
            {
                //Ray-cast algorithm is here onward
                int k, j = polygon.Count - 1;
                bool oddNodes = false; //to check whether number of intersections is odd
                for (k = 0; k < polygon.Count; k++)
                {
                    //fetch adjucent points of the polygon
                    PointF polyK = polygon[k];
                    PointF polyJ = polygon[j];
                    
                    //check the intersections
                    if (((polyK.Y > currentPoint.Y) != (polyJ.Y > currentPoint.Y)) &&
                     (currentPoint.X < (polyJ.X - polyK.X) * (currentPoint.Y - polyK.Y) / (polyJ.Y - polyK.Y) + polyK.X))
                        oddNodes = !oddNodes; //switch between odd and even
                    j = k;
                }

                //if odd number of intersections
                if (oddNodes)
                {
                    //mouse point is inside the polygon
                    isSelected = true;
                }
                else //if even number of intersections
                {
                    //mouse point is outside the polygon so deselect the polygon
                    isSelected = false;
                }

            }
            Refresh();
        }
    }
}
