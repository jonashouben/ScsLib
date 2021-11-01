using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using ScsLib.HashFileSystem;
using ScsLib.HashFileSystem.Named;
using ScsLib.HashFileSystem.Reader;
using ScsLib.ThreeNK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ScsLib.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly IServiceProvider _services;
		private string? filePath;
		private HashFs? hashFs;

		public MainWindow()
		{
			ServiceCollection services = new ServiceCollection();
			services.AddScsLib();
			services.AddThreeNK();
			_services = services.BuildServiceProvider();
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

				IHashFsReader hashFsReader = _services.GetRequiredService<IHashFsReader>();

				using (FileStream fileStream = hashFsReader.Open(dialog.FileName))
				{
					if (await hashFsReader.HasSignatureAsync(fileStream).ConfigureAwait(true))
					{
						hashFs = await hashFsReader.ReadAsync(fileStream).ConfigureAwait(true);
						filePath = dialog.FileName;
					}
					else
					{
						progress.IsIndeterminate = false;
						MessageBox.Show("This is not an scs archive. Please try opening as zip");
						return;
					}
				}

				INamedHashDirectoryReader namedHashDirectoryReader = _services.GetRequiredService<INamedHashDirectoryReader>();

				NamedHashDirectoryTree BuildTree(NamedHashDirectory namedHashDirectory)
				{
					return new NamedHashDirectoryTree
					{
						Header = namedHashDirectory.Header,
						VirtualPath = namedHashDirectory.VirtualPath.Length == 0 ? (namedHashDirectory.IsManual ? "(manual root)" : "(root)") : namedHashDirectory.VirtualPath,
						IsExpanded = namedHashDirectory.VirtualPath.Length == 0,
						Entries = namedHashDirectoryReader.Read(hashFs, namedHashDirectory).Select(row =>
						{
							if (row is NamedHashDirectory currentDirectory)
							{
								return BuildTree(currentDirectory);
							}
							else
							{
								return row;
							}
						}),
						IsManual = namedHashDirectory.IsManual
					};
				}

				NamedHashDirectoryTree tree = BuildTree(hashFs.RootDirectory);

				IEnumerable<ulong> BuildHashes(IEnumerable<INamedHashEntry> tree)
				{
					foreach (INamedHashEntry entry in tree)
					{
						yield return entry.Header.Hash;

						if (entry is NamedHashDirectoryTree treeEntry)
						{
							foreach (ulong hash in BuildHashes(treeEntry.Entries))
							{
								yield return hash;
							}
						}
					}
				}

				HashSet<ulong> hashes = BuildHashes(tree.Entries.Prepend(tree)).ToHashSet();

				IReadOnlyCollection<IHashEntry> unnamed = hashFs.Entries.Where(row => !hashes.Contains(row.Key)).Select(row => row.Value).ToArray();

				trvBrowser.ItemsSource = unnamed.Prepend(tree).ToArray();

				progress.IsIndeterminate = false;
			}
		}

		private async void trvBrowser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (filePath != null && trvBrowser.SelectedItem is HashEntry file)
			{
				progress.IsIndeterminate = true;

				IHashFsReader hashFsReader = _services.GetRequiredService<IHashFsReader>();
				IHashEntryReader hashEntryReader = _services.GetRequiredService<IHashEntryReader>();
				IThreeNKReader threeNKReader = _services.GetRequiredService<IThreeNKReader>();

				using (FileStream fileStream = hashFsReader.Open(filePath))
				{
					byte[] data = await hashEntryReader.ReadAsync(fileStream, file).ConfigureAwait(true);

					using (MemoryStream ms = new MemoryStream(data))
					{
						if (await threeNKReader.HasSignatureAsync(ms).ConfigureAwait(true))
						{
							data = await threeNKReader.ReadAsync(ms).ConfigureAwait(true);
						}
					}

					trvText.Text = Encoding.UTF8.GetString(data);
				}

				progress.IsIndeterminate = false;
			}
			else
			{
				trvText.Text = "";
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}
	}
}
