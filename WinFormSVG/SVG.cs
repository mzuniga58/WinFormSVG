using System.Drawing.Drawing2D;
using System.Xml;

namespace WinFormSVG
{
    public class SVG
    {
        #region Private properties
        private readonly XmlDocument _document;
        private float Width = 0;
        private float Height = 0;
        private RectangleF ViewBox = Rectangle.Empty;
        private Rectangle rcRender = Rectangle.Empty;
        private readonly Dictionary<string, string> properties = [];
        private float scaleX = 1.0f;
		private float scaleY = 1.0f;
		#endregion

		#region Constructor
		public SVG()
        {
            _document = new XmlDocument();
        }
		#endregion

		#region Loading
		public void Load(string filename)
		{
            properties.Clear();
			_document.Load(filename);
		}

		public void LoadXml(string xml)
		{
			properties.Clear();
			_document.LoadXml(xml);
		}
		#endregion

		#region Drawing
		public Bitmap Render(Rectangle rc, Dictionary<string, string>? Properties = null)
		{
			rcRender = new Rectangle(0, 0, rc.Width, rc.Height);
			Bitmap canvas = new(rc.Width, rc.Height);
			var svgNodes = _document.GetElementsByTagName("svg");

			if (svgNodes != null)
			{
				if (svgNodes.Count > 0)
				{
					var svgNode = svgNodes[0];

					if (svgNode != null)
					{
						ExtractSVGParameters(svgNode);

						if (Properties != null)
						{
							foreach (var property in Properties)
							{
								var key = property.Key;
								var value = property.Value;

								if (properties.ContainsKey(key))
									properties[key] = value;
							}
						}

						using var g = Graphics.FromImage(canvas);
						g.SmoothingMode = SmoothingMode.HighQuality;
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;

						scaleX = rcRender.Width / ViewBox.Width;
						scaleY = rcRender.Height / ViewBox.Height;

						foreach (XmlNode node in svgNode.ChildNodes)
						{
							RenderNode(g, 1, node);
						}
					}
				}
			}

			return canvas;
		}

		public Bitmap Render(RectangleF rc, Dictionary<string, string>? Properties = null)
		{
			rcRender = new Rectangle(0, 0, Convert.ToInt32(rc.Width), Convert.ToInt32(rc.Height));
			Bitmap canvas = new(Convert.ToInt32(rc.Width), Convert.ToInt32(rc.Height));
			var svgNodes = _document.GetElementsByTagName("svg");

			if (svgNodes != null)
			{
				if (svgNodes.Count > 0)
				{
					var svgNode = svgNodes[0];

					if (svgNode != null)
					{
						ExtractSVGParameters(svgNode);

						if (Properties != null)
						{
							foreach (var property in Properties)
							{
								var key = property.Key;
								var value = property.Value;

								if (properties.ContainsKey(key))
									properties[key] = value;
							}
						}

						using var g = Graphics.FromImage(canvas);
						g.SmoothingMode = SmoothingMode.HighQuality;
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;

						scaleX = rcRender.Width / ViewBox.Width;
						scaleY = rcRender.Height / ViewBox.Height;

						foreach (XmlNode node in svgNode.ChildNodes)
						{
							RenderNode(g, 1, node);
						}
					}
				}
			}

			return canvas;
		}

		public void Render(Graphics canvasGraphics, Rectangle rc, Dictionary<string, string>? Properties = null)
		{
			rcRender = new Rectangle(0, 0, rc.Width, rc.Height);
			Bitmap canvas = new(rc.Width, rc.Height);
			var svgNodes = _document.GetElementsByTagName("svg");

			if (svgNodes != null)
			{
				if (svgNodes.Count > 0)
				{
					var svgNode = svgNodes[0];

					if (svgNode != null)
					{
						ExtractSVGParameters(svgNode);

						if (Properties != null)
						{
							foreach (var property in Properties)
							{
								var key = property.Key;
								var value = property.Value;

								if (properties.ContainsKey(key))
									properties[key] = value;
							}
						}

						using var g = Graphics.FromImage(canvas);
						g.SmoothingMode = SmoothingMode.HighQuality;
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;

						scaleX = rcRender.Width / ViewBox.Width;
						scaleY = rcRender.Height / ViewBox.Height;

						foreach (XmlNode node in svgNode.ChildNodes)
						{
							RenderNode(g, 1, node);
						}
					}
				}
			}

			canvasGraphics.DrawImage(canvas, rc);
		}

