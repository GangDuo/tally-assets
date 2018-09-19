@echo off

REM -------------------------------------------------
REM Ý’è
REM -------------------------------------------------
REM ’I‰µ”NEŒŽ
SET /A YEAR=2018
SET /A MONTH=8

REM TANALOG‚ÌŠi”[ƒtƒHƒ‹ƒ_
SET JUNCTION=Y:\t2018”N8ŒŽ’I‰µ\HT\
SET TANALOG=%JUNCTION%tanalog\
SET OUTSOURCING=%JUNCTION%outsourcing\

REM WŒvŒ‹‰Ê‚ÌŠi”[ƒtƒHƒ‹ƒ_
SET OUTPUT=Y:\t2018”N8ŒŽ’I‰µ\

REM -------------------------------------------------
REM Main
REM -------------------------------------------------
CALL :InvokeMake 001 ‘O‹´–{“X
CALL :InvokeMake 004 ˆÉ¨è“X
CALL :InvokeMake 005 ã’†‹“X
CALL :InvokeMake 006 ã•À‰|“X
CALL :InvokeMake 007 ‘¾“c”Ñ’Ë“X
CALL :InvokeMake 008 ŒF’J”ì’Ë“X
CALL :InvokeMake 009 ’ßƒ–“‡Žá—t“X
CALL :InvokeMake 010 ‰ªè“X
CALL :InvokeMake 011 •l¼Žu“s˜C“X
CALL :InvokeMake 013 ¬ŽRé“ì“X
CALL :InvokeMake 015 ‰F“s‹{²ÝÀ°Êß°¸“X
CALL :InvokeMake 016 ˆÉ¨è½Ï°¸“X
CALL :InvokeMake 017 VÀ“X
CALL :InvokeMake 018 “Œ¼ŽRËßµÆ³«°¸“X
CALL :InvokeMake 019 ‘Š–ÍŒ´‰º‹ã‘ò“X
CALL :InvokeMake 020 ‰F“s‹{FKD“X
CALL :InvokeMake 021 ”ª‰¤Žq•À–Ø“X
CALL :InvokeMake 022 ‘å‹{½Ã×À³Ý“X
CALL :InvokeMake 023 ‚Ð‚½‚¿‚È‚©Ì§¯¼®Ý¸Ù°½Þ“X
CALL :InvokeMake 024 V‘O‹´“X
CALL :InvokeMake 025 ‹ž“sŒjì“X
CALL :InvokeMake 026 ã”ö“X
CALL :InvokeMake 027 –k‘å˜H“X
CALL :InvokeMake 028 ŠC˜V–¼
CALL :InvokeMake 029 ¼ŽR
CALL :InvokeMake 030 Žç’J
CALL :InvokeMake 031 Š€Œ´
CALL :InvokeMake 032 ±Øµ”
CALL :InvokeMake 033 ±Øµ‹´–{
CALL :InvokeMake 034 ’·‰ª

GOTO end

REM -------------------------------------------------
REM ƒTƒuƒ‹[ƒ`ƒ“
REM -------------------------------------------------
:InvokeMake
echo %YEAR%/%MONTH%
echo %%1:%1 %%2:%2 %%3:%3 %%~3:%~3 "%OUTPUT%%1.xlsx"
.Make\Make\bin\Release\Make.exe -store-code:%1 -store-name:%2 -year:%YEAR% -month:%MONTH% -tanalog-dir:"%TANALOG%%1" -outsourcing-dir:"%OUTSOURCING%%1" -output-file:"%OUTPUT%%1.xlsx">>log.txt
exit /b

:end
SETLOCAL
ENDLOCAL
PAUSE
