#pragma checksum "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "45c76d117749995cd691d9617d82c78153380be1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Administrator_Views_User_Invoice), @"mvc.1.0.view", @"/Areas/Administrator/Views/User/Invoice.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Administrator/Views/User/Invoice.cshtml", typeof(AspNetCore.Areas_Administrator_Views_User_Invoice))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\_ViewImports.cshtml"
using Maple2.AdminLTE.Uil;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"45c76d117749995cd691d9617d82c78153380be1", @"/Areas/Administrator/Views/User/Invoice.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f8c0b256960fef22c94cbde626e0e0db070e1f05", @"/Areas/Administrator/Views/_ViewImports.cshtml")]
    public class Areas_Administrator_Views_User_Invoice : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Maple2.AdminLTE.Uil.Areas.Administrator.Models.InvoiceModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/report/invoice.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
  
    ViewData["Title"] = "Invoice";
    Layout = null;

#line default
#line hidden
            BeginContext(131, 8, true);
            WriteLiteral("<html>\r\n");
            EndContext();
            BeginContext(139, 94, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "45c76d117749995cd691d9617d82c78153380be15043", async() => {
                BeginContext(145, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(151, 73, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "45c76d117749995cd691d9617d82c78153380be15428", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(224, 2, true);
                WriteLiteral("\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(233, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(235, 2378, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "45c76d117749995cd691d9617d82c78153380be17644", async() => {
                BeginContext(241, 249, true);
                WriteLiteral("\r\n    <div class=\"invoice-box\">\r\n        \r\n        <table cellpadding=\"0\" cellspacing=\"0\">\r\n            <tr class=\"top\">\r\n                <td colspan=\"2\">\r\n                    <table>\r\n                        <tr>\r\n                            <td>\r\n");
                EndContext();
#line 19 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                                 if (ViewBag.QRCodeImage != null)
                                {

#line default
#line hidden
                BeginContext(592, 40, true);
                WriteLiteral("                                    <img");
                EndContext();
                BeginWriteAttribute("src", " src=\"", 632, "\"", 658, 1);
#line 21 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
WriteAttributeValue("", 638, ViewBag.QRCodeImage, 638, 20, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(659, 43, true);
                WriteLiteral(" alt=\"\" style=\"height:70px;width:70px\" />\r\n");
                EndContext();
#line 22 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                                }

#line default
#line hidden
                BeginContext(737, 114, true);
                WriteLiteral("                            </td>\r\n                            <td>\r\n\r\n                                Invoice #: ");
                EndContext();
                BeginContext(852, 12, false);
#line 26 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                                      Write(Model.Number);

#line default
#line hidden
                EndContext();
                BeginContext(864, 48, true);
                WriteLiteral("\r\n                                <br> Created: ");
                EndContext();
                BeginContext(913, 32, false);
#line 27 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                                         Write(DateTime.Now.ToShortDateString());

#line default
#line hidden
                EndContext();
                BeginContext(945, 44, true);
                WriteLiteral("\r\n                                <br> Due: ");
                EndContext();
                BeginContext(990, 44, false);
#line 28 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                                     Write(DateTime.Now.AddDays(20).ToShortDateString());

#line default
#line hidden
                EndContext();
                BeginContext(1034, 339, true);
                WriteLiteral(@"
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class=""information "">
                <td colspan=""2 "">
                    <table>
                        <tr>
                            <td>
                                ");
                EndContext();
                BeginContext(1374, 17, false);
#line 39 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                           Write(Model.Seller.Name);

#line default
#line hidden
                EndContext();
                BeginContext(1391, 38, true);
                WriteLiteral("<br>\r\n                                ");
                EndContext();
                BeginContext(1430, 17, false);
#line 40 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                           Write(Model.Seller.Road);

#line default
#line hidden
                EndContext();
                BeginContext(1447, 38, true);
                WriteLiteral("<br>\r\n                                ");
                EndContext();
                BeginContext(1486, 20, false);
#line 41 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                           Write(Model.Seller.Country);

#line default
#line hidden
                EndContext();
                BeginContext(1506, 103, true);
                WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                ");
                EndContext();
                BeginContext(1610, 16, false);
#line 44 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                           Write(Model.Buyer.Name);

#line default
#line hidden
                EndContext();
                BeginContext(1626, 38, true);
                WriteLiteral("<br>\r\n                                ");
                EndContext();
                BeginContext(1665, 16, false);
#line 45 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                           Write(Model.Buyer.Road);

#line default
#line hidden
                EndContext();
                BeginContext(1681, 38, true);
                WriteLiteral("<br>\r\n                                ");
                EndContext();
                BeginContext(1720, 19, false);
#line 46 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                           Write(Model.Buyer.Country);

#line default
#line hidden
                EndContext();
                BeginContext(1739, 337, true);
                WriteLiteral(@"
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class=""heading "">
                <td>
                    Item
                </td>
                <td>
                    Price
                </td>
            </tr>
");
                EndContext();
#line 60 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
             foreach (var item in Model.Items)
            {

#line default
#line hidden
                BeginContext(2139, 85, true);
                WriteLiteral("                <tr class=\"item\">\r\n                    <td>\r\n                        ");
                EndContext();
                BeginContext(2225, 9, false);
#line 64 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                   Write(item.Name);

#line default
#line hidden
                EndContext();
                BeginContext(2234, 81, true);
                WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        $ ");
                EndContext();
                BeginContext(2316, 10, false);
#line 67 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                     Write(item.Price);

#line default
#line hidden
                EndContext();
                BeginContext(2326, 52, true);
                WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
                EndContext();
#line 70 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
            }

#line default
#line hidden
                BeginContext(2393, 109, true);
                WriteLiteral("            <tr class=\"total \">\r\n                <td></td>\r\n                <td>\r\n                    Total: ");
                EndContext();
                BeginContext(2503, 29, false);
#line 74 "D:\Learning\Repositories\AdminLTE-Maple2-Master\trunk\src\Areas\Administrator\Views\User\Invoice.cshtml"
                      Write(Model.Items.Sum(i => i.Price));

#line default
#line hidden
                EndContext();
                BeginContext(2532, 74, true);
                WriteLiteral("\r\n                </td>\r\n            </tr>\r\n        </table>\r\n    </div>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2613, 17, true);
            WriteLiteral("\r\n</html>\r\n\r\n\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Maple2.AdminLTE.Uil.Areas.Administrator.Models.InvoiceModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
