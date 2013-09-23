using System;
using System.Collections.Specialized;
using System.Web.Http;
using Sitecore.Dashboard.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Reflection;
using Sitecore.Diagnostics;
using Sitecore.Web;

namespace Sitecore.Dashboard.Controllers
{
    public class WidgetsController : ApiController
    {
        /// <summary>
        /// Returns JSON-serialized model for the given widget item ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetById(Guid id)
        {
            // Get Sitecore item from ID and validate that is is a widget
            Database coreDb = Sitecore.Configuration.Factory.GetDatabase("core");
            Item widgetItem = coreDb.GetItem(ID.Parse(id));
            Assert.IsNotNull(widgetItem, "widgetItem");
            Assert.IsTrue(widgetItem.TemplateID.ToString() == Constants.TemplateIDs.Widget, "Item is not based on the Widget template");

            // Get Widget Type
            string widgetTypeId = widgetItem.GetFieldValueOrDefault(Constants.FieldIDs.WidgetType);
            Assert.IsNotNullOrEmpty("widgetTypeId", "Widget type not selected");
            Item widgetType = coreDb.GetItem(ID.Parse(widgetTypeId));
            Assert.IsNotNull(widgetType, "widgetType");

            // Get Model Type signature
            string modelType = widgetType.GetFieldValueOrDefault(Constants.FieldIDs.WidgetTypeModelType);
            Assert.IsNotNullOrEmpty("modelType", "Model Type not specified");

            // Instantiate and return model object
            object model = ReflectionUtil.CreateObject(modelType, new object[] { });
            Assert.IsNotNull(model, "model");
            Assert.IsTrue(model is WidgetModel, "Widget model must inherit Sitecore.Dashboard.Model.WidgetModel");
            try
            {
                // Pass parameters to model
                string widgetParams = widgetItem.GetFieldValueOrDefault(Constants.FieldIDs.WidgetParameters);
                (model as WidgetModel).Parameters = WebUtil.ParseUrlParameters(widgetParams);

                ReflectionUtil.CallMethod(model, "Initialize");
                try
                {
                    return model.ToJson();
                }
                catch (Exception ex)
                {
                    Log.Error("Error serializing widget model to JSON", this);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error initializing widget", ex, this);
            }
            return "";
        }
    }
}