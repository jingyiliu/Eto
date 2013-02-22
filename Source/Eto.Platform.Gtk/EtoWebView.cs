using System;
using WebKit;
using GLib;
using System.Runtime.InteropServices;

namespace Eto.Platform.GtkSharp
{
	public class NewWindowPolicyDecisionRequestedArgs : SignalArgs
	{
		public WebFrame Frame { get { return base.Args [0] as WebFrame; } }

		public NetworkRequest Request { get { return base.Args [1] as NetworkRequest; } }

		public WebNavigationAction Action { get { return base.Args [0] as WebNavigationAction; } }

		public WebPolicyDecision Decision { get { return base.Args [0] as WebPolicyDecision; } }
	}

	public delegate void NewWindowPolicyDecisionRequestedHandler (object sender,NewWindowPolicyDecisionRequestedArgs e);

	public class EtoWebView : WebView
	{

		public EtoWebView ()
		{
		}

		public EtoWebView (IntPtr raw) : base (raw)
		{
		}

		[Signal("new-window-policy-decision-requested")]
		public event NewWindowPolicyDecisionRequestedHandler NewWindowPolicyDecisionRequested {
			add {
				var signal = Signal.Lookup (this, "new-window-policy-decision-requested", typeof(NewWindowPolicyDecisionRequestedArgs));
				signal.AddDelegate (value);
			}
			remove {
				var signal = Signal.Lookup (this, "new-window-policy-decision-requested", typeof(NewWindowPolicyDecisionRequestedArgs));
				signal.RemoveDelegate (value);
			}
		}

		private static bool newwindowpolicydecisionrequested_cb (IntPtr webview, IntPtr frame, IntPtr request, IntPtr action, IntPtr decision)
		{
			bool result;
			try {
				var webView = GLib.Object.GetObject (webview, false) as EtoWebView;
				result = webView.OnNewWindowPolicyDecisionRequested (GLib.Object.GetObject (frame) as WebFrame, GLib.Object.GetObject (request) as NetworkRequest, GLib.Object.GetObject (action) as WebNavigationAction, GLib.Object.GetObject (decision) as WebPolicyDecision);
			} catch (Exception ex) {
				ExceptionManager.RaiseUnhandledException (ex, true);
				throw ex;
			}
			return result;
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate bool NewWindowPolicyDecisionRequestedVMDelegate (IntPtr webview,IntPtr frame,IntPtr request,IntPtr action,IntPtr decision);

		private static NewWindowPolicyDecisionRequestedVMDelegate NewWindowPolicyDecisionRequestedVMCallback;

		private static void OverrideNewWindowPolicyDecisionRequested (GType gtype)
		{
			if (EtoWebView.NewWindowPolicyDecisionRequestedVMCallback == null) {
				EtoWebView.NewWindowPolicyDecisionRequestedVMCallback = new EtoWebView.NewWindowPolicyDecisionRequestedVMDelegate (EtoWebView.newwindowpolicydecisionrequested_cb);
			}
			GLib.Object.OverrideVirtualMethod (gtype, "new-window-policy-decision-requested", EtoWebView.NewWindowPolicyDecisionRequestedVMCallback);
		}

		[DefaultSignalHandler (Type = typeof(EtoWebView), ConnectionMethod = "OverrideNewWindowPolicyDecisionRequested")]
		protected virtual bool OnNewWindowPolicyDecisionRequested (WebFrame frame, NetworkRequest request, WebNavigationAction action, WebPolicyDecision decision)
		{
			var val = new Value (GType.Int);
			var valueArray = new ValueArray (5u);
			var array = new Value[5];

			array [0] = new Value (this);
			valueArray.Append (array [0]);
			array [1] = new Value (frame);
			valueArray.Append (array [1]);
			array [2] = new Value (request);
			valueArray.Append (array [2]);
			array [3] = new Value (action);
			valueArray.Append (array [3]);
			array [4] = new Value (decision);
			valueArray.Append (array [4]);
			GLib.Object.g_signal_chain_from_overridden (valueArray.ArrayPtr, ref val);
			var array2 = array;
			for (int i = 0; i < array2.Length; i++) {
				var value = array2 [i];
				value.Dispose ();
			}
			bool result = (bool)val;
			val.Dispose ();
			return result;
		}
	}
}

