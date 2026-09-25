# Drop Invoker

Drop Invoker is a small Windows desktop launcher. It displays nine command slots in a 3 × 3 grid. Select a scene, then drop text, files, or directories onto a slot to run its command. Each slot shows a description and a tooltip with the command line; a nonzero exit code normally produces an error dialog.

## Requirements

Windows with the .NET 10 Desktop Runtime is required to run the application. Building from source requires the .NET 10 SDK (`dotnet build DropInvoker.sln`).

## Configuration location

Create configuration files under `%LOCALAPPDATA%\DropInvoker` (usually `C:\Users\<username>\AppData\Local\DropInvoker`):

```text
DropInvoker\
  scenes\       Scene files (.json only)
  commands\     Command files (.yaml, .json, or .yml)
  runners\      Runner files (.yaml, .json, or .yml)
```

The application creates these directories when it starts. Scene names come from scene file names. Scenes are discovered at startup, so restart the application after adding a scene. Command and runner references use file names without extensions. For those files, the application checks `.yaml`, then `.json`, then `.yml`; keep one file per name to avoid ambiguity.

## Complete example

Create `%LOCALAPPDATA%\DropInvoker\scenes\default.json`:

```json
{
  "slots": [
    "open-in-notepad", null, null,
    null, null, null,
    null, null, null
  ]
}
```

Slots are ordered left to right, top to bottom. Use a command file name without its extension in each slot; `null` leaves a slot empty. Only the first nine entries are used, and missing entries are empty.

Create `%LOCALAPPDATA%\DropInvoker\commands\open-in-notepad.yaml`:

```yaml
name: Open in Notepad
description: Open file
runner: notepad
accepts: [file]
arguments: ["$*"]
```

Create `%LOCALAPPDATA%\DropInvoker\runners\notepad.yaml`:

```yaml
executable: 'C:\Windows\System32\notepad.exe'
arguments: ["$*"]
```

Start the application, select `default`, and drop one file onto the first slot. The command's `$*` inserts the dropped file path into the command arguments; the runner's `$*` then inserts those command arguments after `notepad.exe`.

## Configuration fields

| File | Field | Meaning |
| --- | --- | --- |
| Scene | `slots` | Array of up to nine command file names or `null` values. Scenes must be JSON. |
| Command | `name` | Command name used in error messages. |
| Command | `description` | Text shown in the slot and its tooltip. |
| Command | `runner` | Runner file name without extension. If omitted, the first expanded command argument is the executable. |
| Command | `accepts` | Allowed drop types: `text`, `file`, `files`, `dir`, `dirs`. |
| Command | `arguments` | Array of argument strings. A standalone `"$*"` inserts all dropped values. |
| Command | `working_directory` | Process working directory. Overrides the runner's value. |
| Command | `ignore_exit_code` | Set to `true` to suppress the error dialog for a nonzero exit code; defaults to `false`. |
| Runner | `executable` | Program to start. Required when a runner is used. |
| Runner | `arguments` | Array of argument strings. A standalone `"$*"` inserts the expanded command arguments. |
| Runner | `working_directory` | Process working directory when the command does not set one. |

`file` and `dir` accept exactly one file or directory. `files` and `dirs` accept one or more of the corresponding type. A path list mixing files and directories does not match any of these four types. `text` accepts dropped Unicode text as one argument. `accepts` values are case insensitive.

If `runner` is omitted, put the executable first in the command's `arguments`, for example `arguments: ["notepad.exe", "$*"]`. An array entry equal to `$*` expands into separate arguments; `$*` embedded inside a longer string does not. Environment variables such as `%USERPROFILE%` are expanded in arguments and working directories. A leading `~` followed by `\` or `/` in those values also resolves through `%USERPROFILE%`. When no working directory is set, the application uses the directory of the executable path.
