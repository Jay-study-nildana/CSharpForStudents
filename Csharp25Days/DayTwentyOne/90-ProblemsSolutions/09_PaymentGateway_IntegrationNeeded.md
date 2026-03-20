# Problem 09 — Payment gateway: Which tests must be integration tests?

Context
- Unit tests above mock the payment gateway to ensure fast, deterministic tests for logic (retries, notification, save).
- Some behaviors must be validated against real infrastructure (integration tests) because mocking cannot guarantee identical runtime behavior.

Which tests should be integration tests and why
1. Real payment processing (sandbox)
   - Purpose: Validate the end-to-end payment flow with the actual gateway SDK or HTTP contract.
   - What to test: Successful charge, declined card behavior, 3D Secure / redirect flows (if used), error codes mapping.
   - Why: Gateways enforce specific error codes, response shapes, and timing that mocks may not reproduce.

2. Database persistence and transactions
   - Purpose: Ensure Save + commit behavior under real DB transactions, migrations, and isolation levels.
   - What to test: Order saved after payment, rollbacks on failure, concurrency issues.
   - Why: In-memory fakes cannot emulate DB locks, transaction behavior, or SQL migrations.

3. Network resilience and timeouts
   - Purpose: Validate retry/backoff settings against real timeouts and transient network issues.
   - What to test: Behavior when gateway times out, slow responses, and circuit-breaker interactions.
   - Why: Mocked exceptions may differ from actual transient conditions.

4. Security and authentication
   - Purpose: Ensure credentials, tokens, and TLS configurations are correct.
   - What to test: Token refresh, API key rotation, sandbox credential expiry handling.
   - Why: Security settings are environment-specific and cannot safely be mocked for full assurance.

Suggested integration test strategy
- Use the gateway’s official sandbox environment and dedicated test accounts.
- Tag integration tests so CI can run them separately (e.g., nightly or on-demand) and not on every PR.
- Use test doubles only for services that are truly out-of-scope (e.g., third-party analytics).
- Keep data isolation: create unique test orders and perform clean-up; use test database schemas and disposable test containers (Docker).
- Clean-up: delete test orders where API supports it, or run isolated DB migrations that are destroyed after the test run.

Test data and cleanup
- Use environment variables or CI secrets to inject sandbox credentials.
- Ensure idempotent test data identifiers (GUIDs/time-based).
- Use retry/backoff in tests to avoid intermittent network flakiness, and fail fast with clear diagnostics to avoid false positives.

Conclusion
- Mock for fast, deterministic unit tests that validate logic.
- Use integration tests for behavior that depends on real network, SDK, or DB behavior. Schedule them separately and instrument them for reproducibility.