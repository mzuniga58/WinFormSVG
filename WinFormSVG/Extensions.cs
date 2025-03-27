using System.Drawing.Drawing2D;

namespace WinFormSVG
{
	internal static class Extensions
	{
		public static void Union(this Rectangle source, Rectangle target, int cx = 0, int cy = 0)
		{
			var x1 = source.Left;
			var y1 = source.Top;
			var x2 = source.Right;
			var y2 = source.Bottom;

			var emptyRectangle = (source.Width == 0 || source.Height == 0);

			Rectangle rc = target;

			if (cx != 0 || cy != 0)
				rc.Inflate(cx, cy);

			if (rc.Left < x1 || emptyRectangle)
				x1 = rc.Left;

			if (rc.Top < y1 || emptyRectangle)
				y1 = rc.Top;

			if (rc.Right > x2 || emptyRectangle)
				x2 = rc.Right;

			if (rc.Bottom > y2 || emptyRectangle)
				y2 = rc.Bottom;

			source.X = x1;
			source.Y = y1;
			source.Width = x2 - x1;
			source.Height = y2 - y1;
		}

		public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int? cornerRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(pen);

			using GraphicsPath path = RoundedRect(bounds, cornerRadius);
			graphics.DrawPath(pen, path);
		}

		public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, RectangleF bounds, float? cornerRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(pen);

