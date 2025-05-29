# Scratchpad: xUnit Namespace Not Found Issue

## Problem Summary
- The test file `UserControllerTest.cs` uses `[Fact]` and `Assert` from xUnit, but VS Code/Omnisharp underlines them red and the build fails with errors like:
  - `The type or namespace name 'Xunit' could not be found (are you missing a using directive or an assembly reference?)`
  - `The type or namespace name 'Fact' could not be found...`
- All xUnit and runner packages are present in the test project and at compatible versions for .NET 8.0.
- The test project is not nested inside the main project directory, and the structure is correct.

## Troubleshooting Steps Taken
- Downgraded test project to .NET 8.0 (from .NET 9.0) for xUnit compatibility.
- Ensured all xUnit packages are the same major version (2.4.x) and compatible runner/SDK versions.
- Removed unnecessary/legacy package references (e.g., Microsoft.CodeDom.Providers.DotNetCompilerPlatform).
- Ran `dotnet restore` and `dotnet build` multiple times.
- Confirmed correct project structure (test project is a sibling, not nested).

## Online Research & Solutions
- [xUnit.net Docs: Getting Started](https://xunit.net/docs/getting-started/netcore/cmdline)
- [Stack Overflow: Xunit Namespace Could not be Found in Visual Studio Code](https://stackoverflow.com/questions/45121744/xunit-namespace-could-not-be-found-in-visual-studio-code)
- [Microsoft Docs: Unit testing C# in .NET Core using dotnet test and xUnit](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
- [GitHub Issues: xunit/xunit - Namespace Not Found](https://github.com/xunit/xunit/issues/2141)

## Common Fixes (from research)
- Restart Omnisharp in VS Code (Command Palette: "Restart Omnisharp").
- Clear NuGet cache: `dotnet nuget locals all --clear`.
- Open the test project folder directly in VS Code, not the parent/main project.
- Ensure all xUnit packages are the same version.
- Do not nest test projects inside the main project directory.
- Remove any `<Using Include="Xunit" />` from the .csproj.

## Next Steps
- Try clearing the NuGet cache and restoring again.
- Try opening only the `MyMvcApp.Tests` folder in VS Code.
- If still not resolved, consider deleting the `bin` and `obj` folders and rebuilding.
- If all else fails, try creating a new xUnit test project and copying the test code over.
