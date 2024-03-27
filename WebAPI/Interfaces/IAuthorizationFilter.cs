//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Authentication;

//namespace Lab1.Interfaces
//{
//     This is the attribute class. This allows you to 
//     decorate endpoints in your controllers with a
//     [AuthTokenRequired] attribute
//    public class AuthTokenRequiredAttribute : TypeFilterAttribute
//    {
//        public AuthTokenRequiredAttribute() : base(typeof(TokenAuthorizationFilter))
//        {
//        }
//    }

//     This is the actual code that runs the authorization. By splitting
//     the code into 2 classes, you can inject your dependencies into this class  
//    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
//    public class TokenAuthorizationFilter : Attribute, IAuthorizationFilter
//    {
//         Sample private variables, yours will look different
//        private readonly SomeSampleRepository _rep;
//        private readonly IWebHostEnvironment _environment;

//        public TokenAuthorizationFilter(SomeSampleRepository rep, IWebHostEnvironment environment)
//        {
//             This is just some sample code. This is added 
//             to show you that you can now use dependency injection
//             in this class.
//             In your code you will add the classed needed to 
//             do the proper authorization for you.
//            _rep = rep;
//            _environment = environment;
//        }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//             Here you will add your authorization code. This is just an 
//             example of the possibilities in this method. Do your
//             method the way you need it to work.

//            if (_environment.IsDevelopment())
//                return;

//             You can for example retrieve a request header and check the value
//            var token = context.HttpContext.Request.Headers["AuthToken"];
//            if (string.IsNullOrEmpty(token))
//                throw new AuthenticationException("Unauthorized");
//            if (token != "my-token")
//                throw new AuthenticationException("Unauthorized");
//        }
//    }
//}
