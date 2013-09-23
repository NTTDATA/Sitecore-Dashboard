using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Sitecore.Dashboard.Models
{
    public abstract class WidgetModel
    {
        /// <summary>
        /// Widget parameters
        /// </summary>
        public NameValueCollection Parameters { get; set; }

        /// <summary>
        /// Populate the widget model with data
        /// </summary>
        public abstract void Initialize();
    }
}
