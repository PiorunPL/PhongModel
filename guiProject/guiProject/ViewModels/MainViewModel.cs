
using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Media.Imaging;
using guiProject.Other;
using MainProject;
using ReactiveUI;
using SharpHook;
using SkiaSharp;

namespace guiProject.ViewModels;

public class MainViewModel : ReactiveObject
{
    private readonly double _moveDiff = 0.1;
    private readonly double _turnDiff = 0.005;
    private readonly double _zoomDiff = 0.2;

    private InputController _inputController = new InputController();
    private TaskPoolGlobalHook _hook = new TaskPoolGlobalHook();

    private readonly int _targetWidthPixel = 1900;
    private readonly int _targetHeightPixel = 1000;
    private readonly PixelSize _pixelSize;
    private readonly Vector _dpiVector;
    
    private TimeSpan _readKeyTimeSpan = TimeSpan.FromMilliseconds(5);
    private TimeSpan _displayTimeSpan = TimeSpan.FromMilliseconds(20);
    
    private WriteableBitmap? _image;
    public WriteableBitmap? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    private SKBitmap? _cameraViewImage;
    private SKBitmap? CameraViewImage
    {
        get => _cameraViewImage;
        set => this.RaiseAndSetIfChanged(ref _cameraViewImage, value);
    }
    
    private SKSurface? _currentSurface;
    public Controller Controller = new Controller();
    
    public MainViewModel()
    {
        SetUpHooks();
        
        _pixelSize = new PixelSize(_targetWidthPixel, _targetHeightPixel);
        _dpiVector = new Vector(96, 96);
        Image = new WriteableBitmap(_pixelSize, _dpiVector);
        

        this.WhenAnyValue(x => x.CameraViewImage)
            .Subscribe(_ => DrawImage());
        
        Observable.Interval(_displayTimeSpan).Select(_ => Controller.CreatePhoto())
                .Subscribe(newPhoto => CameraViewImage = newPhoto);
        
    }
    
    
    public void DrawImage()
    {
        Image = new WriteableBitmap(_pixelSize, _dpiVector);

        if (Image == null || CameraViewImage == null)
            return;

        using (var lockedBitmap = Image.Lock())
        {
            SKImageInfo info = new SKImageInfo(lockedBitmap.Size.Width, lockedBitmap.Size.Height, CameraViewImage.ColorType);

            _currentSurface = SKSurface.Create(info, lockedBitmap.Address, lockedBitmap.RowBytes);
            _currentSurface.Canvas.Clear(new SKColor(255, 255, 255));
            _currentSurface.Canvas.DrawBitmap(CameraViewImage,0,0);
        }
    }

    private void SetUpHooks()
    {
        _hook.KeyPressed += (_, args) => { KeyPressed((char)args.Data.RawCode);};
        _hook.KeyReleased += (_, args) => { KeyReleased((char)args.Data.RawCode); };
        _hook.RunAsync();
    }
    
    private void KeyPressed(char charCode)
    {
        switch (charCode)
        {
            case 'w':
                if (_inputController.ObservatorW == null)
                    _inputController.ObservatorW = Observable.Interval(_readKeyTimeSpan).Do(_ => GoForward()).Subscribe();
                break;
            case 'a':
                if (_inputController.ObservatorA == null)
                    _inputController.ObservatorA = Observable.Interval(_readKeyTimeSpan).Do(_ => GoLeft()).Subscribe();
                break;
            case 's':
                if (_inputController.ObservatorS == null)
                    _inputController.ObservatorS = Observable.Interval(_readKeyTimeSpan).Do(_ => GoBackward()).Subscribe();
                break;
            case 'd':
                if (_inputController.ObservatorD == null)
                    _inputController.ObservatorD = Observable.Interval(_readKeyTimeSpan).Do(_ => GoRight()).Subscribe();
                break;
            case 'u':
                if (_inputController.ObservatorU == null)
                    _inputController.ObservatorU = Observable.Interval(_readKeyTimeSpan).Do(_ => GoUp()).Subscribe();
                break;
            case 'i':
                if (_inputController.ObservatorI == null)
                    _inputController.ObservatorI = Observable.Interval(_readKeyTimeSpan).Do(_ => GoDown()).Subscribe();
                break;
            case 'q':
                if (_inputController.ObservatorQ == null)
                    _inputController.ObservatorQ = Observable.Interval(_readKeyTimeSpan).Do(_ => TurnLeft()).Subscribe();
                break;
            case 'e':
                if (_inputController.ObservatorE == null)
                    _inputController.ObservatorE = Observable.Interval(_readKeyTimeSpan).Do(_ => TurnRight()).Subscribe();
                break;
            case 'j':
                if (_inputController.ObservatorJ == null)
                    _inputController.ObservatorJ = Observable.Interval(_readKeyTimeSpan).Do(_ => TurnDown()).Subscribe();
                break;
            case 'k':
                if (_inputController.ObservatorK == null)
                    _inputController.ObservatorK = Observable.Interval(_readKeyTimeSpan).Do(_ => TurnUp()).Subscribe();
                break;
            case 'h':
                if (_inputController.ObservatorH == null)
                    _inputController.ObservatorH = Observable.Interval(_readKeyTimeSpan).Do(_ => TurnCounterClockwise()).Subscribe();
                break;
            case 'l':
                if (_inputController.ObservatorL == null)
                    _inputController.ObservatorL = Observable.Interval(_readKeyTimeSpan).Do(_ => TurnClockwise()).Subscribe();
                break;
            case '-':
                if (_inputController.ObservatorMinus == null)
                    _inputController.ObservatorMinus = Observable.Interval(_readKeyTimeSpan).Do(_ => ZoomOut()).Subscribe();
                break;
            case '=':
                if (_inputController.ObservatorEqual == null)
                    _inputController.ObservatorEqual = Observable.Interval(_readKeyTimeSpan).Do(_ => ZoomIn()).Subscribe();
                break;
        }
    }

