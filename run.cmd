@echo off

REM -------------------------------------------------
REM �ݒ�
REM -------------------------------------------------
REM �I���N�E��
SET /A YEAR=2018
SET /A MONTH=8

REM TANALOG�̊i�[�t�H���_
SET JUNCTION=Y:\t2018�N8���I��\HT\
SET TANALOG=%JUNCTION%tanalog\
SET OUTSOURCING=%JUNCTION%outsourcing\

REM �W�v���ʂ̊i�[�t�H���_
SET OUTPUT=Y:\t2018�N8���I��\

REM -------------------------------------------------
REM Main
REM -------------------------------------------------
CALL :InvokeMake 001 �O���{�X
CALL :InvokeMake 004 �ɐ���X
CALL :InvokeMake 005 �㒆���X
CALL :InvokeMake 006 ����|�X
CALL :InvokeMake 007 ���c�ђ˓X
CALL :InvokeMake 008 �F�J��˓X
CALL :InvokeMake 009 �߃�����t�X
CALL :InvokeMake 010 ����X
CALL :InvokeMake 011 �l���u�s�C�X
CALL :InvokeMake 013 ���R���X
CALL :InvokeMake 015 �F�s�{�����߰��X
CALL :InvokeMake 016 �ɐ���ϰ��X
CALL :InvokeMake 017 �V���X
CALL :InvokeMake 018 �����R�ߵƳ����X
CALL :InvokeMake 019 ���͌������X
CALL :InvokeMake 020 �F�s�{FKD�X
CALL :InvokeMake 021 �����q���ؓX
CALL :InvokeMake 022 ��{�����ݓX
CALL :InvokeMake 023 �Ђ����Ȃ�̧���ݸٰ�ޓX
CALL :InvokeMake 024 �V�O���X
CALL :InvokeMake 025 ���s�j��X
CALL :InvokeMake 026 ����X
CALL :InvokeMake 027 �k��H�X
CALL :InvokeMake 028 �C�V��
CALL :InvokeMake 029 ���R
CALL :InvokeMake 030 ��J
CALL :InvokeMake 031 ����
CALL :InvokeMake 032 �ص��
CALL :InvokeMake 033 �ص���{
CALL :InvokeMake 034 ����

GOTO end

REM -------------------------------------------------
REM �T�u���[�`��
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
