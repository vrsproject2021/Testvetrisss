@echo off
CLS
FOR %%A in (%1) do set SRCPATH=%%~A
FOR %%A in (%2) do set TGTPATH=%%~A
FOR %%A in (%3) do set PREFIX=%%~A
FOR %%A in (%4) do set SUBPFX=%%~A

cd /D %1
setlocal EnableDelayedExpansion
setlocal ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

set len=6
set charpool=abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789
set len_charpool=62

for /r %%A in (*.*) do (
    set "txt=%%~nA"
    set gen_str=
   for /L %%b IN (1, 1, %len%) do (
     set /A rnd_index=!RANDOM! * %len_charpool% / 32768
     for /F %%i in ('echo %%charpool:~!rnd_index!^,1%%') do set gen_str=!gen_str!%%i
    )
    
    set PREFIX = %PREFIX%_%gen_str%
    if not "!txt:~0,12!"=="%SUBPFX%" (
        ren "%%A" "%PREFIX%%%~nxA"
        move %PREFIX%%%~nxA %2
    ) else (
	move %%~nxA %2
    )
)
