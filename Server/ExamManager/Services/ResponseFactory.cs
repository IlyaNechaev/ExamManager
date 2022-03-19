using ExamManager.Models.Response;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace ExamManager.Services;

public static class ResponseFactory
{
    /// <summary>
    /// Создает пустой <see cref="Response"/>
    /// </summary>
    public static Response CreateResponse()
    {
        return new Response
        {
            status = HttpStatusCode.OK
        };
    }
    public static Response CreateResponse(ModelStateDictionary modelState)
    {
        Response response = null;

        if (!modelState.IsValid)
        {
            response = new BadResponse
            {
                status = HttpStatusCode.BadRequest,
                errors = CreateDictionary(modelState)
            };
        }
        else
        {
            response = new Response
            {
                status = HttpStatusCode.OK
            };
        }

        return response;
    }
    public static Response CreateResponse(string jwtToken)
    {
        return new JWTResponse
        {
            status = HttpStatusCode.OK,
            token = jwtToken
        };
    }

    private static Dictionary<string, List<string>> CreateDictionary(ModelStateDictionary modelState)
    {
        var errors = new Dictionary<string, List<string>>();
        foreach (var error in modelState)
        {
            if (error.Value.Errors.Count > 0)
            {
                errors.Add(error.Key, error.Value.Errors.Select(msg => msg.ErrorMessage).ToList());
            }
        }

        return errors;
    }
}
