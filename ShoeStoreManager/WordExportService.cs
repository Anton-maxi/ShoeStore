using System;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Windows;
using Word = Microsoft.Office.Interop.Word;

namespace ShoeStoreManager
{
    public class WordExportService
    {
        private Word.Application? wordApp;
        private Word.Document? wordDoc;
        private readonly string filePath;

        public WordExportService()
        {
            filePath = Environment.CurrentDirectory;
        }

        private void ReplaceText(string findText, string replaceText)
        {
            if (wordApp == null)
            {
                throw new InvalidOperationException("Word application is not initialized.");
            }

            Word.Find findObject = wordApp.Selection.Find;
            findObject.ClearFormatting();
            findObject.Text = findText;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = replaceText;

            object replaceAll = Word.WdReplace.wdReplaceAll;
            object missing = Type.Missing;

            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);
        }

        public void WriteData(DataTable shoesData, string? searchName, int? minPrice, int? maxPrice, string? categoryName)
        {
            string templatePath = Path.Combine(filePath, "ReportTemplate.docx");

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadsPath = Path.Combine(userProfile, "Downloads");
            string reportsDir = Path.Combine(downloadsPath, "Звіти_Магазину_Взуття");

            if (!Directory.Exists(reportsDir))
            {
                Directory.CreateDirectory(reportsDir);
            }

            string savePath = Path.Combine(reportsDir, $"Звіт_{DateTime.Now:yyyyMMdd_HHmmss}.docx");

            if (!File.Exists(templatePath))
            {
                MessageBox.Show($"Файл шаблону не знайдено:\n{templatePath}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                wordApp = new Word.Application { Visible = false };
                wordDoc = wordApp.Documents.Add(templatePath);

                // Назва
                if (!string.IsNullOrWhiteSpace(searchName) && searchName != "Назва")
                {
                    ReplaceText("Введена назва:", $"Введена назва: {searchName}");
                }
                else
                {
                    // Видаляємо маркер разом із перенесенням рядка
                    ReplaceText("Введена назва:^p", "");
                }

                // Ціна
                string priceText = "";
                if (minPrice.HasValue && maxPrice.HasValue)
                    priceText = $"від {minPrice} до {maxPrice} грн";
                else if (minPrice.HasValue)
                    priceText = $"від {minPrice} грн";
                else if (maxPrice.HasValue)
                    priceText = $"до {maxPrice} грн";

                if (!string.IsNullOrEmpty(priceText))
                {
                    ReplaceText("Вибрана ціна:", $"Вибрана ціна: {priceText}");
                }
                else
                {
                    ReplaceText("Вибрана ціна:^p", "");
                }

                // Категорія
                if (!string.IsNullOrWhiteSpace(categoryName) && categoryName != "Всі категорії")
                {
                    ReplaceText("Обрана категорія взуття:", $"Обрана категорія взуття: {categoryName}");
                }
                else
                {
                    ReplaceText("Обрана категорія взуття:^p", "");
                }

                ReplaceText("<TIME>", $"{DateTime.Now:dd.MM.yyyy HH:mm}");

                // Заповнення таблиці
                if (wordDoc.Tables.Count > 0 && shoesData.Rows.Count > 0)
                {
                    Word.Table table = wordDoc.Tables[1];

                    for (int i = 0; i < shoesData.Rows.Count; i++)
                    {
                        if (i > 0) table.Rows.Add();

                        int rowIndex = i + 2;
                        table.Cell(rowIndex, 1).Range.Text = shoesData.Rows[i]["item_number"].ToString();
                        table.Cell(rowIndex, 2).Range.Text = shoesData.Rows[i]["name"].ToString();
                        table.Cell(rowIndex, 3).Range.Text = shoesData.Rows[i]["count"].ToString();
                        table.Cell(rowIndex, 4).Range.Text = shoesData.Rows[i]["price_one_pair"].ToString();
                    }
                }

                wordDoc.SaveAs2(savePath);
                Process.Start(new ProcessStartInfo { FileName = savePath, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при експорті: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                wordDoc?.Close(false);
                if (wordApp != null)
                {
                    wordApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                }
            }
        }
    }
}