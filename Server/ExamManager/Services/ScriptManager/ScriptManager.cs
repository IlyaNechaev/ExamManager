using System.Diagnostics;

namespace ExamManager.Services;

public class ScriptManager
{
    IConfiguration _configuration { get; set; }
    public ScriptManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> Execute(string arguments)
    {
        var scriptPath = _configuration["Scripts:PHP:Default"];
        var processInfo = new ProcessStartInfo(scriptPath, arguments);
        processInfo.RedirectStandardOutput = true;

        using var process = new Process();
        process.StartInfo = processInfo;

        try
        {
            process.Start();
        }
        catch
        {
            process.Close();
        }
        await Task.Run(() => process.WaitForExit());

        var result = process.StandardOutput.ReadToEnd();
        return result;
    }
}

public static class ScriptManagerExtensions
{
    public static void AddScriptManager(this IServiceCollection services)
    {
        services.AddTransient<ScriptManager>(provider => new(provider.GetService<IConfiguration>()!));
    }
}

public interface IScriptHandler
{
    public Task<string> Execute(string scriptType, string arguments);
}