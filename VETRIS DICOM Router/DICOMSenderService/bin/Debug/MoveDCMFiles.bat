@echo off
CLS
FOR %%A in (%1) do set ARG1=%%~A
FOR %%A in (%2) do set ARG2=%%~A
move %1 %2