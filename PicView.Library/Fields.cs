using System.Collections.Generic;
using System.Timers; 
using System.Windows;
using System.Windows.Media;

namespace PicView.Library
{
    public static class Fields
    {
        public const string AppName = "PicView";
        public const string Loading = "Loading...";
        public const string TxtCopy = "Filename added to clipboard";
        public const string FileCopy = "File added to clipboard";
        public const string ImageCopy = "Image added to clipboard";
        public const string ImageCut = "Image added to move clipboard";
        public const string ExpFind = "Locating in file explorer";
        public const string NoImage = "No image loaded";
        public const string CannotRender = "Unable to render image";

        public const string SupportedFilesFilter =
        " *.jpg *.jpeg *.jpe *.png *.bmp *.tif *.tiff *.gif *.ico *.wdp *.svg *.psd *.psb *.orf *.cr2 *.crw *.dng *.raf *.raw *.mrw *.nef *.x3f *.arw *.webp *"
        + ".aai *.ai *.art *.bgra *.bgro *.canvas *.cin *.cmyk *.cmyka *.cur *.cut *.dcm *.dcr *.dcx *.dds *.dfont *.dlib *.dpx *.dxt1 *.dxt5 *.emf *.epi *.eps *.ept"
        + " *.ept2 *.ept3 *.exr *.fax *.fits *.flif *.g3 *.g4 *.gif87 *.gradient *.gray *.group4 *.hald *.hdr *.hrz *.icb *.icon *.ipl *.jc2 *.j2k *.jng *.jnx"
        + " *.jpm *.jps *.jpt *.kdc *.label *.map *.nrw *.otb *.otf *.pbm *.pcd *.pcds *.pcl *.pct *.pcx *.pfa *.pfb *.pfm *.picon *.pict *.pix *.pjpeg *.png00"
        + " *.png24 *.png32 *.png48 *.png64 *.png8 *.pnm *.ppm *.ps *.radialgradient *.ras *.rgb *.rgba *.rgbo *.rla *.rle *.scr *.screenshot *.sgi *.srf *.sun"
        + " *.svgz *.tiff64 *.ttf *.vda *.vicar *.vid *.viff *.vst *.vmf *.wpg *.xbm *.xcf *.yuv";


        /// <summary>
        ///  Files filterering string used for file/save dialog
        ///  TODO update for and check file support
        /// </summary>
        public const string FilterFiles =
            "All Supported files|*.bmp;*.jpg;*.png;*.tif;*.gif;*.ico;*.jpeg;*.wdp;*.psd;*.psb;*.cbr;*.cb7;*.cbt;"
            + "*.cbz;*.xz;*.orf;*.cr2;*.crw;*.dng;*.raf;*.ppm;*.raw;*.mrw;*.nef;*.pef;*.3xf;*.arw;*.webp;"
            + "*.zip;*.7zip;*.7z;*.rar;*.bzip2;*.tar;*.wim;*.iso;*.cab"
            ////////////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            + "|Pictures|*.bmp;*.jpg;*.png;.tif;*.gif;*.ico;*.jpeg*.wdp*"                                   // Common pics
            + "|jpg| *.jpg;*.jpeg*"                                                                         // JPG
            + "|bmp|*.bmp;"                                                                                 // BMP
            + "|PNG|*.png;"                                                                                 // PNG
            + "|gif|*.gif;"                                                                                 // GIF
            + "|ico|*.ico;"                                                                                 // ICO
            + "|wdp|*.wdp;"                                                                                 // WDP
            + "|svg|*.svg;"                                                                                 // SVG
            + "|tif|*.tif;"                                                                                 // Tif
            + "|Photoshop|*.psd;*.psb"                                                                      // PSD
            + "|Archives|*.zip;*.7zip;*.7z;*.rar;*.bzip2;*.tar;*.wim;*.iso;*.cab"                           // Archives
            + "|Comics|*.cbr;*.cb7;*.cbt;*.cbz;*.xz"                                                        // Comics
            + "|Camera files|*.orf;*.cr2;*.crw;*.dng;*.raf;*.ppm;*.raw;*.mrw;*.nef;*.pef;*.3xf;*.arw";      // Camera files

