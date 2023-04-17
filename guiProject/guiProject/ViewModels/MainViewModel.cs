
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using MainProject;
using ReactiveUI;
using SkiaSharp;

using PixelFormat = Avalonia.Platform.PixelFormat;


// using Avalonia.Media.Imaging;

namespace guiProject.ViewModels;

public class MainViewModel : ReactiveObject
{
    private WriteableBitmap? _image;
    private AutoResetEvent _autoEvent;
    private Timer _timer;
    public string Greeting => "Welcome to Avalonia!";
    public WriteableBitmap? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    private SKBitmap _cameraViewImage;
   

    private SKBitmap CameraViewImage
    {
        get => _cameraViewImage;
        set => this.RaiseAndSetIfChanged(ref _cameraViewImage, value);
    }
    private SKSurface CurrentSurface;
    public Controller Controller = new Controller();
    


    public MainViewModel()
    {
        // SetTimer();

        this.WhenAnyValue(x => x.CameraViewImage)
            .Subscribe(x =>
            {
                DrawImage();
                Console.WriteLine("New View!");
            });

        Observable.Interval(TimeSpan.FromMilliseconds(10))
                .Select(_ =>
                {
                    var photo =  Controller.CreatePhoto();
                    Controller.ZoomIn(0.02);
                    return photo;
                })
                .Subscribe(newPhoto => CameraViewImage = newPhoto);
        
    }

    private void DrawImage()
    {
        Image = new WriteableBitmap(new PixelSize(800, 600), new Vector(96, 96));

        if (Image == null || CameraViewImage == null)
            return;

        using (var lockedBitmap = Image.Lock())
        {
            SKImageInfo info = new SKImageInfo(lockedBitmap.Size.Width, lockedBitmap.Size.Height, CameraViewImage.ColorType);

            CurrentSurface = SKSurface.Create(info, lockedBitmap.Address, lockedBitmap.RowBytes);
            CurrentSurface.Canvas.Clear(new SKColor(255, 255, 255));
            CurrentSurface.Canvas.DrawBitmap(CameraViewImage,0,0);
        }
    }

    private void SetTimer()
    {
        _autoEvent = new AutoResetEvent(false);
        _timer = new Timer(OnTick, _autoEvent, 0, 10 );
    }

    private void OnTick(Object? info)
    {
        Controller.ZoomIn(0.01);
    }
    
}
