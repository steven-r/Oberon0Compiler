# .NET 10.0 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Migration Plans](#project-by-project-migration-plans)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
Upgrade all projects in the Oberon0Compiler solution from **.NET 8.0** to **.NET 10.0 (Long Term Support)**.

### Scope
- **Total Projects**: 9 (all SDK-style)
  - 5 Class Libraries
  - 4 Test Projects
- **Current State**: All projects targeting net8.0
- **Target State**: All projects targeting net10.0
- **Total Codebase**: 13,690 lines of code
- **Estimated Impact**: 51+ lines requiring modification (0.4% of codebase)

### Discovered Metrics
- **Projects**: 9 total, all require upgrade
- **NuGet Packages**: 13 total, 1 needs update (xunit 2.9.3 ? latest)
- **Dependency Depth**: 3 levels (shallow, manageable)
- **Circular Dependencies**: None
- **API Compatibility**: 51 source incompatible APIs concentrated in 3 projects
- **Security Vulnerabilities**: None

### Selected Strategy
**All-At-Once Strategy** - All projects will be upgraded simultaneously in a single atomic operation.

### Rationale for All-At-Once
- **Small solution size**: 9 projects (well below 30-project threshold)
- **Homogeneous codebase**: All projects already on .NET 8.0
- **Simple dependency structure**: 3-level depth, no circular dependencies
- **Low external complexity**: 13 packages, all compatible except 1 outdated test package
- **No blocking issues**: No security vulnerabilities or incompatible packages
- **Clear compatibility path**: All API issues are source incompatible (fixable with compilation)

### Critical Issues
1. **API Compatibility**: 51 source incompatible API calls in 3 projects
   - System.Composition APIs: 34 instances across Oberon0Compiler.csproj and Oberon0.Generator.MsilBin.csproj
   - System.CommandLine APIs: 11 instances in Oberon0.Msil.csproj
   - Remaining 6 instances scattered
2. **Outdated Test Package**: xunit 2.9.3 in 4 test projects (should be updated to latest stable)

### Complexity Classification
**Simple Solution** - Suitable for fast-track all-at-once upgrade approach

### Iteration Strategy
Phase-based detail generation with comprehensive project specifications in 2-3 iterations

---

## Migration Strategy

### Selected Approach: All-At-Once Strategy

**All projects will be upgraded simultaneously in a single atomic operation.**

### Justification

This solution exhibits all the ideal characteristics for an all-at-once upgrade:

1. **Solution Size**: 9 projects (well below the 30-project threshold for all-at-once)
2. **Homogeneous Technology**: All projects currently on .NET 8.0 (no mixed framework versions)
3. **Simple Dependency Structure**: 
   - 3-level depth (shallow)
   - No circular dependencies
   - Clear hierarchical relationships
4. **Low External Complexity**: 
   - Only 13 NuGet packages total
   - 12 packages are already compatible with .NET 10.0
   - 1 outdated package (xunit) has clear upgrade path
5. **No Blocking Issues**:
   - Zero security vulnerabilities
   - No incompatible packages
   - All API issues are source incompatible (resolved during compilation)
6. **Manageable Scope**: 51+ lines estimated to change (0.4% of 13,690 total LOC)

### All-At-Once Strategy Principles

**Simultaneity**: All project files and package references will be updated together in a single coordinated operation. No intermediate states or multi-targeting.

**Atomic Execution**: The upgrade follows this sequence as one unified task:
1. Update all 9 project files (TargetFramework: net8.0 ? net10.0)
2. Update xunit package in 4 test projects (2.9.3 ? latest stable)
3. Restore dependencies (`dotnet restore`)
4. Build entire solution
5. Fix compilation errors from API compatibility issues
6. Rebuild to verify fixes
7. Verify: Solution builds with 0 errors

**Benefits of This Approach**:
- **Fastest completion**: Single coordinated update vs. phased approach
- **No multi-targeting complexity**: Clean direct upgrade
- **Unified testing**: All projects tested together in final state
- **Simplified source control**: Single commit for entire upgrade
- **Clear success criteria**: Either everything works or nothing does

### Execution Order Within Atomic Operation

While all changes happen simultaneously, validation follows dependency order:

1. **Project File Updates**: All 9 projects updated to net10.0 in parallel
2. **Package Updates**: xunit updated in 4 test projects
3. **Dependency Restore**: Restore all projects
4. **Build**: Build solution (dependency order handled automatically by MSBuild)
5. **API Compatibility Fixes**: Address compilation errors in this order:
   - Oberon0Compiler.csproj (26 API issues - most dependent-upon project)
   - Oberon0.Generator.MsilBin.csproj (14 API issues)
   - Oberon0.Msil.csproj (11 API issues)
6. **Rebuild**: Verify all fixes applied
7. **Test Execution**: Run all test projects

### Risk Management for All-At-Once

**Low Risk Factors**:
- Simple codebase (13k LOC)
- Well-defined API compatibility issues (concentrated in 3 projects)
- No security vulnerabilities forcing immediate action
- All projects already on modern .NET (8.0)

