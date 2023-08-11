using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;

namespace TxEd
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
			cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
		}

		private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
		{
			object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
			btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
			temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
			btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
			temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
			btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

			temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
			cmbFontFamily.SelectedItem = temp;
			temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
			cmbFontSize.Text = temp.ToString();
		}

		private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
				TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
				ofd.ShowDialog();
				tr.Text = File.ReadAllText(ofd.FileName);
			}
			catch { }
		}

		private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
			TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
			if  (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			File.WriteAllText(sfd.FileName, tr.Text);
		}

		private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cmbFontFamily.SelectedItem != null)
				rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
		}

		private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
		{
			rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
		}


        private void LineHeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			var value = (LineHeight.SelectedItem as TextBlock).Text;
			if (value.Length == 0) return;
			rtbEditor.SetValue(TextBlock.LineHeightProperty, (double)new LengthConverter().ConvertFrom(value));
		}

		private void PrintRTBContent(object sender, RoutedEventArgs e)
        {
			System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
			if ((pd.ShowDialog() == true))
			{
				//use either one of the below
				pd.PrintVisual(rtbEditor as Visual, "printing as visual");
				pd.PrintDocument((((IDocumentPaginatorSource)rtbEditor.Document).DocumentPaginator), "printing as paginator");
			}
		}


        private void Button_Click(object sender, RoutedEventArgs e)
        {
			ColorDialog col = new ColorDialog();
			if (col.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				var wpcol = System.Windows.Media.Color.FromArgb(col.Color.A, col.Color.R, col.Color.G, col.Color.B);
				rtbEditor.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(wpcol));
			}
		}

        private void Save_and_exit_Click(object sender, RoutedEventArgs e)
        {
			System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
			TextRange tr = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
			sfd.ShowDialog();
			File.WriteAllText(sfd.FileName, tr.Text);
			Environment.Exit(1);
		}

        private void Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
			Environment.Exit(1);
		}
    }
}
