# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

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
