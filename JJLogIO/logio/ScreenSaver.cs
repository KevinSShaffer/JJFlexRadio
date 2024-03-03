using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace JJLogIO
{
    /// <summary> screen saver manipulation </summary>
public static class ScreenSaver
{
   // Signatures for unmanaged calls
   [DllImport( "user32.dll", CharSet = CharSet.Auto )]
   private static extern bool SystemParametersInfo( 
      int uAction, int uParam, ref int lpvParam, 
      int flags );

   [DllImport( "user32.dll", CharSet = CharSet.Auto )]
   private static extern bool SystemParametersInfo( 
      int uAction, int uParam, ref bool lpvParam, 
      int flags );

   [DllImport( "user32.dll", CharSet = CharSet.Auto )]
   private static extern int PostMessage( IntPtr hWnd, 
      int wMsg, int wParam, int lParam );

   [DllImport( "user32.dll", CharSet = CharSet.Auto )]
   private static extern IntPtr OpenDesktop( 
      string hDesktop, int Flags, bool Inherit, 
      uint DesiredAccess );

   [DllImport( "user32.dll", CharSet = CharSet.Auto )]
   private static extern bool CloseDesktop( 
      IntPtr hDesktop );

   [DllImport( "user32.dll", CharSet = CharSet.Auto )]
   private static extern bool EnumDesktopWindows( 
      IntPtr hDesktop, EnumDesktopWindowsProc callback, 
      IntPtr lParam );

   [DllImport( "user32.dll", CharSet = CharSet.Auto )]
   private static extern bool IsWindowVisible( 
      IntPtr hWnd );

   /// <summary> get foreground window handle </summary>
   [DllImport("user32.dll", CharSet = CharSet.Auto)]
   public static extern IntPtr GetForegroundWindow( );

   // Callbacks
   private delegate bool EnumDesktopWindowsProc( 
      IntPtr hDesktop, IntPtr lParam );

   // Constants
   private const int SPI_GETSCREENSAVERACTIVE = 16;
   private const int SPI_SETSCREENSAVERACTIVE = 17;
   private const int SPI_GETSCREENSAVERTIMEOUT = 14;
   private const int SPI_SETSCREENSAVERTIMEOUT = 15;
   private const int SPI_GETSCREENSAVERRUNNING = 114;
   private const int SPIF_SENDWININICHANGE = 2;

   private const uint DESKTOP_WRITEOBJECTS = 0x0080;
   private const uint DESKTOP_READOBJECTS = 0x0001;
   private const int WM_CLOSE = 16;


    /// <summary>
   ///  is the screen saver active? 
   ///  (enabled, but not necessarily running).
   /// </summary>
   /// <returns>true if active</returns>
   public static bool GetScreenSaverActive( )
   {
      bool isActive = false;

      SystemParametersInfo( SPI_GETSCREENSAVERACTIVE, 0, 
         ref isActive, 0 );
      return isActive;
   }

    /// <summary>
    /// Activate/deactivate the screen saver
    /// </summary>
    /// <param name="Active">true to activate</param>
   public static void SetScreenSaverActive( bool Active )
   {
       int intVar = (Active) ? 1 : 0;

      SystemParametersInfo( SPI_SETSCREENSAVERACTIVE, 
         intVar, ref intVar, SPIF_SENDWININICHANGE );
   }

   /// <summary> Returns the screen saver timeout setting, in seconds </summary>
   public static Int32 GetScreenSaverTimeout( )
   {
      Int32 value = 0;

      SystemParametersInfo( SPI_GETSCREENSAVERTIMEOUT, 0, 
         ref value, 0 );
      return value;
   }

   // Pass in the number of seconds to set the screen saver
   /// <summary> timeout value </summary>
   public static void SetScreenSaverTimeout( Int32 Value )
   {
      int nullVar = 0;

      SystemParametersInfo( SPI_SETSCREENSAVERTIMEOUT, 
         Value, ref nullVar, SPIF_SENDWININICHANGE );
   }

   /// <summary> Returns TRUE if the screen saver is actually running </summary>
   public static bool GetScreenSaverRunning( )
   {
      bool isRunning = false;

      SystemParametersInfo( SPI_GETSCREENSAVERRUNNING, 0, 
         ref isRunning, 0 );
      return isRunning;
   }

    /// <summary>
    /// From Microsoft's Knowledge Base article #140723: 
    /// http://support.microsoft.com/kb/140723
    /// "How to force a screen saver to close once started 
    /// in Windows NT, Windows 2000, and Windows Server 2003"
    /// </summary>
   public static void KillScreenSaver( )
   {
      IntPtr hDesktop = OpenDesktop( "Screen-saver", 0, 
         false,DESKTOP_READOBJECTS | DESKTOP_WRITEOBJECTS);
      if( hDesktop != IntPtr.Zero )
      {
         EnumDesktopWindows( hDesktop, new 
            EnumDesktopWindowsProc( KillScreenSaverFunc ),
            IntPtr.Zero );
         CloseDesktop( hDesktop );
      }
      else
      {
         PostMessage( GetForegroundWindow( ), WM_CLOSE, 
            0, 0 );
      }
   }

   private static bool KillScreenSaverFunc( IntPtr hWnd, 
      IntPtr lParam )
   {
      if( IsWindowVisible( hWnd ) )
         PostMessage( hWnd, WM_CLOSE, 0, 0 );
      return true;
   }
}
#if dontIgnore
 The more interesting code here is the KillScreenSaver( ) method. Beginning with Microsoft Windows NT, you cannot simply make a call to close the foreground window as you could under previous releases of Windows. Windows NT introduces the concept of separate desktops. Applications can run on one desktop, and screen savers can run on another. For example, under Windows XP, if you check the "On resume, display Welcome screen" option in the Screen Saver tab of Display Properties, the screen saver will be running on a desktop other than the one for your application. In this case, you need to find the screen saver desktop and close its foreground window to terminate the screen saver. If this option is not checked, the screen saver is running on the same desktop as your application, and may be killed merely by closing the foreground window in that desktop.

Another thing worth pointing out is in the KillScreenSaverFunc( ) callback function. Notice the call to IsWindowVisible( hWnd ) just before closing what presumably is the screen saver window. Apparently it's possible for the screen saver to be "running", yet not actually be seen as the foreground application on the desktop. As we'll see in our Test Program, we need to keep this in mind for when the screen saver is running on the same desktop as our application. Otherwise our application could end up being the one killed by the call to PostMessage( GetForegroundWindow( ), WM_CLOSE, 0, 0 ), which might annoy the user.

The Test Program

The Screen Saver Test program demonstrates how to use the ScreenSaver class. It can also serve as a convenient utility to edit the screen saver settings as you're testing your own application. Since it can modify the screen saver values normally set with the Display Properties dialog, it saves off these values so they may be restored on exit or manually using the Restore button. The form is set to be "TopMost" to make it easily accessible when being used with full screen apps.



To change the screen saver timeout, enter the number of seconds in the NumericUpDown control on the right and click the Write button. If the screen saver is "active", it should kick on that many seconds later. If any of the settings are changed outside of the test program, for example by using the Display Properties dialog, click the Refresh button to display the new values.

To facilitate testing the KillScreenSaver( ) method, the test program uses a periodic timer. The timeout period can be set and the timer can be toggled using the Start/Stop Timer button. If the period is greater than the screen saver timeout, a call to KillScreenSaver( ) will terminate the screen saver. If the period is less, the screen saver timeout is reset by invoking SetScreenSaverActive( TRUE ), preventing the screen saver from running until after a full timeout has expired.

 Collapse | Copy Code
// Kill event timer handler
private void KillTimer_Elapsed( object state )
{
   // Toggle kill state to indicate activity
   killState = ( killState == 1 ) ? 0 : 1;
   this.SetText( killState.ToString( ) );

   // Stop the screen saver if it's active and running, 
   // otherwise reset the screen saver timer.
   // Apparently it's possible for GetScreenSaverRunning()
   // to return TRUE before the screen saver has time to 
   // actually become the foreground application. So...
   // Make sure we're not the foreground window to avoid 
   // killing ourself.

   if( ScreenSaver.GetScreenSaverActive( ) )
   {
      if( ScreenSaver.GetScreenSaverRunning( ) )
      {
         if( ScreenSaver.GetForegroundWindow() != hThisWnd)
            ScreenSaver.KillScreenSaver( );
      }
      else
      {
         // Reset the screen saver timer, so the screen 
         // saver doesn't turn on until after a full
         // timeout period. If killPeriod is less than 
         // ssTimeout the screen saver should never 
         // activate.
         ScreenSaver.SetScreenSaverActive( TRUE );
      }
   }
}

 As pointed out earlier, it's possible for the SPI_GETSCREENSAVERRUNNING call to return TRUE before the screen saver actually becomes the foreground window. Perhaps this function should be renamed to SPI_GETSCREENSAVERSORTAKINDARUNNING. Anyways, make sure the foreground window isn't your own app before invoking KillScreenSaver( ), otherwise it might get closed instead.

License


This article, along with any associated source code and files, is licensed under The Code Project Open License (CPOL)

About the Author


 





kschulz

Web Developer

United States 

Member


 

Kurt's programming career began in 1978, developing firmware for the Zilog Z80-A microprocessor on a Mostek development workstation. For 23 years, Kurt led the development of Human Machine Interface (HMI) programs used in factory automation, marketed and sold worldwide by CTC and Parker Hannifin as ScreenWare, Interact and InteractX. For the last 4 years, Kurt has been consulting on agile development practices and working as an independent software contractor focusing on graphical and user interface development.
  
 Other passions include spending time with his wife Janice, daughters Erica and Laura, scuba diving, target shooting, guitar, travel, and digital photo imaging and restoration. Currently residing in southwestern Ohio.
 
.


Article Top

 


 

















Sign Up to vote 

  Poor



Excellent



 




.


Comments and Discussions
 


You must Sign In to use this message board.









Search this forum  
. 





Profile popups    SpacingRelaxedCompactTight  NoiseVery HighHighMediumLowVery Low  LayoutNormalOpen TopicsOpen AllThread ViewNo JavascriptPreview  Per page102550   
 








First PrevNext

 














Dual Screens Issue



Steve Maier

8:25 16 Feb '12  

 













If the screen saver is running on a dual screen system and you call the KillScreenSaver function, it only kills the screensaver on the main screen, not the second one. Since the screensaver is actually still active that can cause unusual behaviors as well.
  
 Any ideas?
 
Steve Maier
 




Sign In·View Thread·Permalink



 









My vote of 5



martinrj30

18:13 1 Dec '11  

 













just what i needed, and a great distillation of complex MSDN information




Sign In·View Thread·Permalink



 









SetScreenSaverActive should use a bool as parameter



Octopod

23:21 10 Feb '11  

 













SetScreenSaverActive method should use a bool as parameter, not an int with a 0/1 value. Take a look to Framework Design Guidelines.




Sign In·View Thread·Permalink



 









Changing the Screen Saver



Jack Amble

22:38 26 Sep '10  

 













Hi,
  
 Do you know of a way to programmatically switch the active screen saver to one of the others in the installed list?




Sign In·View Thread·Permalink

2.00/5 (1 vote) 











Re: Changing the Screen Saver



Freshbrew

14:34 1 Oct '10  

 













I suppose you could enumerate all the screensavers in the Windows\System32 Directory:
  
 C#
  
 DirectoryInfo dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86));
