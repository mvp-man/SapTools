using SAPbobsCOM;
using System;
using Ventura.SAP.Tools.UDO.Utils;

namespace Ventura.SAP.Tools.UDO
{
    public class Remove
    {
        private static int ERROR_CODE;
        private static string ERROR_MESSAGE;

        [STAThread]
        public static void Main(string[] args)
        {
            var company = SAPHelper.GetCompany("SBO_POLYARQ_PROD");
            if (company.Connected)
                company.Disconnect();
            var result = company.Connect();
            if (result != 0)
            {
                company.GetLastError(out ERROR_CODE, out ERROR_MESSAGE);
                Console.Error.Write(string.Format("({0})-{1}", ERROR_CODE, ERROR_MESSAGE));
                Console.Read();
                Environment.Exit(ERROR_CODE);
            }

            var udoManager = (UserObjectsMD)company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            for (int i = 0; i < args.Length; i++)
            {
                var item = args[i];
                if (!udoManager.GetByKey(item))
                {
                    Console.WriteLine("Don't exists the UDO {0}", item);
                    continue;
                }
                Console.Write("Eliminando la tabla: {0} | Resultado: {1}", item, udoManager.Remove() == 0);
            }
            Console.WriteLine("Proceso finalizado");
            Console.Read();
            company.Disconnect();
        }
    }
}