    private void KeyReleased(char charCode)
    {
        switch (charCode)
        {
            case 'w':
                _inputController.ObservatorW?.Dispose();
                _inputController.ObservatorW = null;
                break;
            case 'a' :
                _inputController.ObservatorA?.Dispose();
                _inputController.ObservatorA = null;
                break;
            case 's':
                _inputController.ObservatorS?.Dispose();
                _inputController.ObservatorS = null;
                break;
            case 'd':
                _inputController.ObservatorD?.Dispose();
                _inputController.ObservatorD = null;
                break;
            case 'u':
                _inputController.ObservatorU?.Dispose();
                _inputController.ObservatorU = null;
                break;
            case 'i':
                _inputController.ObservatorI?.Dispose();
                _inputController.ObservatorI = null;
                break;
            case 'q':
                _inputController.ObservatorQ?.Dispose();
                _inputController.ObservatorQ = null;
                break;
            case 'e':
                _inputController.ObservatorE?.Dispose();
                _inputController.ObservatorE = null;
                break;
            case 'j':
                _inputController.ObservatorJ?.Dispose();
                _inputController.ObservatorJ = null;
                break;
            case 'k':
                _inputController.ObservatorK?.Dispose();
                _inputController.ObservatorK = null;
                break;
            case 'h':
                _inputController.ObservatorH?.Dispose();
                _inputController.ObservatorH = null;
                break;
            case 'l':
                _inputController.ObservatorL?.Dispose();
                _inputController.ObservatorL = null;
                break;
            case '-':
                _inputController.ObservatorMinus?.Dispose();
                _inputController.ObservatorMinus = null;
                break;
            case '=':
                _inputController.ObservatorEqual?.Dispose();
                _inputController.ObservatorEqual = null;
                break;
        }
    }

    public void GoForward()
    {
        Controller.GoForward(_moveDiff);   
    }
    public void GoBackward()
    {
        Controller.GoBackward(_moveDiff);   
    }
    public void GoLeft()
    {
        Controller.GoLeft(_moveDiff);   
    }
    public void GoRight()
    {
        Controller.GoRight(_moveDiff);   
    }
    public void GoUp()
    {
        Controller.GoUp(_moveDiff);
    }
    public void GoDown()
    {
        Controller.GoDown(_moveDiff);
    }
    public void TurnLeft()
    {
        Controller.TurnLeft(_turnDiff);
    }
    public void TurnRight()
    {
        Controller.TurnRight(_turnDiff);
    }
    public void TurnUp()
    {
        Controller.TurnUp(_turnDiff);
    }
    public void TurnDown()
    {
        Controller.TurnDown(_turnDiff);
    }
    public void TurnClockwise()
    {
        Controller.TurnClockwise(_turnDiff);
    }
    public void TurnCounterClockwise()
    {
        Controller.TurnCounterClockwise(_turnDiff);
    }
    public void ZoomIn()
    {
        Controller.ZoomIn(_zoomDiff);
    }
    public void ZoomOut()
    {
        Controller.ZoomOut(_zoomDiff);
    }
}
