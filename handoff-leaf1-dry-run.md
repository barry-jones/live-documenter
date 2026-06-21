---

You are picking up a development ticket on an existing .NET / C# console application project. Work
only from this ticket and the code in the repository. Where the ticket says
STOP AND REPORT, follow it — do not work around it. Each `##` section below is
already filled in or marked N/A by the operator who assembled this handoff
(the operator's tick-every-block step is the guard against a dropped section —
§9, §12); treat every section as present and address it.

## Repository & setup  (carried from the packet Harness — confirm, don't re-author)
- Repo / build / layout / permissions: per the packet Harness below.
- Branch: create & switch to `feature/dry-run-mode` off `master`; commit, do NOT push.
- Baseline green CONFIRMED now: 437 passed, 0 failed (26 in Exporter.Tests). Every existing test must still pass.
  ← the one fact the packet can't carry forward; re-run at pre-flight.
- Verifier gates (MECHANICAL — the run's pass is the gate's verdict, not your "all green"):
  N/A — no Coverlet coverage gate or API analyzer is wired yet. Run still must be green.

## Ticket: Dry-run mode for exporter CLI

### Intent
As an automation engineer, I want a `--dry-run` (`-d`) mode that validates the configuration and assemblies without writing output, so that I can verify an export will succeed in CI before committing to it.

### Spec

1. **Why / value:** Silent partial exports in CI cause missed failures. Dry-run lets a pipeline gate on "will this export succeed?" before any files are written.

2. **Seam (old things touched):**
   - `Parameters.cs` — add `--dry-run`/`-d` to `PARAMETERS` array; add `ReadDryRun()` private parser; expose `DryRun` bool property. Also add `--verbose` as a long-form alias for `-v`; `ReadVerbosity()` currently only recognises `-v`.
   - `Program.cs:HandleExport()` — after `IsValid` check, gate `Exporter.Export()` on `!parameters.DryRun`. When dry-run is active, print a validation summary to ILog and return without calling `Export()`.
   - `Exporter.cs` (console) `ExportToOutputMethod()` — on invalid LDEC today only logs `"There are issues with the LDEC file: {file}"` with no detail. Call the new `GetValidationIssues()` method (see FORK 1) and log each reason. Existing message string must NOT be removed — append reasons after it.
   - `ExportConfigFile.cs` — add `GetValidationIssues()` returning `List<Issue>` (FORK 1 decision). `CheckIsValid()` / `IsValid` are not changed; all existing callers are unaffected.
   - `Exporter.cs` (console) `Export()` — wrap `InitialiseDocumentForExport()` in `try-catch` (FORK 2 decision). On catch: log the assembly filename and `exception.Message`; do not call any export methods; return (or propagate exit-code failure via the new `Main()` return path).
   - `Program.cs:Main()` — change from `static void Main(string[] args)` to `static int Main(string[] args)` (FORK 5 decision). Exit 0 on success (including dry-run success). Exit non-zero on any failure path (invalid config, unloadable assembly, invalid LDEC). Non-dry-run error paths that currently exit 0 on error must also become non-zero.

3. **Invariants (must hold):**
   - In dry-run, no files are written to `output.Location`.
   - In dry-run, `exporter.Export()` (Documentation layer) is **never** called.
   - Non-dry-run behaviour is unchanged for all green paths (all existing tests still pass).
   - LDEC validation detail is **added** to the error output — the existing error message string is not removed.
   - Dry-run exits 0 when validation passes and non-zero when it fails.

4. **Constraints:** Output within the existing `ILog`/`IUserInterface` channel. No new output destinations.

5. **Non-goals / out of scope:** WPF app, API surface, LDEC repair, auto-correction of bad config, changing exit-code semantics for green non-dry-run paths.

6. **Stop-and-report triggers:**
   - FORK 2 open sub-question: `InitialiseDocumentForExport()` may fail mid-loop when processing multiple assemblies (sln/project input). The caught exception may not carry the `DocumentedAssembly.FileName`. If you find the exception context does NOT include the offending filename, STOP AND REPORT — do not invent a workaround. Describe what context the exception does carry and what changes would be needed in the Documentation layer to surface the filename.
   - If implementation reveals the spec or any existing test encodes a wrong assumption, STOP AND REPORT. Do not change tests to make them pass.

