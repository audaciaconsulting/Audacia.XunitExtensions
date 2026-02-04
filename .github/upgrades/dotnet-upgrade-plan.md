# .NET 10.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.
3. Upgrade `tests/Audacia.XunitExtensions.Tests/Audacia.XunitExtensions.Tests.csproj`

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name | Description |
|:------------|:-----------:|

### Project upgrade details

#### `tests/Audacia.XunitExtensions.Tests/Audacia.XunitExtensions.Tests.csproj` modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net10.0`
