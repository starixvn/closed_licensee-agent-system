@echo off
set project=%1

if "%project%" == "" goto missing

REM OpenCover
set command=..\packages\OpenCover.4.5.2506\OpenCover.Console.exe -returntargetcode -register:user -target:"..\packages\NUnit.Runners.2.6.3\tools\nunit-console.exe" -targetargs:"/noshadow /xml=output\TestResult.xml ..\%project%\bin\Debug\%project%.dll" -filter:+[*]* -output:output\opencover-nunit.xml
call %command%
if %errorlevel% neq 0 goto error

REM ReportGenerator
set command=..\packages\ReportGenerator.1.9.1.0\ReportGenerator.exe -reports:.\output\opencover-nunit.xml -targetdir:.\output -reporttypes:Html,HtmlSummary -filters:-MonitoredUndoTests
call %command%
if %errorlevel% neq 0 goto error

echo Success.
start .\output\index.htm
goto :eof

:missing
echo Please provide your test project name.
pause
goto :eof

:error
echo Failure.
pause
goto :eof