FileInfo[] scrFiles = dir.GetFiles("*.scr");
  
 VB
  
 Dim dir As New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))
Dim scrFiles() As FileInfo = dir.GetFiles("*.scr")
  
 Then using the registry you could update the screensaver:
  
 C#
  
 RegistryKey scrRegKey = Microsoft.Win32.Registry.CurrentUser().OpenSubKey("Control Panel\\Desktop", true);
scrRegKey.SetValue("SCRNSAVE.EXE", scrFilePath, RegistryValueKind.String);
  
 VB
  
 Dim scrRegKey As RegistryKey = My.Computer.Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", True)
scrRegKey.SetValue("SCRNSAVE.EXE", scrFilePath, RegistryValueKind.String)
  
 NB. The active screensaver registry value is located here:
  
 C#
 string regKeyValue = Microsoft.Win32.Registry.CurrentUser().OpenSubKey("Control Panel\\Desktop").GetValue("SCRNSAVE.EXE").ToString();
  
 VB
 Dim regKeyValue As String = My.Computer.Registry.CurrentUser.OpenSubKey("Control Panel\Desktop").GetValue("SCRNSAVE.EXE").ToString
  
 Note that if the screensaver isn't active this value doesn't exist.
  
 
Hope that helps!
 


Sign In·View Thread·Permalink



 









