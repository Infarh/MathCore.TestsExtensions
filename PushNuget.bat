@echo off
chcp 65001 > nul
cd MathCore.TestsExtensions\MathCore.TestsExtensions\bin\Release\netstandard2.0\publish
dir /b *.nupkg

echo Отправляю файлы
dotnet nuget push *.nupkg -k <api-key> -n --skip-duplicate -s https://api.nuget.org/v3/index.json
echo Команда завершена

cd ..\..\..\..\..\..
pause