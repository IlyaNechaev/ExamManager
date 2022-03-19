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