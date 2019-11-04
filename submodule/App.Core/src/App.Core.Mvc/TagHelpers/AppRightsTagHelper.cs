using App.Core.Business.Services;
using App.Core.Common.Extensions;
using App.Core.Security;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;

namespace App.Core.Mvc.TagHelpers
{
    /// <summary>
    /// app-rights-entity - указываем название сущности (i.e. OrgDeparment)
    /// app-rights-property - название свойства/поля сущности(i.e.Name)
    /// app-rights - если указан только атрибут без значений:
    ///	- entity будет присвоено значение по умолчанию = ИмяКонтроллера, если тег находится внутри внешнео тега, где было указано app-rights-entity(например на теге формы), тогда будет использовано значение внешнего тега.
    ///	- property будет писвоено значение текущего аттрибута Name, в случае его отсутствия - значение по умолчанию = "".
    /// - если для родительского элемента установлен атрибут app-rights-entity, тогда для всех вложенных элементов типа "input", "select" будет также атрибут app-rights.
    /// </summary>
    [HtmlTargetElement(Attributes = "app-rights")]
    [HtmlTargetElement(Attributes = "app-rights-entity")]
    [HtmlTargetElement(Attributes = "app-rights-property")]
    public class AppRightsTagHelper : TagHelper
    {
        private readonly IUserInfoService _userInfoService;

        public AppRightsTagHelper(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [HtmlAttributeName("app-rights-entity")]
        public string Entity { get; set; }

        [HtmlAttributeName("app-rights-property")]
        public string Property { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((Entity==null) &&
                (Property == null) &&
                (output.TagName != "input") &&
                (output.TagName != "a") &&
                (output.TagName != "select"))
                return;

            if (Entity == null)
            {
                if (context.Items.TryGetValue("app-rights-entity", out var entity))
                    Entity = entity.ToString();
                else
                {
                    if (!output.Attributes.ContainsName("app-rights") && (Property == null))
                        return;
                    else
                        Entity = ViewContext.RouteData.Values["Controller"].ToString();
                }
            }
            else
            {
                context.Items.Add("app-rights-entity", Entity);
            }

            if (Entity == null) {
                return;
            }

            // TODO: delete
            //return;

            var property = Property ?? output.Attributes.FirstOrDefault(x => x.Name == "name")?.Value?.ToString() ?? "";

            #region NewRightsCheck
            var userInfo = _userInfoService.GetCurrentUserInfo();
            var accessLevel = userInfo.Rights == null 
                ? AccessLevel.No 
                : userInfo.Rights.GetFieldRight(Entity, property);
            // TODO: TEST CAREFULLY
            if (accessLevel == AccessLevel.No) {
                output.SuppressOutput();
            }
            else if (accessLevel == AccessLevel.Read) {
                var tag = output.TagName;
                output.Attributes.SetAttribute("readonly", "readonly");

                if (tag == "input") // handle datepicker/daterangepicker
                {
                    var classes = output.Attributes.FirstOrDefault(a => a.Name == "class")?.Value?.ToString();
                    if (classes != null) {
                        if (classes.Contains("control-datepicker"))
                            output.Attributes.SetAttribute("class", classes.Replace("control-datepicker", ""));
                        if (classes.Contains("control-daterangepicker"))
                            output.Attributes.SetAttribute("class", classes.Replace("control-daterangepicker", ""));
                    }
                }

                if (tag == "a") {
                    var href = output.Attributes.FirstOrDefault(a => a.Name == "href")?.Value?.ToString().ToLower();

                    if (href != null && href.Contains(Entity + "/delete", StringComparison.InvariantCultureIgnoreCase)) {
                        output.SuppressOutput();
                    }

                    if (href != null && href.Contains(Entity + "/edit", StringComparison.InvariantCultureIgnoreCase)) {
                        output.SuppressOutput();
                    }
                }
            }
            #endregion

            base.Process(context, output);
        }
    }
}
