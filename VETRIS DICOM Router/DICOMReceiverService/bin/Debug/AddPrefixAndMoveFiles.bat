@echo off
CLS
FOR %%A in (%1) do set SRCPATH=%%~A
FOR %%A in (%2) do set TGTPATH=%%~A
FOR %%A in (%3) do set PREFIX=%%~A
FOR %%A in (%4) do set SUBPFX=%%~A

cd /D %1
setlocal EnableDelayedExpansion

for /r %%A in (*.*) do (
    set "txt=%%~nA"
    if not "!txt:~0,12!"=="%SUBPFX%" (
        ren "%%A" "%PREFIX%%%~nxA"
        move %PREFIX%%%~nxA %2
    ) else (
	move %%~nxA %2
    )
)
