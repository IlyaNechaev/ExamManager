using System.Net;

namespace ExamManager.Models.Response;

public class Response
{
    public HttpStatusCode status { get; set; }
}

public class BadResponse : Response
{
    public Dictionary<string, List<string>>? errors { get; set; }
}

public class JWTResponse : Response
{
    public string token { get; set; }
}

public class ExceptionResponse : Response
{
    public string exceptionType { get; set; }
    public string message { get; set; }
    public string stackTrace { get; set; }
}

public class UserDataResponse : Response
{
    public UserViewModel data { get; set; }
}

public class GroupDataResponse : Response
{
    public Group data { get; set; }
}