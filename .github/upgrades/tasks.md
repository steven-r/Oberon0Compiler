# .NET 10.0 Upgrade - Execution Tasks

## Progress Dashboard

- **Start Date**: [To be set on execution start]
- **Target Framework**: .NET 10.0 (LTS)
- **Strategy**: All-At-Once
- **Total Projects**: 9
- **Status**: Not Started
**Progress**: 7/10 tasks complete (70%) ![70%](https://progress-bar.xyz/70)
### Progress Summary
- Total Tasks: 10
- Completed: 1
- Failed: 1
- Failed: 0
- Remaining: 9

---

## Task List

### [?] TASK-001: Prerequisites Validation *(Completed: 2026-01-30 21:04)*
**Objective**: Verify environment readiness for .NET 10 upgrade

**Actions**:
- [?] (1) Verify .NET 10 SDK is installed
  - Run: `dotnet --list-sdks`
  - Confirm .NET 10.x SDK is available
- [?] (2) Verify current branch is `upgrade-to-NET10`
  - Run: `git branch --show-current`
  - Expected: upgrade-to-NET10
- [?] (3) Verify no pending changes in working directory
  - Run: `git status`
  - Expected: clean working directory
- [?] (4) Verify solution builds on current framework (baseline)
  - Run: `dotnet build C:\code\Oberon0Compiler\oberon0.sln`
  - Expected: Build succeeds with 0 errors

**Verification**:
- .NET 10 SDK available
- Correct branch checked out
- Clean working directory
- Baseline build successful

**On Failure**: Stop execution, resolve prerequisites before continuing

---

### [?] TASK-002: Update All Project Files to .NET 10 *(Completed: 2026-01-30 21:07)*
**Objective**: Update TargetFramework in all 9 projects from net8.0 to net10.0

**Actions**:
- [?] (1) Update Oberon0.System\Oberon0.System.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (2) Update oberon0\Oberon0Compiler.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (3) Update Oberon0.Shared\Oberon0.Shared.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (4) Update Oberon0.CompilerSupport\Oberon0.Test.Support.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (5) Update Oberon0.Generator.MsilBin\Oberon0.Generator.MsilBin.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (6) Update Oberon0.Msil\Oberon0.Msil.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (7) Update Oberon0.System.Tests\Oberon0.System.Tests.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (8) Update UnitTestProject1\Oberon0Compiler.Tests.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`
- [?] (9) Update Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj
  - Change `<TargetFramework>net8.0</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

**Verification**:
- All 9 project files contain `<TargetFramework>net10.0</TargetFramework>`
- No project files remain with net8.0

**On Failure**: Check file paths, verify project files are not read-only

---

### [?] TASK-003: Update xunit Package in Test Projects *(Completed: 2026-01-30 21:10)*
**Objective**: Update xunit from 2.9.3 to latest stable in 4 test projects

**Actions**:
- [?] (1) Check latest stable xunit version
  - Run: `dotnet list C:\code\Oberon0Compiler\oberon0.sln package --outdated`
  - Note the recommended xunit version
- [?] (2) Update xunit in Oberon0.CompilerSupport\Oberon0.Test.Support.csproj
  - Run: `dotnet add C:\code\Oberon0Compiler\Oberon0.CompilerSupport\Oberon0.Test.Support.csproj package xunit`
- [?] (3) Update xunit in Oberon0.System.Tests\Oberon0.System.Tests.csproj
  - Run: `dotnet add C:\code\Oberon0Compiler\Oberon0.System.Tests\Oberon0.System.Tests.csproj package xunit`
- [?] (4) Update xunit in UnitTestProject1\Oberon0Compiler.Tests.csproj
  - Run: `dotnet add C:\code\Oberon0Compiler\UnitTestProject1\Oberon0Compiler.Tests.csproj package xunit`
- [?] (5) Update xunit in Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj
  - Run: `dotnet add C:\code\Oberon0Compiler\Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj package xunit`

**Verification**:
- All 4 test projects reference latest stable xunit version
- xunit versions are consistent across all test projects

**On Failure**: Check NuGet connectivity, verify project file paths

---

### [?] TASK-004: Restore Dependencies *(Completed: 2026-01-30 21:12)*
**Objective**: Restore all NuGet packages for updated projects

**Actions**:
- [?] (1) Restore dependencies for entire solution
  - Run: `dotnet restore C:\code\Oberon0Compiler\oberon0.sln`
  - Verify: All packages restore successfully

**Verification**:
- `dotnet restore` completes without errors
- No package version conflicts reported
- All projects can resolve dependencies

**On Failure**: Review package conflicts, check NuGet sources

---

### [?] TASK-005: Initial Build Attempt *(Completed: 2026-01-30 21:15)*
**Objective**: Build solution to identify API compatibility issues

**Actions**:
- [?] (1) Build entire solution
  - Run: `dotnet build C:\code\Oberon0Compiler\oberon0.sln --no-restore`
  - Expected: Build fails with API compatibility errors
- [?] (2) Review and categorize compilation errors
  - Count System.Composition API errors
  - Count System.CommandLine API errors
  - Note any unexpected errors
- [?] (3) Document errors for resolution tracking
  - Save build output for reference

**Verification**:
- Build attempted successfully (even if it fails)
- Compilation errors identified and categorized
- Expected API errors match assessment (51 instances across 3 projects)

**Expected Outcome**: Build fails with ~51 API compatibility errors

**On Failure**: If unexpected errors occur, investigate before proceeding

---

### [?] TASK-006: Fix System.Composition API Issues in Oberon0Compiler.csproj
**Objective**: Resolve 26 System.Composition API compatibility issues in Oberon0Compiler.csproj

**Priority**: High - This is the most depended-upon project

**Actions**:
- [?] (1) Build project to get specific error messages
  - Run: `dotnet build C:\code\Oberon0Compiler\oberon0\Oberon0Compiler.csproj --no-restore`
- [?] (2) Fix System.Composition.ExportAttribute issues (8 instances)
  - Review compiler errors for ExportAttribute
  - Update attribute usage according to error messages
  - Ref: Plan §Breaking Changes Catalog > System.Composition.ExportAttribute
- [?] (3) Fix System.Composition.Hosting.ContainerConfiguration issues (12 instances)
  - Update ContainerConfiguration constructor calls (3 instances)
  - Update WithAssembly() method calls (3 instances)
  - Update CreateContainer() method calls (3 instances)
  - Update other configuration API calls (3 instances)
  - Ref: Plan §Breaking Changes Catalog > ContainerConfiguration
- [?] (4) Fix System.Composition.Hosting.CompositionHost issues (5 instances)
  - Review CompositionHost usage
  - Update according to new API surface
- [?] (5) Fix System.Composition.CompositionContext.GetExports issues (3 instances)
  - Update GetExports<T>() calls
  - Ref: Plan §Breaking Changes Catalog > GetExports
- [?] (6) Fix System.Composition.MetadataAttributeAttribute issues (3 instances)
  - Update metadata attribute usage
- [?] (7) Rebuild project to verify all fixes
  - Run: `dotnet build C:\code\Oberon0Compiler\oberon0\Oberon0Compiler.csproj --no-restore`
  - Expected: 0 errors

**Verification**:
- Oberon0Compiler.csproj builds with 0 errors
- All 26 System.Composition issues resolved
- No new errors introduced

**On Failure**: 
- Review specific compiler errors
- Consult System.Composition .NET 10 migration documentation
- May need to query assessment data for more details

---

### [?] TASK-007: Fix System.Composition API Issues in Oberon0.Generator.MsilBin.csproj
**Objective**: Resolve 14 System.Composition API compatibility issues in Oberon0.Generator.MsilBin.csproj

**Prerequisites**: TASK-006 completed (fixes from Oberon0Compiler.csproj should inform this task)

**Actions**:
- [?] (1) Build project to get specific error messages
  - Run: `dotnet build C:\code\Oberon0Compiler\Oberon0.Generator.MsilBin\Oberon0.Generator.MsilBin.csproj --no-restore`
- [?] (2) Apply fix patterns from TASK-006
  - Use same patterns for ExportAttribute
  - Use same patterns for ContainerConfiguration
  - Use same patterns for other System.Composition APIs
- [?] (3) Fix all 14 System.Composition issues systematically
  - Work through each compilation error
  - Apply consistent patterns
- [?] (4) Rebuild project to verify all fixes
  - Run: `dotnet build C:\code\Oberon0Compiler\Oberon0.Generator.MsilBin\Oberon0.Generator.MsilBin.csproj --no-restore`
  - Expected: 0 errors

**Verification**:
- Oberon0.Generator.MsilBin.csproj builds with 0 errors
- All 14 System.Composition issues resolved
- Fix patterns consistent with Oberon0Compiler.csproj

**On Failure**:
- Review fix patterns from TASK-006
- Check for project-specific differences
- Consult compilation error messages

---

### [?] TASK-008: Fix System.CommandLine API Issues in Oberon0.Msil.csproj
**Objective**: Resolve 11 System.CommandLine API compatibility issues in Oberon0.Msil.csproj

**Actions**:
- [?] (1) Build project to get specific error messages
  - Run: `dotnet build C:\code\Oberon0Compiler\Oberon0.Msil\Oberon0.Msil.csproj --no-restore`
- [?] (2) Fix System.CommandLine.RootCommand constructor issue (1 instance)
  - Update RootCommand initialization
  - Ref: Plan §Breaking Changes Catalog > RootCommand
- [?] (3) Fix System.CommandLine.Command.Add issues (4 instances)
  - Update command option registration
  - May need to use AddOption() instead of Add()
- [?] (4) Fix System.CommandLine.ArgumentArity issues (4 instances)
  - Update arity type usage (3 instances)
  - Update ExactlyOne property reference (1 instance)
  - Ref: Plan §Breaking Changes Catalog > ArgumentArity
- [?] (5) Fix System.CommandLine.Argument.Arity property issue (1 instance)
  - Update argument arity assignment
- [?] (6) Rebuild project to verify all fixes
  - Run: `dotnet build C:\code\Oberon0Compiler\Oberon0.Msil\Oberon0.Msil.csproj --no-restore`
  - Expected: 0 errors

**Verification**:
- Oberon0.Msil.csproj builds with 0 errors
- All 11 System.CommandLine issues resolved
- No new errors introduced

**On Failure**:
- Review System.CommandLine 2.0.2 release notes
- Check compiler error messages for specific guidance
- May need to query assessment data for more details

---

### [?] TASK-009: Full Solution Build and Test *(Completed: 2026-01-30 21:23)*
**Objective**: Verify entire solution builds and all tests pass

**Actions**:
- [?] (1) Clean solution to ensure fresh build
  - Run: `dotnet clean C:\code\Oberon0Compiler\oberon0.sln`
- [?] (2) Rebuild entire solution
  - Run: `dotnet build C:\code\Oberon0Compiler\oberon0.sln --configuration Release`
  - Expected: Build succeeds with 0 errors
- [?] (3) Review any new warnings
  - Document new warnings (if any)
  - Determine if warnings need addressing
- [?] (4) Run all tests
  - Run: `dotnet test C:\code\Oberon0Compiler\oberon0.sln --configuration Release --no-build`
  - Expected: All tests pass
- [?] (5) Review test results
  - Document test count and pass/fail statistics
  - Compare with baseline (if available)

**Verification**:
- Solution builds with 0 errors
- All tests execute successfully
- All tests pass
- No critical new warnings

**On Failure**:
- Investigate build errors (should not occur if previous tasks succeeded)
- Investigate test failures
- Do NOT proceed to commit until all tests pass

---

### [?] TASK-010: Commit Changes *(Completed: 2026-01-30 21:30)*
**Objective**: Commit all upgrade changes to upgrade-to-NET10 branch

**Prerequisites**: TASK-009 completed successfully (all builds and tests pass)

**Actions**:
- [?] (1) Review all changes
  - Run: `git status`
  - Run: `git diff` to review changes
  - Verify only expected files modified
- [?] (2) Stage all project file changes
  - Run: `git add **/*.csproj`
- [?] (3) Stage all code changes (API fixes)
  - Run: `git add oberon0/**/*.cs`
  - Run: `git add Oberon0.Generator.MsilBin/**/*.cs`
  - Run: `git add Oberon0.Msil/**/*.cs`
- [?] (4) Review staged changes
  - Run: `git diff --staged`
  - Verify all changes are intentional
- [?] (5) Commit with descriptive message
- [?] (5) Commit with descriptive message
    ```
    git commit -m "Upgrade to .NET 10.0

- Update all 9 projects from net8.0 to net10.0
- Update xunit to latest stable in 4 test projects
- Fix System.Composition API compatibility (40 instances)
- Fix System.CommandLine API compatibility (11 instances)
- All builds pass, all tests pass"
    ```
- [?] (6) Verify commit
  - Run: `git log -1`
  - Confirm commit message and files included

**Verification**:
- All changes committed to upgrade-to-NET10 branch
- Commit message is descriptive
- Working directory is clean

**On Failure**: Review uncommitted changes, ensure nothing missed

---

## Execution Log

[Log entries will be added here as tasks are executed]

---

## Notes

- **Strategy**: All-At-Once - All projects upgraded simultaneously
- **Branch**: upgrade-to-NET10
- **Source Branch**: feature/issue-67-Create_LLVM_output
- **Rollback**: If critical issues arise, can revert commit or reset branch

### Critical Success Factors
1. All 9 projects must target net10.0
2. xunit updated in 4 test projects
3. All 51 API issues resolved (40 System.Composition + 11 System.CommandLine)
4. Solution builds with 0 errors
5. All tests pass

### References
- Plan: C:\code\Oberon0Compiler\.github\upgrades\plan.md
- Assessment: C:\code\Oberon0Compiler\.github\upgrades\assessment.md