		public void Render(Graphics canvasGraphics, RectangleF rc, Dictionary<string, string>? Properties = null)
		{
			rcRender = new Rectangle(0, 0, Convert.ToInt32(rc.Width), Convert.ToInt32(rc.Height));
			Bitmap canvas = new(Convert.ToInt32(rc.Width), Convert.ToInt32(rc.Height));
			var svgNodes = _document.GetElementsByTagName("svg");

			if (svgNodes != null)
			{
				if (svgNodes.Count > 0)
				{
					var svgNode = svgNodes[0];

					if (svgNode != null)
					{
						ExtractSVGParameters(svgNode);

						if (Properties != null)
						{
							foreach (var property in Properties)
							{
								var key = property.Key;
								var value = property.Value;

								if (properties.ContainsKey(key))
									properties[key] = value;
							}
						}

						using var g = Graphics.FromImage(canvas);
						g.SmoothingMode = SmoothingMode.HighQuality;
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;

						scaleX = rcRender.Width / ViewBox.Width;
						scaleY = rcRender.Height / ViewBox.Height;

						foreach (XmlNode node in svgNode.ChildNodes)
						{
							RenderNode(g, 1, node);
						}
					}
				}
			}

			canvasGraphics.DrawImage(canvas, rc);
		}

		private void ExtractSVGParameters(XmlNode svgNode)
        {
            Width = Convert.ToSingle(svgNode?.Attributes?["width"]?.Value ?? "100");
            Height = Convert.ToSingle(svgNode?.Attributes?["height"]?.Value ?? "100");

            var strViewBox = svgNode?.Attributes?["viewBox"]?.Value ?? "0 0 100 100";

            if (string.IsNullOrWhiteSpace(strViewBox))
            {
                ViewBox = new RectangleF(0, 0, Width, Height);
            }
            else
            {
                var parts = strViewBox.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                var x = Convert.ToSingle(parts[0]);
                var y = Convert.ToSingle(parts[1]);
                var cx = Convert.ToSingle(parts[2]);
                var cy = Convert.ToSingle(parts[3]);

                ViewBox = new RectangleF(x, y, cx, cy);
            }

            if (svgNode != null)
            {
                foreach (XmlNode node in svgNode.ChildNodes)
                {
                    if (node.Name.Equals("property", StringComparison.OrdinalIgnoreCase))
                    {
                        var key = node?.Attributes?["id"]?.Value;
                        var value = node?.Attributes?["value"]?.Value;

                        if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                        {
                            properties.Add(key, value);
                        }
                    }
                }
            }
		}
		#endregion

