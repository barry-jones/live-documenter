 ▐▛███▜▌   Claude Code v2.1.183
▝▜█████▛▘  Sonnet 4.6 · Claude Pro
  ▘▘ ▝▝    D:\projects\live-documenter


❯ /caveman                                                                      

● Caveman mode on. Full level. Ready.

✻ Crunched for 2s

❯ /caveman lite                                                                 

● Caveman mode: lite. No filler, full sentences, professional and tight.

✻ Brewed for 2s

❯ You are drafting a SPEC from a user story against a real codebase. You draft  
  and                                                                           
  surface decisions for a human — you do NOT implement, and you do NOT decide   
  open                                                                          
  questions. Bleed into "how" only as far as needed to locate the seam.         
                                                                                
  ## Inputs                                                                     
  Dry-run validation and verbose skip reporting for the exporter                
                                                                                
  As an automation engineer, I want a --dry-run mode that validates inputs and  
  reports what would be exported, plus --verbose output that names which        
  members were skipped and why, so that I can diagnose failed or incomplete     
  exports in CI without reverse-engineering silent failures.                    
                                                                                
  Out of scope                                                                  
                                                                                
  - Changing the export output formats or the LDEC schema itself                
  - Repairing any specific LDEC/assembly — this story reports, it does not fix  
  - The WPF desktop app and the API surface (exporter.exe console only)         
  - Auto-correcting bad input (guessing the right filter or format)             
                                                                                
  Technical notes                                                               
                                                                                
  - Entry point Program.HandleExport(); flags parsed in Parameters.Read(). New  
  dry-run flag follows the -v pattern. Accept both --dry-run and -d; add both   
  to the PARAMETERS array so they're treated as flags, not values. Verbose      
  should also accept both -v and --verbose (-v already exists, add the long     
  form).                                                                        
  - Validation today is scattered and terminal: Configuration.IsValid(log) only 
  checks the document exists; LDEC validity is checked late in                  
  Exporter.ExportToOutputMethod via config.IsValid, logging a generic "There    
  are issues with the LDEC file: {file}" with no detail — primary cause of the  
  silent/incomplete complaint.                                                  
  - export.Exporter.GetIssues() already returns structured Issue objects, only  
  surfaced at export time. Dry-run should call this and report up front.        
  - Assembly load failures surface as exporter.ExportExceptions, logged as      
  warnings after a partial export already ran.                                  
  - _verbose is passed into Exporter but barely used (exporter_ExportStep logs  
  regardless). Member-skip reporting is new: skips driven by                    
  settings.Settings.VisibilityFilters, counted via EntryCreator.Created; which  
  members were filtered is not currently captured.                              
  - Verbose scope: default to summary-by-reason with counts (readable CI logs); 
  emit full per-member enumeration only under --dry-run.                        
  - Dry-run must not write to output.Location — no file I/O, no                 
  exporter.Export() call.                                                       
  - Help text (Program.IsHelpShown) lists both forms, e.g. -d, --dry-run        
  validate inputs and report what would be exported.                            
                                                                                
  Acceptance Criteria                                                           
                                                                                
  ---                                                                           
                                                                                
  My pick: summarise-by-reason by default, full per-member enumeration under    
  dry-run. Keeps CI logs readable but gives full detail when you're debugging.  
                                                                                
  Want that baked into the story? Then it's complete.                           
                                                                                
  ❯ yes                                                                         
                                                                                
  Baked in. Final story below.                                                  
                                                                                
  ---                                                                           
  Dry-run validation and verbose skip reporting for the exporter                
                                                                                
  As an automation engineer, I want a --dry-run mode that validaould be         
  exported, plus --verbose output that names whichmembers were skipped and why, 
  so that I can diagnose failed or incomplete exports in CI without             
  reverse-engineering silent failures.                                          
                                                                                
  Out of scope                                                                  
                                                                                
  - Changing the export output formats or the LDEC schema itself                
  - Repairing any specific LDEC/assembly — this story reports, it does not fix  
  - The WPF desktop app and the API surface (exporter.exe console only)         
  - Auto-correcting bad input (guessing the right filter or format)             
                                                                                
  Technical notes                                                               
                                                                                
  - Entry point Program.HandleExport(); flags parsed in Parameteollows the -v   
  pattern. Accept both --dry-run and -d; add bothto the PARAMETERS array so     
  they're treated as flags, not values. Verbose should also accept both -v and  
  --verbose (-v already exists, add the long form).                             
  - Validation today is scattered and terminal: Configuration.Iscument exists;  
  LDEC validity is checked late inExporter.ExportToOutputMethod via             
  config.IsValid, logging a generic "There are issues with the LDEC file:       
  {file}" with no detail — primary cause of the silent/incomplete complaint.    
  - export.Exporter.GetIssues() already returns structured Issue objects, only  
  surfaced at export time. Dry-run should call this and report up front.        
  - Assembly load failures surface as exporter.ExportExceptions,artial export   
  already ran.                                                                  
                                                                                
                                                                                
  ---                                                                           
  Dry-run validates without exporting                                           
                                                                                
  Given a valid document and LDEC format                                        
  When I run the exporter with --dry-run                                        
  Then no files are written to the output location                              
  And a summary lists each output (LDEC + target location) that                 
  And the process exits with a success code                                     
                                                                                
  ---                                                                           
  Dry-run reports an invalid LDEC with detail                                   
                                                                                
                                                                                
  Dry-run reports an assembly that cannot be loaded                             
                                                                                
  Given a document referencing an assembly that cannot be read                  
  When I run the exporter with --dry-run                                        
  Then the output names the offending assembly and the reason it could not be   
  loaded                                                                        
  And no partial export is attempted                                            
                                                                                
  ---                                                                           
  Verbose summarises skipped members by reason                                  
                                                                                
  Given a document with members excluded by the active visibility filters       
  When I run the exporter with --verbose                                        
  Then skipped members are summarised by reason with counts (e.ge — 42")        
                                                                                
  Given the --dry-run and --verbose flags are both supplied                     
  When I run the exporter                                                       
  Then each skipped member is listed individually with its reason               
  And validation results are produced together, still without writing output    
                                                                                
  ---                                                                           
  Both flag spellings are accepted                                              
                                                                                
  Given the exporter supports dry-run and verbose                               
  When I run it with -d or --dry-run, or with -v or --verbose                   
  Then each short form and its long form produce identical behaviour            
                                                                                
  ## Do                                                                         
  1. **Locate the SEAM.** Read the codebase and cite the files/types. Name the  
  existing                                                                      
     behaviours, boundaries, modules, entry points this change touches.         
  2. **Fill the Task Packet** — EVERY slot, using the exact template below (do  
  not invent                                                                    
     a shape). Draft Standing context from the repo's CLAUDE.md (link it) and   
  Harness                                                                       
     (verifier command, touchable paths, parent link) from the repo. Everything 
  EXCEPT                                                                        
     the how. Where a slot needs a decision you must not make, write `<FORK n:  
  ...>`.                                                                        
  3. **Goals:** observable before code, inputs→outputs, no internal names,      
  riskiest first;                                                               
     every invariant becomes a goal.                                            
  4. **Size it.** Leaf (spec collapses) or branch? Worth handing off, or is the 
  spec most                                                                     
     of the effort — recommend the human just does it? **If branch: STOP and    
  decompose                                                                     
     into leaves (§4 cut tests) — a packet per leaf, or present the cut for     
  confirmation.                                                                 
     Never hand a whole branch to one agent.**                                  
  5. **Apply what has one answer; fork only what doesn't.** Established         
  conventions /                                                                 
     platform defaults (machine→stdout, diagnostics→stderr) — apply, and NOTE   
  any                                                                           
     behaviour change. A "fork" where only one option survives the story's      
  constraints is                                                                
     not a fork. Then list the genuine FORKS, each with a recommendation:       
     (a) **Intent** — the story didn't say what it wants (e.g. an output        
  contract). The                                                                
         human defines it; not "the how."                                       
     (b) **Design** — real seam trade-offs with competing, legitimate options.  
                                                                                
  ## Output, then STOP                                                          
  - The draft Task Packet (every slot filled or `<FORK n>`), the size verdict,  
  and the                                                                       
    numbered forks with recommendations.                                        
  - End your turn. Do NOT answer the forks, write the handoff, or implement —   
  assembly is                                                                   
    a separate operator step in a fresh context. **The context boundary is the  
  gate.**                                                                       
                                                                                
  ## Task Packet template — produce EXACTLY this shape                          
                                                                                
  ---                                                                           
                                                                                
  ## Task Packet: <leaf title>                                                  
                                                                                
  ### Intent                                                                    
  As a <role>, I want <capability>, so that <value>.   ← governor               
                                                                                
  ### Spec (the serialised Conversation — everything EXCEPT the how)            
  1. Why / value:            <from so-that>                                     
  2. Seam (old thing touched): <which behaviours/boundaries/modules; AI drafts, 
  human verifies>                                                               
  3. Invariants (must hold):  <regression tripwires → each becomes a goal>      
  4. Constraints (non-func):  <perf/security/limits, or N/A>                    
  5. Non-goals / out of scope: <the fence — bounds blast radius>                
  6. Stop-and-report:         "If implementation reveals the spec or a test     
                               encodes a wrong assumption, STOP and report.     
                               Do not work around it or change tests to pass."  
                                                                                
  ### Owner decisions (answered forks — the ONLY "how" the human pinned; carry  
  verbatim, do not re-open)                                                     
  - <FORK n>: <the answer, as a directive>   ← left as `<FORK n>` at draft      
  time; "none" if no forks                                                      
                                                                                
  ### Goals (verifiable BEFORE code — inputs→outputs, no internal names;        
  riskiest first)                                                               
  1. <observable pass/fail>                                                     
  2. ...                                                                        
                                                                                
  ### Standing context (REFERENCED, not repeated — see §5)                      
  - UI:    <link to component lib / UI convention skill, or N/A>                
  - Tests: <where they live + how written — link to convention, or N/A>         
  - Other: <logging, error handling, arch rules — links>                        
                                                                                
  ### Harness                                                                   
  - Verifier:    <command/CI that runs the goals>                               
  - Permissions: <what the agent may touch>                                     
  - Parent:      <link up the tree to the branch/epic goal this serves>         
                                                                                
  ---                                                                           

