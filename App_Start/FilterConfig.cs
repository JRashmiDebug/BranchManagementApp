using BranchManagementApp.Models;
using System.Web.Mvc;

namespace BranchManagementApp.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new BasicAuthenticationAttribute());
        }
    }
}