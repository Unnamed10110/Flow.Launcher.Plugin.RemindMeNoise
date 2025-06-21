using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Media;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.RemindMe.Views;

public partial class NotificationWindow {
    public string NotificationTitle { get; }
    public string NotificationSubtitle { get; }

    public NotificationWindow(string title, string subtitle) {
        NotificationTitle = title switch {
            null or "" => "Reminder",
            _ => title
        };
        NotificationSubtitle = subtitle;

        InitializeComponent();
        
        // Add sound when window loads
        Loaded += NotificationWindow_Loaded;

        try {
            var dllDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var iconPath = Path.Combine(dllDirectory ?? ".", "icon.png");
            Icon = new BitmapImage(new Uri(iconPath));
        } catch {
            // Ignore exceptions, use default icon
        }
    }
    
    private async void NotificationWindow_Loaded(object sender, RoutedEventArgs e) {
        try {
            // Play system notification sound three times
            for (int i = 0; i < 3; i++) {
                SystemSounds.Asterisk.Play();
                if (i < 2) // Don't wait after the last sound
                    await Task.Delay(1000); // 1000ms delay between sounds
            }
        } catch {
            // Ignore exceptions if sound fails to play
        }
    }
}