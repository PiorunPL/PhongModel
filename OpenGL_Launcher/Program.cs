using System;

namespace OpenGL_Launcher;

class Program
{
    [STAThread]
    public static void Main(String[]args)
    {
        using (NewGame game = new NewGame(1900,1000, "LearnOpenTK")){
            game.Run();
            game.Focus();
        }    
    }
    
}