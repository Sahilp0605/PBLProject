using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class DocPrinterSettings
    {
        public Int64 pkID { get; set; }
        public Int64 CompanyID { get; set; }
        public string FormatType { get; set; }
        public string Header_Spacing { get; set; }
        public string Header_FontName { get; set; }
        public Int64 Header_FontSize { get; set; }
        public Boolean Header_QR { get; set; }
        public Int64 Header_QR_Position { get; set; }
        public string Header_QR_Size { get; set; }
        public Boolean Header_Company { get; set; }
        public Int64 Header_Company_Position { get; set; }
        public Boolean Introduction_Show { get; set; }
        public Boolean Introduction_BeforePageBreak { get; set; }
        public Boolean Introduction_AfterPageBreak { get; set; }
        public Boolean ProdDetail_Show { get; set; }
        public string ProdDetail_Spacing { get; set; }
        public Boolean ProdDetail_BeforePageBreak { get; set; }
        public Boolean ProdDetail_AfterPageBreak { get; set; }
        public Boolean ProdDetail_WithSpecification { get; set; }
        public Boolean ProdDetail_WithAssembly { get; set; }
        public Boolean ProdDetail_WithImage { get; set; }
        public string ProdDetail_Image_Size { get; set; }
        public string Footer_Spacing { get; set; }
        public Boolean Footer_PageNo { get; set; }
        public Boolean Footer_PrintDate { get; set; }
        public string Footer_FontName { get; set; }
        public Int64 Footer_FontSize { get; set; }

        public string PrintingMargin_Plain { get; set; }
        public string PrintingMargin_WithHeader { get; set; }
        public Int64 ProdDetail_Lines { get; set; }

    }
}
