// UseFactoryDelegateForRuntimeParams.cs
using System;

namespace DI.Exercises
{
    public interface IReportGenerator
    {
        string Generate(object model);
    }

    // Implementation that depends on a runtime template name
    public class ReportGenerator : IReportGenerator
    {
        private readonly string _templateName;
        public ReportGenerator(string templateName) => _templateName = templateName;
        public string Generate(object model) => $"Report({_templateName})";
    }

    public static class FactoryRegistration
    {
        public static void Register(IServiceProvider sp, Action<Action<Func<string, IReportGenerator>>> register)
        {
            // Example registration (in IServiceCollection style you'd do:)
            // services.AddTransient<Func<string, IReportGenerator>>(sp => template => new ReportGenerator(template));
        }
    }

    // Usage:
    // Func<string, IReportGenerator> factory = sp.GetRequiredService<Func<string, IReportGenerator>>();
    // var generator = factory("InvoiceTemplate");
    // var output = generator.Generate(model);
    //
    // Explanation: Factory delegate (Func<string,IReportGenerator>) supplies runtime parameters without service-locator.
}