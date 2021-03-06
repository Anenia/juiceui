using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;

using Juice.Framework;
using Juice.Framework.TypeConverters;

namespace Juice {

	[ParseChildren(typeof(TabPage), DefaultProperty = "TabPages", ChildrenAsProperties = true)]
	[WidgetEvent("create")]
	[WidgetEvent("select")]
	[WidgetEvent("load")]
	[WidgetEvent("add")]
	[WidgetEvent("remove")]
	[WidgetEvent("enable")]
	[WidgetEvent("disable")]
	public class Tabs : JuiceScriptControl, IAutoPostBackWidget {

		private List<TabPage> _tabPages;

		public Tabs() : base("tabs") {
			_tabPages = new List<TabPage>();
		}

		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[TemplateContainer(typeof(TabPage))]
		public List<TabPage> TabPages { get { return this._tabPages; } }

		#region Widget Options

		/// <summary>
		/// Additional Ajax options to consider when loading tab content (see $.ajax).
		/// Reference: http://jqueryui.com/demos/tabs/#ajaxOptions
		/// </summary>
		[WidgetOption("ajaxOptions", null)]
		[Category("Behavior")]
		[DefaultValue(null)]
		[Description("Additional Ajax options to consider when loading tab content (see $.ajax).")]
		public string AjaxOptions { get; set; }

		/// <summary>
		/// Whether or not to cache remote tabs content, e.g. load only once or with every click. Cached content is being lazy loaded, e.g once and only once for the first click. Note that to prevent the actual Ajax requests from being cached by the browser you need to provide an extra cache: false flag to ajaxOptions.
		/// Reference: http://jqueryui.com/demos/tabs/#cache
		/// </summary>
		[WidgetOption("cache", false)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Whether or not to cache remote tabs content, e.g. load only once or with every click. Cached content is being lazy loaded, e.g once and only once for the first click. Note that to prevent the actual Ajax requests from being cached by the browser you need to provide an extra cache: false flag to ajaxOptions.")]
		public bool Cache { get; set; }

		/// <summary>
		/// Set to true to allow an already selected tab to become unselected again upon reselection.
		/// Reference: http://jqueryui.com/demos/tabs/#collapsible
		/// </summary>
		[WidgetOption("collapsible", false)]
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Set to true to allow an already selected tab to become unselected again upon reselection.")]
		public bool Collapsible { get; set; }

		/// <summary>
		/// Store the latest selected tab in a cookie. The cookie is then used to determine the initially selected tab if the selected option is not defined. Requires cookie plugin, which can also be found in the development-bundle>external folder from the download builder. The object needs to have key/value pairs of the form the cookie plugin expects as options. Available options (example): { expires: 7, path: '/', domain: 'jquery.com', secure: true }. Since jQuery UI 1.7 it is also possible to define the cookie name being used via name property.
		/// Reference: http://jqueryui.com/demos/tabs/#cookie
		/// </summary>
		[WidgetOption("cookie", null)]
		[Category("Data")]
		[DefaultValue(null)]
		[Description("Store the latest selected tab in a cookie. The cookie is then used to determine the initially selected tab if the selected option is not defined. Requires cookie plugin, which can also be found in the development-bundle>external folder from the download builder. The object needs to have key/value pairs of the form the cookie plugin expects as options. Available options (example): { expires: 7, path: '/', domain: 'jquery.com', secure: true }. Since jQuery UI 1.7 it is also possible to define the cookie name being used via name property.")]
		public string Cookie { get; set; }

		/// <summary>
		/// Disables (true) or enables (false) the widget.
		/// - OR -
		/// An array containing the position of the tabs (zero-based index) that should be disabled on initialization.
		/// Reference: http://jqueryui.com/demos/tabs/#disabled
		/// </summary>
		/*
		 * This is really a one-time case specifically for the tabs widget. No other jQuery UI widgets double up on the disabled option.
		 */
		[WidgetOption("disabled", false)]
		[TypeConverter(typeof(BooleanInt32ArrayConverter))]
		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("Disables (true) or enables (false) the widget. - OR - An array containing the position of the tabs (zero-based index) that should be disabled on initialization.")]
		public dynamic Disabled { get; set; }

		/// <summary>
		/// The type of event to be used for selecting a tab.
		/// Reference: http://jqueryui.com/demos/tabs/#event
		/// </summary>
		[WidgetOption("event", "click")]
		[Category("Behavior")]
		[DefaultValue("click")]
		[Description("The type of event to be used for selecting a tab.")]
		public string Event { get; set; }

		/// <summary>
		/// Enable animations for hiding and showing tab panels. The duration option can be a string representing one of the three predefined speeds ("slow", "normal", "fast") or the duration in milliseconds to run an animation (default is "normal").
		/// Reference: http://jqueryui.com/demos/tabs/#fx
		/// </summary>
		[WidgetOption("fx", null)]
		[Category("Behavior")]
		[DefaultValue(null)]
		[Description("Enable animations for hiding and showing tab panels. The duration option can be a string representing one of the three predefined speeds (\"slow\", \"normal\", \"fast\") or the duration in milliseconds to run an animation (default is \"normal\").")]
		public string Fx { get; set; }

