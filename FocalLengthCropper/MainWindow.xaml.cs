using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FocalLengthCropper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var op = new OpenFileDialog();
            op.Title = "Select image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                gridControls.IsEnabled = false;

                var bitmapDec = BitmapDecoder.Create(new Uri(op.FileName), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                var bitmap = bitmapDec.Frames[0];
                int width = bitmap.PixelWidth;
                int height = bitmap.PixelHeight;

                double ratio = height / width;

                flcControl.Height = ratio * flcControl.Width;
                imgPhoto.Source = bitmap;

                gridControls.IsEnabled = true;

            }            
        }

        private void sldCropToFL_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblCropToFL.Content = String.Format("Crop to Focal Length: {0}mm", ((Slider)sender).Value);
        }

        private void sldOriginFL_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblOriginFL.Content = String.Format("Original Focal Length: {0}mm", ((Slider)sender).Value);

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
