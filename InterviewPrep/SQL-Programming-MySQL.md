# SQL Programming — SQL Basics, MySQL In-Depth, Database Design & Data Analysis, Cross-Platform Installation Guides  
Interview & Developer Reference Guide

---

## Table of Contents

1. [Scope & Purpose](#scope--purpose)  
2. [Quick Version & Ecosystem Notes](#quick-version--ecosystem-notes)  
3. [SQL Basics: Concepts & Syntax](#sql-basics-concepts--syntax)  
   - What is SQL?  
   - Relational model primitives (tables, rows, columns, keys)  
   - Basic DDL & DML statements (CREATE, ALTER, DROP, INSERT, UPDATE, DELETE)  
   - SELECT fundamentals: projection, filtering, ordering, limiting  
4. [Core Query Techniques](#core-query-techniques)  
   - Joins (INNER, LEFT/RIGHT OUTER, FULL OUTER, CROSS)  
   - Aggregation & GROUP BY, HAVING  
   - Subqueries (scalar, correlated, IN/EXISTS)  
   - Set operations (UNION / INTERSECT / EXCEPT)  
   - Window functions and ranking (ROW_NUMBER, RANK, PARTITION BY)  
   - Common Table Expressions (CTEs) & recursive queries  
5. [Data Modeling & Database Design](#data-modeling--database-design)  
   - ER modelling and normalization (1NF, 2NF, 3NF, BCNF)  
   - Denormalization and when to use it  
   - Keys: primary, foreign, unique, surrogate vs natural keys  
   - Referential integrity and cascading actions  
   - Data types & choosing them correctly  
   - Index strategies and covers (B-tree vs hash vs full-text)  
6. [MySQL In-Depth](#mysql-in-depth)  
   - MySQL architecture overview (storage engines: InnoDB vs MyISAM, NDB)  
   - InnoDB internals: clustered index, MVCC, buffering, purge/undo, redo log  
   - Configuration and important my.cnf settings (innodb_buffer_pool_size, innodb_log_file_size, max_connections, query_cache (deprecated), sort_buffer_size, tmp_table_size)  
   - Transaction isolation levels and locking (READ UNCOMMITTED..SERIALIZABLE)  
   - Transactions, atomicity, and common pitfalls (lost updates, phantom reads)  
   - Replication basics (asynchronous, semi-sync, row vs statement based)  
   - Partitioning and sharding concepts (range, hash, list partitions)  
   - Full-text search, spatial types, JSON support, generated columns  
7. [Query Optimization & Execution Plans](#query-optimization--execution-plans)  
   - EXPLAIN / EXPLAIN ANALYZE (MySQL 8) interpretation (type, possible_keys, key_len, ref, rows, Extra)  
   - Using ANALYZE TABLE, OPTIMIZE TABLE, and statistics  
   - Index usage: covering indexes, composite index ordering, prefix indexes, index-only scans  
   - SARGability and functions-on-columns pitfalls  
   - Query rewrites, join ordering, and avoiding filesorts/temp tables  
   - Profiling queries and slow query logging  
8. [Stored Programming & Advanced SQL Features](#stored-programming--advanced-sql-features)  
   - Views and materialized view patterns (MySQL lacks built-in materialized views)  
   - Stored procedures, functions, triggers — lifecycle and use-cases  
   - User-defined functions (UDFs) and security considerations  
   - Event scheduler for scheduled jobs  
9. [Data Analysis with SQL](#data-analysis-with-sql)  
   - Analytical aggregates and windowing patterns (moving averages, running totals)  
   - Time-series analysis techniques and best practices  
   - Data cleansing, normalization, and ETL basics in SQL  
   - Using GROUPING SETS, ROLLUP, CUBE for multidimensional aggregates  
   - Approximate queries and sampling strategies for large data sets  
   - Exporting/importing data: CSV, JSON, LOAD DATA INFILE, SELECT ... INTO OUTFILE  
10. [Backup, Restore & High Availability](#backup-restore--high-availability)  
    - Logical backups (mysqldump), physical backups (XtraBackup, mysqlbackup)  
    - Point-in-time recovery using binary logs (binlog)  
    - Replication topologies for HA: master-slave, multi-source, group replication, InnoDB Cluster  
    - Backup strategies & RPO/RTO considerations  
11. [Security & Access Control](#security--access-control)  
    - Authentication plugins, password policies, TLS for connections  
    - GRANT/REVOKE and privilege best practices (least privilege)  
    - Auditing, logging, and encryption-at-rest options  
    - Protecting against SQL injection — parameterized queries & ORM usage  
12. [Cross-Platform Installation Guides (MySQL & Tools)](#cross-platform-installation-guides-mysql--tools)  
    - Linux (Debian/Ubuntu, RHEL/CentOS) step-by-step apt/yum/dnf commands  
    - macOS (Homebrew) installation and launch agents  
    - Windows installer and ZIP distribution notes, Services, PATH, root password setup  
    - Docker-based MySQL quickstart and volumes for persistence  
    - Connecting clients: mysql client, MySQL Workbench, DBeaver, adminer, CLI examples  
    - Installing related tools: Percona XtraBackup, pt-utilities (Percona Toolkit), ProxySQL, Orchestrator  
13. [Cross-Platform Configuration & Operations Examples](#cross-platform-configuration--operations-examples)  
    - Basic security hardening and disabling remote root login  
    - Change bind-address, configure character sets (utf8mb4), timezone, tmpdir  
    - Creating users and granting privileges safely (examples)  
    - Configure replication (steps for setting up source and replica)  
14. [Migration, Data Import/Export & ETL Tools](#migration-data-importexport--etl-tools)  
    - Tools: mydumper/myloader, pt-table-sync, AWS DMS, Flyway, Liquibase  
    - Schema migration strategies: no-downtime deployments, online schema change (gh-ost, pt-online-schema-change)  
15. [Monitoring, Observability & Troubleshooting](#monitoring-observability--troubleshooting)  
    - Metrics to monitor (QPS, TPS, slow queries, connections, buffer pool hit ratio, replication lag)  
    - Using Prometheus exporters, Grafana dashboards, Percona Monitoring and Management (PMM)  
    - Common diagnostics: InnoDB deadlocks, long-running transactions, table locks, binary log issues  
16. [Cloud-managed MySQL & Alternatives](#cloud-managed-mysql--alternatives)  
    - RDS / Aurora (Amazon), Cloud SQL (GCP), Azure Database for MySQL — pros/cons and managed features  
    - MariaDB, Percona Server, and compatibility notes  
17. [Best Practices & Design Guidelines](#best-practices--design-guidelines)  
18. [Common Mistakes & Anti-Patterns](#common-mistakes--anti-patterns)  
19. [Comprehensive Q&A — Developer & Interview Questions (with answers)](#comprehensive-qa--developer--interview-questions-with-answers)  
20. [Practical Exercises & Projects](#practical-exercises--projects)  
21. [Cheat Sheet & Useful Commands / SQL Snippets](#cheat-sheet--useful-commands--sql-snippets)  
22. [References & Further Reading](#references--further-reading)

---

## 1. Scope & Purpose

This guide is a consolidated reference for SQL programming and database engineering focusing on SQL basics, deep MySQL knowledge, database design and data analysis, and practical cross-platform installation/configuration guidance. It's intended for interviews, day-to-day DBA/dev tasks, or learning paths for full-stack and data engineers.

---

## 2. Quick Version & Ecosystem Notes

- MySQL 8.x is the current major version with improved features: window functions, CTEs, JSON enhancements, roles, UTF8MB4 as default, better optimizer.
- Percona Server and MariaDB are commonly used MySQL-compatible alternatives with different features/performance behaviors.
- Always verify feature compatibility and defaults for the target engine/version (e.g., default authentication plugin changed in MySQL 8).

---

## 3. SQL Basics: Concepts & Syntax

Core DDL examples:
```sql
CREATE TABLE customers (
  customer_id INT AUTO_INCREMENT PRIMARY KEY,
  email VARCHAR(255) NOT NULL UNIQUE,
  name VARCHAR(200),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

ALTER TABLE customers ADD COLUMN phone VARCHAR(30);

DROP TABLE obsolete_table;
```

Core DML examples:
```sql
INSERT INTO customers (email, name) VALUES ('alice@example.com','Alice');
UPDATE customers SET name = 'Alice Smith' WHERE customer_id = 1;
DELETE FROM customers WHERE created_at < '2020-01-01';
```

Basic SELECT:
```sql
SELECT customer_id, email, name
FROM customers
WHERE email LIKE '%@example.com'
ORDER BY created_at DESC
LIMIT 10 OFFSET 0;
```

Use parameterized queries from application code (prepared statements) to prevent SQL injection.

---

## 4. Core Query Techniques

Joins:
```sql
-- inner join
SELECT o.id, c.name, o.total
FROM orders o
JOIN customers c ON o.customer_id = c.customer_id;

-- left join
SELECT p.id, i.id AS invoice_id
FROM payments p
LEFT JOIN invoices i ON p.external_id = i.external_id;
```

Aggregation:
```sql
SELECT customer_id, COUNT(*) AS orders_count, SUM(total) AS total_spent
FROM orders
GROUP BY customer_id
HAVING SUM(total) > 1000;
```

Window functions:
```sql
SELECT order_id, customer_id, total,
       SUM(total) OVER (PARTITION BY customer_id ORDER BY order_date ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS running_total
FROM orders;
```

CTE & recursive:
```sql
WITH RECURSIVE nums AS (
  SELECT 1 AS n
  UNION ALL
  SELECT n+1 FROM nums WHERE n < 10
)
SELECT * FROM nums;
```

Subquery example:
```sql
SELECT name FROM customers WHERE customer_id IN (SELECT customer_id FROM orders WHERE total > 100);
```

Set operations:
```sql
SELECT col FROM a
UNION
SELECT col FROM b; -- removes duplicates

SELECT col FROM a
UNION ALL
SELECT col FROM b; -- keeps duplicates
```

---

## 5. Data Modeling & Database Design

Normalization levels:
- 1NF: atomic column values.
- 2NF: no partial dependency on part of a composite PK.
- 3NF: remove transitive dependencies.
- BCNF: stronger constraint for candidate keys.

When to denormalize:
- Read-heavy workloads where joins are expensive; use denormalized columns or materialized aggregates.

Keys:
- Use surrogate keys (INT AUTO_INCREMENT) if natural keys are large or mutable.
- Prefer UNIQUE constraints for natural uniqueness (email, sku), but enforce with caution on nullable columns — MySQL treats NULLs specially.

Design tips:
- Prefer narrow primary keys for InnoDB (clustered index includes PK).
- For InnoDB choose a small, stable primary key (numeric) to reduce secondary index bloat.
- Use appropriate data types (e.g., DATE/TIMESTAMP vs VARCHAR for dates; DECIMAL for currency).

Constraints:
- Use CHECK constraints (MySQL 8 enforces them) where appropriate, but do not rely only on them for complex business logic.

---

## 6. MySQL In-Depth

Storage engines:
- InnoDB: transactional, MVCC, row-level locking — recommended for OLTP.
- MyISAM: non-transactional, table-level locking — legacy, supports full-text in older versions.
- NDB: distributed, used for clustering in MySQL Cluster.

Important configuration:
- innodb_buffer_pool_size: set to ~60–80% of available RAM for dedicated DB servers.
- innodb_log_file_size: balance between recovery time and long transactions.
- innodb_flush_log_at_trx_commit: 1 for durability, 2/0 for performance (risk).
- max_connections: set according to workload and OS limits.

Transactions & isolation:
- Default InnoDB isolation is REPEATABLE READ in MySQL with gap-lock and next-key locking behavior; be aware of phantom prevention semantics.
- Use SELECT ... FOR UPDATE to lock rows in transactions when updating.

Replication:
- Statement-based vs Row-based vs Mixed binlog formats — row-based is more reliable for replication correctness.
- Keep server_id unique per instance.
- Monitor replication delay and configure semi-sync if necessary.

JSON & generated columns:
```sql
ALTER TABLE t ADD COLUMN metadata JSON;
ALTER TABLE t ADD COLUMN full_name VARCHAR(255) GENERATED ALWAYS AS (CONCAT(json_unquote(json_extract(metadata,'$.first')),' ',json_unquote(json_extract(metadata,'$.last')))) STORED;
```

Security note: avoid enabling `local_infile` unless necessary — it can be exploited if not controlled.

---

## 7. Query Optimization & Execution Plans

Using EXPLAIN:
```sql
EXPLAIN FORMAT=JSON SELECT ...;
-- key fields: id, select_type, table, partitions, type, possible_keys, key, key_len, ref, rows, Extra
```

Index best practices:
- Create composite indexes matching WHERE/ORDER BY usage: `(col1, col2)` helps queries with `WHERE col1 = ? AND col2 = ?` and `ORDER BY col1, col2`.
- Leftmost prefix rule in composite indexes.
- Avoid indexing low-cardinality columns (e.g., boolean) unless part of composite index that improves selectivity.
- Covering index: include all referenced columns in the index to allow index-only scan.

SARGability:
- Avoid wrapping indexed columns in functions (e.g., `WHERE DATE(col) = '2025-01-01'`) — use range or computed/stored columns.

Avoid filesort/temp table:
- ORDER BY different than index order or grouping may cause filesort; examine EXPLAIN Extra for "Using filesort" or "Using temporary".

Slow query log:
- Enable `slow_query_log` and tune `long_query_time` to capture problematic queries.

---

## 8. Stored Programming & Advanced SQL Features

Stored procedures example:
```sql
DELIMITER $$
CREATE PROCEDURE add_order(IN c_id INT, IN amt DECIMAL(10,2))
BEGIN
  INSERT INTO orders (customer_id, total) VALUES (c_id, amt);
END$$
DELIMITER ;
```

Triggers:
- Useful for auditing but can cause cascading effects and performance overhead. Use judiciously.

Functions:
- Create scalar functions when you need reusable SQL logic — but these can be slow if called per-row; consider inlining or rewriting as JOIN logic.

Materialized view pattern:
- Simulate materialized views by creating a table with precomputed aggregates and schedule refresh via event scheduler or an application job.

---

## 9. Data Analysis with SQL

Windowing patterns:
- Running totals:
```sql
SELECT order_date, amount,
       SUM(amount) OVER (ORDER BY order_date ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS running_total
FROM sales;
```

Time-series:
- Use proper indexing on timestamp and symbol columns; partition by time for huge tables.
- Use `GENERATE_SERIES` equivalents or CTEs for filling gaps.

GROUPING SETS / ROLLUP:
```sql
SELECT region, product, SUM(sales) FROM sales
GROUP BY region, product WITH ROLLUP;
```

ETL:
- Use staging tables to clean and validate raw data before merging into production tables.
- Use `LOAD DATA INFILE` for high-throughput bulk loads.

Analytical DB considerations:
- For heavy analytics, consider columnar or OLAP stores, or using separate analytics clusters (BigQuery, Redshift, ClickHouse).

---

## 10. Backup, Restore & High Availability

Logical backups:
- `mysqldump --single-transaction --routines --triggers --events --databases dbname > db.sql` for InnoDB consistent dump.

Physical backups:
- Percona XtraBackup for hot physical backups of InnoDB without downtime.

Point-in-time recovery:
- Keep binary logs and use `mysqlbinlog` to replay events up to point.

Replication & HA:
- Group Replication and InnoDB Cluster provide multi-primary/auto-failover options in MySQL ecosystem.
- Use ProxySQL or HAProxy for connection routing and proxying.

Test restore procedures regularly and verify backups.

---

## 11. Security & Access Control

Create least-privilege users:
```sql
CREATE USER 'app'@'apphost' IDENTIFIED BY 'strongpass';
GRANT SELECT, INSERT, UPDATE ON db.* TO 'app'@'apphost';
FLUSH PRIVILEGES;
```

Disable remote root login and use strong passwords. Use `ALTER USER` to set password expiry or validation plugin.

Enable TLS:
- Create server certs and configure `mysqld` with SSL parameters; require `REQUIRE SSL` for sensitive users.

Audit & logging:
- Enable general/slow logs in dev; use audit plugins for compliance requirements.

---

## 12. Cross-Platform Installation Guides (MySQL & Tools)

Linux (Debian/Ubuntu):
```bash
# Debian/Ubuntu (apt)
sudo apt update
sudo apt install mysql-server
# Secure installation
sudo mysql_secure_installation
# Start and enable
sudo systemctl enable --now mysql
# Login
sudo mysql -u root -p
```

RHEL/CentOS (dnf/yum):
```bash
sudo dnf install @mysql
# or download from MySQL repo
sudo dnf module disable mysql
sudo dnf install mysql-community-server
sudo systemctl enable --now mysqld
sudo grep 'temporary password' /var/log/mysqld.log
sudo mysql_secure_installation
```

macOS (Homebrew):
```bash
brew update
brew install mysql
brew services start mysql
mysql_secure_installation
```

Windows (Installer):
- Download MySQL Installer MSI from dev.mysql.com, follow GUI steps, configure root user and Windows service. Alternatively use ZIP distribution and configure `my.ini`.

Docker quickstart:
```bash
docker run --name mysql8 -e MYSQL_ROOT_PASSWORD=ChangeMe -p 3306:3306 -v mysql-data:/var/lib/mysql -d mysql:8.0
# Connect
docker exec -it mysql8 mysql -uroot -p
```

Tools:
- MySQL Workbench (GUI), DBeaver (multi-DB GUI), Adminer (single-file PHP), `mysql` CLI.

Install Percona tools:
- Percona Toolkit via package manager or download; XtraBackup from Percona repository.

---

## 13. Cross-Platform Configuration & Operations Examples

Disable remote root login and create admin user:
```sql
ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY 'pass';
# better: remove root@'%' and use socket auth for local root
CREATE USER 'dba'@'%' IDENTIFIED BY 'strong';
GRANT ALL PRIVILEGES ON *.* TO 'dba'@'%' WITH GRANT OPTION;
FLUSH PRIVILEGES;
```

Set UTF8MB4 in my.cnf:
```ini
[mysqld]
character-set-server = utf8mb4
collation-server = utf8mb4_unicode_ci
innodb_file_per_table = 1
```

Configure replication (basic steps):
- On master:
```sql
SHOW MASTER STATUS;
GRANT REPLICATION SLAVE ON *.* TO 'repl'@'%' IDENTIFIED BY 'slavepass';
FLUSH TABLES WITH READ LOCK;
-- record File and Position from SHOW MASTER STATUS
UNLOCK TABLES;
```
- On replica:
```sql
CHANGE MASTER TO MASTER_HOST='master-host', MASTER_USER='repl', MASTER_PASSWORD='slavepass', MASTER_LOG_FILE='mysql-bin.000001', MASTER_LOG_POS=  12345;
START SLAVE;
SHOW SLAVE STATUS\G
```

---

## 14. Migration, Data Import/Export & ETL Tools

Tools & patterns:
- `mysqldump` for logical export/import.
- `mydumper` for parallel logical dumps.
- `pt-online-schema-change` / `gh-ost` for online schema changes without long locks.
- Use Flyway or Liquibase for versioned migrations in CI/CD.
- Use AWS DMS for cross-engine migrations or continuous replication during migration.

Schema changes:
- Avoid `ALTER TABLE ... MODIFY` on huge tables without online change tools.
- Break large migrations into small steps: add nullable column, backfill in batches, set NOT NULL.

---

## 15. Monitoring, Observability & Troubleshooting

Key metrics:
- Connections, Threads_running, QPS, TPS
- Aborted connections/errors
- Slow queries count and top queries
- InnoDB buffer pool hit ratio: `SHOW STATUS LIKE 'innodb_buffer_pool_read%';`
- Replication lag: `Seconds_Behind_Master` from `SHOW SLAVE STATUS\G`

Tools:
- Prometheus mysqld_exporter + Grafana dashboards
- Percona Monitoring and Management (PMM)
- Datadog/instrumentation for cloud monitoring

Common troubleshooting:
- Deadlocks: check `SHOW ENGINE INNODB STATUS\G` for latest deadlock trace.
- Long-running transactions: check `INFORMATION_SCHEMA.INNODB_TRX`.
- Disk full: check `/var/lib/mysql` and `innodb_log_file` sizes.
- Restart issues: inspect error log (mysqld.log).

---

## 16. Cloud-managed MySQL & Alternatives

Managed services:
- Amazon RDS MySQL & Aurora: automated backups, read replicas, failover. Aurora offers higher performance and distributed storage.
- Google Cloud SQL: managed MySQL with automated backups and maintenance.
- Azure Database for MySQL: managed offering on Azure.

Alternatives:
- MariaDB: fork of MySQL with additional features (Galera cluster).
- Percona Server: performance and observability enhancements.
- For analytics: consider column-store (ClickHouse, Snowflake), or cloud DWH for large-scale analytics.

---

## 17. Best Practices & Design Guidelines

- Model based on access patterns: optimize schema and indexes for common queries.
- Use transactions for consistency and ensure proper isolation where needed.
- Use connection pooling in apps; avoid opening/closing many connections.
- Instrument and monitor from day one.
- Version control schema migrations; test in staging.
- Prefer prepared statements and ORMs that parameterize queries to avoid SQL injection.
- Implement backup and restore drills and document RTO/RPO.
- Document retention policies and archival strategies.

---

## 18. Common Mistakes & Anti-Patterns

- Over-indexing (many indexes slow writes).
- Indexing every column or using too-wide primary keys.
- Using TEXT/CLOB for frequently searched columns (use proper types).
- Relying on AUTOCOMMIT implicit behavior without considering transactions.
- Performing schema changes directly on production without testing (causing long locks).
- Storing multiple values in a single column (comma-separated lists) — violates normalization.
- Ignoring character set issues (UTF-8 vs utf8mb4 misconfigs).

---

## 19. Comprehensive Q&A — Developer & Interview Questions (with answers)

Q1: What is the difference between a clustered and non-clustered index?  
A: InnoDB uses clustered index where the table data is stored with the primary key; secondary indexes reference primary key values. Non-clustered indexes are separate structures mapping keys to row identifiers.

Q2: Explain ACID properties.  
A: Atomicity, Consistency, Isolation, Durability — guarantees for reliable transactions.

Q3: When would you use `EXPLAIN` vs `ANALYZE`?  
A: EXPLAIN shows the optimizer's plan; EXPLAIN ANALYZE (or MySQL 8's `EXPLAIN ANALYZE`) executes and shows actual timing and row counts, helpful for real-world performance analysis.

Q4: How do you prevent duplicate inserts in concurrent environments?  
A: Use UNIQUE constraints and handle duplicate-key errors, or use transactions with SELECT ... FOR UPDATE for application-controlled checks.

Q5: What are common replication pitfalls?  
A: Skipped events due to non-deterministic statements, mismatched schema, different server timezones, insufficient binlog retention, and replication filters misconfiguration.

Q6: How do you optimize a slow JOIN query?  
A: Ensure appropriate indexes on join columns, check EXPLAIN for table order, use EXISTS or semi-join patterns for certain cases, reduce row counts with pre-filtering or selective projections.

Q7: Explain the difference between `DELETE` and `TRUNCATE`.  
A: DELETE logs individual row deletions (can be rolled back in transaction if supported), TRUNCATE is DDL-like (fast, drops and recreates the table) and may not be transactional.

Q8: What is a covering index?  
A: An index that contains all columns required by a query, allowing the engine to satisfy the query from the index alone without reading the table rows.

Q9: How to handle very large tables in schema changes?  
A: Use online schema change tools (gh-ost, pt-online-schema-change), partitioning, perform changes during low-traffic windows, or create new table and backfill in batches.

Q10: What is MVCC and how does it affect reads/writes?  
A: Multi-Version Concurrency Control (MVCC) provides consistent snapshots for reads using versions of rows; it reduces read/write blocking but long-running transactions may retain old row versions increasing storage until purge.

---

## 20. Practical Exercises & Projects

1. Beginner:
   - Write queries to find top 10 customers by revenue, average order per month, and customers with no orders.
   - Normalize a denormalized schema into 3NF and explain trade-offs.

2. Intermediate:
   - Build a sample schema (customers, orders, products, inventory). Implement indexes for common query patterns and measure EXPLAIN plans.
   - Set up master-replica replication in Docker and simulate failover steps.

3. Advanced:
   - Implement an OLTP schema and a nightly ETL that populates a reporting table. Add partitioning and measure query speed improvements.
   - Use Percona XtraBackup to take a hot backup and restore to a test instance. Script the process.
   - Migrate a large table schema change using gh-ost and measure impact compared to direct ALTER.

4. Data Analysis:
   - Use window functions to calculate cohorts, retention rates, and moving averages from sample event data.
   - Build a dashboard using Grafana and Prometheus metrics from mysqld_exporter.

---

## 21. Cheat Sheet & Useful Commands / SQL Snippets

Check server variables:
```sql
SHOW VARIABLES LIKE 'innodb_buffer_pool_size';
SHOW STATUS LIKE 'Threads_connected';
```

Show open transactions and locks:
```sql
SELECT * FROM INFORMATION_SCHEMA.PROCESSLIST;
SELECT * FROM INFORMATION_SCHEMA.INNODB_TRX;
```

Enable slow query log:
```sql
SET GLOBAL slow_query_log = 1;
SET GLOBAL long_query_time = 1;
```

Backup with mysqldump:
```bash
mysqldump --single-transaction --routines --events --triggers -u root -p mydb > mydb.sql
```

Restore:
```bash
mysql -u root -p mydb < mydb.sql
```

Show indexes:
```sql
SHOW INDEX FROM orders;
```

Explain plan:
```sql
EXPLAIN FORMAT=JSON SELECT * FROM orders WHERE customer_id = 123;
```

Create index:
```sql
CREATE INDEX idx_orders_customer_date ON orders (customer_id, order_date);
```

Find duplicate rows:
```sql
SELECT email, COUNT(*) cnt FROM customers GROUP BY email HAVING cnt > 1;
```

Atomic upsert (MySQL):
```sql
INSERT INTO products (sku, qty) VALUES ('sku1', 10)
ON DUPLICATE KEY UPDATE qty = qty + VALUES(qty);
```

---

## 22. References & Further Reading

- Official MySQL Documentation — https://dev.mysql.com/doc/  
- InnoDB Internals — blogs and architecture docs (Percona, Oracle)  
- Percona Toolkit & XtraBackup docs — https://www.percona.com/  
- High Performance MySQL (book) — O'Reilly  
- Use The Index, Luke! — indexing and query performance guide  
- Facebook/Dropbox/Github engineering blog posts on large-scale MySQL use cases  
- Flyway / Liquibase docs for migrations  
- gh-ost and pt-online-schema-change docs for online schema change  
- Prometheus mysqld_exporter & Grafana dashboards for monitoring

---

Prepared as a comprehensive, practical reference for SQL programming, MySQL internals and operations, database design, data analysis with SQL, and cross-platform installation/configuration guidance. Use it for interview preparation, daily DBA/devops tasks, and learning deeper database engineering concepts.