SETLOCAL

SET TANALOG=HT\tanalog
SET OUTSOURCING=HT\outsourcing
SET /A REPEAT=34

MKDIR .fmww
MKDIR %TANALOG%
MKDIR %OUTSOURCING%

CD %TANALOG%
MKDIR .work
CALL :enumerableMkdir
CD ..\..\

CD %OUTSOURCING%
MKDIR .work
CALL :enumerableMkdir
CD ..\..\

REM MKDIR ���_�݌�\item
REM MKDIR ���_�݌�\line
REM MKDIR ���_�݌�\vendor

GOTO end

REM -------------------------------------------------
REM �t�H���_�쐬
REM -------------------------------------------------
:enumerableMkdir
CSCRIPT //NOLOGO ..\..\enumerableMkdir.js 1 %REPEAT%
RMDIR 002
RMDIR 003
RMDIR 012
RMDIR 014
EXIT /B

:end
ENDLOCAL
PAUSE
