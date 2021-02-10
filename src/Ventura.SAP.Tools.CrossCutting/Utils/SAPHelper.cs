using SAPbobsCOM;

namespace Ventura.SAP.Tools.CrossCutting.Utils
{
    public static class SAPHelper
    {
        public static Company GetCompany()
        {
            var company = new Company();
            company.Server = "LAPTOP-4BKA2N4K";
            company.DbServerType = BoDataServerTypes.dst_MSSQL2014;
            company.CompanyDB = "SBO_POLYARQ_PROD";
            company.UserName = "manager";
            company.Password = "1234";            
            return company;
        }
    }
}
