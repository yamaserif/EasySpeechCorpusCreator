rem -ビルドタスク------------------------------------
rem 64bit Windows
dotnet publish ^
    EasySpeechCorpusCreator\EasySpeechCorpusCreator\EasySpeechCorpusCreator.csproj ^
    -c Release ^
    -o EasySpeechCorpusCreator\EasySpeechCorpusCreator\bin\Release\publish\win-x64 ^
    -r win-x64 ^
    --self-contained true

rem 86bit Windows
dotnet publish ^
    EasySpeechCorpusCreator\EasySpeechCorpusCreator\EasySpeechCorpusCreator.csproj ^
    -c Release ^
    -o EasySpeechCorpusCreator\EasySpeechCorpusCreator\bin\Release\publish\win-x86 ^
    -r win-x86 ^
    --self-contained true

rem -公開処理------------------------------------
rem 64bit Windows
mkdir ..\apps\temp\EasySpeechCorpusCreator\__program_data
xcopy /S /E /F /G /H /R /K /Y EasySpeechCorpusCreator\EasySpeechCorpusCreator\bin\Release\publish\win-x64 ^
      ..\apps\temp\EasySpeechCorpusCreator\__program_data
xcopy /S /E /F /G /H /R /K /Y ReleaseBase ^
      ..\apps\temp\EasySpeechCorpusCreator
powershell compress-archive -Force ^
    ..\apps\temp\EasySpeechCorpusCreator\* ^
    ..\apps\EasySpeechCorpusCreator_win-x64.zip
rmdir ..\apps\temp /s /q

rem 86bit Windows
mkdir ..\apps\temp\EasySpeechCorpusCreator\__program_data
xcopy /S /E /F /G /H /R /K /Y EasySpeechCorpusCreator\EasySpeechCorpusCreator\bin\Release\publish\win-x86 ^
      ..\apps\temp\EasySpeechCorpusCreator\__program_data
xcopy /S /E /F /G /H /R /K /Y ReleaseBase ^
      ..\apps\temp\EasySpeechCorpusCreator
powershell compress-archive -Force ^
    ..\apps\temp\EasySpeechCorpusCreator\* ^
    ..\apps\EasySpeechCorpusCreator_win-x86.zip
rmdir ..\apps\temp /s /q

rem -完了------------------------------------
pause