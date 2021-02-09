using SAPbobsCOM;

namespace Ventura.SAP.Tools.UDO.Utils
{
    public class SAPHelper
    {
        public static Company GetCompany(string companyName)
        {
            var company = new Company();
            company.Server = "LAPTOP-4BKA2N4K";
            company.DbServerType = BoDataServerTypes.dst_MSSQL2014;
            company.CompanyDB = companyName;
            company.UserName = "manager";
            company.Password = "1234";            
            return company;
        }
    }
}
