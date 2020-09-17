using System.Web.Mvc;

namespace ZMCMSv2.Areas.ZMLisSys
{
    public class ZMLisSysAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ZMLisSys";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ZMLisSys_default",
                "ZMLisSys/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "ZMLisSys.Controllers" }
            );
        }
    }
}