using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace OneVision.View.UI.UserControls.LiveView
{
    //Customized:
    //1. Bỏ background vì có ảnh hiển thị rồi
    //2. Chỉ được tạo 1 vùng Polygon
    //3. Cho xóa Polygon qua menu
    //4. Cho xóa Point khi chuột phải nếu số Point > 3 (Để vẫn là Polygon)
    //5. Khi khung Resize thì polygon cũng resize theo

    abstract class BaseOverlay
    {
        protected System.Windows.Forms.Control picCanvas;
        /// <summary>
        /// Vẽ đa giác trên 1 control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="color"></param>
        /// <param name="thickness"></param>
        public BaseOverlay(System.Windows.Forms.Control control, Color color, float thickness = 1f)
        {
            picCanvas = control;
            _pen = new Pen(color, thickness);

            //this.picCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseMove_NotDrawing);
            this.picCanvas.SizeChanged += new System.EventHandler(this.picCanvas_Resize);
            //this.picCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseDown);
            this.picCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.picCanvas_Paint);

            MakeBackgroundGrid();
        }

        public void Pause()
        {
            this.picCanvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseMove_NotDrawing);
            this.picCanvas.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseDown);
        }

        public void Resume()
        {
            this.picCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseMove_NotDrawing);
            this.picCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseDown);
        }

        /// <summary>
        /// Vẽ vùng polygon đã có trên màn hình
        /// </summary>
        /// <param name="region">Vùng mà polygon được vẽ trên</param>
        /// <param name="points">Danh sách các đỉnh của poolygon</param>
        public void SetPolygon(Size region, List<PointF> points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            Polygons.Clear();

            Polygons.Add(points);

            //Set và Lưu size hiện tại => Khi resize sẽ tự tính lại theo ClientSize mới
            _currentSize = picCanvas.ClientSize = region;
        }

        public List<PointF> GetPolygon()
        {
            if (Polygons.Count == 0)
                return new List<PointF>();

            return Polygons[0];
        }

        /// <summary>
        /// Clear toàn bộ
        /// </summary>
        public void Close()
        {
            if (picCanvas == null)
                return;
            this.picCanvas.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseMove_NotDrawing);
            this.picCanvas.Resize -= new System.EventHandler(this.picCanvas_Resize);
            this.picCanvas.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.picCanvas_MouseDown);
            this.picCanvas.Paint -= new System.Windows.Forms.PaintEventHandler(this.picCanvas_Paint);
        }

        protected Pen _pen = null;

        //Size hiện tại của picCanvas
        private Size _currentSize;

        // The "size" of an object for mouse over purposes.
        protected const int object_radius = 5;

        // We're over an object if the distance squared
        // between the mouse and the object is less than this.
        private const int over_dist_squared = object_radius * object_radius;

        // Each polygon is represented by a List<PointF>.
        protected List<List<PointF>> Polygons = new List<List<PointF>>();

        // Points for the new polygon.
        protected List<PointF> NewPolygon = null;

        // The current mouse position while drawing a new polygon.
        protected PointF NewPoint;

        // The polygon and index of the corner we are moving.
        protected List<PointF> MovingPolygon = null;
        protected int MovingPoint = -1;
        protected float OffsetX, OffsetY;

        // Start or continue drawing a new polygon,
        // or start moving a corner or polygon.
        protected virtual void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            //// See what we're over.
            //PointF mouse_pt = SnapToGrid(e.Location);
            //List<PointF> hit_polygon;
            //int hit_point, hit_point2;
            //PointF closest_point;

            //switch (e.Button)
            //{
            //    case MouseButtons.Right:
            //        if (NewPolygon != null)
            //        {
            //            // We are already drawing a polygon.
            //            // If it's the right mouse button, finish this polygon.
            //            // Finish this polygon.
            //            if (NewPolygon.Count > 2) Polygons.Add(NewPolygon);
            //            NewPolygon = null;

            //            // We no longer are drawing.
            //            picCanvas.MouseMove += picCanvas_MouseMove_NotDrawing;
            //            picCanvas.MouseMove -= picCanvas_MouseMove_Drawing;
            //        }
            //        else if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
            //        {
            //            //Delete point nếu sau khi add vẫn vẽ được tam giác
            //            if (hit_polygon.Count > 3)
            //                hit_polygon.RemoveAt(hit_point);
            //        }

            //        break;

            //    case MouseButtons.Left:
            //        if (NewPolygon != null)
            //        {
            //            // We are already drawing a polygon.
            //            // If it's the left mouse button, finish this polygon.
            //            // Add a point to this polygon.
            //            if (NewPolygon[NewPolygon.Count - 1] != mouse_pt)
            //            {
            //                NewPolygon.Add(mouse_pt);
            //            }
            //        }
            //        else if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
            //        {
            //            // Start dragging this corner.
            //            picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
            //            picCanvas.MouseMove += picCanvas_MouseMove_MovingCorner;
            //            picCanvas.MouseUp += picCanvas_MouseUp_MovingCorner;

            //            // Remember the polygon and point number.
            //            MovingPolygon = hit_polygon;
            //            MovingPoint = hit_point;

            //            // Remember the offset from the mouse to the point.
            //            OffsetX = hit_polygon[hit_point].X - e.X;
            //            OffsetY = hit_polygon[hit_point].Y - e.Y;
            //        }
            //        else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
            //            out hit_point, out hit_point2, out closest_point))
            //        {
            //            // Add a point.
            //            hit_polygon.Insert(hit_point + 1, closest_point);
            //        }
            //        else if (MouseIsOverPolygon(mouse_pt, out hit_polygon))
            //        {
            //            // Start moving this polygon.
            //            picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
            //            picCanvas.MouseMove += picCanvas_MouseMove_MovingPolygon;
            //            picCanvas.MouseUp += picCanvas_MouseUp_MovingPolygon;

            //            // Remember the polygon.
            //            MovingPolygon = hit_polygon;

            //            // Remember the offset from the mouse to the segment's first point.
            //            OffsetX = hit_polygon[0].X - e.X;
            //            OffsetY = hit_polygon[0].Y - e.Y;
            //        }
            //        else if (Polygons.Count == 0)
            //        {
            //            //Nếu chưa có polygon nào
            //            // Start a new polygon.
            //            NewPolygon = new List<PointF>();
            //            NewPoint = mouse_pt;
            //            NewPolygon.Add(mouse_pt);

            //            // Get ready to work on the new polygon.
            //            picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
            //            picCanvas.MouseMove += picCanvas_MouseMove_Drawing;
            //        }
            //        break;
            //    default:
            //        return;
            //}

            //// Redraw.
            //picCanvas.Invalidate();
        }

        // Move the next point in the new polygon.
        protected void picCanvas_MouseMove_Drawing(object sender, MouseEventArgs e)
        {
            NewPoint = SnapToGrid(e.Location);
            picCanvas.Invalidate();
        }

        // Move the selected corner.
        protected void picCanvas_MouseMove_MovingCorner(object sender, MouseEventArgs e)
        {
            // Move the point.
            MovingPolygon[MovingPoint] =
                SnapToGrid(new PointF(e.X + OffsetX, e.Y + OffsetY));

            // Redraw.
            picCanvas.Invalidate();
        }

        // Finish moving the selected corner.
        protected void picCanvas_MouseUp_MovingCorner(object sender, MouseEventArgs e)
        {
            picCanvas.MouseMove += picCanvas_MouseMove_NotDrawing;
            picCanvas.MouseMove -= picCanvas_MouseMove_MovingCorner;
            picCanvas.MouseUp -= picCanvas_MouseUp_MovingCorner;
        }

        // Move the selected polygon.
        protected void picCanvas_MouseMove_MovingPolygon(object sender, MouseEventArgs e)
        {
            // See how far the first point will move.
            float new_x1 = e.X + OffsetX;
            float new_y1 = e.Y + OffsetY;

            float dx = new_x1 - MovingPolygon[0].X;
            float dy = new_y1 - MovingPolygon[0].Y;

            // Snap the movement to a multiple of the grid distance.
            dx = GridGap * (int)(Math.Round((float)dx / GridGap));
            dy = GridGap * (int)(Math.Round((float)dy / GridGap));

            if (dx == 0 && dy == 0) return;

            // Move the polygon.
            for (int i = 0; i < MovingPolygon.Count; i++)
            {
                MovingPolygon[i] = new PointF(
                    MovingPolygon[i].X + dx,
                    MovingPolygon[i].Y + dy);
            }

            // Redraw.
            picCanvas.Invalidate();
        }

        // Finish moving the selected polygon.
        protected void picCanvas_MouseUp_MovingPolygon(object sender, MouseEventArgs e)
        {
            picCanvas.MouseMove += picCanvas_MouseMove_NotDrawing;
            picCanvas.MouseMove -= picCanvas_MouseMove_MovingPolygon;
            picCanvas.MouseUp -= picCanvas_MouseUp_MovingPolygon;
        }

        // See if we're over a polygon or corner point.
        protected virtual void picCanvas_MouseMove_NotDrawing(object sender, MouseEventArgs e)
        {
            ////Nếu chưa có polygon thì mới dùng Cross để user biết có thể tạo được polygon
            //Cursor new_cursor = Polygons.Count == 0 ? Cursors.Cross : Cursors.Default;

            //// See what we're over.
            //PointF mouse_pt = SnapToGrid(e.Location);
            //List<PointF> hit_polygon;
            //int hit_point, hit_point2;
            //PointF closest_point;

            //if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
            //{
            //    new_cursor = Cursors.NoMove2D;
            //}
            //else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
            //    out hit_point, out hit_point2, out closest_point))
            //{
            //    new_cursor = AddPointCursor;
            //}
            //else if (MouseIsOverPolygon(mouse_pt, out hit_polygon))
            //{
            //    new_cursor = Cursors.Hand;
            //}

            //// Set the new cursor.
            //if (picCanvas.Cursor != new_cursor)
            //{
            //    picCanvas.Cursor = new_cursor;
            //}
        }

        // Redraw old polygons in blue. Draw the new polygon in green.
        // Draw the final segment dashed.
        protected virtual void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //// Draw the old polygons.
            //foreach (List<PointF> polygon in Polygons)
            //{
            //    // Draw the polygon.
            //    HatchBrush hBrush = new HatchBrush(HatchStyle.Percent05, Color.Blue, Color.Transparent);
            //    e.Graphics.FillPolygon(hBrush, polygon.ToArray());
            //    e.Graphics.DrawPolygon(Pens.Blue, polygon.ToArray());

            //    // Draw the corners.
            //    foreach (PointF corner in polygon)
            //    {
            //        Rectangle rect = new Rectangle(
            //            (int)(corner.X - object_radius), (int)(corner.Y - object_radius),
            //            2 * object_radius + 1, 2 * object_radius + 1);
            //        e.Graphics.FillEllipse(Brushes.White, rect);
            //        e.Graphics.DrawEllipse(Pens.Black, rect);
            //    }
            //}

            //// Draw the new polygon.
            //if (NewPolygon != null)
            //{
            //    // Draw the new polygon.
            //    if (NewPolygon.Count > 1)
            //    {
            //        e.Graphics.DrawLines(Pens.Red, NewPolygon.ToArray());
            //    }

            //    // Draw the newest edge.
            //    if (NewPolygon.Count > 0)
            //    {
            //        using (Pen dashed_pen = new Pen(Color.Red))
            //        {
            //            dashed_pen.DashPattern = new float[] { 3, 3 };
            //            e.Graphics.DrawLine(dashed_pen,
            //                NewPolygon[NewPolygon.Count - 1],
            //                NewPoint);
            //        }
            //    }
            //}
        }

        // See if the mouse is over a corner point.
        protected bool MouseIsOverCornerPoint(PointF mouse_pt, out List<PointF> hit_polygon, out int hit_pt)
        {
            // See if we're over a corner point.
            foreach (List<PointF> polygon in Polygons)
            {
                // See if we're over one of the polygon's corner points.
                for (int i = 0; i < polygon.Count; i++)
                {
                    // See if we're over this point.
                    if (FindDistanceToPointSquared(polygon[i], mouse_pt) < over_dist_squared)
                    {
                        // We're over this point.
                        hit_polygon = polygon;
                        hit_pt = i;
                        return true;
                    }
                }
            }

            hit_polygon = null;
            hit_pt = -1;
            return false;
        }

        // See if the mouse is over a polygon's edge.
        protected bool MouseIsOverEdge(PointF mouse_pt, out List<PointF> hit_polygon, out int hit_pt1, out int hit_pt2, out PointF closest_point)
        {
            // Examine each polygon.
            // Examine them in reverse order to check the ones on top first.
            for (int pgon = Polygons.Count - 1; pgon >= 0; pgon--)
            {
                List<PointF> polygon = Polygons[pgon];

                // See if we're over one of the polygon's segments.
                for (int p1 = 0; p1 < polygon.Count; p1++)
                {
                    // Get the index of the polygon's next point.
                    int p2 = (p1 + 1) % polygon.Count;

                    // See if we're over the segment between these points.
                    PointF closest;
                    if (FindDistanceToSegmentSquared(mouse_pt,
                        polygon[p1], polygon[p2], out closest) < over_dist_squared)
                    {
                        // We're over this segment.
                        hit_polygon = polygon;
                        hit_pt1 = p1;
                        hit_pt2 = p2;
                        closest_point = Point.Round(closest);
                        return true;
                    }
                }
            }

            hit_polygon = null;
            hit_pt1 = -1;
            hit_pt2 = -1;
            closest_point = new Point(0, 0);
            return false;
        }

        // See if the mouse is over a polygon's body.
        protected bool MouseIsOverPolygon(PointF mouse_pt, out List<PointF> hit_polygon)
        {
            // Examine each polygon.
            // Examine them in reverse order to check the ones on top first.
            for (int i = Polygons.Count - 1; i >= 0; i--)
            {
                // Make a GraphicsPath representing the polygon.
                GraphicsPath path = new GraphicsPath();
                path.AddPolygon(Polygons[i].ToArray());

                // See if the point is inside the GraphicsPath.
                if (path.IsVisible(mouse_pt))
                {
                    hit_polygon = Polygons[i];
                    return true;
                }
            }

            hit_polygon = null;
            return false;
        }

        #region DistanceFunctions

        // Calculate the distance squared between two points.
        private float FindDistanceToPointSquared(PointF pt1, PointF pt2)
        {
            float dx = pt1.X - pt2.X;
            float dy = pt1.Y - pt2.Y;
            return dx * dx + dy * dy;
        }

        // Calculate the distance squared between
        // point pt and the segment p1 --> p2.
        private double FindDistanceToSegmentSquared(PointF pt, PointF p1, PointF p2, out PointF closest)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            // return Math.Sqrt(dx * dx + dy * dy);
            return dx * dx + dy * dy;
        }

        #endregion DistanceFunctions

        // The grid spacing.
        //Nếu để GridGap = 8 như cũ sẽ thỉnh thoảng có lỗi không bắt được vào điểm góc
        //1 số điểm đặc biệt mà hàm MouseIsOverCornerPoint tính ra không nằm trong vòng tròn góc
        //Đúng ra thì MouseIsOverCornerPoint phải dựa trên vị trí thực chứ không phải từ SnapToGrid 
        private const int GridGap = 1; // 8

        // Snap to the nearest grid point.
        protected PointF SnapToGrid(PointF point)
        {
            int x = GridGap * (int)Math.Round((float)point.X / GridGap);
            int y = GridGap * (int)Math.Round((float)point.Y / GridGap);
            return new PointF(x, y);
        }

        protected void DrawTheCorners(List<PointF> polygon, PaintEventArgs e)
        {
            foreach (PointF corner in polygon)
            {
                Rectangle rect = new Rectangle(
                   (int)(corner.X - object_radius), (int)(corner.Y - object_radius),
                   2 * object_radius + 1, 2 * object_radius + 1);
                e.Graphics.FillEllipse(Brushes.White, rect);
                e.Graphics.DrawEllipse(Pens.Black, rect);
            }
        }

        // Give the PictureBox a grid background.
        private void picCanvas_Resize(object sender, EventArgs e)
        {
            var polygon = GetPolygon();
            ResizePolygon(polygon, _currentSize, picCanvas.ClientSize);

            _currentSize = picCanvas.ClientSize;

            // Redraw.
            //picCanvas.Invalidate();

            MakeBackgroundGrid();
        }

        private void ResizePolygon(List<PointF> polygon, Size oldSize, Size newSize)
        {
            float ratioX = (float)picCanvas.ClientSize.Width / (float)_currentSize.Width;
            float ratioY = (float)picCanvas.ClientSize.Height / (float)_currentSize.Height;

            for (int i = 0; i < polygon.Count; i++)
            {
                var item = polygon[i];
                var newX = (item.X * ratioX);
                if (float.IsNaN(newX))
                    newX = 0;

                var newY = (item.Y * ratioY);
                if (float.IsNaN(newY))
                    newY = 0;

                polygon[i] = new PointF(newX, newY);
            }
        }

        private void MakeBackgroundGrid()
        {
            //Background là ảnh rồi
            //Bitmap bm = new Bitmap(
            //    picCanvas.ClientSize.Width,
            //    picCanvas.ClientSize.Height);
            //for (int x = 0; x < picCanvas.ClientSize.Width; x += GridGap)
            //{
            //    for (int y = 0; y < picCanvas.ClientSize.Height; y += GridGap)
            //    {
            //        bm.SetPixel(x, y, Color.Black);
            //    }
            //}

            //picCanvas.BackgroundImage = bm;
        }
    }

    class LineOverlay : BaseOverlay
    {
        public LineOverlay(Control control, Color color)
            : base(control, color, 4f)
        { }

        protected override void picCanvas_MouseMove_NotDrawing(object sender, MouseEventArgs e)
        {
            //Nếu chưa có polygon thì mới dùng Cross để user biết có thể tạo được polygon
            Cursor new_cursor = Polygons.Count == 0 ? Cursors.Cross : Cursors.Default;

            // See what we're over.
            PointF mouse_pt = SnapToGrid(e.Location);
            List<PointF> hit_polygon;
            int hit_point, hit_point2;
            PointF closest_point;

            if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
            {
                new_cursor = Cursors.NoMove2D;
            }
            else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
                out hit_point, out hit_point2, out closest_point))
            {
                new_cursor = Cursors.Hand;
            }

            // Set the new cursor.
            if (picCanvas.Cursor != new_cursor)
            {
                picCanvas.Cursor = new_cursor;
            }
        }

        // Start or continue drawing a new polygon,
        // or start moving a corner or polygon.
        protected override void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            // See what we're over.
            PointF mouse_pt = SnapToGrid(e.Location);
            List<PointF> hit_polygon;
            int hit_point, hit_point2;
            PointF closest_point;

            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (NewPolygon != null)
                    {
                        // We are already drawing a polygon.
                        // If it's the right mouse button, finish this polygon.
                        // Finish this polygon.
                        if (NewPolygon.Count > 1)
                            Polygons.Add(NewPolygon);
                        NewPolygon = null;

                        // We no longer are drawing.
                        picCanvas.MouseMove += picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove -= picCanvas_MouseMove_Drawing;
                    }

                    break;

                case MouseButtons.Left:
                    if (NewPolygon != null)
                    {
                        // We are already drawing a polygon.
                        // If it's the left mouse button, finish this polygon.
                        // Add a point to this polygon.
                        if (NewPolygon[NewPolygon.Count - 1] != mouse_pt)
                        {
                            NewPolygon.Add(mouse_pt);
                            //Đã có đủ 2 point => Kết thúc
                            Polygons.Add(NewPolygon);
                            NewPolygon = null;

                            // We no longer are drawing.
                            picCanvas.MouseMove += picCanvas_MouseMove_NotDrawing;
                            picCanvas.MouseMove -= picCanvas_MouseMove_Drawing;
                        }
                    }
                    else if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
                    {
                        // Start dragging this corner.
                        picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove += picCanvas_MouseMove_MovingCorner;
                        picCanvas.MouseUp += picCanvas_MouseUp_MovingCorner;

                        // Remember the polygon and point number.
                        MovingPolygon = hit_polygon;
                        MovingPoint = hit_point;

                        // Remember the offset from the mouse to the point.
                        OffsetX = hit_polygon[hit_point].X - e.X;
                        OffsetY = hit_polygon[hit_point].Y - e.Y;
                    }
                    else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
                        out hit_point, out hit_point2, out closest_point))
                    {
                        // Start moving this polygon.
                        picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove += picCanvas_MouseMove_MovingPolygon;
                        picCanvas.MouseUp += picCanvas_MouseUp_MovingPolygon;

                        // Remember the polygon.
                        MovingPolygon = hit_polygon;

                        // Remember the offset from the mouse to the segment's first point.
                        OffsetX = hit_polygon[0].X - e.X;
                        OffsetY = hit_polygon[0].Y - e.Y;
                    }
                    else if (Polygons.Count == 0)
                    {
                        //Nếu chưa có polygon nào
                        // Start a new polygon.
                        NewPolygon = new List<PointF>();
                        NewPoint = mouse_pt;
                        NewPolygon.Add(mouse_pt);

                        // Get ready to work on the new polygon.
                        picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove += picCanvas_MouseMove_Drawing;
                    }
                    break;
                default:
                    return;
            }

            // Redraw.
            picCanvas.Invalidate();
        }

        // Redraw old polygons in blue. Draw the new polygon in green.
        // Draw the final segment dashed.
        protected override void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw the old polygons.
            foreach (List<PointF> polygon in Polygons)
            {
                // Draw the line.
                if (polygon.Count > 1)
                {
                    e.Graphics.DrawLines(_pen, polygon.ToArray());
                }

                // Draw the corners.
                DrawTheCorners(polygon, e);
            }

            // Draw the new polygon.
            if (NewPolygon != null)
            {
                // Draw the new polygon.
                if (NewPolygon.Count > 1)
                {
                    e.Graphics.DrawLines(_pen, NewPolygon.ToArray());
                }

                // Draw the newest edge.
                if (NewPolygon.Count > 0)
                {
                    using (Pen dashed_pen = new Pen(_pen.Color, _pen.Width))
                    {
                        dashed_pen.DashPattern = new float[] { 3, 3 };
                        e.Graphics.DrawLine(dashed_pen,
                            NewPolygon[NewPolygon.Count - 1],
                            NewPoint);
                    }
                }
            }
        }
    }

    class VectorOverlay : LineOverlay
    {
        public VectorOverlay(Control control, Color color)
            : base(control, color)
        {
            _pen.StartCap = LineCap.RoundAnchor;
            _pen.EndCap = LineCap.ArrowAnchor;
        }

        // Redraw old polygons in blue. Draw the new polygon in green.
        // Draw the final segment dashed.
        protected override void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw the old polygons.
            foreach (List<PointF> polygon in Polygons)
            {
                // Draw the line.
                if (polygon.Count > 1)
                {
                    e.Graphics.DrawLines(_pen, polygon.ToArray());
                }

                // Draw the Vector
                DrawVector(polygon[0], polygon[1], e);
            }

            // Draw the new polygon.
            if (NewPolygon != null)
            {
                // Draw the new polygon.
                if (NewPolygon.Count > 1)
                {
                    // Draw the Vector
                    DrawVector(NewPolygon[0], NewPolygon[1], e);
                }

                // Draw the newest edge.
                if (NewPolygon.Count > 0)
                {
                    DrawVector(NewPolygon[NewPolygon.Count - 1], NewPoint, e);
                }
            }
        }

        private void DrawVector(PointF start, PointF end, PaintEventArgs e)
        {

            e.Graphics.DrawLine(_pen, start.X, start.Y, end.X, end.Y);
        }
    }

    class PolygonOverlay : BaseOverlay
    {

        // The add point cursor.
        private Cursor AddPointCursor;

        public PolygonOverlay(Control control, Color color)
            : base(control, color, 2f)
        {
            // Create the add point cursor.
            AddPointCursor = Cursors.Cross; //new Cursor(Properties.Resources.add_point.GetHicon());
        }

        protected override void picCanvas_MouseMove_NotDrawing(object sender, MouseEventArgs e)
        {
            //Nếu chưa có polygon thì mới dùng Cross để user biết có thể tạo được polygon
            Cursor new_cursor = Polygons.Count == 0 ? Cursors.Cross : Cursors.Default;

            // See what we're over.
            PointF mouse_pt = SnapToGrid(e.Location);
            List<PointF> hit_polygon;
            int hit_point, hit_point2;
            PointF closest_point;

            if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
            {
                new_cursor = Cursors.NoMove2D;
            }
            else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
                out hit_point, out hit_point2, out closest_point))
            {
                new_cursor = AddPointCursor;
            }
            else if (MouseIsOverPolygon(mouse_pt, out hit_polygon))
            {
                new_cursor = Cursors.Hand;
            }

            // Set the new cursor.
            if (picCanvas.Cursor != new_cursor)
            {
                picCanvas.Cursor = new_cursor;
            }
        }

        // Start or continue drawing a new polygon,
        // or start moving a corner or polygon.
        protected override void picCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            // See what we're over.
            PointF mouse_pt = SnapToGrid(e.Location);
            List<PointF> hit_polygon;
            int hit_point, hit_point2;
            PointF closest_point;

            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (NewPolygon != null)
                    {
                        // We are already drawing a polygon.
                        // If it's the right mouse button, finish this polygon.
                        // Finish this polygon.
                        if (NewPolygon.Count > 2) Polygons.Add(NewPolygon);
                        NewPolygon = null;

                        // We no longer are drawing.
                        picCanvas.MouseMove += picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove -= picCanvas_MouseMove_Drawing;
                    }
                    else if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
                    {
                        //Delete point nếu sau khi add vẫn vẽ được tam giác
                        if (hit_polygon.Count > 3)
                            hit_polygon.RemoveAt(hit_point);
                    }

                    break;

                case MouseButtons.Left:
                    if (NewPolygon != null)
                    {
                        // We are already drawing a polygon.
                        // If it's the left mouse button, finish this polygon.
                        // Add a point to this polygon.
                        if (NewPolygon[NewPolygon.Count - 1] != mouse_pt)
                        {
                            NewPolygon.Add(mouse_pt);
                        }
                    }
                    else if (MouseIsOverCornerPoint(mouse_pt, out hit_polygon, out hit_point))
                    {
                        // Start dragging this corner.
                        picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove += picCanvas_MouseMove_MovingCorner;
                        picCanvas.MouseUp += picCanvas_MouseUp_MovingCorner;

                        // Remember the polygon and point number.
                        MovingPolygon = hit_polygon;
                        MovingPoint = hit_point;

                        // Remember the offset from the mouse to the point.
                        OffsetX = hit_polygon[hit_point].X - e.X;
                        OffsetY = hit_polygon[hit_point].Y - e.Y;
                    }
                    else if (MouseIsOverEdge(mouse_pt, out hit_polygon,
                        out hit_point, out hit_point2, out closest_point))
                    {
                        // Add a point.
                        hit_polygon.Insert(hit_point + 1, closest_point);
                    }
                    else if (MouseIsOverPolygon(mouse_pt, out hit_polygon))
                    {
                        // Start moving this polygon.
                        picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove += picCanvas_MouseMove_MovingPolygon;
                        picCanvas.MouseUp += picCanvas_MouseUp_MovingPolygon;

                        // Remember the polygon.
                        MovingPolygon = hit_polygon;

                        // Remember the offset from the mouse to the segment's first point.
                        OffsetX = hit_polygon[0].X - e.X;
                        OffsetY = hit_polygon[0].Y - e.Y;
                    }
                    else if (Polygons.Count == 0)
                    {
                        //Nếu chưa có polygon nào
                        // Start a new polygon.
                        NewPolygon = new List<PointF>();
                        NewPoint = mouse_pt;
                        NewPolygon.Add(mouse_pt);

                        // Get ready to work on the new polygon.
                        picCanvas.MouseMove -= picCanvas_MouseMove_NotDrawing;
                        picCanvas.MouseMove += picCanvas_MouseMove_Drawing;
                    }
                    break;
                default:
                    return;
            }

            // Redraw.
            picCanvas.Invalidate();
        }

        // Redraw old polygons in blue. Draw the new polygon in green.
        // Draw the final segment dashed.
        protected override void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw the old polygons.
            foreach (List<PointF> polygon in Polygons)
            {
                // Draw the polygon.
                HatchBrush hBrush = new HatchBrush(HatchStyle.Percent05, _pen.Color, Color.Transparent);
                e.Graphics.FillPolygon(hBrush, polygon.ToArray());
                e.Graphics.DrawPolygon(_pen, polygon.ToArray());

                // Draw the corners.
                DrawTheCorners(polygon, e);
            }

            // Draw the new polygon.
            if (NewPolygon != null)
            {
                // Draw the new polygon.
                if (NewPolygon.Count > 1)
                {
                    e.Graphics.DrawLines(_pen, NewPolygon.ToArray());
                }

                // Draw the newest edge.
                if (NewPolygon.Count > 0)
                {
                    using (Pen dashed_pen = new Pen(_pen.Color, _pen.Width))
                    {
                        dashed_pen.DashPattern = new float[] { 3, 3 };
                        e.Graphics.DrawLine(dashed_pen,
                            NewPolygon[NewPolygon.Count - 1],
                            NewPoint);
                    }
                }
            }
        }
    }
}
