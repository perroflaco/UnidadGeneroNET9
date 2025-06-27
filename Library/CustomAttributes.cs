using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace SEV.Library
{
    public class Alias : System.Attribute
    {
        String[] _names;
        public Alias(params String[] names)
        {
            Names = names;
        }

        public String[] Names
        {
            get { return _names; }
            set { _names = value; }
        }
    }

    public class MustTrim : System.Attribute
    {
        private Boolean _mustTrim = false;

        public MustTrim()
        {
            _mustTrim = true;
        }
    }

    public class DataParamName : System.Attribute
    {
        private String _Name = "";

        public DataParamName(String name = null){
            Name = name;
        }

        public String Name{
            get{ return _Name;}
            set{ _Name = value;}
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HALEntityBase : ActionFilterAttribute
    {
        protected string _EntityName { get; set; }

        public String Entity
        {
            get { return _EntityName; }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            IActionResult ActionResult = context.Result;
            HALResponseBase  CurrentValue = (HALResponseBase) (((ObjectResult)ActionResult).Value);
            CurrentValue.Build(_EntityName,context);
        }
    }

    public class HALWithEntity : HALEntityBase
    {
        public HALWithEntity(String EntityName)
        {
            _EntityName = EntityName;
        }
    }
    public class HALWithoutEntity : HALEntityBase
    {
        public HALWithoutEntity()
        {
            _EntityName = "Default";
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string FullReferer = context.HttpContext.Request.Headers["Referer"].ToString();
            string Origin = context.HttpContext.Request.Headers["Origin"].ToString();
            int HostPosition = FullReferer.IndexOf(Origin);
            bool IsBlankReferer = string.IsNullOrEmpty(FullReferer);
            bool IsBlankOrigin = string.IsNullOrEmpty(Origin);
            bool IsHostInsideOrigin = ( HostPosition < 0 || HostPosition > 10 );
            bool IsCapacitorProtocol = Origin.StartsWith("capacitor");
            bool IsIonicProtocol = Origin.StartsWith("ionic");

            bool FilterMustDeny (TypeLess Value) { return IsBlankReferer || IsHostInsideOrigin || IsBlankOrigin; };
            bool FilterIsIonicMobileApp (TypeLess Value){ return IsCapacitorProtocol || IsIonicProtocol; };

            void GrantAccess(){/*Let pass the current request*/};
            void DenyAccess() 
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Result = new ContentResult{Content = $"No direct connections allowed"};
            }

            ActionDictionary<TypeLess>.
                Create().
                    ExclusivelyMode(). // Enable just one execution
                        When(FilterIsIonicMobileApp, GrantAccess).
                        When(FilterMustDeny, DenyAccess).
                        Default(GrantAccess).
                Evaluate(new TypeLess());
        }
    }
}