using Microsoft.Win32;
using ScsLib.HashFileSystem;
using System.Text;
using System.Windows;

namespace ScsLib.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ScsFile? scsFile;

		public MainWindow()
		{
			InitializeComponent();
		}

		private async void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				DefaultExt = ".scs",
				Filter = "SCS files|*.scs"
			};

			if (dialog.ShowDialog() == true)
			{
				progress.IsIndeterminate = true;

				scsFile?.Dispose();
				scsFile = await ScsFile.Read(dialog.FileName).ConfigureAwait(true);

				trvBrowser.ItemsSource = scsFile.RootDirectory.Entries;

				progress.IsIndeterminate = false;
			}
		}

		private async void trvBrowser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (scsFile != null && trvBrowser.SelectedItem is HashFile file)
			{
				progress.IsIndeterminate = true;

				byte[] content = await scsFile.ReadFile(file).ConfigureAwait(true);

				trvText.Text = Encoding.UTF8.GetString(content);

				progress.IsIndeterminate = false;
			}
			else
			{
				trvText.Text = "";
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			scsFile?.Dispose();
		}
	}
}
