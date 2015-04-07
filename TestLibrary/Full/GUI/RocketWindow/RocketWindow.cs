using System;
using Anomalous.GuiFramework;
using Anomalous.libRocketWidget;
using MyGUIPlugin;
using libRocketPlugin;

namespace Anomalous.Minimus.Full.GUI
{
	public class RocketWindow : MDIDialog
	{
		private RocketWidget rocketWidget;

		public RocketWindow ()
            : base("Anomalous.Minimus.Full.GUI.RocketWindow.RocketWindow.layout")
		{
			RocketInterface.Instance.FileInterface.addExtension(new RocketAssemblyResourceLoader(typeof(RocketWindow).Assembly));

			rocketWidget = new RocketWidget (window.findWidget ("Image") as ImageBox, false);
			//rocketWidget.Context.LoadDocument ("~/Phoneworld_droid.GUI.RocketWindow.TestRml.rml");

			RocketWidgetInterface.clearAllCaches();
			rocketWidget.Context.UnloadAllDocuments();
			using (ElementDocument document = rocketWidget.Context.LoadDocumentFromMemory (testRml))
			{
				if (document != null)
				{
					document.Show();
					rocketWidget.removeFocus();
					rocketWidget.renderOnNextFrame();
				}
			}

			window.WindowChangedCoord += HandleWindowChangedCoord;
		}

		void HandleWindowChangedCoord (Widget source, EventArgs e)
		{
			rocketWidget.resized();
		}

		public override void Dispose ()
		{
			rocketWidget.Dispose ();
			base.Dispose ();
		}

		static string testRml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<rml>
	<head>
		<link type=""text/rcss"" href=""~/libRocketPlugin.Resources.rkt.rcss""/>
    	<link type=""text/rcss"" href=""~/libRocketPlugin.Resources.Anomalous.rcss""/>	
	</head>
	<body>
		<p>This is an rml paragraph</p>
        <input type=""submit"">Submit</input>
	</body>
</rml>";
	}
}

