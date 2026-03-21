// 04-Facade_UserOnboarding.cs
// Intent: UserOnboardingFacade orchestrates multiple subsystems to onboard a user with a single method.
// DI/Lifetime: Facade may be Scoped/Transient. If it triggers DB operations, ensure the underlying UoW/DbContext is Scoped.
// Testability: Inject fake NotificationFacade and repository to test orchestration without external I/O.

using System;
using System.Collections.Generic;

// Simple DTO for onboarding
public class UserDto { public string Email { get; set; } public string DisplayName { get; set; } }

public interface IUserRepository { void AddUser(UserDto user); }
public interface ITaskScheduler { void ScheduleTask(string userEmail, string taskName); }

public class UserOnboardingFacade
{
    private readonly IUserRepository _repo;
    private readonly NotificationFacade _notifications;
    private readonly IMetrics _metrics;
    private readonly ITaskScheduler _scheduler;

    public UserOnboardingFacade(IUserRepository repo, NotificationFacade notifications, IMetrics metrics, ITaskScheduler scheduler)
    {
        _repo = repo; _notifications = notifications; _metrics = metrics; _scheduler = scheduler;
    }

    // Public high-level API
    public void Onboard(UserDto user)
    {
        // Step 1: persist user
        _repo.AddUser(user);

        // Step 2: send welcome notification
        _notifications.SendNotification(user.Email, "Welcome", $"Welcome {user.DisplayName}");

        // Step 3: record metric
        _metrics.Increment("user.onboarded");

        // Step 4: schedule initial tasks
        _scheduler.ScheduleTask(user.Email, "SendWelcomeSurvey");
    }
}

/*
Transaction considerations:
- If persistence and other steps must be atomic, consider using a UnitOfWork and performing Commit() after all steps,
  or perform compensation logic if later steps fail.
*/