		/// <summary>
		/// If the remote tab, its anchor element that is, has no title attribute to generate an id from, an id/fragment identifier is created from this prefix and a unique id returned by $.data(el), for example "ui-tabs-54".
		/// Reference: http://jqueryui.com/demos/tabs/#idPrefix
		/// </summary>
		[WidgetOption("idPrefix", "ui-tabs-")]
		[Category("Layout")]
		[DefaultValue("ui-tabs-")]
		[Description("If the remote tab, its anchor element that is, has no title attribute to generate an id from, an id/fragment identifier is created from this prefix and a unique id returned by $.data(el), for example \"ui-tabs-54\".")]
		public string IdPrefix { get; set; }

		/// <summary>
		/// HTML template from which a new tab panel is created in case of adding a tab with the add method or when creating a panel for a remote tab on the fly.
		/// Reference: http://jqueryui.com/demos/tabs/#panelTemplate
		/// </summary>
		[WidgetOption("panelTemplate", "<div></div>", HtmlEncoding = true)]
		[Category("Layout")]
		[DefaultValue("<div></div>")]
		[Description("HTML template from which a new tab panel is created in case of adding a tab with the add method or when creating a panel for a remote tab on the fly.")]
		public string PanelTemplate { get; set; }

		/// <summary>
		/// Zero-based index of the tab to be selected on initialization. To set all tabs to unselected pass -1 as value.
		/// Reference: http://jqueryui.com/demos/tabs/#selected
		/// </summary>
		[WidgetOption("selected", 0)]
		[Category("Behavior")]
		[DefaultValue(0)]
		[Description("Zero-based index of the tab to be selected on initialization. To set all tabs to unselected pass -1 as value.")]
		public int Selected { get; set; }

		/// <summary>
		/// The HTML content of this string is shown in a tab title while remote content is loading. Pass in empty string to deactivate that behavior. An span element must be present in the A tag of the title, for the spinner content to be visible.
		/// Reference: http://jqueryui.com/demos/tabs/#spinner
		/// </summary>
		[WidgetOption("spinner", "<em>Loading&#8230;</em>", HtmlEncoding = true)]
		[Category("Layout")]
		[DefaultValue("<em>Loading&#8230;</em>")]
		[Description("The HTML content of this string is shown in a tab title while remote content is loading. Pass in empty string to deactivate that behavior. An span element must be present in the A tag of the title, for the spinner content to be visible.")]
		public string Spinner { get; set; }

		/// <summary>
		/// HTML template from which a new tab is created and added. The placeholders #{href} and #{label} are replaced with the url and tab label that are passed as arguments to the add method.
		/// Reference: http://jqueryui.com/demos/tabs/#tabTemplate
		/// </summary>
		[WidgetOption("tabTemplate", "<li><a href=\"#{href}\"><span>#{label}</span></a></li>", HtmlEncoding = true)]
		[Category("Layout")]
		[DefaultValue("<li><a href=\"#{href}\"><span>#{label}</span></a></li>")]
		[Description("HTML template from which a new tab is created and added. The placeholders #{href} and #{label} are replaced with the url and tab label that are passed as arguments to the add method.")]
		public string TabTemplate { get; set; }

		#endregion

		#region Widget Events

		/// <summary>
		/// This event is triggered when clicking a tab.
		/// Reference: http://jqueryui.com/demos/tabs/#select
		/// </summary>
		[WidgetEvent("show", AutoPostBack = true)]
		[Category("Action")]
		[Description("This event is triggered when clicking a tab.")]
		public event EventHandler SelectedTabChanged;

		#endregion

		protected override HtmlTextWriterTag TagKey {
			get {
				return HtmlTextWriterTag.Div;
			}
		}

		public override ControlCollection Controls {
			get {
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		protected override void OnPreRender(EventArgs e) {

			this.Controls.Clear();

			if(TabPages != null) {
				foreach(TabPage page in TabPages) {
					this.Controls.Add(page);
				}
			}

			base.OnPreRender(e);
		}

		protected override void Render(HtmlTextWriter writer) {

			this.RenderBeginTag(writer);

			writer.WriteBeginTag("ul");

			writer.Write(HtmlTextWriter.TagRightChar);

			if(TabPages != null) {
				foreach(TabPage page in TabPages) {
					writer.WriteFullBeginTag("li");

					writer.WriteBeginTag("a");
					writer.WriteAttribute("href", "#" + page.ClientID);
					writer.Write(HtmlTextWriter.TagRightChar);
					writer.Write(page.Title);
					writer.WriteEndTag("a");

					writer.WriteEndTag("li");
				}
			}

			writer.WriteEndTag("ul");

			this.RenderChildren(writer);

			this.RenderEndTag(writer);
		}
	}
}
