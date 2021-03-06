#pragma checksum "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ec168d87fa2e89fc2f74853823b88d03cd23e96e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_DoingWhat_Index), @"mvc.1.0.view", @"/Views/DoingWhat/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/DoingWhat/Index.cshtml", typeof(AspNetCore.Views_DoingWhat_Index))]
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
#line 1 "C:\Users\merve\Desktop\PMP\PMPCore\Views\_ViewImports.cshtml"
using PMPCore;

#line default
#line hidden
#line 2 "C:\Users\merve\Desktop\PMP\PMPCore\Views\_ViewImports.cshtml"
using PMPCore.Models;

#line default
#line hidden
#line 6 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
using PMPDAL.Entities;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ec168d87fa2e89fc2f74853823b88d03cd23e96e", @"/Views/DoingWhat/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4fb46c8f1508255f860685ecf7a4631592682f6c", @"/Views/_ViewImports.cshtml")]
    public class Views_DoingWhat_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<PMPDAL.Models.DoingWhatViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
  
    ViewBag.Title = "Kim Ne Yapıyor";
    ViewBag.ActivePageName = "Kim Ne Yapıyor?";

#line default
#line hidden
#line 7 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
  
    var activePerson = ViewBag.ActivePerson as Person;
    if (activePerson.IsAdmin == 0)
    {

        

#line default
#line hidden
            BeginContext(281, 86, false);
#line 12 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
   Write(Html.Raw("<script>window.location = '" + @Url.Action("Index", "Home") + "';</script>"));

#line default
#line hidden
            EndContext();
#line 12 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                                                               
    }

#line default
#line hidden
            BeginContext(379, 1747, true);
            WriteLiteral(@"<style>
    .checkbox label, .radio label, label, .label-on-left, .label-on-right {
        color: white;
    }

    .pagination > li > a, .pagination > li > span {
        color: white;
    }

    th {
        font-size: 18px;
    }

    td {
        font-size: 18px;
    }

    .table > tbody > tr {
        background-color: #07a0b1;
    }

    .table-striped > tbody > tr:nth-of-type(odd) {
        background-color: #54bcc8;
    }
</style>
<div class=""container-fluid"">
    <!-- AKTİF GÖREVLER BURADAN BAŞLIYOR -->
    <div class=""col-md-12 mb-5 pb-5"">
        <div class=""card""
             style=""background: #486d89; color: white; box-shadow: 0px 0px 9px 2px #48484896; border-radius: 10px;"">


            <div class=""card-content"">
                <h3 class=""card-title"" style=""color: white;""><b>KİM NE YAPIYOR</b></h3>
                <hr>
                <div id=""divGorevListesi"">
                    <table class=""table table-striped table-no-bordered table-hover dataTable ");
            WriteLiteral(@"dtr-inline"">
                        <thead>
                            <tr style=""background-color: #304a5e;"">
                                <th>Ad&nbsp;Soyad</th>
                                <th style=""width: 90px;"">Takımı</th>
                                <th style=""width: 90px;"">İterasyon</th>
                                <th style=""width: 150px;"">Milestone</th>
                                <th>Görev</th>
                                <th style=""width: 140px;"">Başlangıç&nbsp;Tarihi</th>
                                <th style=""width: 200px;"">Kaç&nbsp;Saattir&nbsp;Çalışıyor</th>

                            </tr>
                        </thead>
                        <tbody>
");
            EndContext();
#line 65 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                              
                                foreach (var item in Model)
                                {

#line default
#line hidden
            BeginContext(2254, 135, true);
            WriteLiteral("                                    <tr style=\"padding-top: 10px;\">\r\n                                        <td style=\"height: 30px;\">");
            EndContext();
            BeginContext(2390, 49, false);
#line 69 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                             Write(Html.Raw(item.NameSurname.Replace(" ", "&nbsp;")));

#line default
#line hidden
            EndContext();
            BeginContext(2439, 73, true);
            WriteLiteral("</td>\r\n                                        <td style=\"height: 30px;\">");
            EndContext();
            BeginContext(2513, 13, false);
#line 70 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                             Write(item.TeamName);

#line default
#line hidden
            EndContext();
            BeginContext(2526, 73, true);
            WriteLiteral("</td>\r\n                                        <td style=\"height: 30px;\">");
            EndContext();
            BeginContext(2600, 48, false);
#line 71 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                             Write(Html.Raw(item.SprintName.Replace(" ", "&nbsp;")));

#line default
#line hidden
            EndContext();
            BeginContext(2648, 73, true);
            WriteLiteral("</td>\r\n                                        <td style=\"height: 30px;\">");
            EndContext();
            BeginContext(2722, 18, false);
#line 72 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                             Write(item.MileStoneName);

#line default
#line hidden
            EndContext();
            BeginContext(2740, 73, true);
            WriteLiteral("</td>\r\n                                        <td style=\"height: 30px;\">");
            EndContext();
            BeginContext(2814, 13, false);
#line 73 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                             Write(item.StepName);

#line default
#line hidden
            EndContext();
            BeginContext(2827, 73, true);
            WriteLiteral("</td>\r\n                                        <td style=\"height: 30px;\">");
            EndContext();
            BeginContext(2901, 43, false);
#line 74 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                             Write(item.StartDate.ToString("dd/MM/yyyy HH:mm"));

#line default
#line hidden
            EndContext();
            BeginContext(2944, 73, true);
            WriteLiteral("</td>\r\n                                        <td style=\"height: 30px;\">");
            EndContext();
            BeginContext(3018, 24, false);
#line 75 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                                             Write(item.KacSaattirCalisiyor);

#line default
#line hidden
            EndContext();
            BeginContext(3042, 50, true);
            WriteLiteral("</td>\r\n                                    </tr>\r\n");
            EndContext();
#line 77 "C:\Users\merve\Desktop\PMP\PMPCore\Views\DoingWhat\Index.cshtml"
                                }
                            

#line default
#line hidden
            BeginContext(3158, 839, true);
            WriteLiteral(@"                        </tbody>
                    </table>
                </div>
            </div>

            <!-- end content-->
        </div>
        <!--  end card  -->
    </div>
    <!-- AKTİF GÖREVLER BURADA BİTİYOR -->
</div>
            </div>


<script>
$(document).ready(function(){
                  $('#divGorevListesi table').DataTable( {
                            ""language"": {
                                ""url"": ""http://cdn.datatables.net/plug-ins/1.10.19/i18n/Turkish.json""
                            },
                             ""initComplete"": function(settings, json) {
                                            $("".dataTables_wrapper select option"").css(""background-color"", ""#191919"");
                                        }
                        });
       });</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<PMPDAL.Models.DoingWhatViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