**Mitigation**:
- Dedicated upgrade branch (`upgrade-to-NET10`) already created
- Source branch (`feature/issue-67-Create_LLVM_output`) preserved
- Can revert entire operation if critical issues discovered
- Breaking changes catalog prepared in advance (see §Breaking Changes Catalog)

### Parallel vs Sequential

**Simultaneous Project Updates** with **Sequential Validation**:
- Update all project files at once (parallel)
- MSBuild automatically handles build order (sequential based on dependencies)
- Fix API issues project-by-project following dependency order (sequential)
- Test execution project-by-project (sequential)

### Success Criteria for Atomic Operation

The upgrade is complete when:
- ? All 9 projects target net10.0
- ? xunit updated to latest stable in 4 test projects
- ? `dotnet restore` succeeds
- ? `dotnet build` completes with 0 errors
- ? All API compatibility issues resolved
- ? All test projects execute successfully
- ? No new warnings introduced (or explicitly acknowledged)

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution follows a clean hierarchical structure with 3 dependency levels:

**Level 0 (Foundation - No Dependencies)**
- `Oberon0.System.csproj` - Core system library (322 LOC)

**Level 1 (Core - Depends on Level 0)**
- `Oberon0Compiler.csproj` - Main compiler library (3,907 LOC)
  - Depends on: Oberon0.System
- `Oberon0.System.Tests.csproj` - System tests (168 LOC)
  - Depends on: Oberon0.System

**Level 2 (Extended - Depends on Level 0-1)**
- `Oberon0.Shared.csproj` - Shared utilities (259 LOC)
  - Depends on: Oberon0Compiler
- `Oberon0.Test.Support.csproj` - Test support library (212 LOC)
  - Depends on: Oberon0Compiler
- `Oberon0.Generator.MsilBin.csproj` - MSIL binary generator (1,908 LOC)
  - Depends on: Oberon0Compiler, Oberon0.System, Oberon0.Shared

**Level 3 (Top - Test Projects)**
- `Oberon0.Msil.csproj` - MSIL command-line tool (158 LOC)
  - Depends on: Oberon0Compiler, Oberon0.System, Oberon0.Generator.MsilBin
- `Oberon0Compiler.Tests.csproj` - Compiler tests (4,253 LOC)
  - Depends on: Oberon0Compiler, Oberon0.Test.Support
- `Oberon0.Generator.MsilBin.Tests.csproj` - Generator tests (2,503 LOC)
  - Depends on: Oberon0Compiler, Oberon0.System, Oberon0.Msil, Oberon0.Generator.MsilBin, Oberon0.Test.Support

### Project Groupings for All-At-Once Migration

Since this is an **All-At-Once Strategy**, all projects will be upgraded simultaneously. However, for understanding and validation purposes, they are organized by dependency level:

**Phase 1: Atomic Upgrade of All Projects**
All projects listed below will be updated in a single coordinated operation:

1. **Foundation Projects** (can build independently):
   - Oberon0.System.csproj

2. **Core Projects** (depend on foundation):
   - Oberon0Compiler.csproj
   - Oberon0.System.Tests.csproj

3. **Extended Projects** (depend on core):
   - Oberon0.Shared.csproj
   - Oberon0.Test.Support.csproj
   - Oberon0.Generator.MsilBin.csproj

4. **Top-Level Projects** (depend on extended):
   - Oberon0.Msil.csproj
   - Oberon0Compiler.Tests.csproj
   - Oberon0.Generator.MsilBin.Tests.csproj

### Critical Path
Oberon0.System ? Oberon0Compiler ? Oberon0.Generator.MsilBin ? Oberon0.Msil

### Circular Dependencies
**None** - Clean dependency hierarchy enables straightforward upgrade

### Key Dependency Relationships
- **Oberon0.System** is the most depended-upon project (5 dependants)
- **Oberon0Compiler** is the second most depended-upon (6 dependants)
- **Test projects** have the most dependencies (Oberon0.Generator.MsilBin.Tests depends on 5 other projects)
- All dependencies are project-to-project references (no transitive package conflicts expected)

---

## Project-by-Project Migration Plans

### Oberon0.System\Oberon0.System.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: ClassLibrary (SDK-style)
- Dependencies: None (foundation project)
- Dependants: 5 projects
- Lines of Code: 322
- Files: 6
- NuGet Packages: 0
- Risk Level: ?? Low

**Target State**: 
- Target Framework: net10.0
- No package updates required

**Migration Steps**:

1. **Prerequisites**
   - None - foundation project with no dependencies

