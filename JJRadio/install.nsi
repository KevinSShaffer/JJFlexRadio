; based on example2.nsi
;
; This script is based on example1.nsi, but it remember the directory, 
; has uninstall support and (optionally) installs start menu shortcuts.
;
; It will install JJRadio.nsi into a directory that the user selects,

;--------------------------------

; The name of the installer
Name "JJRadio 4.5.3"
; The file to write
OutFile "Setup JJRadio 4.5.3.exe"

; The default installation directory
InstallDir "$PROGRAMFILES\jjshaffer\JJRadio 4.5.3"

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\NSIS_JJRadio 4.5.3" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin


; Get a welcome message
Function .onInit
MessageBox MB_OK "\
Welcome to JJRadio, an amateur radio monitoring/control program by Jim Shaffer, KE5AL.$\r\
JJRadio is designed with blind users in mind.$\r\
It works best with a screen reader using a braille display."
FunctionEnd

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

; The stuff to install
Section "JJRadio (required)"

  SectionIn RO

  ; install for all users.
  SetShellVarContext all  
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put files there
  File /x src "bin\release\*.*"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\JJRadio 4.5.3" "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\JJRadio 4.5.3" "DisplayName" "JJRadio 4.5.3"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\JJRadio 4.5.3" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\JJRadio 4.5.3" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\JJRadio 4.5.3" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  ; install for all users.
  SetShellVarContext all  
  
  ; working dirrectory
  SetOutPath $INSTDIR
  
  CreateShortcut "$SMPROGRAMS\JJRadio 4.5.3.lnk" "$INSTDIR\JJRadio 4.5.3.exe" ""
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Desktop Shortcuts"

  ; install for all users.
  SetShellVarContext all  
  
  ; working dirrectory
  SetOutPath $INSTDIR
  
  CreateShortcut "$DESKTOP\JJRadio 4.5.3.lnk" "$INSTDIR\JJRadio 4.5.3.exe" ""
  
SectionEnd
;--------------------------------

; Uninstaller

Section "Uninstall"

  ; uninstall for all users.
  SetShellVarContext all  
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\JJRadio 4.5.3"
  DeleteRegKey HKLM "SOFTWARE\JJRadio 4.5.3"

  ; Remove files
!include "deleteList.txt"
  Delete "$INSTDIR\uninstall.exe"

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\JJRadio 4.5.3.lnk"
  Delete "$DESKTOP\JJRadio 4.5.3.lnk"

  ; Remove directories used
  RMDir "$INSTDIR"

SectionEnd
