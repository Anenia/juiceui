﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Juice_Sample_Site {
	public partial class Accordion : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			_Postback.Click += delegate(object s, EventArgs ea) {
				var o = _Accordion.Animated;
			};
		}
	}
}