using System;
using SAPbobsCOM;
using Ventura.SAP.Tools.CrossCutting.Utils;

namespace Ventura.SAP.Tools.UDT
{
    public class Remove
    {
        private static int ERROR_CODE;
        private static string ERROR_MESSAGE;

        static void Main(string[] args)
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

            var udoManager = (UserTablesMD)company.GetBusinessObject(BoObjectTypes.oUserTables);
            for (int i = 0; i < args.Length; i++)
            {
                var item = args[i];
                if (!udoManager.GetByKey(item))
                {
                    Console.WriteLine("Don't exists the UDO {0}", item);
                    continue;
                }
                udoManager.Remove();
            }
            Console.Read();
            company.Disconnect();
        }
    }
}