2. **Framework Update**
   - Update `TargetFramework` in `Oberon0.System\Oberon0.System.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - None required

4. **Expected Breaking Changes**
   - None - no API compatibility issues detected

5. **Code Modifications**
   - None expected

6. **Testing Strategy**
   - Build project independently: `dotnet build Oberon0.System\Oberon0.System.csproj`
   - Verify 0 errors and no new warnings
   - Execute Oberon0.System.Tests after its upgrade

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] `dotnet restore` succeeds
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] No new warnings introduced
   - [ ] Dependent projects can reference successfully

---

### oberon0\Oberon0Compiler.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: ClassLibrary (SDK-style)
- Dependencies: Oberon0.System
- Dependants: 6 projects
- Lines of Code: 3,907
- Files: 65
- NuGet Packages: 4 (Antlr4.Runtime.Standard, Antlr4BuildTasks, JetBrains.Annotations, System.Composition.TypedParts)
- API Issues: 26 source incompatible (System.Composition)
- Risk Level: ?? Medium (API compatibility issues)

**Target State**: 
- Target Framework: net10.0
- System.Composition API updates required

**Migration Steps**:

1. **Prerequisites**
   - Oberon0.System.csproj must be updated to net10.0 first
   - Review System.Composition breaking changes for .NET 10

2. **Framework Update**
   - Update `TargetFramework` in `oberon0\Oberon0Compiler.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - No package version changes required
   - All packages compatible with net10.0:
     - Antlr4.Runtime.Standard 4.13.1
     - Antlr4BuildTasks 12.14.0
     - JetBrains.Annotations 2025.2.4
     - System.Composition.TypedParts 10.0.2

4. **Expected Breaking Changes**
   - **System.Composition API Changes** (26 instances):
     - `System.Composition.ExportAttribute` constructor changes (8 instances)
     - `System.Composition.Hosting.ContainerConfiguration` API changes (12 instances total):
       - Constructor signature changes (3 instances)
       - `WithAssembly()` method changes (3 instances)
       - `CreateContainer()` method changes (3 instances)
     - `System.Composition.Hosting.CompositionHost` API changes (5 instances)
     - `System.Composition.CompositionContext.GetExports<T>()` changes (3 instances)
     - `System.Composition.MetadataAttributeAttribute` constructor changes (3 instances)
   
   **Files Likely Affected** (based on assessment):
   - Files using MEF/composition patterns
   - Plugin loading infrastructure
   - Dependency injection setup

5. **Code Modifications**
   - **Action Required**: Address System.Composition API compatibility issues
   - Specific changes depend on breaking change details from compilation errors
   - Common patterns:
     - Update `[Export]` attribute usage
     - Update composition container initialization
     - Update export retrieval patterns
   - Estimated: 26+ lines to modify across affected files

6. **Testing Strategy**
   - Build project: `dotnet build oberon0\Oberon0Compiler.csproj`
   - Review all compilation errors
   - Fix API compatibility issues iteratively
   - Rebuild after each fix batch
   - Execute Oberon0Compiler.Tests after its upgrade
   - Verify plugin/composition functionality

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] All packages remain at compatible versions
   - [ ] `dotnet restore` succeeds
   - [ ] All System.Composition API issues resolved
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] No new warnings introduced
   - [ ] Dependent projects can reference successfully
   - [ ] Composition/MEF functionality works correctly

---

### Oberon0.Shared\Oberon0.Shared.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: ClassLibrary (SDK-style)
- Dependencies: Oberon0Compiler
- Dependants: 1 project
- Lines of Code: 259
- Files: 5
- NuGet Packages: 2 (Microsoft.CodeAnalysis.CSharp, Microsoft.Extensions.DependencyModel)
- Risk Level: ?? Low

**Target State**: 
- Target Framework: net10.0
- No package updates or API fixes required

**Migration Steps**:

1. **Prerequisites**
   - Oberon0Compiler.csproj must be updated to net10.0 first

2. **Framework Update**
   - Update `TargetFramework` in `Oberon0.Shared\Oberon0.Shared.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - No package version changes required
   - All packages compatible with net10.0:
     - Microsoft.CodeAnalysis.CSharp 5.0.0
     - Microsoft.Extensions.DependencyModel 10.0.2

4. **Expected Breaking Changes**
   - None - no API compatibility issues detected

5. **Code Modifications**
   - None expected

6. **Testing Strategy**
   - Build project: `dotnet build Oberon0.Shared\Oberon0.Shared.csproj`
   - Verify 0 errors and no new warnings
   - Validate via dependent projects (Oberon0.Generator.MsilBin)

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] `dotnet restore` succeeds
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] No new warnings introduced
   - [ ] Dependent projects can reference successfully

---

### Oberon0.CompilerSupport\Oberon0.Test.Support.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: ClassLibrary (SDK-style)
- Dependencies: Oberon0Compiler
- Dependants: 2 projects
- Lines of Code: 212
- Files: 3
- NuGet Packages: 4 (Antlr4.Runtime.Standard, Antlr4BuildTasks, JetBrains.Annotations, xunit)
- Outdated Package: xunit 2.9.3
- Risk Level: ?? Low

**Target State**: 
- Target Framework: net10.0
- xunit updated to latest stable

**Migration Steps**:

1. **Prerequisites**
   - Oberon0Compiler.csproj must be updated to net10.0 first

2. **Framework Update**
   - Update `TargetFramework` in `Oberon0.CompilerSupport\Oberon0.Test.Support.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - Update xunit to latest stable version:
     - Current: 2.9.3
     - Target: Latest stable (check NuGet for current version, likely 2.9.x or 3.x)
   - No changes required for:
     - Antlr4.Runtime.Standard 4.13.1
     - Antlr4BuildTasks 12.14.0
     - JetBrains.Annotations 2025.2.4

4. **Expected Breaking Changes**
   - xunit: Minimal breaking changes expected within same major version
   - If upgrading to xunit 3.x, review migration guide

