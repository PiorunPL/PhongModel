using System;

namespace OpenGL_Launcher;

class Program
{
    [STAThread]
    public static void Main(String[]args)
    {
        using (Game game = new Game(800,600, "LearnOpenTK")){
            game.Run();
            game.Focus();
        }    
    }
    
}