using ChatGPTAI;
using ChatGPTAI.ChatGPT;
using ChatGPTAI.MirAIModules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IConfiguration config = new ConfigurationBuilder().AddJsonFile("ChatGPT.json").AddEnvironmentVariables().Build();
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(config);
        services.AddHttpClient("ChatGPT").ConfigurePrimaryHttpMessageHandler(() =>
        {
            HttpClientHandler handler = new()
            {
                AllowAutoRedirect = false,
                UseDefaultCredentials = true
            };
            return handler;
        });

        services.AddSingleton<ChatGPTClient>();   
        services.AddHostedService<MiraiManager>();
    }).Build();

await host.RunAsync();