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
			var brBackground = new SolidBrush(Color.White);
			e.Graphics.FillRectangle(brBackground, ClientRectangle);

			SVG svg = new();

			svg.Load("Sample.svg");

			int borderSize = 100;
			var rc = new Rectangle(borderSize,
								   borderSize,
								   24,
								   24);

			Dictionary<string, string> properties = new()
			{
				{ "BackColor", "#ffffff" },
				{ "BorderColor", "none" },
				{ "IconOutline", "#000000" }
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