5. **Code Modifications**
   - None expected for minor xunit version updates
   - If xunit 3.x: Review test attribute changes (unlikely major impact)

6. **Testing Strategy**
   - Build project: `dotnet build Oberon0.CompilerSupport\Oberon0.Test.Support.csproj`
   - Verify 0 errors and no new warnings
   - Execute dependent test projects (Oberon0Compiler.Tests, Oberon0.Generator.MsilBin.Tests)

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] xunit updated to latest stable
   - [ ] `dotnet restore` succeeds
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] No new warnings introduced
   - [ ] Test helper methods still compile
   - [ ] Dependent test projects can reference successfully

---

### Oberon0.Generator.MsilBin\Oberon0.Generator.MsilBin.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: ClassLibrary (SDK-style)
- Dependencies: Oberon0Compiler, Oberon0.System, Oberon0.Shared
- Dependants: 2 projects
- Lines of Code: 1,908
- Files: 22
- NuGet Packages: 4 (AnyClone, Microsoft.CodeAnalysis.CSharp, Microsoft.CodeAnalysis.CSharp.Workspaces, Microsoft.Extensions.DependencyModel)
- API Issues: 14 source incompatible (System.Composition)
- Risk Level: ?? Medium (API compatibility issues)

**Target State**: 
- Target Framework: net10.0
- System.Composition API updates required

**Migration Steps**:

1. **Prerequisites**
   - Oberon0Compiler.csproj must be updated to net10.0 first
   - Oberon0.System.csproj must be updated to net10.0 first
   - Oberon0.Shared.csproj must be updated to net10.0 first
   - System.Composition fixes from Oberon0Compiler.csproj should inform this project's fixes

2. **Framework Update**
   - Update `TargetFramework` in `Oberon0.Generator.MsilBin\Oberon0.Generator.MsilBin.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - No package version changes required
   - All packages compatible with net10.0:
     - AnyClone 1.1.6
     - Microsoft.CodeAnalysis.CSharp 5.0.0
     - Microsoft.CodeAnalysis.CSharp.Workspaces 5.0.0
     - Microsoft.Extensions.DependencyModel 10.0.2

4. **Expected Breaking Changes**
   - **System.Composition API Changes** (14 instances):
     - Similar patterns to Oberon0Compiler.csproj
     - `System.Composition.ExportAttribute` usage
     - Composition container initialization
     - Export retrieval patterns
   
   **Files Likely Affected**:
   - Generator plugin infrastructure
   - Binary generator composition setup

5. **Code Modifications**
   - **Action Required**: Address System.Composition API compatibility issues
   - Apply same patterns as Oberon0Compiler.csproj fixes
   - Estimated: 14+ lines to modify across affected files

6. **Testing Strategy**
   - Build project: `dotnet build Oberon0.Generator.MsilBin\Oberon0.Generator.MsilBin.csproj`
   - Review all compilation errors
   - Fix API compatibility issues using patterns from Oberon0Compiler.csproj
   - Rebuild after fixes
   - Execute Oberon0.Generator.MsilBin.Tests after its upgrade

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] All packages remain at compatible versions
   - [ ] `dotnet restore` succeeds
   - [ ] All System.Composition API issues resolved
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] No new warnings introduced
   - [ ] Generator composition functionality works correctly
   - [ ] Dependent projects (Oberon0.Msil) can reference successfully

---

### Oberon0.Msil\Oberon0.Msil.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: DotNetCoreApp (SDK-style)
- Dependencies: Oberon0Compiler, Oberon0.System, Oberon0.Generator.MsilBin
- Dependants: 1 project
- Lines of Code: 158
- Files: 2
- NuGet Packages: 2 (Microsoft.Extensions.DependencyModel, System.CommandLine)
- API Issues: 11 source incompatible (System.CommandLine)
- Risk Level: ?? Medium (API compatibility issues, high % of code impact)

**Target State**: 
- Target Framework: net10.0
- System.CommandLine API updates required

**Migration Steps**:

1. **Prerequisites**
   - Oberon0Compiler.csproj must be updated to net10.0 first
   - Oberon0.System.csproj must be updated to net10.0 first
   - Oberon0.Generator.MsilBin.csproj must be updated to net10.0 first

2. **Framework Update**
   - Update `TargetFramework` in `Oberon0.Msil\Oberon0.Msil.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - No package version changes required
   - All packages compatible with net10.0:
     - Microsoft.Extensions.DependencyModel 10.0.2
     - System.CommandLine 2.0.2

4. **Expected Breaking Changes**
   - **System.CommandLine API Changes** (11 instances):
     - `System.CommandLine.RootCommand` constructor changes (1 instance)
     - `System.CommandLine.Command.Add(Option)` method changes (4 instances)
     - `System.CommandLine.ArgumentArity` API changes (4 instances total):
       - Type usage (3 instances)
       - `ExactlyOne` property (1 instance)
     - `System.CommandLine.Argument.Arity` property changes (1 instance)
   
   **Files Likely Affected**:
   - Program.cs or command-line setup files
   - Command/option registration code
   - High concentration of changes in small codebase (7% of LOC)

