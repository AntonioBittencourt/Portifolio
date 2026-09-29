@echo off
title Compilando e Executando DemoDI...
echo =========================================
echo   Compilando as alteracoes do codigo...
echo =========================================
dotnet publish -c Release -r win-x64 --self-contained true --nologo
if %errorlevel% neq 0 (
    echo.
    echo [ERRO] A compilacao falhou. Verifique o codigo C#.
    pause
    exit /b
)
cls
echo =========================================
echo   Executando DemoDI.exe
echo =========================================
echo.
".\bin\Release\net9.0\win-x64\publish\DemoDI.exe"
echo.
echo =========================================
echo   Programa finalizado.
echo =========================================
pause