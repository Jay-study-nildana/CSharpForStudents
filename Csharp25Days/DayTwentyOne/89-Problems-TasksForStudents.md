# Unit testing & test doubles — 10 Problems

Instructions
- For each problem below, write unit tests (using xUnit + Moq) and/or refactor the production code so it becomes easily testable.
- Indicate which dependencies should be mocked, faked, or tested via integration tests.
- Where appropriate, use the Arrange–Act–Assert pattern and demonstrate verification of interactions.

Problems

01_Process_SuccessfulPayment_SavesOrder
- Implement tests for an OrderProcessor that charges a payment gateway, saves an order to a repository, and sends a confirmation email on success. Verify that Save and the confirmation email are called exactly once.

02_Process_PaymentFails_SendsFailureEmail
- Implement tests for the same OrderProcessor to ensure when payment fails it does not save the order and it sends a failure email.

03_Process_ThrowsOnNullOrder
- Test that OrderProcessor.Process throws ArgumentNullException when passed a null order and ArgumentException when card token is null/empty. Verify no side-effects (no repository save and no email sent).

04_TimeDependent_DiscountViaIClock
- Create a small ordering scenario where a discount is applied only if the order is placed before noon (UTC). Abstract time via IClock. Write tests that use a fake IClock to assert discount applied/not applied.

05_Retry_OnTransientPaymentFailures
- Implement a PaymentRetrier that calls IPaymentGateway.Charge and retries up to N times for transient errors (simulated via thrown exceptions or false return) before failing. Write tests that verify the retry behavior using Moq.SetupSequence or exceptions.

06_FileLogger_UsesIFileWriter
- Refactor a FileLogger that previously wrote directly to System.IO into a form that depends on IFileWriter. Write a unit test that verifies the correct file contents are written, using an IFileWriter mock or in-memory fake.

07_BackgroundJob_EnqueuesMessage
- Given a BackgroundJob runner that processes orders and enqueues messages to IQueueClient, write a test to verify the correct message payload is enqueued and the queue API is called once. Demonstrate a spy-like verification.

08_AuthenticationService_HashesPassword
- Implement tests for an AuthenticationService that hashes passwords via IPasswordHasher and saves users via IUserRepository. Use a stub for the hasher and verify repository.Save was called with the hashed password.

09_PaymentGateway_IntegrationNeeded
- For the above payment scenarios, write a one-page test-plan (plain English) explaining which tests must be integration tests (with real payment gateway sandbox or DB) and why. Include suggested test data and cleanup strategy.

10_Refactor_AvoidStaticClockAndEnv
- Refactor a component that uses DateTime.Now and Environment.GetEnvironmentVariable directly into a design that uses IClock and IConfiguration (or an IEnvironment abstraction). Write tests that confirm behavior changes when environment/config is changed, using injected fakes/mocks.

Deliverables
- One Markdown file that lists the 10 problems above (this file).
- Ten solution files whose names match the problem titles.