        public const int picGalleryItem_Size = 260;
        public const int picGalleryItem_Size_s = picGalleryItem_Size - 30;

        public const int zoomSpeed = 45;

        ///// <summary>
        ///// File path of current image
        ///// </summary>
        //public static string Pics[FolderIndex] { get; set; }

        /// <summary>
        /// Backup of Previous file, if changed folder etc.
        /// </summary>
        public static string xPicPath;

        /// <summary>
        /// File path for the extracted folder
        /// </summary>
        public static string TempZipPath { get; set; }

        /// <summary>
        /// File path for the extracted zip file
        /// </summary>
        public static string TempZipFile { get; set; }

        public static bool LeftbuttonClicked { get; set; }
        public static bool RightbuttonClicked { get; set; }
        public static bool FastPicRunning { get; set; }
        public static bool isZoomed { get; set; }
        public static bool Flipped { get; set; }
        public static bool CanNavigate { get; set; }
        public static bool FreshStartup { get; set; }
        public static bool AutoScrolling { get; set; }
        public static bool ClickArrowRightClicked { get; set; }
        public static bool ClickArrowLeftClicked { get; set; }
        public static bool Reverse { get; set; }
        public static bool IsDialogOpen { get; set; }
        public static bool IsUnsupported { get; set; }

        /// <summary>
        /// Backup of Width data
        /// </summary>
        public static double xWidth;

        /// <summary>
        /// Backup of Height data
        /// </summary>
        public static double xHeight;

        /// <summary>
        /// Counter used to get/set current index
        /// </summary>
        public static int FolderIndex { get; set; }

        ///// <summary>
        ///// Backup of FolderIndex
        ///// </summary>
        //public static int xFolderIndex;

        /// <summary>
        /// Counter used to check if preloading is neccesary.
        /// </summary>
        public static int PreloadCount { get; set; }

        /// <summary>
        /// Used to get and set Aspect Ratio
        /// </summary>
        public static double AspectRatio { get; set; }

        /// <summary>
        /// Used to get and set image rotation by degrees
        /// </summary>
        public static int Rotateint { get; set; }

        public static Point origin;
        public static Point start;

        /// <summary>
        /// Starting point of AutoScroll
        /// </summary>
        public static Point? autoScrollOrigin;
        /// <summary>
        /// Current point of AutoScroll
        /// </summary>
        public static Point autoScrollPos;

        public static ScaleTransform st;
        public static TranslateTransform tt;

        /// <summary>
        /// List of file paths to supported files
        /// </summary>
        public static List<string> Pics { get; set; }


        //public static List<ImageSource> Images { get; set; }
        /// <summary>
        /// Timer used to continously scroll with AutoScroll
        /// </summary>
        public static Timer autoScrollTimer;

        /// <summary>
        /// Timer used to hide interface and/or scrollbar
        /// </summary>
        public static Timer activityTimer;

        /// <summary>
        /// Timer used for FastPic()
        /// </summary>
        //public static Timer fastPicTimer;

        /// <summary>
        /// Timer used for hide/show cursor.
        /// </summary>
        //public static Timer HideCursorTimer;

        /// <summary>
        /// Timer used to check if mouse is idle.
        /// </summary>
        //public static Timer MouseIdleTimer;

        /// <summary>
        /// Timer used for slideshow
        /// </summary>
        public static Timer SlideTimer;

        /// <summary>
        /// Backup of image
        /// </summary>
        public static ImageSource prevPicResource;

        /// <summary>
        /// Helper for user color settings
        /// </summary>
        public static Color backgroundBorderColor;
        /// <summary>
        /// Helper for user color settings
        /// </summary>
        public static Color mainColor;

    }
}
