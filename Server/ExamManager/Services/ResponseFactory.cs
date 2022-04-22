using ExamManager.Models;
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
    public static Response CreateResponse(Exception ex)
    {
        return new ExceptionResponse
        {
            exceptionType = ex.GetType().Name,
            status = HttpStatusCode.BadRequest,
            message = ex.Message,
            stackTrace = ex.StackTrace
        };
    }

    public static Response CreateResponse(User user)
    {
        if (user is null)
        {
            return new UserDataResponse
            {
                status = HttpStatusCode.BadRequest,
                id = null,
                firstName = null,
                lastName = null,
                role = null
            };
        }

        return new UserDataResponse
        {
            status = HttpStatusCode.OK,
            id = user.ObjectID,
            firstName = user.FirstName,
            lastName = user.LastName,
            role = user.Role
        };
    }

    public static Response CreateResponse(Group group)
    {
        if (group is null)
        {
            return new GroupDataResponse
            {
                status = HttpStatusCode.BadRequest,
                id = null,
                name = null,
                studentsCount = null
            };
        }

        return new GroupDataResponse
        {
            status = HttpStatusCode.OK,
            id = group.ObjectID,
            name = group.Name,
            studentsCount = group.Students?.Count()
        };
    }

    public static Response CreateResponse(IEnumerable<User> users, string groupName = null)
    {
        if (users is null)
        {
            return new UsersDataResponse
            {
                status = HttpStatusCode.BadRequest,
                users = null
            };
        }

        return new UsersDataResponse
        {
            status = HttpStatusCode.OK,
            users = users.Select(u =>
            new UsersDataResponse.UserView
            {
                id = u.ObjectID.ToString(),
                firstName = u.FirstName,
                lastName = u.LastName,
                groupName = groupName
            }).ToArray()
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