5. **Code Modifications**
   - **Action Required**: Address System.CommandLine API compatibility issues
   - Specific changes depend on System.CommandLine 2.0 breaking changes
   - Common patterns:
     - Update `RootCommand` initialization
     - Update option/argument registration with `Add()` method
     - Update `ArgumentArity` usage patterns
   - Estimated: 11+ lines to modify (likely concentrated in main entry point)

6. **Testing Strategy**
   - Build project: `dotnet build Oberon0.Msil\Oberon0.Msil.csproj`
   - Review all compilation errors
   - Fix API compatibility issues iteratively
   - Rebuild after fixes
   - Manual testing: Run command-line tool with various options
   - Verify help text generation
   - Verify argument parsing

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] All packages remain at compatible versions
   - [ ] `dotnet restore` succeeds
   - [ ] All System.CommandLine API issues resolved
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] No new warnings introduced
   - [ ] Command-line tool runs successfully
   - [ ] All command options work correctly
   - [ ] Help text displays correctly

---

### Oberon0.System.Tests\Oberon0.System.Tests.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: DotNetCoreApp (SDK-style)
- Dependencies: Oberon0.System
- Lines of Code: 168
- Files: 4
- NuGet Packages: 4 (coverlet.collector, Microsoft.NET.Test.Sdk, xunit, xunit.runner.visualstudio)
- Outdated Package: xunit 2.9.3
- Risk Level: ?? Low

**Target State**: 
- Target Framework: net10.0
- xunit updated to latest stable

**Migration Steps**:

1. **Prerequisites**
   - Oberon0.System.csproj must be updated to net10.0 first

2. **Framework Update**
   - Update `TargetFramework` in `Oberon0.System.Tests\Oberon0.System.Tests.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - Update xunit to latest stable version:
     - Current: 2.9.3
     - Target: Latest stable (check NuGet, likely 2.9.x or 3.x)
   - No changes required for:
     - coverlet.collector 6.0.4
     - Microsoft.NET.Test.Sdk 18.0.1
     - xunit.runner.visualstudio 3.1.5

4. **Expected Breaking Changes**
   - xunit: Minimal breaking changes expected within same major version
   - If upgrading to xunit 3.x, review migration guide

5. **Code Modifications**
   - None expected for minor xunit version updates

6. **Testing Strategy**
   - Build project: `dotnet build Oberon0.System.Tests\Oberon0.System.Tests.csproj`
   - Execute tests: `dotnet test Oberon0.System.Tests\Oberon0.System.Tests.csproj`
   - Verify all tests pass
   - Review test output for any warnings

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] xunit updated to latest stable
   - [ ] `dotnet restore` succeeds
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] `dotnet test` executes successfully
   - [ ] All tests pass (same count as before upgrade)
   - [ ] No new warnings introduced

---

### UnitTestProject1\Oberon0Compiler.Tests.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: DotNetCoreApp (SDK-style)
- Dependencies: Oberon0Compiler, Oberon0.Test.Support
- Lines of Code: 4,253
- Files: 26
- NuGet Packages: 4 (coverlet.collector, Microsoft.NET.Test.Sdk, xunit, xunit.runner.visualstudio)
- Outdated Package: xunit 2.9.3
- Risk Level: ?? Low

**Target State**: 
- Target Framework: net10.0
- xunit updated to latest stable

**Migration Steps**:

1. **Prerequisites**
   - Oberon0Compiler.csproj must be updated to net10.0 first
   - Oberon0.Test.Support.csproj must be updated to net10.0 first

2. **Framework Update**
   - Update `TargetFramework` in `UnitTestProject1\Oberon0Compiler.Tests.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - Update xunit to latest stable version:
     - Current: 2.9.3
     - Target: Latest stable (check NuGet, likely 2.9.x or 3.x)
   - No changes required for:
     - coverlet.collector 6.0.4
     - Microsoft.NET.Test.Sdk 18.0.1
     - xunit.runner.visualstudio 3.1.5

4. **Expected Breaking Changes**
   - xunit: Minimal breaking changes expected within same major version
   - If upgrading to xunit 3.x, review migration guide

5. **Code Modifications**
   - None expected for minor xunit version updates
   - Large test suite (26 files, 4,253 LOC) may surface edge cases

6. **Testing Strategy**
   - Build project: `dotnet build UnitTestProject1\Oberon0Compiler.Tests.csproj`
   - Execute tests: `dotnet test UnitTestProject1\Oberon0Compiler.Tests.csproj`
   - Verify all tests pass
   - Pay attention to tests validating composition/MEF functionality (may be affected by Oberon0Compiler fixes)

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] xunit updated to latest stable
   - [ ] `dotnet restore` succeeds
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] `dotnet test` executes successfully
   - [ ] All tests pass (same count as before upgrade)
   - [ ] No new warnings introduced
   - [ ] Composition/MEF tests still pass

---

### Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj

**Current State**: 
- Target Framework: net8.0
- Project Type: DotNetCoreApp (SDK-style)
- Dependencies: Oberon0Compiler, Oberon0.System, Oberon0.Msil, Oberon0.Generator.MsilBin, Oberon0.Test.Support
- Lines of Code: 2,503
- Files: 24
- NuGet Packages: 4 (coverlet.collector, Microsoft.NET.Test.Sdk, xunit, xunit.runner.visualstudio)
- Outdated Package: xunit 2.9.3
- Risk Level: ?? Low

