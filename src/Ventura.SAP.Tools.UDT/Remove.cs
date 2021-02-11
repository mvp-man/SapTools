using System;
using SAPbobsCOM;
using Ventura.SAP.Tools.CrossCutting.Utils;

namespace Ventura.SAP.Tools.UDT
{
    public class Remove
    {
        private static int ERROR_CODE;
        private static string ERROR_MESSAGE;
        private static int RESULT_FUNCTIONS;

        static void Main(string[] args)
        {
            Company company = SAPHelper.GetCompany();
            if (company.Connected)
                company.Disconnect();
            RESULT_FUNCTIONS = company.Connect();
            if (RESULT_FUNCTIONS != 0)
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
                    Console.WriteLine("Don't exists the UDT {0}", item);
                    continue;
                }
                RESULT_FUNCTIONS = udoManager.Remove();
                Console.WriteLine("Removing UDT: {0} | Result: {1}", item, RESULT_FUNCTIONS == 0);                
                if(RESULT_FUNCTIONS != 0)
                {
                    company.GetLastError(out ERROR_CODE, out ERROR_MESSAGE);
                    Console.Error.WriteLine($"Error for removing: ({ERROR_CODE})-{ERROR_MESSAGE}");
                }
            }
            Console.WriteLine("Finish process");
            Console.Read();
            company.Disconnect();
        }
    }
}
