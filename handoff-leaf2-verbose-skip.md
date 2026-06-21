---

You are picking up a development ticket on an existing .NET / C# console application project. Work
only from this ticket and the code in the repository. Where the ticket says
STOP AND REPORT, follow it — do not work around it. Each `##` section below is
already filled in or marked N/A by the operator who assembled this handoff
(the operator's tick-every-block step is the guard against a dropped section —
§9, §12); treat every section as present and address it.

## Repository & setup  (carried from the packet Harness — confirm, don't re-author)
- Repo / build / layout / permissions: per the packet Harness below.
- Branch: create & switch to `feature/verbose-skip-reporting` off `feature/dry-run-mode` (Leaf 1 must be merged first); commit, do NOT push.
- Baseline green CONFIRMED now: 437 passed, 0 failed (all tests from Leaf 1 included). Every existing test must still pass.
  ← the one fact the packet can't carry forward; re-run at pre-flight.
- Verifier gates (MECHANICAL — the run's pass is the gate's verdict, not your "all green"):
  N/A — no Coverlet coverage gate or API analyzer is wired yet. Run still must be green.

## Ticket: Verbose skip reporting for exporter CLI

### Intent
As an automation engineer, I want `--verbose` (`-v`) output that names which members were skipped by the visibility filter and why, so that I can diagnose incomplete exports in CI without reverse-engineering silent failures.

### Spec

1. **Why / value:** Members excluded by the visibility filter are silently dropped today. Verbose mode makes the filter's effect visible for post-export diagnosis and dry-run pre-flight.

2. **Seam (old things touched):**
   - `Exporter.cs` (console) `InitialiseDocumentForExport()` — subscribe an external skip-observer to `DocumentMapper.PreEntryAdded` here (FORK 4 decision). The observer collects `(memberName, memberAccess)` tuples. This follows the same pattern as existing event subscriptions in the console Exporter (`exporter_ExportStep` etc.).
   - `Exporter.cs` (console) `Export()` / `ExportToOutputMethod()` — `_verbose` is stored but never read. After export (or after dry-run validation), if `_verbose` is true, log the skip summary using the collected tuples. `Parameters.DryRun` (added in Leaf 1) controls format: dry-run → per-member list; non-dry-run → grouped count summary.
   - `Document.cs` / `DocumentMapper.cs` — read-only unless accumulation moves there (it does not — FORK 4 decision keeps accumulation in the console layer). Do NOT add public surface to the Documentation library for skip reporting.
   - `Parameters.cs` — `Parameters.DryRun` and `--verbose` long-form are already present from Leaf 1. No further parameter changes needed.

3. **Invariants (must hold):**
   - Non-verbose export: no change to output whatsoever (regression).
   - Skip reporting adds output — it never suppresses or replaces error output.
   - The filter logic in `IsMemberFiltered()` is **not changed** by this leaf.
   - No changes to the Documentation library (`Document.cs`, `DocumentMapper.cs`) other than subscribing the existing `PreEntryAdded` event from the console layer.

4. **Constraints:** Count/list output uses existing `ILog` channels; no new output mechanism.

5. **Non-goals / out of scope:** Changing filter logic, changing which members are filtered, reporting reasons other than visibility, WPF app, API surface.

6. **Stop-and-report triggers:**
   - If `DocumentMapper.PreEntryAdded` does not fire for members excluded by `IsMemberFiltered()` (i.e., filtered members never reach the event), STOP AND REPORT — describe the actual call sequence and what surface would need to change to observe filtered members from the console layer.
   - If `memberAccess` is not available on the event args passed to `PreEntryAdded`, STOP AND REPORT — identify what is available and what the grouping key would have to be instead.
   - If implementation reveals the spec or any existing test encodes a wrong assumption, STOP AND REPORT. Do not change tests to make them pass.

### Owner decisions (VERBATIM — do not re-open)
- FORK 3 — Output format: `"Private — 42 members excluded."` (en-dash, singular/plural not specified — match the exact format). Zero-excluded case still gets output: `"[Visibility] — 0 members excluded."`. One line per visibility group present in the filter result. Per-member list (dry-run + verbose): one line per member naming the member and its visibility.
- FORK 4: External skip-observer subscribed to `DocumentMapper.PreEntryAdded` from the console `Exporter.InitialiseDocumentForExport()`. No Documentation-layer changes.
- FORK 5 (inherited from Leaf 1): `Main()` already returns `int`. Skip-reporting itself is not a failure path — it is informational output only, exit code unaffected.

