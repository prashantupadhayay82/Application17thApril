/// Author: Kevin Zink of BrightMix.com
/// 
/// This class is derived from ActionResult. It is meant to be used in conjunction with an Ajax call, as it returns Prototype-based javascript. 
/// The concept is modeled after Ruby on Rails' RJS framework (rendering javascript from the server) as well as Rails javascript/prototype 
/// generator http://api.rubyonrails.org/classes/ActionView/Helpers/PrototypeHelper/JavaScriptGenerator/GeneratorMethods.html
/// 
/// Example Usage in your Controller:
/// 
///     public ActionResult HideElement(string elementId)
///     {
///         RjsResult r = new RjsResult();
///         r.Effect(elementId, RjsResult.EffectTypes.Hide, null);
///     }
/// 
/// A more complicated but powerful example of returning the contents of two UserControls and inserting them into two different <div>'s
/// 
///     public ActionResult InsertUserControl()
///     {
///         ObjectViewData viewData = new ObjectViewData { SomeString = "dude", SomeNumber = 1 };
///
///         var r = new RjsResult();
///         r.Insert("div_one", RjsResult.Positions.Top, "~/Views/Controls/UserControl1.ascx", ObjectViewData, ControllerContext);
///         r.Insert("div_two", RjsResult.Positions.Bottom, "~/Views/Controls/UserControl2.ascx", ObjectViewData, ControllerContext);
///
///         return r;
///     }
/// 