		#region Render group
		private void RenderGroup(Graphics g, float op, XmlNode node)
        {
            var opacity = Convert.ToSingle(Convert.ToSingle(node?.Attributes?["opacity"]?.Value ?? "1"));

            if (node != null)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    RenderNode(g, opacity, child);
                }
            }
        }
		#endregion

		#region Render shapes
		private void RenderNode(Graphics g, float opacity, XmlNode node)
        {
            if (node.Name.Equals("rect", StringComparison.OrdinalIgnoreCase))
                RenderRectangle(g, opacity, node);
			else if (node.Name.Equals("circle", StringComparison.OrdinalIgnoreCase))
				RenderCircle(g, opacity, node);
			else if (node.Name.Equals("line", StringComparison.OrdinalIgnoreCase))
				RenderLine(g, opacity, node);
			else if (node.Name.Equals("polyline", StringComparison.OrdinalIgnoreCase))
				RenderPolyLine(g, opacity, node);
			else if (node.Name.Equals("text", StringComparison.OrdinalIgnoreCase))
				RenderText(g, opacity, node);
			else if (node.Name.Equals("g", StringComparison.OrdinalIgnoreCase))
            {
                RenderGroup(g, opacity, node);
            }
        }

        /// <summary>
        /// Draw a polyline on the canvas
        /// </summary>
        /// <param coordinate="g">The <see cref="Graphics"/> surface of the canvas</param>
        /// <param coordinate="opacity">The opacity setting: 1 = completely opaque, 0 = completely transparent</param>
        /// <param coordinate="node">The node to render, must be of type "polyline"</param>
        private void RenderPolyLine(Graphics g, float opacity, XmlNode node)
        {
            if (node.Name.Equals("polyline", StringComparison.OrdinalIgnoreCase))
            {
                string? points = node?.Attributes?["points"]?.Value;
                Color? fill = ParseColor(node?.Attributes?["fill"]?.Value);
                Color? stroke = ParseColor(node?.Attributes?["stroke"]?.Value);
                float? strokeWidth = ParseCoordinate(node?.Attributes?["stroke-width"]?.Value);

                if (points != null)
                {
                    var pts = points.Split([' '], StringSplitOptions.RemoveEmptyEntries);

                    List<PointF> pointArray = [];
                    List<byte> pointTypesArray = [];
                    bool start = true;
                    foreach (var p in pts)
                    {
                        var ptparts = p.Split([','], StringSplitOptions.RemoveEmptyEntries);

                        var pt = new PointF(rcRender.Left + Convert.ToSingle(Convert.ToSingle(ptparts[0]) * scaleX),
                                            rcRender.Top + Convert.ToSingle(Convert.ToSingle(ptparts[1]) * scaleY));
                        pointArray.Add(pt);
                        pointTypesArray.Add(start ? (byte)PathPointType.Start : (byte)PathPointType.Line);
                        start = false;
                    }

                    GraphicsPath path = new(pointArray.ToArray(), [.. pointTypesArray]);

                    if (fill != null)
                    {
                        path.CloseFigure();
                        Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), fill.Value.R, fill.Value.G, fill.Value.B);
                        var brFill = new SolidBrush(clr);
                        g.FillPath(brFill, path);
                    }

                    if (stroke != null)
                    {
                        var w = strokeWidth == null ? 1.0f : strokeWidth.Value;

                        var wx = w * scaleX;
                        var wy = w * scaleY;

                        w = wx < wy ? wx : wy;

                        if (w < 1)
                            w = 1;

                        Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), stroke.Value.R, stroke.Value.G, stroke.Value.B);
                        var penStroke = new Pen(clr, w);
                        g.DrawPath(penStroke, path);
                    }
                }
            }
        }

		/// <summary>
		/// Render a line on the canvas
		/// </summary>
		/// <param coordinate="g">The <see cref="Graphics"/> surface of the canvas</param>
		/// <param coordinate="opacity">The opacity setting: 1 = completely opaque, 0 = completely transparent</param>
		/// <param coordinate="node">The node to render, must be of type "circle"</param>
		private void RenderLine(Graphics g, float opacity, XmlNode node)
        {
            if (node.Name.Equals("line", StringComparison.OrdinalIgnoreCase))
            {
                var rX1 = ParseCoordinate(node?.Attributes?["x1"]?.Value);
                var rY1 = ParseCoordinate(node?.Attributes?["y1"]?.Value);
                var rX2 = ParseCoordinate(node?.Attributes?["x2"]?.Value);
                var rY2 = ParseCoordinate(node?.Attributes?["y2"]?.Value);

                var stroke = ParseColor(node?.Attributes?["stroke"]?.Value);
                var strokeWidth = ParseCoordinate(node?.Attributes?["stroke-width"]?.Value);

                var x1 = Convert.ToSingle(rcRender.Left + (rX1 - ViewBox.Left) * scaleX);
                var y1 = Convert.ToSingle(rcRender.Top + (rY1 - ViewBox.Top) * scaleY);
                var x2 = Convert.ToSingle(rcRender.Left + (rX2 - ViewBox.Left) * scaleX);
                var y2 = Convert.ToSingle(rcRender.Top + (rY2 - ViewBox.Top) * scaleY);

                if (stroke != null)
                {
                    float sw = strokeWidth == null ? 1.0f : strokeWidth.Value;

                    float swx = sw * scaleX;
                    float swy = sw * scaleY;

                    float w = swx < swy ? swx : swy;

                    Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), stroke.Value.R, stroke.Value.G, stroke.Value.B);
                    var penStroke = new Pen(clr, w);

                    g.DrawLine(penStroke, new PointF(x1, y1), new PointF(x2, y2));
				}
            }
        }

		/// <summary>
		/// Renders a circle on the canvas
		/// </summary>
		/// <param coordinate="g">The <see cref="Graphics"/> surface of the canvas</param>
		/// <param coordinate="opacity">The opacity setting: 1 = completely opaque, 0 = completely transparent</param>
		/// <param coordinate="node">The node to render, must be of type "circle"</param>
		private void RenderCircle(Graphics g, float opacity, XmlNode node)
		{
            if (node.Name.Equals("circle", StringComparison.OrdinalIgnoreCase))
            {
                var rCX = ParseCoordinate(node?.Attributes?["cx"]?.Value);
                var rCY = ParseCoordinate(node?.Attributes?["cy"]?.Value);
                var rRadius = ParseCoordinate(node?.Attributes?["r"]?.Value);
                var fill = ParseColor(node?.Attributes?["fill"]?.Value);
                var stroke = ParseColor(node?.Attributes?["stroke"]?.Value);
                var strokeWidth = ParseCoordinate(node?.Attributes?["stroke-width"]?.Value);

                var cx = Convert.ToSingle(rcRender.Left + (rCX - ViewBox.Left) * scaleX);
                var cy = Convert.ToSingle(rcRender.Top + (rCY - ViewBox.Top) * scaleY);
                var rx = Convert.ToSingle(rRadius * scaleX);
                var ry = Convert.ToSingle(rRadius * scaleY);
                var r = rx < ry ? rx : ry;

                if (fill != null)
                {
					float sw = (strokeWidth == null) ? 1.0f : strokeWidth.Value;

					if (stroke == null || sw == 0)
                    {
						Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), fill.Value.R, fill.Value.G, fill.Value.B);
						var brFill = new SolidBrush(clr);
                        g.FillEllipse(brFill, cx - r, cy - r, r + r, r + r);
					}
					else
                    {
                        Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), fill.Value.R, fill.Value.G, fill.Value.B);
                        var brFill = new SolidBrush(clr);
                        g.FillEllipse(brFill, cx - r - 1, cy - r - 1, r + r - 2, r + r - 2);
                    }
                }

                if (stroke != null )
                {
                    float sw = (strokeWidth == null) ? 1.0f : strokeWidth.Value;

                    if (sw != 0)
                    {
                        float wx = sw * scaleX;
                        float wy = sw * scaleY;

                        var w = (wx < wy) ? wx : wy;

                        if (w < 1.0f)
                            w = 1.0f;

                        Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), stroke.Value.R, stroke.Value.G, stroke.Value.B);
                        var penStroke = new Pen(clr, w);
                        g.DrawEllipse(penStroke, cx - r - 1, cy - r - 1, r + r - 2, r + r - 2);
                    }
                }
            }
		}

        /// <summary>
        /// Renders a rectangle on the canvas
        /// </summary>
        /// <param coordinate="g">The <see cref="Graphics"/> surface of the canvas</param>
        /// <param coordinate="opacity">The opacity setting: 1 = completely opaque, 0 = completely transparent</param>
        /// <param coordinate="node">The node to render, must be of type "rect"</param>
		private void RenderRectangle(Graphics g, float opacity, XmlNode node)
        {
            if (node.Name.Equals("rect", StringComparison.OrdinalIgnoreCase))
            {
                var rX = ParseCoordinate(node?.Attributes?["x"]?.Value);
                var rY = ParseCoordinate(node?.Attributes?["y"]?.Value);
                var rRX = ParseCoordinate(node?.Attributes?["rx"]?.Value);
                var rRY = ParseCoordinate(node?.Attributes?["ry"]?.Value);
                var rCX = ParseCoordinate(node?.Attributes?["width"]?.Value);
                var rCY = ParseCoordinate(node?.Attributes?["height"]?.Value);
                var fill = ParseColor(node?.Attributes?["fill"]?.Value);
                var stroke = ParseColor(node?.Attributes?["stroke"]?.Value);
                var strokeWidth = ParseCoordinate(node?.Attributes?["stroke-width"]?.Value);

                //  In order to draw a rectangle, at a minimum, we must have a set of coordinates
                if (rX != null && rY != null && rCX != null && rCY != null)
                {
                    var x = Convert.ToSingle(rcRender.Left + (rX - ViewBox.Left) * scaleX);
                    var y = Convert.ToSingle(rcRender.Top + (rY - ViewBox.Top) * scaleY);
                    var cx = Convert.ToSingle(rCX * scaleX);
                    var cy = Convert.ToSingle(rCY * scaleY);

                    float? rx = rRX != null ? Convert.ToSingle(rRX * scaleX) : null;
                    float? ry = rRY != null ? Convert.ToInt32(rRY * scaleY) : null;

                    if (rx != null && ry == null)
                        ry = rx;

                    if (rx == null && ry != null)
                        rx = ry;

                    if (fill != null)
                    {
                        Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), fill.Value.R, fill.Value.G, fill.Value.B);
                        var brFill = new SolidBrush(clr);

                        if (stroke != null)
                        {
                            if ((rx == null || rx == 0) && (ry == null || ry == 0))
                                g.FillRectangle(brFill, new RectangleF(x, y, cx, cy));
                            else
                                g.FillRoundedRectangle(brFill, new RectangleF(x, y, cx - 1, cy - 1), rx, ry, rx, ry);
                        }
                        else
                        {
                            if ((rx == null || rx == 0) && (ry == null || ry == 0))
                                g.FillRectangle(brFill, new RectangleF(x, y, cx, cy));
                            else
                                g.FillRoundedRectangle(brFill, new RectangleF(x, y, cx, cy), rx, ry, rx, ry);
                        }
                    }

                    if (stroke != null )
                    {
                        Color clr = Color.FromArgb(Convert.ToInt32(opacity * 255), stroke.Value.R, stroke.Value.G, stroke.Value.B);
						var wx = strokeWidth.Value * scaleX;
						var wy = strokeWidth.Value * scaleY;

						var w = (wx < wy) ? wx : wy;

						var penStroke = new Pen(clr, w);

                        if ((rx == null || rx == 0) && (ry == null || ry == 0))
                            g.DrawRectangle(penStroke, new RectangleF(x, y, cx - 1, cy - 1));
                        else
                            g.DrawRoundedRectangle(penStroke, new RectangleF(x, y, cx - 1, cy - 1), rx, ry, rx, ry);
                    }
                }
            }
		}

		/// <summary>
		/// Renders a string of text on the canvas
		/// </summary>
		/// <param coordinate="g">The <see cref="Graphics"/> surface of the canvas</param>
		/// <param coordinate="opacity">The opacity setting: 1 = completely opaque, 0 = completely transparent</param>
		/// <param coordinate="node">The node to render, must be of type "text"</param>
		private void RenderText(Graphics g, float opacity, XmlNode node)
        {
            if ( node.Name.Equals("text", StringComparison.OrdinalIgnoreCase) ) 
            {
				var rX = ParseCoordinate(node?.Attributes?["x"]?.Value);
				var rY = ParseCoordinate(node?.Attributes?["y"]?.Value);
				var rFontSize = ParseCoordinate(node?.Attributes?["font-size"]?.Value);
				var rFontFamily = ParseName(node?.Attributes?["font-family"]?.Value);
				var fill = ParseColor(node?.Attributes?["fill"]?.Value);
                var textString = node?.InnerText.Trim();

                if (rFontFamily != null)
                {
                    var fontSize = (rFontSize == null) ? 9.0f : rFontSize.Value;
                    var fsize = fontSize * scaleY;
                    Color clr = fill == null ? Color.Black : fill.Value;
                    var x = (rX == null) ? 0.0f : rX.Value * scaleX;
                    var y = (rY == null) ? 0.0f : (rY.Value * scaleY);

                    var font = new Font(rFontFamily, fsize);
                    var brString = new SolidBrush(clr);
                    g.DrawString(textString, font, brString, new PointF(x, y));
                }
			}
		}
		#endregion

		#region Utilities
		private float? ParseCoordinate(string? coordinate)
		{
			if (string.IsNullOrWhiteSpace(coordinate)) return null;
            var coord = coordinate;

            if (coordinate.Equals("none", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

			if (coordinate.StartsWith("url(#"))
			{
				var key = coordinate.Substring(5, coordinate.Length - 6);
				coord = properties[key];
			}

			if (coord.EndsWith("px", StringComparison.OrdinalIgnoreCase))
			{
				var c = coord.Substring(0, coord.Length - 2);
				return Convert.ToSingle(c);
			}

			return Convert.ToSingle(coord);
		}

		private string? ParseName(string? name)
		{
			if (string.IsNullOrWhiteSpace(name)) return null;

			var nameString = name;

			if (nameString.StartsWith("url(#"))
			{
				var key = name.Substring(5, name.Length - 6);
				nameString = properties[key];
			}

			return nameString;
		}

		private Color? ParseColor(string? clr)
        {
            if ( string.IsNullOrWhiteSpace( clr ) ) return null;
            if ( clr.Equals("none", StringComparison.OrdinalIgnoreCase)) return null;

            var color = clr;

            if (clr.StartsWith("url(#"))
            {
                var key = clr.Substring(5, clr.Length - 6);
                color = properties[key];

                if (color.Equals("none", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }
            }

            if (color[0] == '#')
            {
                if (color.Length == 4)
                {
                    var r = Convert.ToInt32(color.Substring(1, 1), 16) + 16 * Convert.ToInt32(color.Substring(1, 1), 16);
                    var g = Convert.ToInt32(color.Substring(2, 1), 16) + 16 * Convert.ToInt32(color.Substring(2, 1), 16);
                    var b = Convert.ToInt32(color.Substring(3, 1), 16) + 16 * Convert.ToInt32(color.Substring(3, 1), 16);

                    return Color.FromArgb(r, g, b);
                }
                else if (color.Length == 7)
                {
                    var r = Convert.ToInt32(color.Substring(1, 2), 16);
                    var g = Convert.ToInt32(color.Substring(3, 2), 16);
                    var b = Convert.ToInt32(color.Substring(5, 2), 16);

                    return Color.FromArgb(r, g, b);
                }
                else
                    return Color.Black;
            }
            else if (color.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
            {
                var values = color.Split(['r', 'g', 'b', '(', ')', ','], StringSplitOptions.RemoveEmptyEntries);
				int r, g, b;
				if (values[0].EndsWith('%'))
					r = Convert.ToInt32(255 * Convert.ToSingle(values[0][..^1]) / 100);
				else
					r = Convert.ToInt32(values[0]);

				if (values[1].EndsWith('%'))
                    g = Convert.ToInt32(255 * Convert.ToSingle(values[1][..^1]) / 100);
                else
                    g = Convert.ToInt32(values[1]);

                if (values[2].EndsWith('%'))
                    b = Convert.ToInt32(255 * Convert.ToSingle(values[2][..^1]) / 100);
                else
                    b = Convert.ToInt32(values[2]);

                return Color.FromArgb(r, g, b);
            }
            else
            {
                return Color.FromName(color);
            }
        }
        #endregion
    }
}
