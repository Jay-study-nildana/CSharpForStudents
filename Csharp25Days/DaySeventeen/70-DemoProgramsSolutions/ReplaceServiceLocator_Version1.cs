// ReplaceServiceLocator.cs
using System;

namespace DI.Exercises
{
    // Legacy anti-pattern: using service locator inside business code
    /*
    public class LegacyReportService
    {
        public void Run() {
            var repo = (IReportRepository)ServiceLocator.GetService(typeof(IReportRepository));
            // ...
        }
    }
    */

    public interface IReportRepository { string GetData(); }
    public class ReportRepository : IReportRepository { public string GetData() => "data"; }

    // Refactored: constructor injection
    public class ReportService
    {
        private readonly IReportRepository _repo;
        public ReportService(IReportRepository repo) => _repo = repo;
        public string Run() => _repo.GetData();
    }

    // Composition root: register IReportRepository -> ReportRepository and ReportService is constructed by DI.
    // Note: Removing service-locator calls improves testability by making dependencies explicit;
    // register concrete ReportRepository in the composition root (Program.cs).
}