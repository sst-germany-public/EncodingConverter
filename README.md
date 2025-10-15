# SST.Encoding.Converter

[![NuGet Version](https://img.shields.io/nuget/v/EncodingConverter.svg)](https://www.nuget.org/packages/SST.Encoding.Converter/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/sst-germany-public/SST.Encoding.Converter/blob/main/LICENSE.md)

## 🧩 Overview

**SST.Encoding.Converter** is a command-line tool designed to **convert text files between different encodings / code pages**.  
For example, Visual Studio often creates source files using **Code Page 1252 (Windows-1252)**. If you use non-ASCII characters such as `ä`, `ö`, or `ü`, these files typically need to be converted to **UTF-8 (Code Page 65001)**.

Doing this manually for dozens or hundreds of files is tedious — **SST.Encoding.Converter** automates the entire process quickly.

---

## 🚀 Installation

The tool is distributed as a **NuGet package**.

```bash
dotnet tool install --global SST.Encoding.Converter
````

Or within a specific project:

```bash
dotnet add package SST.Encoding.Converter
```

After installation, the executable `SST.Encoding.Converter` (or `SST.Encoding.Converter.exe`) becomes available.

---

## ⚙️ Usage

Basic syntax:

```bash
SST.Encoding.Converter [options]
```

### Example

```bash
SST.Encoding.Converter -d "C:\Projects\MyApp" -s "*.h,*.cpp" --OutputCodePage 65001 -r --verbose
```

This example recursively searches the given directory for `.h` and `.cpp` files
and converts them from **Code Page 1252** to **UTF-8 (65001)**.

---

## 🧾 Command Line Options

| Short / Long            | Type     | Default | Description                                                       |
| ----------------------- | -------- | ------- | ----------------------------------------------------------------- |
| `-d`, `--directory`     | `string` | `.\`    | Specifies the directory to search for files.                      |
| `-s`, `--searchpattern` | `string` | `*.*`   | File search pattern(s). Example: `"*.h"` or `"*.h,*.cpp"`.        |
| `-r`, `--recursive`     | `bool`   | `false` | Enables recursive directory processing.                           |
| `--InputCodePage`       | `int`    | `1252`  | Input code page used to read the file (e.g. 1252 = Windows-1252). |
| `--OutputCodePage`      | `int`    | `65001` | Output code page used to write the file (e.g. 65001 = UTF-8).     |
| `--verbose`             | `bool`   | `false` | Enables more detailed console messages.                           |

---

## 💡 Notes

* The tool uses **NLog** for colored console output and structured logging.
* Files are converted **in-place** — no backup copies are created automatically.
* It is recommended to **create a backup** before converting production files.
* Multiple file patterns are supported by separating them with commas (`,`).

---

## 🧨 Exit Codes

| Code | Name                     | Description                                     |
| ---- | ------------------------ | ----------------------------------------------- |
| `0`  | `Ok`                     | Conversion completed successfully.              |
| `1`  | `FoundProblems`          | Some files could not be converted successfully. |
| `2`  | `CommandLine`            | Invalid or incomplete command-line parameters.  |
| `3`  | `InputDirectoryNotFound` | The specified input directory was not found.    |
| `4`  | `UnexpectedException`    | An unexpected runtime exception occurred.       |

---

## 🧱 Technical Details

* Uses the following libraries:

  * [`CommandLineParser`](https://github.com/commandlineparser/commandline)
  * [`NLog`](https://nlog-project.org/)
