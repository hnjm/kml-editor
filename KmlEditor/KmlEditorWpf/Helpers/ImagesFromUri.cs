using KmlEditorWpf.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KmlEditorWpf.Helpers
{
    public class ImagesFromUri
    {
        List<ImageByUri> imageByUriList = new List<ImageByUri>();

        public BitmapImage FindImageByUri(Uri uri)
        {
            if (uri != null)
            {
                ImageByUri imageByUri = imageByUriList.FirstOrDefault(iu => iu.Uri.Equals(uri));
                if (imageByUri == null)
                {
                    BitmapImage bi = new BitmapImage();
                    // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                    bi.DownloadFailed += (sender, args) =>
                    {
                        if (sender is BitmapImage)
                        {
                            BitmapImage bit = sender as BitmapImage;
                        }
                    };

                    if (uri != null)
                    {
                        // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                        bi.BeginInit();
                        bi.UriSource = uri;
                        bi.EndInit();
                    }

                    imageByUri = new ImageByUri()
                    {
                        Bi = bi,
                        Uri = uri
                    };
                    imageByUriList.Add(imageByUri);
                }
                return imageByUri.Bi;
            }
            else
            {
                return null;
            }
        }
    }
}
