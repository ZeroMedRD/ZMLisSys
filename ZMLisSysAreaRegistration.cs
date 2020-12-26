using System.Web.Mvc;

namespace ZMCMSv2.Areas.ZMLISSys
{
    public class ZMLISSysAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ZMLISSys";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ZMLISSys_default",
                "ZMLISSys/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "ZMLISSys.Controllers" }
            );
        }
    }
}