● Reading the codebase to locate the seam before drafting.

  Searched for 2 patterns, read 8 files (ctrl+o to expand)

● Now let me check the DocumentMapper to understand the filter/skip mechanism.

  Read 5 files (ctrl+o to expand)

● Let me check how assembly failures surface during document map building.

  Searched for 1 pattern, read 2 files (ctrl+o to expand)

● I have enough to draft. The seam is clear. Writing 3 leaf packets now.

  ---
  Branch: Dry-run validation and verbose skip reporting

  Three clean leaf cuts. B and C both depend on A flags; B and C are independent
  of each other.

  ┌───────────────────────┬──────────────────────────────────────┬──────────┐
  │         Leaf          │                Scope                 │ Blocks   │
  │                       │                                      │    on    │
  ├───────────────────────┼──────────────────────────────────────┼──────────┤
  │ A — Flag parsing &    │ Parameters.cs, Program.cs            │ —        │
  │ help                  │                                      │          │
  ├───────────────────────┼──────────────────────────────────────┼──────────┤
  │ B — Dry-run           │ Exporter.cs (exporter),              │ A        │
  │ validation            │ ExportConfigFile.cs                  │          │
  ├───────────────────────┼──────────────────────────────────────┼──────────┤
  │ C — Verbose skip      │ Document.cs, Exporter.cs (exporter)  │ A, FORK  │
  │ reporting             │                                      │ 2        │
  └───────────────────────┴──────────────────────────────────────┴──────────┘

  ---
  Task Packet A: Flag parsing and help text

  Intent

  As an automation engineer, I want -d/--dry-run and --verbose flags parsed
  correctly and described in help, so that the exporter accepts the new flags
  without treating them as values.

  Spec

  1. Why / value: Flags that don't appear in PARAMETERS are consumed as values
  for the preceding flag (see ValueIsAParameter). If --dry-run is not
  registered, it silently becomes the value of whatever precedes it.
  2. Seam:
    - Parameters.cs:22 — PARAMETERS string array gates what is recognized as a
  flag vs. a value
    - Parameters.ReadVerbosity() line 94 — only checks "-v", not "--verbose"
    - Parameters.cs has no DryRun field or property
    - Program.IsHelpShown() line 113 — hard-coded help string, no mention of -d
  or --verbose
  3. Invariants:
    - -v continues to set Verbose = true (no regression)
    - -h continues to show help
    - All existing flags (-format, -f, -filters, -to) continue to work
    - A value immediately after a registered flag is not consumed as the flag
  itself
  4. Constraints: N/A
  5. Non-goals: No behavior change to export logic. No long forms for -h,
  -format, -filters, -to, -f.
  6. Stop-and-report: If PARAMETERS has a secondary usage path not visible in
  ValueIsAParameter (e.g., ReadFormats uses Array.IndexOf directly), stop and
  report before adding entries.

  Owner decisions

  - None yet (no open forks for this leaf)

  Goals

  1. exporter mylib.dll --dry-run — parameters.DryRun == true,
  parameters.Verbose == false
  2. exporter mylib.dll -d — parameters.DryRun == true
  3. exporter mylib.dll --verbose — parameters.Verbose == true,
  parameters.DryRun == false
  4. exporter mylib.dll -v — parameters.Verbose == true (regression)
  5. exporter mylib.dll -d --verbose -to c:\out -format web-msdn.ldec — DryRun 
  && Verbose && To == "c:\out" (combined, no value theft)
  6. exporter mylib.dll -to --dry-run — To reads empty (flag not consumed as
  value for -to)
  7. Help output contains the strings -d, --dry-run and --verbose when -h or no
  args given

  Standing context

  - UI: N/A
  - Tests: Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Un
  it/ParameterTests.cs — NUnit 3, arrange-act-assert, Parameters.Read(string[])
  tested directly
  - Other: See CLAUDE.md for run/test commands

  Harness

  - Verifier: dotnet test 
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftw
  are.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj
  - Permissions:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Parameters.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Program.cs, test
  file ParameterTests.cs
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  Task Packet B: Dry-run validation without export

  Intent

  As an automation engineer, I want --dry-run to validate all inputs and report
  what would be exported — without writing any files — so that I can diagnose
  configuration failures in CI before committing to a real run.

  Spec

  1. Why / value: Current failures are silent or post-hoc: LDEC validity is
  checked late inside ExportToOutputMethod (after document map is built);
  assembly load failures only appear in ExportExceptions after a partial export
  already ran.
  2. Seam:
    - Exporter.cs (exporter) line 43 Export() — entry point for the real export;
  must be skipped in dry-run
    - Exporter.cs line 111 ExportToOutputMethod() — where config.IsValid is
  checked and GetIssues() is called; the LDEC detail gap is here ("There are 
  issues with the LDEC file: {file}" with no reason)
    - ExportConfigFile.cs line 190 CheckIsValid() — private; validates XSLT
  node, XSLT file presence in ZIP, name not empty. Returns reasons implicitly
  via IsValid boolean only.
    - DocumentedAssembly.IsCompiled (line 124) = File.Exists(FileName) —
  silently skipped in GenerateMap() if false; does not catch PE parse failures
  from AssemblyDef.Create()
    - Exporter.cs (exporter) line 166 InitialiseDocumentForExport() — calls
  document.UpdateDocumentMap() which triggers AssemblyDef.Create() per assembly;
  unhandled exceptions here propagate up uncaught
    - Program.cs line 92 — constructs Exporter with parameters.Verbose; will
  need to also pass parameters.DryRun
  3. Invariants:
    - When --dry-run is NOT set, Export() is called as before (no regression to
  existing export behavior)
    - When --dry-run IS set, zero bytes are written to output.Location or any
  temp path (PrepareForExport must not be called)
    - GetIssues() on export.Exporter continues to gate the real export (existing
  behavior preserved)
  4. Constraints: LDEC files live in ApplicationData\ relative to the exe.
  Assembly parse (AssemblyDef.Create) may be expensive on large binaries —
  acceptable for dry-run; no perf requirement.
  5. Non-goals: Not fixing any specific LDEC or assembly. Not changing LDEC
  schema. Not the WPF app. Not auto-correcting bad input.
  6. Stop-and-report: If ExportConfigFile.CheckIsValid is found to have side
  effects beyond setting IsValid, or if AssemblyDef.Create does not throw on a
  corrupt-but-extant PE file (meaning the validation would give a false pass),
  stop and report.

  Owner decisions

  - FORK 1: answer required before implementation. See FORK 1 below.
  - FORK 3: answer required before implementation. See FORK 3 below.

  Goals

  1. exporter mylib.dll -d -to c:\out -format web-msdn.ldec with valid inputs →
  output reports each (LDEC file, target location) pair that would be written;
  no files created under c:\out
  2. exporter mylib.dll -d -format missing.ldec -to c:\out → output names
  missing.ldec and the specific reason it is invalid (e.g., missing XSLT entry);
  no files created
  3. exporter mylib.dll -d -to c:\out -format web-msdn.ldec with a
  DocumentedAssembly whose file does not exist → output names the missing
  assembly file; no partial export attempted
  4. exporter mylib.dll -d -to c:\out with an extant but corrupt/unreadable PE
  file → output names the file and the exception reason; no partial export
  5. exporter mylib.dll -to c:\out -format web-msdn.ldec (no -d) → real export
  runs as before; goals 1–4 do not apply (regression)

  Standing context

  - UI: N/A
  - Tests:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/ —
  NUnit 3 with Moq; see ProgramTests.cs for how Program is tested via
  IUserInterface/ILog mocks
  - Other: CLAUDE.md §Architecture — dependency chain; Exporter.cs (exporter)
  does not currently inject IFileSystem (uses System.IO.Path and System.IO.File
  directly in some places)

  Harness

  - Verifier: dotnet test 
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftw
  are.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj
  - Permissions:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Program.cs,
  Source/TheBoxSoftware.Documentation/Exporting/ExportConfigFile.cs (to expose
  issue detail per FORK 3 answer), exporter test project
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  Task Packet C: Verbose skip reporting

  Intent

  As an automation engineer, I want --verbose to show which members were skipped
  and why (summarised by reason normally; per-member under --dry-run), so that
  I can understand why an export is incomplete without parsing binary assemblies
  manually.

  Spec

  1. Why / value: ExportExceptions only surfaces render-time failures; members
  excluded by visibility filters are silently absent. entryCreator.Created
  counts included members but there is no record of what was excluded.
  2. Seam:
    - Document.PreEntryAdded() (line 236) — sets e.Filter = 
  IsMemberFiltered(e.Member). The only filter reason today is visibility.
  e.Member.MemberAccess (a Visibility enum) is available at this point.
    - Document.IsMemberFiltered() (line 59) — the sole filter gate; always
  returns based on Settings.VisibilityFilters
    - EntryCreator.Created (line 73) — counts entries ADDED (post-filter). No
  count or list of entries skipped.
    - Exporter.cs (exporter) line 168 InitialiseDocumentForExport() — only logs
  entryCreator.Created (included members); skipped count not computed
    - _verbose field in Exporter.cs (exporter) is set but the only use is via
  exporter_ExportStep which logs step names regardless
  3. Invariants:
    - Without --verbose, no additional output beyond current behavior
    - entryCreator.Created count (included members) continues to be logged as
  today
    - Skip reporting is purely additive; it does not change which members are
  exported
  4. Constraints: Verbose output must not significantly slow a non-dry-run
  export. Per-member enumeration only under --dry-run --verbose (human-confirmed
  in story).
  5. Non-goals: Not changing which members are exported. Not adding new filter
  types. Not changing the WPF app.
  6. Stop-and-report: If inspection reveals additional filter paths outside
  IsMemberFiltered() (e.g., compiler-generated types skipped in
  GroupedNamespaceDocumentMapper.GenerateDocumentForAssembly line 203
  IsCompilerGenerated — which are skipped silently before PreEntryAdded fires),
  stop and report. Compiler-generated types may need a separate skip category.

  Owner decisions

  - FORK 2: answer required before implementation. See FORK 2 below.

  Goals

  1. exporter mylib.dll -v with members excluded by visibility filters → output
  includes "Skipped: N members — visibility: Private" (or equivalent per-reason
  counts) after the included-member count line
  2. exporter mylib.dll -v with no excluded members → no skip section in output
  3. exporter mylib.dll -d -v → per-member list output: one line per skipped
  member showing fully-qualified name and reason; validation output also
  present; no files written
  4. exporter mylib.dll (no flags) → no skip output; behavior identical to today
  (regression)
  5. exporter mylib.dll -v (verbose, no dry-run) → summary only, no per-member
  enumeration

  Standing context

  - UI: N/A
  - Tests:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/, plus
  Source/TheBoxSoftware.Documentation.Tests/ if Document is modified
  - Other: CLAUDE.md §Architecture; Document.cs is in
  TheBoxSoftware.Documentation (netstandard2.0), a library project — changes
  here are visible to WPF and API surfaces even though those are out of scope
  for this story

  Harness

  - Verifier: dotnet test developersuite.sln
  - Permissions: Source/TheBoxSoftware.Documentation/Document.cs (per FORK 2
  answer),
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs,
  exporter test project, Documentation test project
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  Forks requiring human decisions

  FORK 1 (Intent) — Exit code contract (blocks Leaf B)

  The story says dry-run "exits with a success code" for valid inputs — implying
  non-zero for invalid. The current process never calls Environment.Exit(); all
  paths return from HandleExport() with implicit exit 0.

  Decision needed: Does dry-run (and by extension normal validation failures)
  set an explicit non-zero process exit code? If yes — what mapping? Suggested:
  0 = success, 1 = validation/configuration error, 2 = bad parameters. If no
  preference, suggest mapping and ask to confirm.

  ---
  FORK 2 (Design) — Exposing per-member skip data (blocks Leaf C)

  Document.PreEntryAdded() is private. There is no public surface to observe
  which members were filtered. Two options:

  - Option A (recommended): Add IReadOnlyList<SkippedMember> SkippedMembers to
  Document, populated inside the existing PreEntryAdded() handler. SkippedMember
  carries ReflectedMember + reason string. Clean, self-contained, testable
  without touching event wiring. Visible to the netstandard2.0 library callers
  (WPF, API) but additive — no behavior change for them.
  - Option B: Expose the mapper's PreEntryAdded event publicly via Document.
  Callers subscribe and inspect e.Filter + e.Member after the Document's own
  handler has run. Keeps Document leaner but leaks mapper internals and requires
  external callers to re-implement the reason derivation.

  Recommendation: A.

  ---
  FORK 3 (Intent) — LDEC issue detail surface (blocks Leaf B)

  ExportConfigFile.IsValid is a boolean; CheckIsValid() (private) knows why it
  failed (missing XSLT node, missing XSLT file in ZIP, missing name) but
  discards the reasons. Dry-run must report these.

  - Option A (recommended): Add List<string> GetConfigIssues() to
  ExportConfigFile — parallel to export.Exporter.GetIssues(), consistent
  pattern, keeps logic in the class that holds the knowledge.
  - Option B: Make CheckIsValid return a List<string> instead of setting IsValid
  — changes the existing call site in Initialise().

  Recommendation: A. Note: ExportConfigFile.cs is in
  TheBoxSoftware.Documentation (public API surface); adding a method is
  non-breaking.

✻ Worked for 4m 1s

❯ fork 1: agree. fork 2: a.                                                     

● FORK 3 is still open — Leaf B can't be finalized without it. Updated B and C
  with the confirmed answers; B still carries FORK 3.

  ---
  Task Packet B (updated): Dry-run validation without export

  Intent

  As an automation engineer, I want --dry-run to validate all inputs and report
  what would be exported — without writing any files — so that I can diagnose
  configuration failures in CI before committing to a real run.

  Spec

  1. Why / value: Current failures are silent or post-hoc: LDEC validity is
  checked late inside ExportToOutputMethod (after the document map is built);
  assembly load failures only appear in ExportExceptions after a partial export
  already ran.
  2. Seam (old thing touched):
    - Exporter.cs (exporter) Export() line 43 — real export entry point; must be
  skipped in dry-run
    - Exporter.cs (exporter) ExportToOutputMethod() line 111 — where
  config.IsValid is checked and GetIssues() is called; LDEC detail is lost here
  ("There are issues with the LDEC file: {file}", no reason)
    - ExportConfigFile.cs CheckIsValid() line 190 — private; validates XSLT
  node, XSLT file in ZIP, name not empty; exposes only a boolean
    - DocumentedAssembly.IsCompiled line 124 — File.Exists(FileName); missing
  files silently skipped in GenerateMap(); does not catch PE parse failures from
  AssemblyDef.Create()
    - Exporter.cs (exporter) InitialiseDocumentForExport() line 166 — calls
  document.UpdateDocumentMap(), which calls AssemblyDef.Create() per assembly;
  parse exceptions propagate uncaught
    - Program.cs line 92 — constructs Exporter(configuration, 
  parameters.Verbose, _log); will also pass parameters.DryRun
  3. Invariants:
    - When --dry-run is not set, Export() is called as before (no regression)
    - When --dry-run is set, zero bytes are written to output.Location or any
  temp path; PrepareForExport() must not be called
    - GetIssues() on export.Exporter continues to gate the real export (existing
  behavior preserved)
  4. Constraints: No perf requirement for assembly parse validation in dry-run.
  LDEC files resolve relative to exe ApplicationData\.
  5. Non-goals: Not fixing any specific LDEC or assembly. Not changing LDEC
  schema. Not the WPF app or API. Not auto-correcting bad input.
  6. Stop-and-report: If AssemblyDef.Create() does not throw on a
  corrupt-but-extant PE file (i.e., parse failure is silent), stop and report —
  the dry-run assembly check would give a false pass.

  Owner decisions

  - FORK 1: Exit code mapping is 0 = success, 1 = validation/configuration 
  error, 2 = bad parameters. Implement via Environment.Exit() at the end of
  HandleExport(). All current early-return paths that log an error implicitly
  exit 0 today — those must now exit 1.
  - FORK 3: <FORK 3> — still open; determines how LDEC issue detail is surfaced
  from ExportConfigFile.

  Goals

  1. exporter mylib.dll -d -to c:\out -format web-msdn.ldec with all valid
  inputs → stdout lists each (LDEC file, target location) pair that would be
  written; no files created under c:\out; process exits 0
  2. exporter mylib.dll -d -format missing.ldec -to c:\out → output names
  missing.ldec and the specific reason it is invalid (per FORK 3 answer);
  process exits 1
  3. exporter mylib.dll -d -to c:\out with a DocumentedAssembly whose file does
  not exist → output names the missing assembly; no partial export; process
  exits 1
  4. exporter mylib.dll -d -to c:\out with an extant but corrupt/unreadable PE
  file → output names the file and the exception message; no partial export;
  process exits 1
  5. exporter mylib.dll -to c:\out -format web-msdn.ldec (no -d) → real export
  runs as before; goals 1–4 do not apply; exit code follows existing behavior
  made explicit (0 on clean run, 1 on validation failure)
  6. An existing test that previously expected a return on bad input now asserts
  exit code 1

  Standing context

  - UI: N/A
  - Tests: Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Un
  it/ProgramTests.cs — NUnit 3 + Moq; Program is injected with
  IUserInterface/ILog mocks
  - Other: CLAUDE.md §Architecture; Exporter.cs (exporter) uses
  System.IO.File/Path directly in places (not all I/O goes through IFileSystem)

  Harness

  - Verifier: dotnet test 
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftw
  are.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj
  - Permissions:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Program.cs,
  Source/TheBoxSoftware.Documentation/Exporting/ExportConfigFile.cs (per FORK 3
  answer), exporter test project
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  Task Packet C (updated): Verbose skip reporting

  Intent

  As an automation engineer, I want --verbose to show which members were skipped
  and why — summarised by reason normally, per-member under --dry-run — so that
  I can understand why an export is incomplete without parsing binaries
  manually.

  Spec

  1. Why / value: Members excluded by visibility filters are silently absent
  from output. entryCreator.Created counts included entries only; there is no
  record of what was excluded or why.
  2. Seam (old thing touched):
    - Document.PreEntryAdded() line 236 — sets e.Filter = 
  IsMemberFiltered(e.Member). The member (e.Member) and its MemberAccess
  visibility are available here. No skip data is currently recorded.
    - Document.IsMemberFiltered() line 59 — sole filter gate; always returns
  based on Settings.VisibilityFilters. Reason is always visibility;
  e.Member.MemberAccess gives the specific level.
    - EntryCreator.Created line 73 — counts entries added (post-filter). No
  count or list of filtered entries.
    - Exporter.cs (exporter) InitialiseDocumentForExport() line 168 — logs
  entryCreator.Created (included count only); skip count not computed.
    - _verbose field in Exporter.cs (exporter) line 24 — set but the only
  current use is exporter_ExportStep which logs regardless of _verbose.
    - GroupedNamespaceDocumentMapper.GenerateDocumentForAssembly() line 203 —
  compiler-generated types (IsCompilerGenerated || Name[0] == '<') are skipped
  silently before PreEntryAdded fires and will not appear in SkippedMembers.
  3. Invariants:
    - Without --verbose, no additional output beyond current behavior
    - entryCreator.Created count continues to be logged as today
    - Skip reporting is additive — it does not change which members are exported
    - Per-member enumeration appears only under --dry-run --verbose; --verbose
  alone produces summary counts
  4. Constraints: Verbose summary must not noticeably slow a non-dry-run export
  (list is built during the document map pass which already iterates every
  member).
  5. Non-goals: Not changing which members are exported. Not adding new filter
  types. Compiler-generated member skips are intentionally excluded from this
  report — they are an implementation detail, not a user-visibility decision.
  6. Stop-and-report: If additional filter paths exist outside
  IsMemberFiltered() (beyond the compiler-generated skip in §2 which is already
  excluded), stop and report before deciding whether to capture them.

  Owner decisions

  - FORK 2: Add IReadOnlyList<SkippedMember> SkippedMembers to Document (in
  TheBoxSoftware.Documentation). SkippedMember carries the ReflectedMember
  reference and a reason string (format: "visibility: {MemberAccess}"). Populate
  it inside the existing Document.PreEntryAdded() handler when e.Filter is set
  to true. This is the only mechanism for skip capture — do not add a second
  event or expose mapper internals.

  Goals

  1. exporter mylib.dll -v with members excluded by visibility filters → output
  after the included-count line contains one summary line per visibility level
  that has skips, e.g., "  Skipped 42 members — visibility: Private"
  2. exporter mylib.dll -v with no excluded members → no skip section in output;
  included-count line unchanged
  3. exporter mylib.dll -d -v → one line per skipped member with fully-qualified
  name and reason; validation results also present; no files written; exit code
  0 on valid inputs
  4. exporter mylib.dll (no flags) → no skip output; output identical to today
  (regression)
  5. exporter mylib.dll -v (verbose only, no dry-run) → summary counts only; no
  per-member list
  6. Document.SkippedMembers is empty before UpdateDocumentMap() is called, and
  populated after it returns (unit-testable without running a real export)

  Standing context

  - UI: N/A
  - Tests:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/ +
  Source/TheBoxSoftware.Documentation.Tests/ for the Document change
  - Other: Document.cs is in TheBoxSoftware.Documentation (netstandard2.0) —
  visible to WPF and API surfaces; adding SkippedMembers is non-breaking but
  agent must not add the property to the public XML comment API docs without
  checking those call sites

  Harness

  - Verifier: dotnet test developersuite.sln
  - Permissions: Source/TheBoxSoftware.Documentation/Document.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs,
  Documentation test project, exporter test project
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  FORK 3 still needed to finalize Leaf B. Recommendation was Option A: add
  List<string> GetConfigIssues() to ExportConfigFile. Confirm or redirect?

✻ Churned for 1m 7s

❯ fork 3: a.                                                                    

● Task Packet B (final): Dry-run validation without export

  Intent

  As an automation engineer, I want --dry-run to validate all inputs and report
  what would be exported — without writing any files — so that I can diagnose
  configuration failures in CI before committing to a real run.

  Spec

  1. Why / value: Current failures are silent or post-hoc: LDEC validity is
  checked late inside ExportToOutputMethod (after the document map is built);
  assembly load failures only appear in ExportExceptions after a partial export
  already ran.
  2. Seam (old thing touched):
    - Exporter.cs (exporter) Export() line 43 — real export entry point; must be
  skipped in dry-run
    - Exporter.cs (exporter) ExportToOutputMethod() line 111 — where
  config.IsValid is checked and GetIssues() is called; LDEC detail is lost here
  ("There are issues with the LDEC file: {file}", no reason)
    - ExportConfigFile.cs CheckIsValid() line 190 — private; validates XSLT
  node, XSLT file in ZIP, name not empty; exposes only a boolean today
    - DocumentedAssembly.IsCompiled line 124 — File.Exists(FileName); missing
  files silently skipped in GenerateMap(); does not catch PE parse failures from
  AssemblyDef.Create()
    - Exporter.cs (exporter) InitialiseDocumentForExport() line 166 — calls
  document.UpdateDocumentMap() which calls AssemblyDef.Create() per assembly;
  parse exceptions propagate uncaught
    - Program.cs line 92 — constructs Exporter(configuration, 
  parameters.Verbose, _log); will also pass parameters.DryRun
  3. Invariants:
    - When --dry-run is not set, Export() is called as before (no regression)
    - When --dry-run is set, zero bytes are written to output.Location or any
  temp path; PrepareForExport() must not be called
    - GetIssues() on export.Exporter continues to gate the real export (existing
  behavior preserved)
  4. Constraints: No perf requirement for assembly parse validation in dry-run.
  LDEC files resolve relative to exe ApplicationData\.
  5. Non-goals: Not fixing any specific LDEC or assembly. Not changing LDEC
  schema. Not the WPF app or API. Not auto-correcting bad input.
  6. Stop-and-report: If AssemblyDef.Create() does not throw on a
  corrupt-but-extant PE file (i.e., parse failure is silent), stop and report —
  the dry-run assembly check would give a false pass.

  Owner decisions

  - FORK 1: Exit codes: 0 = success, 1 = validation/configuration error, 2 = bad
  parameters. Implement via Environment.Exit() at the end of HandleExport().
  All existing early-return paths that log an error must now exit 1 rather
  than 0.
  - FORK 3: Add List<string> GetConfigIssues() to ExportConfigFile — returns
  human-readable reason strings from CheckIsValid logic (e.g., "XSLT entry 
  missing from export.config", "XSLT file not found in archive", "Export name is
  empty"). Call this in dry-run instead of relying on IsValid boolean alone.
  CheckIsValid remains private; GetConfigIssues() is the new public surface.

  Goals

  1. exporter mylib.dll -d -to c:\out -format web-msdn.ldec with all valid
  inputs → stdout lists each (LDEC file, target location) pair that would be
  written; no files created under c:\out; exits 0
  2. exporter mylib.dll -d -format missing.ldec -to c:\out → output names
  missing.ldec and each string returned by GetConfigIssues(); exits 1
  3. exporter mylib.dll -d -to c:\out with a DocumentedAssembly whose file does
  not exist → output names the missing assembly file; no partial export; exits 1
  4. exporter mylib.dll -d -to c:\out with an extant but unreadable/corrupt PE →
  output names the file and the exception message from AssemblyDef.Create(); no
  partial export; exits 1
  5. exporter mylib.dll -to c:\out -format web-msdn.ldec (no -d) → real export
  runs as before; exit 0 on clean run, 1 on validation failure (exit codes now
  explicit where they were previously implicit 0)
  6. ExportConfigFile.GetConfigIssues() returns a non-empty list for each known
  invalid state (no XSLT node, XSLT file absent from ZIP, empty name); returns
  empty list when valid — unit-testable without a real export

  Standing context

  - UI: N/A
  - Tests: Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Un
  it/ProgramTests.cs — NUnit 3 + Moq; Program is injected with
  IUserInterface/ILog mocks
  - Other: CLAUDE.md §Architecture; Exporter.cs (exporter) uses
  System.IO.File/Path directly in places — not all I/O goes through IFileSystem

  Harness

  - Verifier: dotnet test 
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftw
  are.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj
  - Permissions:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Program.cs,
  Source/TheBoxSoftware.Documentation/Exporting/ExportConfigFile.cs, exporter
  test project
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  All three packets are now complete. All forks resolved. Leaves A → B → C in
  dependency order; B and C can run in parallel once A is merged.

✻ Cogitated for 35s

❯ write out the task packets again                                              
● ---
  Task Packet A: Flag parsing and help text

  Intent

  As an automation engineer, I want -d/--dry-run and --verbose flags parsed
  correctly and described in help, so that the exporter accepts the new flags
  without treating them as values.

  Spec

  1. Why / value: Flags that don't appear in PARAMETERS are consumed as values
  for the preceding flag (see ValueIsAParameter). If --dry-run is not
  registered, it silently becomes the value of whatever precedes it.
  2. Seam (old thing touched):
    - Parameters.cs:22 — PARAMETERS string array gates what is recognized as a
  flag vs. a value
    - Parameters.ReadVerbosity() line 94 — only checks "-v", not "--verbose"
    - Parameters.cs has no DryRun field or property
    - Program.IsHelpShown() line 113 — hard-coded help string; no mention of -d
  or --verbose
  3. Invariants:
    - -v continues to set Verbose = true (no regression)
    - -h continues to show help
    - All existing flags (-format, -f, -filters, -to) continue to work
    - A value immediately after a registered flag is not consumed as the flag
  itself
  4. Constraints: N/A
  5. Non-goals: No behavior change to export logic. No long forms for -h,
  -format, -filters, -to, -f.
  6. Stop-and-report: If PARAMETERS has a secondary usage path not visible in
  ValueIsAParameter (e.g., ReadFormats uses Array.IndexOf directly), stop and
  report before adding entries.

  Owner decisions

  - None — no open forks for this leaf.

  Goals

  1. exporter mylib.dll --dry-run → parameters.DryRun == true,
  parameters.Verbose == false
  2. exporter mylib.dll -d → parameters.DryRun == true
  3. exporter mylib.dll --verbose → parameters.Verbose == true,
  parameters.DryRun == false
  4. exporter mylib.dll -v → parameters.Verbose == true (regression)
  5. exporter mylib.dll -d --verbose -to c:\out -format web-msdn.ldec → DryRun 
  && Verbose && To == "c:\out" (combined; no value theft)
  6. exporter mylib.dll -to --dry-run → To reads empty (flag not consumed as
  value for -to)
  7. Help output contains the strings -d, --dry-run and --verbose when -h or no
  args given

  Standing context

  - UI: N/A
  - Tests: Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Un
  it/ParameterTests.cs — NUnit 3, arrange-act-assert, Parameters.Read(string[])
  tested directly
  - Other: CLAUDE.md for build/test commands

  Harness

  - Verifier: dotnet test 
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftw
  are.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj
  - Permissions:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Parameters.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Program.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/Parame
  terTests.cs
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  Task Packet B: Dry-run validation without export

  Intent

  As an automation engineer, I want --dry-run to validate all inputs and report
  what would be exported — without writing any files — so that I can diagnose
  configuration failures in CI before committing to a real run.

  Spec

  1. Why / value: Current failures are silent or post-hoc: LDEC validity is
  checked late inside ExportToOutputMethod (after the document map is built);
  assembly load failures only appear in ExportExceptions after a partial export
  already ran.
  2. Seam (old thing touched):
    - Exporter.cs (exporter) Export() line 43 — real export entry point; must be
  skipped in dry-run
    - Exporter.cs (exporter) ExportToOutputMethod() line 111 — where
  config.IsValid is checked and GetIssues() is called; LDEC detail is lost here
  ("There are issues with the LDEC file: {file}", no reason)
    - ExportConfigFile.cs CheckIsValid() line 190 — private; validates XSLT
  node, XSLT file in ZIP, name not empty; exposes only a boolean today
    - DocumentedAssembly.IsCompiled line 124 — File.Exists(FileName); missing
  files silently skipped in GenerateMap(); does not catch PE parse failures from
  AssemblyDef.Create()
    - Exporter.cs (exporter) InitialiseDocumentForExport() line 166 — calls
  document.UpdateDocumentMap() which calls AssemblyDef.Create() per assembly;
  parse exceptions propagate uncaught
    - Program.cs line 92 — constructs Exporter(configuration, 
  parameters.Verbose, _log); will also pass parameters.DryRun
  3. Invariants:
    - When --dry-run is not set, Export() is called as before (no regression)
    - When --dry-run is set, zero bytes are written to output.Location or any
  temp path; PrepareForExport() must not be called
    - GetIssues() on export.Exporter continues to gate the real export (existing
  behavior preserved)
  4. Constraints: No perf requirement for assembly parse validation in dry-run.
  LDEC files resolve relative to exe ApplicationData\.
  5. Non-goals: Not fixing any specific LDEC or assembly. Not changing LDEC
  schema. Not the WPF app or API. Not auto-correcting bad input.
  6. Stop-and-report: If AssemblyDef.Create() does not throw on a
  corrupt-but-extant PE file (i.e., parse failure is silent), stop and report —
  the dry-run assembly check would give a false pass.

  Owner decisions

  - FORK 1: Exit codes: 0 = success, 1 = validation/configuration error, 2 = bad
  parameters. Implement via Environment.Exit() at the end of HandleExport().
  All existing early-return paths that log an error must now exit 1 rather
  than 0.
  - FORK 3: Add List<string> GetConfigIssues() to ExportConfigFile — returns
  human-readable reason strings from CheckIsValid logic (e.g., "XSLT entry 
  missing from export.config", "XSLT file not found in archive", "Export name is
  empty"). Call this in dry-run instead of relying on the IsValid boolean
  alone. CheckIsValid remains private; GetConfigIssues() is the new public
  surface.

  Goals

  1. exporter mylib.dll -d -to c:\out -format web-msdn.ldec with all valid
  inputs → stdout lists each (LDEC file, target location) pair that would be
  written; no files created under c:\out; exits 0
  2. exporter mylib.dll -d -format missing.ldec -to c:\out → output names
  missing.ldec and each string returned by GetConfigIssues(); exits 1
  3. exporter mylib.dll -d -to c:\out with a DocumentedAssembly whose file does
  not exist → output names the missing assembly file; no partial export; exits 1
  4. exporter mylib.dll -d -to c:\out with an extant but unreadable/corrupt PE →
  output names the file and the exception message from AssemblyDef.Create(); no
  partial export; exits 1
  5. exporter mylib.dll -to c:\out -format web-msdn.ldec (no -d) → real export
  runs as before; exits 0 on clean run, 1 on validation failure (exit codes now
  explicit where previously implicit 0)
  6. ExportConfigFile.GetConfigIssues() returns a non-empty list for each known
  invalid state (no XSLT node, XSLT file absent from ZIP, empty name); returns
  empty list when valid — unit-testable without a real export

  Standing context

  - UI: N/A
  - Tests: Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Un
  it/ProgramTests.cs — NUnit 3 + Moq; Program is injected with
  IUserInterface/ILog mocks
  - Other: CLAUDE.md §Architecture; Exporter.cs (exporter) uses
  System.IO.File/Path directly in places — not all I/O goes through IFileSystem

  Harness

  - Verifier: dotnet test 
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/TheBoxSoftw
  are.DeveloperSuite.LiveDocumenter.Exporter.Tests.csproj
  - Permissions:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Program.cs,
  Source/TheBoxSoftware.Documentation/Exporting/ExportConfigFile.cs, exporter
  test project
  - Parent: Branch — Dry-run validation and verbose skip reporting

  ---
  Task Packet C: Verbose skip reporting

  Intent

  As an automation engineer, I want --verbose to show which members were skipped
  and why — summarised by reason normally, per-member under --dry-run — so that
  I can understand why an export is incomplete without parsing binaries
  manually.

  Spec

  1. Why / value: Members excluded by visibility filters are silently absent
  from output. entryCreator.Created counts included entries only; there is no
  record of what was excluded or why.
  2. Seam (old thing touched):
    - Document.PreEntryAdded() line 236 — sets e.Filter = 
  IsMemberFiltered(e.Member). The member (e.Member) and its MemberAccess
  visibility are available here. No skip data is currently recorded.
    - Document.IsMemberFiltered() line 59 — sole filter gate; always returns
  based on Settings.VisibilityFilters. Reason is always visibility;
  e.Member.MemberAccess gives the specific level.
    - EntryCreator.Created line 73 — counts entries added (post-filter). No
  count or list of filtered entries.
    - Exporter.cs (exporter) InitialiseDocumentForExport() line 168 — logs
  entryCreator.Created (included count only); skip count not computed.
    - _verbose field in Exporter.cs (exporter) line 24 — set but the only
  current use is exporter_ExportStep which logs regardless of _verbose.
    - GroupedNamespaceDocumentMapper.GenerateDocumentForAssembly() line 203 —
  compiler-generated types (IsCompilerGenerated || Name[0] == '<') are skipped
  silently before PreEntryAdded fires and will not appear in SkippedMembers.
  3. Invariants:
    - Without --verbose, no additional output beyond current behavior
    - entryCreator.Created count continues to be logged as today
    - Skip reporting is additive — it does not change which members are exported
    - Per-member enumeration appears only under --dry-run --verbose; --verbose
  alone produces summary counts
  4. Constraints: Verbose summary must not noticeably slow a non-dry-run export
  (the list is built during the document map pass which already iterates every
  member).
  5. Non-goals: Not changing which members are exported. Not adding new filter
  types. Compiler-generated member skips are intentionally excluded from this
  report — they are an implementation detail, not a user-visibility decision.
  6. Stop-and-report: If additional filter paths exist outside
  IsMemberFiltered() (beyond the compiler-generated skip in §2 which is already
  excluded), stop and report before deciding whether to capture them.

  Owner decisions

  - FORK 2: Add IReadOnlyList<SkippedMember> SkippedMembers to Document (in
  TheBoxSoftware.Documentation). SkippedMember carries the ReflectedMember
  reference and a reason string (format: "visibility: {MemberAccess}"). Populate
  it inside the existing Document.PreEntryAdded() handler when e.Filter is set
  to true. This is the only mechanism for skip capture — do not add a second
  event or expose mapper internals.

  Goals

  1. exporter mylib.dll -v with members excluded by visibility filters → output
  after the included-count line contains one summary line per visibility level
  that has skips, e.g., "  Skipped 42 members — visibility: Private"
  2. exporter mylib.dll -v with no excluded members → no skip section in output;
  included-count line unchanged
  3. exporter mylib.dll -d -v → one line per skipped member with fully-qualified
  name and reason; validation results also present; no files written; exits 0
  on valid inputs
  4. exporter mylib.dll (no flags) → no skip output; output identical to today
  (regression)
  5. exporter mylib.dll -v (verbose only, no dry-run) → summary counts only; no
  per-member list
  6. Document.SkippedMembers is empty before UpdateDocumentMap() is called and
  populated after it returns — unit-testable without running a real export

  Standing context

  - UI: N/A
  - Tests:
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests/Unit/ +
  Source/TheBoxSoftware.Documentation.Tests/ for the Document change
  - Other: Document.cs is in TheBoxSoftware.Documentation (netstandard2.0) —
  visible to WPF and API surfaces; adding SkippedMembers is non-breaking but do
  not add the property to public XML doc comments without checking those call
  sites

  Harness

  - Verifier: dotnet test developersuite.sln
  - Permissions: Source/TheBoxSoftware.Documentation/Document.cs,
  Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/Exporter.cs,
  Source/TheBoxSoftware.Documentation.Tests/, exporter test project
  - Parent: Branch — Dry-run validation and verbose skip reporting

✻ Crunched for 1m 7s

❯ pasting usage stats here for record: Session                                  
                                                                                
                                                                                
   Total cost:            $0.98                                                 
   Total duration (API):  6m 57s                                                
   Total duration (wall): 11m 36s                                               
   Total code changes:    0 lines added, 0 lines removed                        
   Usage by model:                                                              
                                                                                
  claude-sonnet-4-6:  1.1k input, 19.9k output, 955.2k cache read, 65.0k cache  
  write ($0.98)                                                                 
  claude-haiku-4-5:  2.7k input, 18 output, 0 cache read, 0 cache write         
  ($0.0028)                                                                     

● Saving to the session cost log.

  Recalled 1 memory, wrote 1 memory (ctrl+o to expand)

● Logged.

✻ Cooked for 12s