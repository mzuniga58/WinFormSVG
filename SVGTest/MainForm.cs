using WinFormSVG;

namespace SVGTest
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var brBackground = new SolidBrush(Color.Black);
			e.Graphics.FillRectangle(brBackground, ClientRectangle);

			SVG svg = new();

			string xml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<svg viewBox=""0 0 200 154"">
	<property id=""BackColor"" value=""#ffffff"" />
	<property id=""BorderColor"" value=""#cccccc"" />
	<property id=""Banner"" value=""#217346"" />
	<property id=""ArrowBackground"" value=""#e8e8e8"" />
	<rect fill=""url(#BackColor)"" stroke=""url(#BorderColor)"" stroke-width=""1"" x=""0"" y=""0"" width=""200"" height=""154""/>
	<rect fill=""url(#Banner)""  x=""0"" y=""105"" width=""159"" height=""48""/>
	<circle fill=""url(#ArrowBackground)"" stroke=""url(#BackColor)"" stroke-width=""5"" cx=""159"" cy=""129"" r=""24"" />
	<line stroke=""url(#Banner)"" stroke-width=""4"" x1=""157"" y1=""117"" x2=""167"" y2=""128"" />
	<line stroke=""url(#Banner)"" stroke-width=""4"" x1=""146"" y1=""127"" x2=""166"" y2=""127"" />
	<line stroke=""url(#Banner)"" stroke-width=""4"" x1=""157"" y1=""137"" x2=""167"" y2=""126"" />
	<text x=""10"" y=""46"" font-family=""Generic-sans-serif"" font-size=""12"" fill=""url(#Banner)"" >
		Blank image
	</text>
	<rect fill=""none"" stroke=""url(#BorderColor)"" stroke-width=""1"" x=""0"" y=""0"" width=""200"" height=""154""/>
</svg>";
			svg.LoadXml(xml);

			int borderSize = 100;
			var rc = new Rectangle(borderSize,
								   borderSize,
								   200,
								   154);

			//Dictionary<string, string> properties = new()
			//{
			//	{ "BackColor", "#000000" },
			//	{ "BorderColor", "#555555" },
			//	{ "Banner", "#555555" },
			//	{ "ArrowBackground", "#888888" }
			//};

			Dictionary<string, string> properties = new()
			{
				{ "BackColor", "#ffffff" },
				{ "BorderColor", "#cccccc" },
				{ "Banner", "#217346" },
				{ "ArrowBackground", "#e8e8e8" }
			};

			var bmp = svg.Render(rc, properties);

			e.Graphics.DrawImage(bmp, rc);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			Invalidate();
		}
	}
}
