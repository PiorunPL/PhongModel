using System;

namespace guiProject.Other;

public class InputController
{
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
    
    // Light
    public IDisposable? ObservatorZ = null;     //Left
    public IDisposable? ObservatorX = null;     //Right
    public IDisposable? ObservatorC = null;     //Down
    public IDisposable? ObservatorV = null;     //Up
    public IDisposable? ObservatorB = null;     //Counter Clockwise
    public IDisposable? ObservatorN = null;     //Clockwise
    
    //Material
    public IDisposable? ObservatorM = null;
    
    //Sphere
    public IDisposable? ObservatorSlash = null;

}