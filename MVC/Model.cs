﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FileNameChanger.MVC
{
    public class Model
    {
        public static List<string> selectedFiles = new List<string>();

        public string Prefix = "";
        public string Suffix = "";

        public bool AutoInc = false;
        public bool AutoABC = false;
        public void RenameFiles(TextBox txtNames)
        {
            var newNames = txtNames.Text
                 .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (newNames.Length != selectedFiles.Count)
            {
                MessageBox.Show("The number of new names must match the number of selected files.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < selectedFiles.Count; i++)
            {
                string oldPath = selectedFiles[i];
                string directory = Path.GetDirectoryName(oldPath);
                string extension = Path.GetExtension(oldPath);
                string newName = newNames[i].Trim();

                if (string.IsNullOrWhiteSpace(newName) || newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    MessageBox.Show($"Invalid name: '{newName}'");
                    continue;
                }

                // Applying File Names
                string newPath = Path.Combine(directory,GetFileName(newName,i) + extension);

                try
                {
                    File.Move(oldPath, newPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to rename '{oldPath}': {ex.Message}", "Failed", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("Files renamed successfully.", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public string GetFileName(string RawName,int index)
        {
            string temp = "";
            index++;
            if (AutoInc)
            {
                temp = index.ToString();
            }
            else if(AutoABC)
            {
                temp = ((char)('A' + index-1)).ToString();
            }
            
            temp += Prefix + RawName + Suffix;

            return temp;
        }
    }
}
