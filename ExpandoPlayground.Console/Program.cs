// See https://aka.ms/new-console-template for more information
using ExpandoPlayground;
using Microsoft.Extensions.DependencyInjection;

Startup startup = new();

startup.ConfigureServices(new ServiceCollection());
