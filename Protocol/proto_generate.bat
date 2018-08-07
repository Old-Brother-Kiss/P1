color 0A && echo off

set "WORK_DIR=%cd%"

cd ../Server/Server/Socket/Protocol/
set "CS_OUT_PATH=%cd%"
cd %WORK_DIR%

cd ../Client/Protocol/
set "JS_OUT_PATH=%cd%"
cd %WORK_DIR%

set "PROTOC_EXE=%WORK_DIR%/protoc.exe"

%PROTOC_EXE% --version

cd ./Proto/

echo ===================================
echo CompileProto
echo ===================================

for /f "delims=" %%i in ('dir /b "*.proto"') do (
	echo.generate %%i
	"%PROTOC_EXE%" --proto_path="%cd%" --csharp_out="%CS_OUT_PATH%" %%i
)

echo ===================================
echo MakeProtoCode
echo ===================================

python ../MakeProtoCode.py

pause