**Target State**: 
- Target Framework: net10.0
- xunit updated to latest stable

**Migration Steps**:

1. **Prerequisites**
   - All dependency projects must be updated to net10.0 first:
     - Oberon0Compiler.csproj
     - Oberon0.System.csproj
     - Oberon0.Msil.csproj
     - Oberon0.Generator.MsilBin.csproj
     - Oberon0.Test.Support.csproj

2. **Framework Update**
   - Update `TargetFramework` in `Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj`:
     ```xml
     <TargetFramework>net10.0</TargetFramework>
     ```

3. **Package Updates**
   - Update xunit to latest stable version:
     - Current: 2.9.3
     - Target: Latest stable (check NuGet, likely 2.9.x or 3.x)
   - No changes required for:
     - coverlet.collector 6.0.4
     - Microsoft.NET.Test.Sdk 18.0.1
     - xunit.runner.visualstudio 3.1.5

4. **Expected Breaking Changes**
   - xunit: Minimal breaking changes expected within same major version
   - If upgrading to xunit 3.x, review migration guide

5. **Code Modifications**
   - None expected for minor xunit version updates
   - May indirectly test System.Composition and System.CommandLine fixes

6. **Testing Strategy**
   - Build project: `dotnet build Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj`
   - Execute tests: `dotnet test Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj`
   - Verify all tests pass
   - These tests validate generator functionality end-to-end

7. **Validation Checklist**
   - [ ] Project file updated to net10.0
   - [ ] xunit updated to latest stable
   - [ ] `dotnet restore` succeeds
   - [ ] `dotnet build` succeeds with 0 errors
   - [ ] `dotnet test` executes successfully
   - [ ] All tests pass (same count as before upgrade)
   - [ ] No new warnings introduced
   - [ ] Generator tests validate fixes work correctly

---

## Package Update Reference

### Overview
Only **1 package** requires updating across the solution: **xunit 2.9.3**

All other packages (12 of 13 total) are already compatible with .NET 10.0.

### Package Updates by Scope

#### Test Framework Updates (4 projects)

| Package | Current | Target | Projects Affected | Update Reason |
|---------|---------|--------|-------------------|---------------|
| xunit | 2.9.3 | Latest stable (2.9.x or 3.x) | 4 projects | Outdated test framework version |

**Projects requiring xunit update:**
- Oberon0.Test.Support.csproj
- Oberon0.System.Tests.csproj
- Oberon0Compiler.Tests.csproj
- Oberon0.Generator.MsilBin.Tests.csproj

**Update Strategy:**
- Check latest stable xunit version on NuGet at time of upgrade
- If latest is 2.9.x: Simple version bump, no breaking changes expected
- If latest is 3.x: Review xunit 3.0 migration guide, minimal changes expected

### Compatible Packages (No Updates Required)

All remaining packages are already compatible with .NET 10.0:

| Package | Version | Projects | Notes |
|---------|---------|----------|-------|
| Antlr4.Runtime.Standard | 4.13.1 | 2 | ? Compatible |
| Antlr4BuildTasks | 12.14.0 | 2 | ? Compatible |
| AnyClone | 1.1.6 | 1 | ? Compatible |
| coverlet.collector | 6.0.4 | 3 | ? Compatible |
| JetBrains.Annotations | 2025.2.4 | 2 | ? Compatible |
| Microsoft.CodeAnalysis.CSharp | 5.0.0 | 2 | ? Compatible |
| Microsoft.CodeAnalysis.CSharp.Workspaces | 5.0.0 | 1 | ? Compatible |
| Microsoft.Extensions.DependencyModel | 10.0.2 | 3 | ? Compatible |
| Microsoft.NET.Test.Sdk | 18.0.1 | 3 | ? Compatible |
| System.CommandLine | 2.0.2 | 1 | ? Compatible (API changes, not version) |
| System.Composition.TypedParts | 10.0.2 | 1 | ? Compatible (API changes, not version) |
| xunit.runner.visualstudio | 3.1.5 | 3 | ? Compatible |

### Package Update Command Reference

```bash
# Update xunit in all 4 test projects
dotnet add Oberon0.CompilerSupport\Oberon0.Test.Support.csproj package xunit
dotnet add Oberon0.System.Tests\Oberon0.System.Tests.csproj package xunit
dotnet add UnitTestProject1\Oberon0Compiler.Tests.csproj package xunit
dotnet add Oberon0.Generator.MsilBin.Tests\Oberon0.Generator.MsilBin.Tests.csproj package xunit
```

*Note: `dotnet add package` without version will install latest stable*

---

## Breaking Changes Catalog

### Overview
This section documents expected breaking changes from .NET 8.0 ? .NET 10.0 that affect this solution.

### API Breaking Changes

#### 1. System.Composition (MEF) API Changes

**Impact**: 40 instances across 2 projects
- Oberon0Compiler.csproj: 26 instances
- Oberon0.Generator.MsilBin.csproj: 14 instances

**Affected APIs:**

