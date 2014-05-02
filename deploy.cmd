@echo off

set srcPath=D:\Work\Git\Publish\LAS
set descPath[0]=K:\WEB209\LASROOT\1.LAS\V1
set descPath[1]=K:\WEB209\LASROOT\2.InternalLas\V1

for /F "tokens=2 delims==" %%s in ('set descPath[') do (
    copy /y app_offline.htm %%s
    xcopy /I /S /D /Y %srcPath% %%s
    del /F /Q %%s\app_offline.htm
)

set /p DUMMY=Everything is done. Hit ENTER to close...
