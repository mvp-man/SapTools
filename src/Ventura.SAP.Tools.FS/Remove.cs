using System;
using SAPbobsCOM;
using SAPbouiCOM;
using Ventura.SAP.Tools.CrossCutting.Utils;
using Company = SAPbobsCOM.Company;

namespace Ventura.SAP.Tools.FS
{
    public class Remove
    {
        private static int ERROR_CODE;
        private static string ERROR_MESSAGE;

        [STAThread]
        public static void Main(string[] args)
        {
            Company company = SAPHelper.GetCompany();
            if (company.Connected)
                company.Disconnect();
            int result = company.Connect();
            if (result != 0)
            {
                company.GetLastError(out ERROR_CODE, out ERROR_MESSAGE);
                Console.Error.Write($"({ERROR_CODE})-{ERROR_MESSAGE}");
                Console.Read();
                Environment.Exit(ERROR_CODE);
            }

            var udoManager = (FormattedSearches)company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
            var oRecordSet = (Recordset) company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery("select FRQ.IndexID, QRY.IntrnalKey, CAT.CategoryId from OQCN CAT, OUQR QRY, CSHS FRQ where CAT.CategoryId = QRY.QCategory and QRY.IntrnalKey = FRQ.QueryId and CAT.CatName like '%Producción%'");
            while (!oRecordSet.EoF)
            {
                int mappedIndexForm = int.Parse(oRecordSet.Fields.Item("IndexID").Value.ToString());
                if (!udoManager.GetByKey(mappedIndexForm))
                {
                    Console.WriteLine("Don't exists the match between UDO form and query");
                    continue;
                }
                Console.WriteLine("Removing match between UDO form and query: {0}", udoManager.Remove() == 0);
                oRecordSet.MoveNext();
            }
            oRecordSet.MoveFirst();
            var userQueries = (UserQueries)company.GetBusinessObject(BoObjectTypes.oUserQueries);
            while (!oRecordSet.EoF)
            {
                int queryId = int.Parse(oRecordSet.Fields.Item("IntrnalKey").Value.ToString());
                int categoryId = int.Parse(oRecordSet.Fields.Item("CategoryId").Value.ToString());
                if (!userQueries.GetByKey(queryId,categoryId))
                {
                    Console.Error.WriteLine("Don't exists the query");
                    continue;
                }
                Console.WriteLine("Removing the query: {0}", userQueries.Remove() == 0);
                oRecordSet.MoveNext();
            }
            Console.WriteLine("Finish process, press any key");
            Console.Read();
            company.Disconnect();
        }
    }
}