##### `System.Composition.ExportAttribute` Constructor (8 instances)
- **Change**: Constructor signature modified
- **Files Affected**: Classes using `[Export]` attributes
- **Fix Strategy**: Update attribute usage to match new constructor signature
- **Example Fix Pattern**:
  ```csharp
  // Before (NET 8.0)
  [Export(typeof(IGenerator))]
  
  // After (NET 10.0) - exact pattern depends on breaking change details
  [Export(typeof(IGenerator), ...)]
  ```

##### `System.Composition.Hosting.ContainerConfiguration` (12 instances)
- **Constructor Change** (3 instances): `new ContainerConfiguration()` signature modified
- **`WithAssembly()` Method** (3 instances): Method signature or behavior changed
- **`CreateContainer()` Method** (3 instances): Return type or parameters modified
- **Files Affected**: Composition container initialization code
- **Fix Strategy**: Update container setup to match new API patterns
- **Example Fix Pattern**:
  ```csharp
  // Before (NET 8.0)
  var config = new ContainerConfiguration()
      .WithAssembly(assembly);
  var container = config.CreateContainer();
  
  // After (NET 10.0) - exact pattern depends on breaking change details
  var config = ContainerConfiguration.Create()
      .WithAssembly(assembly);
  var container = config.CreateContainer();
  ```

##### `System.Composition.Hosting.CompositionHost` (5 instances)
- **Change**: Class API surface modified
- **Files Affected**: Code using `CompositionHost` directly
- **Fix Strategy**: Update host interactions to match new API
- **Impact**: Central composition infrastructure

##### `System.Composition.CompositionContext.GetExports<T>()` (3 instances)
- **Change**: Method signature or return type modified
- **Files Affected**: Export retrieval code
- **Fix Strategy**: Update export retrieval patterns
- **Example Fix Pattern**:
  ```csharp
  // Before (NET 8.0)
  var exports = context.GetExports<IGenerator>();
  
  // After (NET 10.0) - exact pattern depends on breaking change details
  var exports = context.GetExports<IGenerator>(...);
  ```

##### `System.Composition.MetadataAttributeAttribute` (6 instances)
- **Change**: Constructor and/or usage pattern modified
- **Files Affected**: Custom metadata attributes
- **Fix Strategy**: Update metadata attribute definitions

**General Mitigation Strategy for System.Composition**:
1. Build projects after framework update to identify exact breaking changes
2. Review compiler error messages for specific API guidance
3. Consult System.Composition .NET 10 migration notes
4. Apply fixes systematically, starting with Oberon0Compiler.csproj
5. Reuse fix patterns in Oberon0.Generator.MsilBin.csproj

---

#### 2. System.CommandLine API Changes

**Impact**: 11 instances in 1 project
- Oberon0.Msil.csproj: 11 instances (7% of project LOC)

**Affected APIs:**

##### `System.CommandLine.RootCommand` Constructor (1 instance)
- **Change**: Constructor parameters modified
- **Files Affected**: Program.cs or main entry point
- **Fix Strategy**: Update `RootCommand` initialization
- **Example Fix Pattern**:
  ```csharp
  // Before (NET 8.0)
  var rootCommand = new RootCommand("Description");
  
  // After (NET 10.0) - exact pattern depends on breaking change details
  var rootCommand = new RootCommand { Description = "Description" };
  ```

##### `System.CommandLine.Command.Add(Option)` Method (4 instances)
- **Change**: Method signature or fluent API pattern modified
- **Files Affected**: Command option registration code
- **Fix Strategy**: Update option registration calls
- **Example Fix Pattern**:
  ```csharp
  // Before (NET 8.0)
  command.Add(new Option<string>("--name"));
  
  // After (NET 10.0) - exact pattern depends on breaking change details
  command.AddOption(new Option<string>("--name"));
  ```

##### `System.CommandLine.ArgumentArity` (4 instances)
- **Type Usage** (3 instances): Arity API surface changed
- **`ExactlyOne` Property** (1 instance): Property renamed or behavior modified
- **Files Affected**: Argument arity configuration
- **Fix Strategy**: Update arity specifications
- **Example Fix Pattern**:
  ```csharp
  // Before (NET 8.0)
  Arity = ArgumentArity.ExactlyOne
  
  // After (NET 10.0) - exact pattern depends on breaking change details
  Arity = ArgumentArity.Single
  ```

##### `System.CommandLine.Argument.Arity` Property (1 instance)
- **Change**: Property type or setter behavior modified
- **Files Affected**: Argument definitions
- **Fix Strategy**: Update argument arity assignment

**General Mitigation Strategy for System.CommandLine**:
1. Review System.CommandLine 2.0.2 release notes and migration guide
2. Build Oberon0.Msil.csproj after framework update
3. Fix compilation errors systematically
4. Test command-line tool thoroughly after fixes (all options, help text, etc.)

---

### Framework-Level Breaking Changes

#### General .NET 8 ? 10 Changes
No solution-specific framework breaking changes detected beyond API changes above.

**Standard .NET 10 Considerations:**
- Performance improvements may affect timing-sensitive code
- Garbage collection improvements (unlikely to affect this solution)
- Some obsolete APIs fully removed (none detected in this solution)

---

### Package-Level Breaking Changes