namespace System.Web.Mvc
{
    using System;
    using System.Text;
    using System.Web;
    using System.Collections.Generic;
    using System.Security.Policy;
    using System.Web.Script.Serialization;
    using MvcContrib.UI;
    using System.Web.Mvc.Html;
    using System.IO;

    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class RjsResult : ActionResult
    {
        #region Constructors
        public RjsResult(bool setContentType)
        {
            Actions = new List<IRjsActionBase>();
            this.SetContentType = setContentType;
        }

        public RjsResult()
            : this(false)
        {
        }
        #endregion

        #region Private Vars
        private bool SetContentType { get; set; }

        private List<IRjsActionBase> Actions
        {
            get;
            set;
        }

        #endregion

        #region Rjs Actions
        public interface IRjsActionBase
        {
            string Render();
        }

        private class RjsUpdateAction : IRjsActionBase
        {
            public RjsUpdateAction(string element, string contents)
            {
                this.element = element;
                this.contents = contents;
            }

            public string element { get; set; }
            public string contents { get; set; }

            #region IRjsActionBase Members

            public string Render()
            {
                if (!contents.StartsWith("\""))
                    contents = "\"" + contents + "\"";

                //return string.Format("Element.update(\"{0}\", {1});", element, contents);
                return string.Format("$(\'#{0}\').html({1});", element, contents);
            }

            #endregion
        }

        private class RjsInsertAction : IRjsActionBase
        {
            public RjsInsertAction(string element1, string element2, Positions p)
            {
                this.element1 = element1;
                this.element2 = element2;
                this.position = p;
            }

            public RjsInsertAction(string element, Positions p, string contents)
            {
                this.element1 = element;
                this.contents = contents;
                this.position = p;
            }

            public string element1 { get; set; }
            public string element2 { get; set; }
            public string contents { get; set; }
            public Positions position { get; set; }

            #region IRjsActionBase Members

            public string Render()
            {
                if (!contents.StartsWith("\""))
                    contents = "\"" + contents + "\"";

                if (!string.IsNullOrEmpty(element2) && string.IsNullOrEmpty(contents))
                {
                    //$("p").insertAfter("#foo");
                    return string.Format("$(\"#{0}\").{1}(\"#{2}\");", element1, position, element2);
                }
                else
                {
                    //$("p").after("<b>Hello</b>");
                    return string.Format("$(\"#{0}\").{1}({2});", element1, position, contents);
                }
            }

            #endregion
        }

        private class RjsStringAction : IRjsActionBase
        {
            public string contents { get; set; }

            public RjsStringAction(string contents)
            {
                this.contents = contents;
            }

            #region IRjsActionBase Members

            public string Render()
            {
                return this.contents;
            }

            #endregion
        }

        private class RjsShowAction : IRjsActionBase
        {
            public RjsShowAction(string element)
            {
                this.element = element;
            }
            public string element { get; set; }


            #region IRjsActionBase Members

            public string Render()
            {
                return string.Format("$(\"#{0}\").show();", element);
            }

            #endregion
        }

        private class RjsAlertAction : IRjsActionBase
        {
            public RjsAlertAction(string message)
            {
                this.message = message;
            }
            public string message { get; set; }


            #region IRjsActionBase Members

            public string Render()
            {
                return string.Format("alert('{0}');", message);
            }

            #endregion
        }

        private class RjsHideAction : IRjsActionBase
        {
            public RjsHideAction(string element)
            {
                this.element = element;
            }
            public string element { get; set; }


            #region IRjsActionBase Members

            public string Render()
            {
                return string.Format("$(\"#{0}\").hide();", element);
            }

            #endregion
        }

        private class RjsRemoveAction : IRjsActionBase
        {
            public RjsRemoveAction(string element)
            {
                this.element = element;
            }
            public string element { get; set; }


            #region IRjsActionBase Members

            public string Render()
            {
                return string.Format("$(\"#{0}\").remove();", element);
            }

            #endregion
        }

        private class RjsEffectAction : IRjsActionBase
        {

            public string element { get; set; }
            public EffectTypes effect { get; set; }
            public Dictionary<string, string> options { get; set; }
            public string effectBehaviour { get; set; }

            public RjsEffectAction(string element, EffectTypes effect, KeyValuePair<string, string>[] options)
            {
                this.options = new Dictionary<string, string>();

                if (options != null)
                {
                    foreach (KeyValuePair<string, string> pair in options)
                        this.options.Add(pair.Key, pair.Value);
                }


                this.element = element;
                this.effect = effect;
            }

            //public RjsEffectAction(string element, EffectTypes effect,string effectBehaviour)
            //{

            //    if (!string.IsNullOrEmpty(effectBehaviour))
            //    {
            //        this.effectBehaviour = "'" + effectBehaviour + "'";
            //    }
            //    this.element = element;
            //    this.effect = effect;
            //}

            #region IRjsActionBase Members

            public string Render()
            {
                //string ret = string.Format("$(\'#{1}\').{0}({2});", effect.ToString(), element,options);
                StringBuilder ret = new StringBuilder();
                foreach (string key in options.Keys)
                {
                    ret.Append(string.Format("$(\'#{1}\').{0}(", effect.ToString(), element));
                    //ret += ", {";


                    ret.Append(string.Format("'{0}','{1}'", key.ToLower(), options[key]));
                    ret.Append(");");
                }

                return ret.ToString();

            }

            #endregion
        }

        /// <summary>
        /// Call me from your controller and pass me your ControllerContext and I'll render a UserControl for you and return the contents as a string!
        /// </summary>
        /// <param name="userControl"></param>
        /// <param name="viewData"></param>
        /// <param name="controllerContext"></param>
        /// <returns></returns>
        private string RenderPartialToString(string userControl, ViewDataDictionary viewData, ControllerContext controllerContext)
        {
            HtmlHelper h = new HtmlHelper(new ViewContext(controllerContext, new WebFormView(controllerContext,userControl), viewData, new TempDataDictionary(), controllerContext.HttpContext.Response.Output), new ViewPage());

            var blockRenderer = new BlockRenderer(controllerContext.HttpContext);

            var r = new RjsResult();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 5000000;
            string s = blockRenderer.Capture(
                () => RenderPartialExtensions.RenderPartial(h, userControl, viewData)
            );
            /*ViewPage vp = new ViewPage();
            vp.ViewData = viewData;
            System.Web.UI.Control c = vp.LoadControl(userControl);
            vp.Controls.Add(c);
            StringBuilder sb = new StringBuilder();
            using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
            {
                using(System.Web.UI.HtmlTextWriter tw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    vp.RenderControl(tw);
                }
            }
            string s = sb.ToString();*/
            return serializer.Serialize(s);
        }
        #endregion

        #region Fun Enums
        public enum Positions
        {
            append = 1,
            prepend,
            appendTo,
            prependTo,
            after,
            before,
            insertBefore,
            insertAfter
        }

        public enum EffectTypes
        {
            Appear = 1,
            Fade,
            SlideDown,
            SlideUp,
            Highlight,
            hide,
            toggle,
            show,
            css
        }
        #endregion

        #region Rjs Methods
        /// <summary>
        /// Renders a user control and inserts the contents into specified element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        /// <param name="userControl"></param>
        /// <param name="viewData"></param>
        /// <param name="controllerContext"></param>
        public void Insert(string element, Positions p, string userControl, ViewDataDictionary viewData, ControllerContext controllerContext)
        {
            Insert(element, p, RenderPartialToString(userControl, viewData, controllerContext));
        }

        /// <summary>
        /// Inserts content into specific element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        /// <param name="contents"></param>
        public void Insert(string element, Positions p, string contents)
        {
            Actions.Add(new RjsInsertAction(element, p, contents));
        }

        public void Insert(string element1, string element2, Positions p)
        {
            Actions.Add(new RjsInsertAction(element1, element2, p));
        }

        /// <summary>
        /// Update a part of the page by rendering a user control
        /// </summary>
        /// <param name="element"></param>
        /// <param name="userControl"></param>
        /// <param name="viewData"></param>
        /// <param name="controllerContext"></param>
        public void Update(string element, string userControl, ViewDataDictionary viewData, ControllerContext controllerContext)
        {
            Update(element, RenderPartialToString(userControl, viewData, controllerContext));
        }

        public void Update(string element, string contents)
        {
            Actions.Add(new RjsUpdateAction(element, contents));
        }

        /// <summary>
        /// Alert some text
        /// </summary>
        /// <param name="message"></param>
        public void Alert(string message)
        {
            Actions.Add(new RjsAlertAction(message));
        }

        /// <summary>
        /// Render some javascript code.. whatever you want
        /// </summary>
        /// <param name="contents"></param>
        public void RenderString(string contents)
        {
            Actions.Add(new RjsStringAction(contents));
        }

        /// <summary>
        /// Shows an element
        /// </summary>
        /// <param name="element"></param>
        public void Show(string element)
        {
            Actions.Add(new RjsShowAction(element));
        }

        /// <summary>
        /// Hides an element
        /// </summary>
        /// <param name="element"></param>
        public void Hide(string element)
        {
            Actions.Add(new RjsHideAction(element));
        }

        /// <summary>
        /// Delets an element
        /// </summary>
        /// <param name="element"></param>
        public void Remove(string element)
        {
            Actions.Add(new RjsRemoveAction(element));
        }

        /// <summary>
        /// Calls an effect of type EffectTypes, also takes array of prototype options
        /// </summary>
        /// <param name="element"></param>
        /// <param name="effect"></param>
        /// <param name="options"></param>
        public void Effect(string element, EffectTypes effect, KeyValuePair<string, string>[] options)
        {
            Actions.Add(new RjsEffectAction(element, effect, options));
        }

        //public void Effect(string element, EffectTypes effect, string effectBehaviour)
        //{
        //    Actions.Add(new RjsEffectAction(element, effect, effectBehaviour));
        //}

        #endregion

        #region ActionResult Override
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (this.SetContentType)
                response.ContentType = "application/javascript";

            string result = string.Empty;

            foreach (IRjsActionBase action in Actions)
            {
                result += action.Render();
            }


            response.Write(result);
        }
        #endregion
    }
}