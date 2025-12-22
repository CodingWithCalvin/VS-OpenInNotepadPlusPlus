# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Critical Rules

**These rules override all other instructions:**

1. **NEVER commit directly to main** - Always create a feature branch and submit a pull request
2. **Conventional commits** - Format: `type(scope): description`
3. **GitHub Issues for TODOs** - Use `gh` CLI to manage issues, no local TODO files. Use conventional commit format for issue titles
4. **Pull Request titles** - Use conventional commit format (same as commits)
5. **Branch naming** - Use format: `type/scope/short-description` (e.g., `feat/ui/settings-dialog`)
6. **Working an issue** - Always create a new branch from an updated main branch
7. **Check branch status before pushing** - Verify the remote tracking branch still exists. If a PR was merged/deleted, create a new branch from main instead
8. **WPF for all UI** - All UI must be implemented using WPF (XAML/C#). No web-based technologies (HTML, JavaScript, WebView)

---

### GitHub CLI Commands

```bash
gh issue list                    # List open issues
gh issue view <number>           # View details
gh issue create --title "type(scope): description" --body "..."
gh issue close <number>
```

### Conventional Commit Types

| Type | Description |
|------|-------------|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `refactor` | Code change that neither fixes a bug nor adds a feature |
| `test` | Adding or updating tests |
| `chore` | Maintenance tasks |

---

## Project Overview

This is a Visual Studio extension that adds a right-click context menu command to open solution files, project files, or individual files in Notepad++. It targets .NET Framework 4.8 and uses the VS SDK.

## Build Commands

Build the extension (requires Visual Studio with VS SDK or .NET Framework 4.8 targeting pack):
```
msbuild src\CodingWithCalvin.OpenInNotepadPlusPlus.sln
```

Debug by pressing F5 in Visual Studio - this launches the VS Experimental Instance (`/rootsuffix Exp`).

## Architecture

The extension has a simple structure:

- **OpenInNotepadPlusPlusPackage.cs** - The VS package entry point, registers the command and settings page
- **Commands/OpenExecutableCommand.cs** - Handles the context menu command, gets the selected path and launches Notepad++
- **Helpers/ProjectHelpers.cs** - Resolves file paths from Solution Explorer selection (handles solutions, projects, project items, and unloaded projects)
- **Dialogs/SettingsDialogPage.cs** - Options page for configuring the Notepad++ executable path
- **VSCommandTable.vsct** - Defines the context menu command placement

## Key Implementation Details

Path resolution in `ProjectHelpers.GetSelectedPath()` handles three selection types:
1. `ProjectItem` - uses `FileNames[1]`
2. `Project` - uses `FullName` with fallback to `UniqueName` for unloaded projects
3. `Solution` - uses `FullName`

The extension auto-detects Notepad++ in the default install location but allows custom paths via Tools > Options.

## Development Requirements

Install the [Extensibility Essentials 2022](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ExtensibilityEssentials2022) extension for Visual Studio.