Re: Changing the Screen Saver



dc_2000

17:18 4 Dec '11  

 













One warning here. If the following GPO is defined, changing the screensaver path in HKCU\Control Panel\Desktop\SCRNSAVE.EXE will have no effect. So be aware of that!
  
 Screen saver executable name
 User Configuration\Administrative Templates\Control Panel\Display




Sign In·View Thread·Permalink



 









Using your app in another program.



rspercy58

1:17 18 Mar '09  

 













Hi Kurt,
 I am using your app in a program i am working on and I am checking the status of my screensaver on start-up. This works fine. What I would like to know is, "Does your app detect if there is no screensaver activated...Control Panel Screensaver applet set to ( 'NONE' )"? 
 
 Thanks in advance for your time and patience.
 Regards,
 rspercy58 
 
 
rspercy
 1 + 1 = 186,440....Depending on the species.

 


Sign In·View Thread·Permalink



 









Re: Using your app in another program.



kschulz

7:57 18 Mar '09  

 













Hi rspercy,
  
 I believe the most reliable way to see if a screensaver is active is to look at the registry key HKEY_CURRENT_USER\Control Panel\Desktop and examine the SCRNSAVE.EXE value. If the Control Panel Screensaver applet is set to NONE, this member will not be present. If a screen saver is specified (active), this member's value will contain the path and filename of the assigned screen saver.
  
 Refer to http://support.microsoft.com/kb/318781[^] for more information. 
 
 