### Goals
1. `exporter file.dll -to /out -v` with members excluded by filter: output contains a count grouped by visibility, format `"[Visibility] — N members excluded."` for each group. Zero-excluded groups also appear.
2. `exporter file.dll -to /out -v --dry-run` (both flags): output lists each skipped member individually with its visibility reason; no files written.
3. `exporter file.dll -to /out` (no `-v`): output contains no skip listing (regression).
4. `exporter file.dll -to /out -v` with zero members excluded across all groups: output contains `"[Visibility] — 0 members excluded."` lines (one per filter group present).
5. `Parameters.Verbose` is `true` when `--verbose` is passed — verified by Leaf 1 tests; no new `Parameters` test needed here.

### Standing context
- UI: N/A
- Tests: NUnit 3 + Moq. Mock `ILog` to assert on logged strings. Mock or stub `DocumentMapper.PreEntryAdded` subscriber pattern to simulate filtered-member events without disk I/O. Match the existing style in `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/ProgramTests.cs` exactly (one `[Test]` per scenario, `Times.Once`/`Times.Never` for log assertion).
- Also add tests to `Source/TheBoxSoftware.Documentation.Tests/` if the observer wiring requires testing at the Documentation layer — only if needed.
- Other: `ILog` for output; do not call `File.*` directly.

### Harness
- Repo & build: `D:\projects\live-documenter` · `dotnet build developersuite.sln` / `dotnet test developersuite.sln`
- Touches: `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs`; read `Source/TheBoxSoftware.Documentation/Document.cs` and `DocumentMapper.cs` (subscribe event only, no edits unless the STOP AND REPORT triggers fire).
- Verifier: `dotnet test Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj`
- Baseline: 437 passed / 0 failed (includes Leaf 1 tests) — operator re-confirms green at pre-flight.
- Branch: `feature/verbose-skip-reporting` off `feature/dry-run-mode`
- Permissions:
  - **MAY touch:** `Exporter.cs` (console); add/edit test files under `Exporter.Tests/Unit/` and `Documentation.Tests/`
  - **READ-ONLY (subscribe event, no structural edits):** `Document.cs`, `DocumentMapper.cs`
  - **MUST NOT touch:** `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter/` (WPF), `Source/TheBoxSoftware.API.LiveDocumenter/`, `Parameters.cs`, `Program.cs`
- Parent: Dry-run validation and verbose skip reporting epic.

## Standing conventions (match the codebase — as important as the feature)
- Comments: XML doc comments on public members only; `//` on internal/private members where non-obvious.
- Surface: internal-by-default; widen only via `InternalsVisibleTo`, not by making things public unnecessarily.
- Abstraction: prefer a little duplication over premature abstraction (rule of three).
- Single source of truth for any value that must stay consistent.
- Tests: NUnit 3 + Moq. Match the existing style in `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/ProgramTests.cs` and `ParameterTests.cs` exactly: one `[Test]` per scenario, `[TestFixture]` classes, arrange-mock-assert, `Times.Once`/`Times.Never` for log assertion, no `[SetUp]` unless already in the class being extended.

## Your final report must include (concise, factual)

1. Branch name + files changed.  (→ §6)
2. Test result: total/passed/failed + new test names with pass/fail.
2a. **GOAL→TEST MAP:** list every Goal (1–5) and the exact test method name that covers it; write "none" + reason for any uncovered goal. A self-reported "all green" is not coverage — the mechanical check decides.  (→ §6 goal→test survival, H6)
3. Key implementation choices made (and how, for the riskiest seam point — the `PreEntryAdded` subscription wiring and how filtered-member data is collected and keyed are the riskiest here).
4. Any STOP AND REPORT trigger hit, and any assumption you had to make.  (→ §6 Stop-and-report)
5. Anything in the ticket that was wrong, ambiguous, or encodes a mistaken assumption.  (→ §6 Promote)
6. Was the work HEAVIER or LIGHTER than implied — was the seam deeper/shallower than described?  (→ §6 Actual size + Spec-weight vs reality)
7. Where the genuine FRICTION was — the part that was actually hard, not the boilerplate.  (→ §6 Where it was hard)
8. Run COST: wall-clock always; tokens in/out only if the harness exposes them — if not, say "no counter".  (→ §6 Cost)
