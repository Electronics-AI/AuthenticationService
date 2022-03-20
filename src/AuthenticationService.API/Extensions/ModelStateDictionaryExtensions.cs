using System.Collections.Generic;
using System.Linq;
using AuthenticationService.API.Models.Responses.Error;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthenticationService.API.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static IEnumerable<string> SelectAllErrors(
            this ModelStateDictionary modelState
            )
        {
            IEnumerable<string> errorMessages = modelState.Values.SelectMany(
                    values => values.Errors.Select(error => error.ErrorMessage) 
                );
            
            return errorMessages;
        }
    }
}