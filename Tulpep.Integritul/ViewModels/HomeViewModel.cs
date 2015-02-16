using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;

namespace Tulpep.Integritul.ViewModels
{
    public class HomeViewModel : Screen
    {
        public async Task GenerateIntegrity()
        {
            string folderToScan;

            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.Description = "Select the folder you want to scan";
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderToScan = folderDialog.SelectedPath;
            }
            else
            {
                return;
            }

            string integrityFile;
            System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
            saveDialog.Filter = "Integritier Database Files (*.integritier)|*.integritier";
            saveDialog.DefaultExt = "integritier";
            saveDialog.FileName = "IntegritierDatabase";
            saveDialog.ValidateNames = true;
            saveDialog.Title = "Select where you want to save the integrity file";
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                integrityFile = saveDialog.FileName;
            }
            else
            {
                return;
            }

            ProgressDialogController progressDialog = await ShowProgressAsync("Please Wait", String.Empty);
            var progressHandler = new Progress<string>(value =>
            {
                progressDialog.SetMessage(value);
            });
            var progress = progressHandler as IProgress<string>;
            await Task.Run(() => {
                InitialStatusOfFolder(folderToScan, integrityFile, progress);
            });
            await progressDialog.CloseAsync();
            await ShowMessageAsync("Done", "Integrity file created in " + integrityFile, MessageDialogStyle.Affirmative);

        }

        public async Task CompareIntegrity()
        {
            string folderToScan;

            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.Description = "Select the folder you want to scan";
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderToScan = folderDialog.SelectedPath;
            }
            else
            {
                return;
            }

            string integrityFile;
            System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
            saveDialog.Filter = "Integritier Database Files (*.integritier)|*.integritier";
            saveDialog.DefaultExt = "integritier";
            saveDialog.FileName = "IntegritierDatabase";
            saveDialog.ValidateNames = true;
            saveDialog.Title = "Select where you want to save the integrity file";
            if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                integrityFile = saveDialog.FileName;
            }
            else
            {
                return;
            }

            ProgressDialogController progressDialog = await ShowProgressAsync("Please Wait", String.Empty);
            var progressHandler = new Progress<string>(value =>
            {
                progressDialog.SetMessage(value);
            });
            var progress = progressHandler as IProgress<string>;
            await Task.Run(() =>
            {
                CompareFolder(folderToScan, integrityFile, progress);
            });
            await progressDialog.CloseAsync();
            await ShowMessageAsync("Done", "Integrity file created in " + integrityFile, MessageDialogStyle.Affirmative);

        }

        private static void InitialStatusOfFolder(string folder, string zipFile, IProgress<string> progress)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var fileName in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
            {
                progress.Report(fileName);
                result.Add(fileName, GetChecksum(fileName));                
            }

            File.WriteAllText(zipFile, JsonConvert.SerializeObject(result, Formatting.Indented));
        }
        private static void CompareFolder(string folder, string zipFile, IProgress<string> progress)
        {
            Dictionary<string, string> original = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(zipFile));
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var fileName in Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories))
            {
                progress.Report(fileName);
                if(original.ContainsKey(fileName))
                {
                    if(original[fileName] == GetChecksum(fileName))
                    {
                        result.Add(fileName, "Ok");
                    }
                    else
                    {
                        result.Add(fileName, "Changed");
                    }
                }
                else
                {
                    result.Add(fileName, "New File");
                }

            }
        }

        private static async Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle messageStyle)
        {
            MetroWindow metroWindow = (Application.Current.MainWindow as MetroWindow);
            return await metroWindow.ShowMessageAsync(title, message, messageStyle,
            new MetroDialogSettings
            {
                AnimateShow = false,
                AnimateHide = false,
            });
        }

        private async Task<ProgressDialogController> ShowProgressAsync(string title, string message)
        {
            MetroWindow metroWindow = (Application.Current.MainWindow as MetroWindow);
            return await metroWindow.ShowProgressAsync(title, message, false,
            new MetroDialogSettings
            {
                AnimateShow = false,
                AnimateHide = false
            }
            );
        }


        private static string GetChecksum(string file)
        {
            try
            {
                using (var stream = new BufferedStream(File.OpenRead(file), 1200000))
                {
                    //SHA256Managed hash = new SHA256Managed();
                    MD5 hash = new MD5CryptoServiceProvider();
                    byte[] checksum = hash.ComputeHash(stream);
                    return BitConverter.ToString(checksum).Replace("-", String.Empty);
                }

            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