### Owner decisions (VERBATIM — do not re-open)
- FORK 1: Add `List<Issue> GetValidationIssues()` to `ExportConfigFile`. `IsValid` / `CheckIsValid()` unchanged.
- FORK 2: Wrap `InitialiseDocumentForExport()` in `try-catch` in the console `Exporter.Export()`. Log filename + `exception.Message`. See STOP AND REPORT above for the open filename-context sub-question.
- FORK 5: `Main()` returns `int`. Dry-run success = 0, dry-run failure (invalid config / bad assembly) = non-zero. Non-dry-run error paths also non-zero.

### Goals
1. `exporter file.dll -to /out -d` with valid inputs: zero files created under `/out`; stdout contains a line naming the LDEC and target location; process exits 0.
2. `exporter file.dll -to /out --dry-run` (long form): identical behaviour to goal 1.
3. `exporter file.dll -to /out -d` with an invalid/missing LDEC: error output names the LDEC file and at least one specific reason it is invalid; process exits non-zero.
4. `exporter bad-assembly.dll -to /out -d` where the assembly cannot be read: error output names the assembly file and the exception message; no partial output written; process exits non-zero.
5. `exporter file.dll -to /out` (no flags): export behaviour is identical to before this change (regression).
6. `Parameters.DryRun` is `true` when `-d` is passed, `false` by default.
7. `Parameters.Verbose` is `true` when `--verbose` is passed (as well as existing `-v`), `false` by default.

### Standing context
- UI: N/A
- Tests: NUnit 3 + Moq. Match the pattern in `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/ProgramTests.cs` (arrange-act-assert, mock `ILog` to assert on logged strings, no disk I/O). `ParameterTests.cs` has 13 existing tests — add new parameter tests in the same file. Do not create a new test class for parameters; add to the existing one.
- Other: `ILog` interface for all output; `IFileSystem` for filesystem calls (already injected).

### Harness
- Repo & build: `D:\projects\live-documenter` · `dotnet build developersuite.sln` / `dotnet test developersuite.sln`
- Exporter project: `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/`
- Test project: `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/`
- Verifier: `dotnet test Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj`
- Baseline: 437 passed / 0 failed across all test projects (26 in Exporter.Tests) — operator re-confirms green at pre-flight.
- Branch: `feature/dry-run-mode` off `master`
- Permissions:
  - **MAY touch:** `Parameters.cs`, `Program.cs`, `Exporter.cs` (console), `ExportConfigFile.cs`, `Configuration.cs`; add/edit test files under `Exporter.Tests/Unit/`
  - **MUST NOT touch:** `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter/` (WPF), `Source/TheBoxSoftware.API.LiveDocumenter/`, Documentation layer (except `ExportConfigFile.cs`)
- Parent: Dry-run validation and verbose skip reporting epic.

## Standing conventions (match the codebase — as important as the feature)
- Comments: XML doc comments on public members only; `//` on internal/private members where non-obvious.
- Surface: internal-by-default; widen only via `InternalsVisibleTo`, not by making things public unnecessarily.
- Abstraction: prefer a little duplication over premature abstraction (rule of three).
- Single source of truth for any value that must stay consistent.
- Tests: NUnit 3 + Moq. Match the existing style in `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/ProgramTests.cs` and `ParameterTests.cs` exactly: one `[Test]` method per scenario, `[TestFixture]` classes, arrange-mock-assert, `Times.Once` / `Times.Never` for log assertion, no `[SetUp]` unless it already exists in the class being extended.

## Your final report must include (concise, factual)

1. Branch name + files changed.  (→ §6)
2. Test result: total/passed/failed + new test names with pass/fail.
2a. **GOAL→TEST MAP:** list every Goal (1–7) and the exact test method name that covers it; write "none" + reason for any uncovered goal. A self-reported "all green" is not coverage — the mechanical check decides.  (→ §6 goal→test survival, H6)
3. Key implementation choices made (and how, for the riskiest seam point — the `try-catch` scope in `Exporter.Export()` and `Main()` return-path wiring are the riskiest here).
4. Any STOP AND REPORT trigger hit, and any assumption you had to make.  (→ §6 Stop-and-report)
5. Anything in the ticket that was wrong, ambiguous, or encodes a mistaken assumption.  (→ §6 Promote)
6. Was the work HEAVIER or LIGHTER than implied — was the seam deeper/shallower than described?  (→ §6 Actual size + Spec-weight vs reality)
7. Where the genuine FRICTION was — the part that was actually hard, not the boilerplate.  (→ §6 Where it was hard)
8. Run COST: wall-clock always; tokens in/out only if the harness exposes them — if not, say "no counter".  (→ §6 Cost)