-kschulz

 


Sign In·View Thread·Permalink



 









Re: Using your app in another program.



rspercy58

10:26 18 Mar '09  

 













THNX Very Much for that tip. I used My.Computer.Registry.GetValue() function along with your app and it works just fine. Once again, THNX
  
 Regards,
 rspercy58 
 
 
rspercy
 1 + 1 = 186,440....Depending on the species.

 


Sign In·View Thread·Permalink



 









Where to copy the code to control Windows XP Pro screensavers?



Rodlergg

4:13 9 Dec '08  

 













I am new member to The Code Forum and a novice at C programming and controlling Windows functionality. The test program provided works on my system but I would like to incorporate it into Windows XP Pro to shut off the screensaver so the PC can enter hibernate mode. Can someone tell me what file need modified? Thanks.




Sign In·View Thread·Permalink

1.00/5 (1 vote) 











Re: Where to copy the code to control Windows XP Pro screensavers?



kschulz

4:41 9 Dec '08  

 













Take a look at the ScreenSaverTest code. The ScreenSaver class has been copied into ScreenSaverTest.cs at the bottom. You can copy this class as is into your own (C#) project, and call the functions as needed. In particular, look at the KillTimer_Elapsed( ) function to get an idea of the call sequence to kill the screen saver. Tested under WinXP Pro. 
 
 
-kschulz

 


Sign In·View Thread·Permalink



 









Setting SCREENSAVERRUNNING flag



czesiek120

21:05 11 Mar '08  

 













How about notifying the system that the screensaver is running without actually running it? In other words, I need to enable SCREENSAVERRUNNING flag, so that SPI_GETSCREENSAVERRUNNING will return TRUE. Unfortunately SPI_SETSCREENSAVERRUNNING does not do this, as one would think. SPI_SETSCREENSAVEACTIVE succesfully sets the screensaver active (as in enabled) without even a screensaver present. The method someone wrote here:
 SendMessage(GetDesktopWindow(), WM_SYSCOMMAND, SC_SCREENSAVE, 0); requires that the screensaver is set in the Screen properties dialog box as a system screensaver. However, when system runs screensaver it sets the ..RUNNING flags, because then SPI_GETSCREENSAVERRUNNING returns true, but I don't know how it does that.
  
 Thanks for any pointers. 




Sign In·View Thread·Permalink



 









If 'On resume, password protect' option selected, screen saver does not come.



Ashwini Appajigowda

1:40 6 Sep '07  

 













Hi,
  
 I have an simple MFC dialog based application. On launch of that application ‘Password protected screen saver’ is not getting activated. 
 
 If ‘On resume, password protect’ option unchecked, I am getting the Screen saver. 
 
 Application not generating any of the keyboard or any user events to disturb the system. If I exit the application every thing works normal(Means password protected screen saver gets activates after screen saver time out period.)
  
 Please help me in understanding, why ‘Password protected screen saver’ is not getting activated.
  
 
Thanks & regard’s
 Ashwini





Sign In·View Thread·Permalink

1.00/5 (1 vote) 











The Easyist way to prevent the screensaver and / or mmonitor power off



sagaert

23:06 13 Aug '07  

 













No need to make a timer kicking the screensver,
 Simply capture the messages and return -1 if you want to disable the screensaver
 or monitor power off (wich are 2 different things !)
  
 
protected override void WndProc(ref Message m)
 {
 const int SC_SCREENSAVE = 0xF140;
 const int SC_MONITORPOWER = 0xF170;
 const int WM_SYSCOMMAND = 0x0112;
  
 if ((m.WParam == (IntPtr)SC_SCREENSAVE) && (m.Msg == WM_SYSCOMMAND))
 m.Result = (IntPtr)(-1); // prevent screensaver
 else if ((m.WParam == (IntPtr)SC_MONITORPOWER) && (m.Msg == WM_SYSCOMMAND))
 m.Result = (IntPtr)(-1); // prevent monitor power-off
 else
 base.WndProc(ref m);
 } 




Sign In·View Thread·Permalink



 









Re: The Easyist way to prevent the screensaver and / or mmonitor power off



kschulz

2:43 14 Aug '07  

 













The timer is only used by the test program, as a convenient way to fire off the screen saver disabling code after the screen saver has kicked on. The timer would not be used in an actual application.
  
 Also, the goal in this example is not to disable the screen saver permanently, but rather to allow it to work normally and disengage it programmatically upon detection of some event. 
 
 
-kschulz

 


Sign In·View Thread·Permalink

4.00/5 (1 vote) 











ScreenSaver password



JohannesAckermann

0:05 31 May '07  

 













Hi,
  
 Is it possible to programmatically enable/disable the "On resume, display logon screen" option for the screensaver? I'm writing a utility to manually start the screensaver (easy enough) as a locking mechanism, but want to ensure that the password option is enabled. So the idea is that when the utility is run, it automatically enables the password option and then starts the screensaver.
  
 Thanks,
  

  
 Johannes




Sign In·View Thread·Permalink



 









Re: ScreenSaver password



kschulz

3:04 31 May '07  

 













Hi Johannes,
  
 You may want to look at the ScreenSaverIsSecure setting in the system registry. Under WinXP, it's under HKEY_CURRENT_USER\Control Panel\Desktop\ScreenSaverIsSecure (you may also need to look under HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Control Panel\Desktop\ScreenSaverIsSecure).
  
 Setting this to a 1 will enable (check) the "On resume, show Welcome screen" option, and setting to a 0 will disable (uncheck) it. I have not tried it programmatically, but it appears this should work.

  

  
 
-kschulz

 


Sign In·View Thread·Permalink



 









sizeof problem?



K-ballo

14:40 7 May '07  

 













SystemParameterInfo specifies that screen saver parameters use BOOL values, is sizeof(BOOL) == sizeof(bool) in C# or you are just lucky?
 With C++, sizeof(BOOL) = 4 while sizeof(bool) = 1 




Sign In·View Thread·Permalink



 









Re: sizeof problem?



TobiasP

23:12 27 Nov '07  

 













Just in case you are still looking for an answer more than half a year after you asked the question... 
 
 A C# boolean is one byte but marshalled as four bytes when used where a native BOOL would be used. See, i.e., this MSDN entry and this "size of a *bool*" discussion. 




Sign In·View Thread·Permalink



 









Not working for me on XP-Pro



jr0000000000

14:43 13 Apr '07  

 













I just downloaded this example program and tried it and it did NOT work at all. I set break points at each of the functions, and they seem to be getting called fine. I'm using XP-Pro on a corporate network. 
 
 I took a look at the registry, and it was not changing either. I know when I change the registry myself, the change doesn't take effect until I logout. Is this code supposed to resolve this problem also?
  
 For my code to function correctly, I need to recieve an event when the screen saver is started/finished. I also need to be able to reset the screen saver time-out value (the same as if the user had typed a key or moved the mouse.)
  
 Has anyone else had this program fail? How can I debug it? What's the next step?

  
 -Jim.




Sign In·View Thread·Permalink

4.00/5 (1 vote) 











Re: Not working for me on XP-Pro



kschulz

17:11 13 Apr '07  

 













Jim - the examples were created using VS2005 under XP-Pro. Are you logged in as Admin? Not sure if that will make a difference - I've only tested it under Admin accounts, mostly using the set of screen savers that come with XP-Pro. I would not expect to see any changes to the registry. 
 
 
-kschulz

 


Sign In·View Thread·Permalink



 









Re: Not working for me on XP-Pro [modified]



jr0000000000

7:21 16 Apr '07  

 













>>VS2005 under XP-Pro.
- Yes, 
VS2005 version 8.0.50727.762 (SP.050727-7600)
 C# 2005 77626-009-...
  
 >>logged in as Admin?
- Yes
  
 >>...screen savers that come with XP-Pro.
- Using, "3D Text"
  
 >>...not expect to see any changes to the registry.
- The registry location for the Screen Saver:
 [HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Control Panel\Desktop
- My Values:
 "ScreenSaveActive"="1"
 "ScreenSaverIsSecure"="1"
 "ScreenSaveTimeOut"="120"
- These values are correctly being read from the registry and displayed in your appication. If I change these values manually, they are not used by Windows until after logging out. Your application does not change any of these values, and I would expect them to be changed by your appication.
  
 Should this also work in Vista? 
 
 -Jim.
  
 
-- modified at 13:33 Monday 16th April, 2007 




Sign In·View Thread·Permalink



 









Re: Not working for me on XP-Pro



kschulz

8:03 16 Apr '07  

 













Jim - "3D Text" was one of the screen savers not recognized as the foreground window, and therefore did not close using the method suggested by Microsoft (whereas seems "Windows XP" is always recognized). Try the method described by Kurt Callebaut posted below. I tested this successfully using the "3D Text" screen saver. Here's the code:
  
 
..if( ScreenSaver.GetScreenSaverActive( ) )
 ..{
 ....if( ScreenSaver.GetScreenSaverRunning( ) )
 ....{
 ......bool ret = ScreenSaver.KillProcessByName( GetScreenSaverFilename( ) );
 ....}
 ....else
 ....{
 ......// Reset the screen saver timer
 ......ScreenSaver.SetScreenSaverActive( TRUE );
 ....}
 ..}
  
 
 
 public static class ScreenSaver
 {

  
 
..private static string GetScreenSaverFileName( )
 ..{
 ....string result = string.Empty;
  
 ....try
 ....{
 ......string keystr = "Control Panel\\Desktop";
 ......RegistryKey key = Registry.CurrentUser.OpenSubKey( keystr, false );
 ......result = key.GetValue( "SCRNSAVE.EXE" ).ToString( );
 ......if( result != string.Empty )
 ........result = Path.GetFileName( result );
 ......key.Close( );
 ......key = null;
 ......return result;
 ....}
 ....catch
 ....{
 ......return string.Empty;
 ....}
 ..}
  
 ..private static bool KillProcessByName( string ProcessName )
 ..{
 ....Process[ ] Processes = Process.GetProcessesByName( ProcessName );
  
 ....if( Processes.Length > 0 )
 ....{
 ......Processes[ 0 ].Kill( );
 ......return true;
 ....}
 ....else
 ....{
 ......return false;
 ....}
 ..}
  
 
}

  
 
-kschulz

 


Sign In·View Thread·Permalink



 









Re: Not working for me on XP-Pro



jr0000000000

12:33 16 Apr '07  

 













Thanks for this additional information. I have been playing around with things, and finally got it working correctly. The reason I knew it was not working was because when the code asked for the current time-out value [ScreenSaver.GetScreenSaverTimeout( ).ToString( );], it always returned the same value, not the new value that should have just been set.
  
 The clue provided that gives the problem is the registry key of:
 [HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Control Panel\Desktop
 which is NOT the location of where the real values are stored, but the location of where the corporate policy values are stored. When these policy values exist, items in the Screen Saver options dialog are grayed-out, and it seems they are also not available through these Win32 command sets either. 
 
 Solution: Remove the policy settings from the registry.





Sign In·View Thread·Permalink

3.00/5 (2 votes) 











set the screen saver to running



NikoTanghe

4:06 6 Apr '07  

 













Maybe a nice add-on to this class:
 set the screen saver to running
  
 
[DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
 private static extern IntPtr GetDesktopWindow();
  
 [DllImport("user32.dll")]
 private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
  
 //...
  
 private const int SC_SCREENSAVE = 0xF140;
 private const int WM_SYSCOMMAND = 0x0112;
  
 //...
  
 public static void SetScreenSaverRunning()
 {
 SendMessage(GetDesktopWindow(), WM_SYSCOMMAND, SC_SCREENSAVE, 0);
 }
#endif
}
