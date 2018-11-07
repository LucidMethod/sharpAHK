#sharpAHK :  AutoHotkey Scripting's Simplicity/Speed/Power for .Net 

sharpAHK consists of a collection wrappers (for HotKeyIt's AutoHotkey.dll project) that aim to recreate the majority of AutoHotkey scripting commands. Manage files, folders, windows, text, and automation that AHK was best known for, all from .Net. 


Credits: 
     Many thanks to HotKeyIt and Lexikos who have continued progress on the AutoHotkey language. 
     Along with the countless contributors to the AHK library over the years. 
     Source: https://github.com/HotKeyIt/ahkdll



Execute Your Own AHK Functions (Using Functions Documented Below)

Or Use Library of Wrappers Included (Documentation Coming Soon - Example Here)
    
Example Uses: 

    using sharpAHK;    // Declare SharpAHK.dll reference

    _AHK ahk = new _AHK();  
    
    ahk.MsgBox("Hello AHK World"); 
    
    string answer = ahk.InputBox("Are you hungry?");
    
    if (answer != "") // If User Provides Answer to InputBox question, write that answer to a test file and open it
    { 
        ahk.FileAppend(answer, ahk.AppDir() + "\\OutFile.txt");  // write text to new/existing text file
        
        ahk.Run(ahk.AppDir() + "\\OutFile.txt");   // open text file using default application
    }
    
    
    

     

