using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Bison.Razor.Tests;

// This class is a fixture that will be used for tests
// it is a fixture class, something xUnit uses to
// be able to set enviroments up before every test.
// this class will be used to run the Bison program 
// and shut it down once done. It can also share context
// between test classes
// source: https://xunit.net/docs/shared-context

public class BisonFixture : IAsyncLifetime
{
    private Process? _app;
    public string BaseUrl { get; } = "http://localhost:5123";
    private readonly StringBuilder _output = new();

    // Starts once iniated 
    public async Task InitializeAsync() {
        // launch the app with base url
        var psi = new ProcessStartInfo("dotnet",
        $"run --project ../../../../../src/Bison.Razor --no-build --no-launch-profile --urls {BaseUrl}")
        {
            UseShellExecute = false,
            RedirectStandardError = true,
        };

        // Fixes keys
        psi.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        psi.Environment["Authentication__GitHub__ClientId"] = "test-client-id";
        psi.Environment["Authentication__GitHub__ClientSecret"] = "test-client-secret";

        _app = Process.Start(psi)!;
 
        // check if its actually running 
        // it might be slow in the beginning 
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        for (var i = 0; i < 60; i++){
            if (_app.HasExited)
                throw new Exception("App crashed on startup:\n" + await _app.StandardError.ReadToEndAsync());

            try
            {
                var res = await http.GetAsync(BaseUrl);
                if (res.IsSuccessStatusCode) return;
            }
            // dont do anything if excption is thrown and wait
            catch (HttpRequestException) {}
            catch (TaskCanceledException) {}
            await Task.Delay(500);
        }
        // if app didnt startup
        throw new Exception($"App did not respond at {BaseUrl} within 30 seconds. Output:\n{_output}");
    }

    // called once done
    public Task DisposeAsync() {
        if (_app is { HasExited: false }) {
            _app.Kill(entireProcessTree: true);
        } 
        return Task.CompletedTask;
    }
}