			using GraphicsPath path = RoundedRect(bounds, cornerRadius);
			graphics.DrawPath(pen, path);
		}

		public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int? topLeftRadius, int? topRightRadius, int? bottomLeftRadius, int? bottomRightRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(pen);

			using GraphicsPath path = RoundedRect(bounds, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
			graphics.DrawPath(pen, path);
		}

		public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, RectangleF bounds, float? topLeftRadius, float? topRightRadius, float? bottomLeftRadius, float? bottomRightRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(pen);

			using GraphicsPath path = RoundedRect(bounds, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
			graphics.DrawPath(pen, path);
		}

		public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int? cornerRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(brush);

			using GraphicsPath path = RoundedRect(bounds, cornerRadius);
			graphics.FillPath(brush, path);
		}

		public static void FillRoundedRectangle(this Graphics graphics, Brush brush, RectangleF bounds, int? cornerRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(brush);

			using GraphicsPath path = RoundedRect(bounds, cornerRadius);
			graphics.FillPath(brush, path);
		}

		public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int? topLeftRadius, int? topRightRadius, int? bottomLeftRadius, int? bottomRightRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(brush);

			using GraphicsPath path = RoundedRect(bounds, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
			graphics.FillPath(brush, path);
		}

		public static void FillRoundedRectangle(this Graphics graphics, Brush brush, RectangleF bounds, float? topLeftRadius, float? topRightRadius, float? bottomLeftRadius, float? bottomRightRadius)
		{
			ArgumentNullException.ThrowIfNull(graphics);
			ArgumentNullException.ThrowIfNull(brush);

			using GraphicsPath path = RoundedRect(bounds, topLeftRadius, topRightRadius, bottomLeftRadius, bottomRightRadius);
			graphics.FillPath(brush, path);
		}

		public static GraphicsPath RoundedRect(Rectangle bounds, int? radius)
		{
			int r = radius == null ? 0 : radius.Value;
			int diameter = r * 2;
			Size size = new(diameter, diameter);
			Rectangle arc = new(bounds.Location, size);
			GraphicsPath path = new();

			if (radius == 0)
			{
				path.AddRectangle(bounds);
				return path;
			}

			// top left arc  
			path.AddArc(arc, 180, 90);

			// top right arc  
			arc.X = bounds.Right - diameter;
			path.AddArc(arc, 270, 90);

			// bottom right arc  
			arc.Y = bounds.Bottom - diameter;
			path.AddArc(arc, 0, 90);

			// bottom left arc 
			arc.X = bounds.Left;
			path.AddArc(arc, 90, 90);

			path.CloseFigure();
			return path;
		}

		public static GraphicsPath RoundedRect(RectangleF bounds, float? radius)
		{
			float r = radius == null ? 0.0f : radius.Value;
			float diameter = r * 2;
			SizeF size = new(diameter, diameter);
			RectangleF arc = new(bounds.Location, size);
			GraphicsPath path = new();

			if (radius == 0)
			{
				path.AddRectangle(bounds);
				return path;
			}

			// top left arc  
			path.AddArc(arc, 180, 90);

			// top right arc  
			arc.X = bounds.Right - diameter;
			path.AddArc(arc, 270, 90);

			// bottom right arc  
			arc.Y = bounds.Bottom - diameter;
			path.AddArc(arc, 0, 90);

			// bottom left arc 
			arc.X = bounds.Left;
			path.AddArc(arc, 90, 90);

			path.CloseFigure();
			return path;
		}

		public static GraphicsPath RoundedRect(Rectangle bounds, int? topLeftRadius, int? topRightRadius, int? bottomLeftRadius, int? bottomRightRadius)
		{
			GraphicsPath path = new();

			var tlr = topLeftRadius == null ? 0 : topLeftRadius.Value;
			var trr = topRightRadius == null ? 0 : topRightRadius.Value;
			var blr = bottomLeftRadius == null ? 0 : bottomLeftRadius.Value;
			var brr = bottomRightRadius == null ? 0 : bottomRightRadius.Value;

			if (tlr == 0 && trr == 0 && blr == 0 && brr == 0)
			{
				path.AddRectangle(bounds);
				return path;
			}

			// top left arc  

			if (topLeftRadius > 0)
			{
				int diameter = tlr * 2;
				Size size = new(diameter, diameter);
				Rectangle arc = new(bounds.Location, size);
				path.AddArc(arc, 180, 90);
			}

			path.AddLine(new Point(bounds.Left + tlr, bounds.Top),
						 new Point(bounds.Right - trr, bounds.Top));

			if (topRightRadius > 0)
			{
				// top right arc  
				int diameter = trr * 2;
				Size size = new(diameter, diameter);
				Rectangle arc = new(bounds.Location, size)
				{
					X = bounds.Right - diameter
				};
				path.AddArc(arc, 270, 90);
			}

			path.AddLine(new Point(bounds.Right, bounds.Top + trr),
							new Point(bounds.Right, bounds.Bottom - brr));

			if (bottomRightRadius > 0)
			{
				// bottom right arc  
				int diameter = brr * 2;
				Size size = new(diameter, diameter);
				Rectangle arc = new(bounds.Location, size)
				{
					X = bounds.Right - diameter,
					Y = bounds.Bottom - diameter
				};
				path.AddArc(arc, 0, 90);
			}

			path.AddLine(new Point(bounds.Right - brr, bounds.Bottom),
						 new Point(bounds.Left + blr, bounds.Bottom));


			if (bottomLeftRadius > 0)
			{
				// bottom left arc 
				int diameter = blr * 2;
				Size size = new(diameter, diameter);
				Rectangle arc = new(bounds.Location, size)
				{
					Y = bounds.Bottom - diameter
				};
				path.AddArc(arc, 90, 90);
			}

			path.AddLine(new Point(bounds.Left, bounds.Bottom - blr),
						 new Point(bounds.Left, bounds.Top + tlr));

			path.CloseFigure();
			return path;
		}

		public static GraphicsPath RoundedRect(RectangleF bounds, float? topLeftRadius, float? topRightRadius, float? bottomLeftRadius, float? bottomRightRadius)
		{
			GraphicsPath path = new();

			var tlr = topLeftRadius == null ? 0 : topLeftRadius.Value;
			var trr = topRightRadius == null ? 0 : topRightRadius.Value;
			var blr = bottomLeftRadius == null ? 0 : bottomLeftRadius.Value;
			var brr = bottomRightRadius == null ? 0 :bottomRightRadius.Value;

			if (tlr == 0 && trr == 0 && blr == 0 && brr == 0)
			{
				path.AddRectangle(bounds);
				return path;
			}

			// top left arc  

			if (topLeftRadius > 0)
			{
				float diameter = tlr * 2;
				SizeF size = new(diameter, diameter);
				RectangleF arc = new(bounds.Location, size);
				path.AddArc(arc, 180, 90);
			}

			path.AddLine(new PointF(bounds.Left + tlr, bounds.Top),
						 new PointF(bounds.Right - trr, bounds.Top));

			if (topRightRadius > 0)
			{
				// top right arc  
				float diameter = trr * 2;
				SizeF size = new(diameter, diameter);
				RectangleF arc = new(bounds.Location, size)
				{
					X = bounds.Right - diameter
				};
				path.AddArc(arc, 270, 90);
			}

			path.AddLine(new PointF(bounds.Right, bounds.Top + trr),
							new PointF(bounds.Right, bounds.Bottom - brr));

			if (bottomRightRadius > 0)
			{
				// bottom right arc  
				float diameter = brr * 2;
				SizeF size = new(diameter, diameter);
				RectangleF arc = new(bounds.Location, size)
				{
					X = bounds.Right - diameter,
					Y = bounds.Bottom - diameter
				};
				path.AddArc(arc, 0, 90);
			}

			path.AddLine(new PointF(bounds.Right - brr, bounds.Bottom),
						 new PointF(bounds.Left + blr, bounds.Bottom));


			if (bottomLeftRadius > 0)
			{
				// bottom left arc 
				float diameter = blr * 2;
				SizeF size = new(diameter, diameter);
				RectangleF arc = new(bounds.Location, size)
				{
					Y = bounds.Bottom - diameter
				};
				path.AddArc(arc, 90, 90);
			}

			path.AddLine(new PointF(bounds.Left, bounds.Bottom - blr),
						 new PointF(bounds.Left, bounds.Top + tlr));

			path.CloseFigure();
			return path;
		}
	}
}
