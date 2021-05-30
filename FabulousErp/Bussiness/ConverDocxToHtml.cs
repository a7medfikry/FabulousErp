using SautinSoft.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FabulousErp.Bussiness
{
    public class ConverDocxToHtml
    {
        public static void ConvertFromFile()
        {
            string inpFile = @"D:\Khaled\FabulousErp_LastFromAtef\FabulousErp\PrintDocument\نموذج02.docx";
            string outFile = @"D:\Khaled\FabulousErp_LastFromAtef\FabulousErp\PrintDocument\Result.html";

            DocumentCore dc = DocumentCore.Load(inpFile);
            dc.Save(outFile);
            
            // Open the result for demonstration purposes.
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outFile) { UseShellExecute = true });
        }
    }
}