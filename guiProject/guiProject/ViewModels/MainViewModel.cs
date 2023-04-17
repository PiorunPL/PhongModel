
using Avalonia;
using Avalonia.Media.Imaging;
using MainProject;
using ReactiveUI;
using SkiaSharp;

using PixelFormat = Avalonia.Platform.PixelFormat;


// using Avalonia.Media.Imaging;

namespace guiProject.ViewModels;

public class MainViewModel : ViewModelBase
{
    
    public string Greeting => "Welcome to Avalonia!";
    // public Bitmap? Image => new Bitmap(800,600);
    // public Avalonia.Media.Imaging.Bitmap? Image => new Avalonia.Media.Imaging.WriteableBitmap(new Avalonia.PixelSize(800,600), new Vector());
    private WriteableBitmap? _image;
    public WriteableBitmap? Image
    {
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }

    private SKSurface CurrentSurface;
    public Controller Controller = new Controller();
    


    public MainViewModel() : base()
    {
        Controller.Camera.PassActualWorld(Controller.World.Lines);
        SKBitmap sourceBitmap = Controller.Camera.CreatePhoto();

        Image =
            new WriteableBitmap(new PixelSize((int)sourceBitmap.Width, (int)sourceBitmap.Height), new Vector(96, 96));

        if(sourceBitmap is null)
            return;
        
        
        using (var lockedBitmap = Image.Lock())
        {
            SKImageInfo info = new SKImageInfo(lockedBitmap.Size.Width, lockedBitmap.Size.Height, sourceBitmap.ColorType);

            CurrentSurface = SKSurface.Create(info, lockedBitmap.Address, lockedBitmap.RowBytes);
            CurrentSurface.Canvas.Clear(new SKColor(255, 255, 255));
            CurrentSurface.Canvas.DrawBitmap(sourceBitmap,0,0);
        }

        // Image = sourceBitmap;
        // Image = Controller.Camera.CreatePhoto()
        // using (MemoryStream memoryStream = new MemoryStream())
        // {
        //     var bitmap = Controller.Camera.CreatePhoto();
        //     bitmap.Save(memoryStream, ImageFormat.Png);
        //     memoryStream.Position = 0;
        //
        //     Image = new Avalonia.Media.Imaging.Bitmap(memoryStream);
        // }
        // Image = 

    }
}
