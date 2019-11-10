BOZES - Bruce's Own Zim Extension - Selections

This project attempts to be cross-platform and has been written in C# targeting [.NET Core 3.0](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-core-3-0).

BOZES provides a mechanism for the evaluation of embedded programming language blocks in a Zim document. It has nothing to do with a [pinnacle of the same name in Israel](https://biblehub.com/topical/b/bozes.htm).

Zim's online help have a section on [Custom Tools](https://zim-wiki.org/manual/Help/Custom_Tools.html) which gives a good introduction to how to extend Zim and describes the framework BOZES leverages.

The Zim document may contains, for example
```
[8th]42 dup * .[/8th]

[euphoria]
atom a = 42
printf(1, "%d", a * a)
[/euphoria]

[cmd]
@echo off 
set a=42
set /a b=a*a
echo %b%
[/cmd]

[php]<?php
echo pow(42,2);
?>
[/php]

[perl]print 42*42[/perl]

[python]print(42*42)[/python]

[bash]
#! /bin/bash
let a=42*42
echo $a
[/bash]
```
BOZES makes it possible to select a block of language code, click a button on the taskbar or an item from a menu, and have the code between the markup be evaluated and the result returned.

The same document after each of the blocks have been individually selected and evaluated:
```
[8th]42 dup * .[/8th]
Result: 1764

[euphoria]
atom a = 42
printf(1, "%d", a * a)
[/euphoria]
Result: 1764

[cmd]
@echo off 
set a=42
set /a b=a*a
echo %b%
[/cmd]
Result: 1764


[php]<?php
echo pow(42,2);
?>
[/php]
Result: 1764

[perl]print 42*42[/perl]
Result: 1764

[python]print(42*42)[/python]
Result: 1764


[bash]
#! /bin/bash
let a=42*42
echo $a
[/bash]
Result: 1764
```
The languages are controlled by a bozes.json file which is stored beside the executable. 

```json
{
    "8th": {
        "Extension": "8th",
        "Binary": "c:\\8th\\bin\\win64\\8th.exe",
        "Tail": "$F"
    },
    "euphoria": {
        "Extension": "ex",
        "Binary": "C:\\Euphoria\\bin\\eui.EXE",
        "Tail": "$F"
    },
    "cmd": {
        "Extension": "cmd",
        "Binary": "c:\\windows\\system32\\cmd.EXE",
        "Tail": "/C $F"
    },
    "php": {
        "Extension": "php",
        "Binary": "C:\\Program Files\\iis express\\PHP\\v7.3\\php.EXE",
        "Tail": "-f $F"
    },
    "perl": {
        "Extension": "pl",
        "Binary": "C:\\Users\\bugma\\scoop\\apps\\perl\\current\\perl\\bin\\perl.EXE",
        "Tail": "$F"
    },
    "python": {
        "Extension": "py",
        "Binary": "C:\\Users\\bugma\\AppData\\Local\\Microsoft\\WindowsApps\\python.exe",
        "Tail": "$F"
    },
    "bash": {
        "Extension": "sh",
        "Binary": "C:\\Users\\bugma\\scoop\\shims\\bash.EXE",
        "Tail": "-c $f"
    }
}
```

`$F` evaluates to the Windows path to the temporary file containing the language code. `$f` evaluates to the Linux/MacOS path to it. 

![Alt Text](https://thepracticaldev.s3.amazonaws.com/i/pm0ep432j9t4xawtjrsa.png)

MIT licensed.
