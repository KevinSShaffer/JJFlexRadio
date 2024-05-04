if "%2" == ""Debug"" exit 0
echo "%2" "%3"
rem Get program name w/o quotes
set _pgm=%3%
set pgm=%_pgm:~1,-1%
echo %pgm%
cd %1
rem modify the install script
echo "modifying install script"
type "install template.nsi" | "GnuWin32\bin\sed" "s/MYPGM/%pgm%/g" >install.nsi
rem create the delete list.
echo "creating delete list"
dir /b bin\release | "GnuWin32\bin\sed" -f src.sed >deleteList.txt
echo "creating OutFile from the install.nsi script"
"NSIS\makensis" install.nsi