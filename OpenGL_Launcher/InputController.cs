using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using MainProject.Controller;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGL_Launcher;

public class InputController
{
    public InputController(Controller controller)
    {
        Controller = controller;
    }
    
    Controller Controller;
    private TimeSpan _readKeyTimeSpan = TimeSpan.FromMilliseconds(10);
    private readonly double _moveDiff = 0.2;
    private readonly double _turnDiff = 0.01;
    private readonly double _zoomDiff = 0.2;
    
    //Move
    public IDisposable? ObservatorW = null;     //Forward
    public IDisposable? ObservatorA = null;     //Left
    public IDisposable? ObservatorS = null;     //Backward
    public IDisposable? ObservatorD = null;     //Right
    public IDisposable? ObservatorU = null;     //Up
    public IDisposable? ObservatorI = null;     //Down
    
    //Rotation
    public IDisposable? ObservatorQ = null;     //Left
    public IDisposable? ObservatorE = null;     //Right
    public IDisposable? ObservatorJ = null;     //Down
    public IDisposable? ObservatorK = null;     //Up
    public IDisposable? ObservatorH = null;     //Counter Clockwise
    public IDisposable? ObservatorL = null;     //Clockwise
    
    //Zoom
    public IDisposable? ObservatorMinus = null; //Zoom out
    public IDisposable? ObservatorEqual = null; //Zoom in
    
    public void HandleKeyboardInput(KeyboardState input)
    {
        // Console.Write(input[]);
        // Console.WriteLine($"Is key W pressed? {input.IsKeyPressed(Keys.W)}");
        HandleKeysReleased(input);
        HandleKeysPressed(input);
    }

    public void HandleKeysPressed(KeyboardState input)
    {
        if (ObservatorW == null && input.IsKeyDown(Keys.W))
            ObservatorW = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.GoForward(_moveDiff)).Subscribe();
        if (ObservatorA == null && input.IsKeyDown(Keys.A))
            ObservatorA = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.GoLeft(_moveDiff)).Subscribe();
        if (ObservatorS == null && input.IsKeyDown(Keys.S))
            ObservatorS = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.GoBackward(_moveDiff)).Subscribe();
        if (ObservatorD == null && input.IsKeyDown(Keys.D))
            ObservatorD = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.GoRight(_moveDiff)).Subscribe();
        if (ObservatorU == null && input.IsKeyDown(Keys.U))
            ObservatorU = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.GoUp(_moveDiff)).Subscribe();
        if (ObservatorI == null && input.IsKeyDown(Keys.I))
            ObservatorI = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.GoDown(_moveDiff)).Subscribe();
        
        if (ObservatorQ == null && input.IsKeyDown(Keys.Q))
            ObservatorQ = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.TurnLeft(_turnDiff)).Subscribe();
        if (ObservatorE == null && input.IsKeyDown(Keys.E))
            ObservatorE = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.TurnRight(_turnDiff)).Subscribe();
        if (ObservatorJ == null && input.IsKeyDown(Keys.J))
            ObservatorJ = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.TurnDown(_turnDiff)).Subscribe();
        if (ObservatorK == null && input.IsKeyDown(Keys.K))
            ObservatorK = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.TurnUp(_turnDiff)).Subscribe();
        if (ObservatorH == null && input.IsKeyDown(Keys.H))
            ObservatorH = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.TurnCounterClockwise(_turnDiff)).Subscribe();
        if (ObservatorL == null && input.IsKeyDown(Keys.L))
            ObservatorL = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.TurnClockwise(_turnDiff)).Subscribe();
        
        if (ObservatorEqual == null && input.IsKeyDown(Keys.Equal))
            ObservatorEqual = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.ZoomIn(_zoomDiff)).Subscribe();
        if (ObservatorMinus == null && input.IsKeyDown(Keys.Minus))
            ObservatorMinus = Observable.Interval(_readKeyTimeSpan).Do(_ => Controller.ZoomOut(_zoomDiff)).Subscribe();
        
    }

    public void HandleKeysReleased(KeyboardState input)
    {
        if (ObservatorW != null && !input[Keys.W])
        {
            ObservatorW.Dispose();
            ObservatorW = null;
        }
        if (ObservatorA != null && !input[Keys.A])
        {
            ObservatorA.Dispose();
            ObservatorA = null;
        }
        if (ObservatorS != null && !input[Keys.S])
        {
            ObservatorS.Dispose();
            ObservatorS = null;
        }
        if (ObservatorD != null && !input[Keys.D])
        {
            ObservatorD.Dispose();
            ObservatorD = null;
        }
        if (ObservatorQ != null && !input[Keys.Q])
        {
            ObservatorQ.Dispose();
            ObservatorQ = null;
        }
        if (ObservatorE != null && !input[Keys.E])
        {
            ObservatorE.Dispose();
            ObservatorE = null;
        }
        if (ObservatorU != null && !input[Keys.U])
        {
            ObservatorU.Dispose();
            ObservatorU = null;
        }
        if (ObservatorI != null && !input[Keys.I])
        {
            ObservatorI.Dispose();
            ObservatorI = null;
        }
        if (ObservatorJ != null && !input[Keys.J])
        {
            ObservatorJ.Dispose();
            ObservatorJ = null;
        }
        if (ObservatorK != null && !input[Keys.K])
        {
            ObservatorK.Dispose();
            ObservatorK = null;
        }
        if (ObservatorH != null && !input[Keys.H])
        {
            ObservatorH.Dispose();
            ObservatorH = null;
        }
        if (ObservatorL != null && !input[Keys.L])
        {
            ObservatorL.Dispose();
            ObservatorL = null;
        }
        if (ObservatorMinus != null && !input[Keys.Minus])
        {
            ObservatorMinus.Dispose();
            ObservatorMinus = null;
        }
        if (ObservatorEqual != null && !input[Keys.Equal])
        {
            ObservatorEqual.Dispose();
            ObservatorEqual = null;
        }
 
            
    }

}