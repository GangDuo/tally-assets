@echo off

REM -------------------------------------------------
REM Ýè
REM -------------------------------------------------
REM IµNE
SET /A YEAR=2018
SET /A MONTH=8

REM TANALOGÌi[tH_
SET JUNCTION=Y:\t2018N8Iµ\HT\
SET TANALOG=%JUNCTION%tanalog\
SET OUTSOURCING=%JUNCTION%outsourcing\

REM WvÊÌi[tH_
SET OUTPUT=Y:\t2018N8Iµ\

REM -------------------------------------------------
REM Main
REM -------------------------------------------------
CALL :InvokeMake 001 O´{X
CALL :InvokeMake 004 É¨èX
CALL :InvokeMake 005 ãX
CALL :InvokeMake 006 ãÀ|X
CALL :InvokeMake 007 ¾cÑËX
CALL :InvokeMake 008 FJìËX
CALL :InvokeMake 009 ßátX
CALL :InvokeMake 010 ªèX
CALL :InvokeMake 011 l¼usCX
CALL :InvokeMake 013 ¬RéìX
CALL :InvokeMake 015 Fs{²ÝÀ°Êß°¸X
CALL :InvokeMake 016 É¨è½Ï°¸X
CALL :InvokeMake 017 VÀX
CALL :InvokeMake 018 ¼RËßµÆ³«°¸X
CALL :InvokeMake 019 Í´ºãòX
CALL :InvokeMake 020 Fs{FKDX
CALL :InvokeMake 021 ª¤qÀØX
CALL :InvokeMake 022 å{½Ã×À³ÝX
CALL :InvokeMake 023 Ð½¿È©Ì§¯¼®Ý¸Ù°½ÞX
CALL :InvokeMake 024 VO´X
CALL :InvokeMake 025 sjìX
CALL :InvokeMake 026 ãöX
CALL :InvokeMake 027 kåHX
CALL :InvokeMake 028 CV¼
CALL :InvokeMake 029 ¼R
CALL :InvokeMake 030 çJ
CALL :InvokeMake 031 ´
CALL :InvokeMake 032 ±Øµ
CALL :InvokeMake 033 ±Øµ´{
CALL :InvokeMake 034 ·ª

GOTO end

REM -------------------------------------------------
REM Tu[`
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
