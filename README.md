#sharpAHK :  AutoHotkey Scripting's Simplicity/Speed/Power for .Net 

sharpAHK consists of a collection wrappers (for HotKeyIt's AutoHotkey.dll project) that aim to recreate the majority of AutoHotkey scripting commands. Manage files, folders, windows, text, and automation that AHK was best known for, all from .Net. 

My primary use for the library was incorporating familar file/string/window mananagement commands into my .Net applications. This library has the ability to run your own AHK commands / scripts, but this functionality hasn't been tested / aware there are bugs. The basic AHK commands all seem to work well. 

Credits: 
     Many thanks to Founder Chris as well as HotKeyIt and Lexikos who have continued progress on the AutoHotkey language. Them and the countless contributors to the AHK library over the years. https://www.autohotkey.com/
     
     HotKeyIt's AHK dll was the first essential: https://github.com/HotKeyIt/ahkdll
     
     Then burque505's AHK InterOp made sharpAHK possible, I was stuck until I found his project.
     https://github.com/burque505/AutoHotkey.Interop


Version 1 of sharpAHK is essentially as close to AutoHotkey's original syntax as I could get, copying option names and documentation from the AutoHotkey.chm. I used burque505's InterOp to recreate every command that still worked in C#. 

Version 2 - Currently working on upgrading functions by using more efficience C# commands, keeping familiar AHK syntax. Next version will have more documentation along with examples. Stay tuned. (8/8/19 - LucidMethod)



Execute Your Own AHK Functions (Using Functions Documented Below)

Or Use Library of Wrappers Included: 
   

    using sharpAHK;    // Declare SharpAHK.dll reference

    _AHK ahk = new _AHK();  // New Instance of AHK
    
    ahk.MsgBox("Hello AHK World");  
    
    string answer = ahk.InputBox("Are you hungry?");
    
    if (answer != "") // If User Provides Answer to InputBox question, write that answer to a test file and open it
    { 
        ahk.FileAppend(answer, ahk.AppDir() + "\\OutFile.txt");  // write text to new/existing text file
        
        ahk.Run(ahk.AppDir() + "\\OutFile.txt");   // open text file using default application
    }
    
    
    

     

