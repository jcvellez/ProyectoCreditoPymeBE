using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.infrastructure.seguridad
{
    public class RequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            var controladorBase = descriptor.ControllerTypeInfo.BaseType.Name;
           
            if (descriptor != null && controladorBase == "BaseApiAuthController")
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "client-token",
                    Description = "JWt Cliente Token",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema() { Type = "string" },
                    Required = true,
                    Example = new OpenApiString("b6bb5b8a-1779-496b-beed-b7e8d4d09850")
                });
                //}
            }
        }
    }
}