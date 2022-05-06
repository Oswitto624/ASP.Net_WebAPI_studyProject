@echo off

dotnet restore
echo.
for /f %%f in (
  'dir *.csproj /s /d /b'
) do (
  echo.
  echo Updateing packages in %%~nxf
  for /f "tokens=2 skip=9" %%s in (
    'dotnet list "%%f" package --outdated'
  ) do (
    echo --== Update %%s ==-- 
    dotnet add "%%f" package %%s
  )
  echo Updateing packages in %%~nxf complete!
  dotnet restore %%f
)

pause