#### xunit 2.9.3 ? Latest
- **If upgrading to 2.9.x**: No breaking changes expected
- **If upgrading to 3.x**: Review xunit 3.0 migration guide
  - Assert API may have new overloads
  - Test discovery may have minor changes
  - Minimal impact expected (xunit 3 is largely backward compatible)

---

### Risk Assessment of Breaking Changes

| Change Category | Instances | Risk | Mitigation Complexity |
|----------------|-----------|------|----------------------|
| System.Composition | 40 | ?? Medium | Medium - Systematic fix patterns |
| System.CommandLine | 11 | ?? Medium | Medium - Concentrated in one file |
| xunit Update | 4 projects | ?? Low | Low - Version bump only |

**Overall Risk**: ?? Medium - Manageable with systematic approach

---

### Validation Strategy for Breaking Changes

After applying fixes:
1. **Compilation**: All projects must build with 0 errors
2. **Unit Tests**: All tests must pass (validates System.Composition fixes)
3. **Manual Testing**: Oberon0.Msil command-line tool (validates System.CommandLine fixes)
4. **Integration**: Full solution functionality test

---

## Testing & Validation Strategy

[To be filled]

---

## Complexity & Effort Assessment

### Overall Complexity: Simple

This is a straightforward upgrade with well-understood scope and manageable API compatibility issues.

### Per-Project Complexity

| Project | Complexity | Dependencies | API Issues | Package Updates | Risk Factors |
|---------|-----------|--------------|------------|-----------------|--------------|
| Oberon0.System.csproj | ?? Low | 0 | 0 | 0 | None |
| Oberon0Compiler.csproj | ?? Medium | 1 | 26 | 0 | System.Composition API changes concentrated here |
| Oberon0.Shared.csproj | ?? Low | 1 | 0 | 0 | None |
| Oberon0.Test.Support.csproj | ?? Low | 1 | 0 | 1 | xunit update (low risk) |
| Oberon0.Generator.MsilBin.csproj | ?? Medium | 3 | 14 | 0 | System.Composition API changes |
| Oberon0.Msil.csproj | ?? Medium | 3 | 11 | 0 | System.CommandLine API changes, high % code impact |
| Oberon0.System.Tests.csproj | ?? Low | 1 | 0 | 1 | xunit update (low risk) |
| Oberon0Compiler.Tests.csproj | ?? Low | 2 | 0 | 1 | xunit update (low risk) |
| Oberon0.Generator.MsilBin.Tests.csproj | ?? Low | 5 | 0 | 1 | xunit update (low risk) |

### Complexity Rating Explanation

**?? Low Complexity** (6 projects):
- No API compatibility issues
- Simple or no package updates
- Small to medium codebase
- Few dependencies

**?? Medium Complexity** (3 projects):
- Source incompatible API issues requiring code changes
- Oberon0Compiler.csproj: 26 API issues (0.7% of 3,907 LOC)
- Oberon0.Generator.MsilBin.csproj: 14 API issues (0.7% of 1,908 LOC)
- Oberon0.Msil.csproj: 11 API issues (7.0% of 158 LOC - highest percentage)

**?? High Complexity**: None

### Phase Complexity Assessment

**Phase 1: Atomic Upgrade (All Projects)**
- Complexity: Medium
- All projects upgraded simultaneously
- 3 projects require API compatibility fixes
- 4 projects require xunit package update
- Dependency ordering handled automatically by MSBuild

### Effort Factors

**Reducing Effort**:
- ? All projects already on .NET 8.0 (modern framework)
- ? All projects are SDK-style (simple project files)
- ? No circular dependencies
- ? No security vulnerabilities
- ? API issues concentrated in specific namespaces (System.Composition, System.CommandLine)
- ? Well-defined breaking changes from .NET 8 ? 10

**Increasing Effort**:
- ?? 51 source incompatible API calls across 3 projects
- ?? System.Composition and System.CommandLine APIs may have non-trivial changes
- ?? Limited test coverage visibility (requires execution to validate)

### Relative Effort Estimate

**Not estimated in time units** - relative complexity indicators only:

- **Project File Updates**: Low effort (mechanical changes)
- **Package Updates**: Low effort (single package, clear upgrade path)
- **API Compatibility Fixes**: Medium effort (51 instances, concentrated in 3 projects)
  - System.Composition (40 instances): Medium effort per fix
  - System.CommandLine (11 instances): Medium effort per fix
- **Build & Test Validation**: Low effort (automated)
- **Overall**: Medium effort upgrade

### Resource Requirements

**Skills Required**:
- .NET development experience (proficient with C# and .NET SDK)
- Understanding of System.Composition (MEF) patterns
- Familiarity with System.CommandLine library
- Build and testing automation experience

**Parallel Capacity**:
- All-at-once strategy requires coordination but not parallelization
- Single developer can execute entire upgrade
- API fixes can be addressed project-by-project in dependency order

### Risk vs Effort Balance

**Low Risk, Medium Effort** - Ideal combination for all-at-once strategy
- Clear scope
- Well-understood compatibility issues
- No blocking unknowns
- Predictable execution path

---

## Source Control Strategy

[To be filled]

---

## Success Criteria

[To be filled]
