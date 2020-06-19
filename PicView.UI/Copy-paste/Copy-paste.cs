﻿using PicView.PreLoading;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using static PicView.Error_Handling;
using static PicView.Fields;
using static PicView.FileFunctions;
using static PicView.FileLists;
using static PicView.Navigation;
using static PicView.Tooltip;

namespace PicView
{
    internal static class Copy_Paste
    {
        /// <summary>
        /// Copy image location to clipboard
        /// </summary>
        internal static void CopyText()
        {
            Clipboard.SetText(Pics[FolderIndex]);
            ShowTooltipMessage(TxtCopy);
        }

        /// <summary>
        /// Add file to clipboard
        /// </summary>
        internal static void Copyfile()
        {
            if (Pics == null)
            {
                return;
            }

            if (Pics.Count == 0)
            {
                CopyBitmap();
                return;
            }

            // Copy pic if from web
            if (string.IsNullOrWhiteSpace(Pics[FolderIndex]) || Uri.IsWellFormedUriString(Pics[FolderIndex], UriKind.Absolute))
            {
                CopyBitmap();
            }
            else
            {
                Copyfile(Pics[FolderIndex]);
            }
        }

        /// <summary>
        /// Add file to clipboard
        /// </summary>
        internal static void Copyfile(string path)
        {
            var paths = new System.Collections.Specialized.StringCollection { path };
            Clipboard.SetFileDropList(paths);
            ShowTooltipMessage(FileCopy);
        }

        internal static void CopyBitmap()
        {
            if (Pics.Count == 0 && mainWindow.img.Source != null)
            {
                Clipboard.SetImage((BitmapSource)mainWindow.img.Source);
            }
            else if (Preloader.Contains(Pics[FolderIndex]))
            {
                Clipboard.SetImage(Preloader.Load(Pics[FolderIndex]));
            }
            else if (mainWindow.img.Source != null)
            {
                Clipboard.SetImage((BitmapSource)mainWindow.img.Source);
            }
            else
            {
                return;
            }

            ShowTooltipMessage("Copied Image to clipboard");
        }


        /// <summary>
        /// Retrieves the data from the clipboard and attemps to load image, if possible
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison", Justification = "<Pending>")]
        internal static void Paste()
        {
            // file

            if (Clipboard.ContainsFileDropList()) // If Clipboard has one or more files
            {
                var files = Clipboard.GetFileDropList().Cast<string>().ToArray();

                if (files != null)
                {
                    var x = files[0];

                    if (Pics.Count != 0)
                    {
                        // If from same folder
                        if (!string.IsNullOrWhiteSpace(Pics[FolderIndex]) && Path.GetDirectoryName(x) == Path.GetDirectoryName(Pics[FolderIndex]))
                        {


                            Pic(Pics.IndexOf(x));
                        }
                        else
                        {
                            Pic(x);
                        }
                    }
                    else
                    {
                        Pic(x);
                    }

                    if (files.Length > 1)
                    {
                        for (int i = 1; i < files.Length; i++)
                        {
                            using var n = new Process();
                            n.StartInfo.FileName = Assembly.GetExecutingAssembly().Location;
                            n.StartInfo.Arguments = files[i];
                            n.Start();
                        }
                    }
                    return;
                }
            }

            // Clipboard Image
            if (Clipboard.ContainsImage())
            {
                Pic(Clipboard.GetImage(), "Clipboard Image");
                return;
            }

            // text/string/adddress

            var s = Clipboard.GetText(TextDataFormat.Text);

            if (string.IsNullOrEmpty(s))
            {
                return;
            }

            if (Base64.IsBase64String(s))
            {
                Pic64(s);
                return;
            }

            if (FilePathHasInvalidChars(s))
            {
                MakeValidFileName(s);
            }

            s = s.Replace("\"", "");
            s = s.Trim();

            if (File.Exists(s))
            {
                Pic(s);
            }
            else if (Directory.Exists(s))
            {
                ChangeFolder();
                Pics = FileList(s);
                if (Pics.Count > 0)
                {
                    Pic(Pics[0]);
                }
                else if (Pics.Count == 0)
                {
                    Unload();
                }
                else if (!string.IsNullOrWhiteSpace(Pics[FolderIndex]))
                {
                    Pic(Pics[FolderIndex]);
                }
                else
                {
                    Unload();
                }
            }
            else if (Uri.IsWellFormedUriString(s, UriKind.Absolute)) // Check if from web
            {
                LoadFromWeb.PicWeb(s);
            }
            else
            {
                ShowTooltipMessage("An error occured while trying to paste file");
            }
        }

        /// <summary>
        /// Add file to move/paste clipboard
        /// </summary>
        /// <param name="path"></param>
        internal static void Cut(string path)
        {
            var x = new System.Collections.Specialized.StringCollection
            {
                path
            };

            byte[] moveEffect = new byte[] { 2, 0, 0, 0 };
            using (var dropEffect = new MemoryStream())
            {
                dropEffect.Write(moveEffect, 0, moveEffect.Length);

                DataObject data = new DataObject();
                data.SetFileDropList(x);
                data.SetData("Preferred DropEffect", dropEffect);

                Clipboard.Clear();
                Clipboard.SetDataObject(data, true);
            }
            // Force Preloader to add new images, to minimize slowdown errors
            PreloadCount = 4;

            ShowTooltipMessage(ImageCut);
        }
    }
}
