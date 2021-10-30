﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using ScsLib.HashFileSystem;
using ScsLib.HashFileSystem.Named;
using ScsLib.HashFileSystem.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

				filePath = dialog.FileName;

				IHashFsReader hashFsReader = _services.GetRequiredService<IHashFsReader>();

				using (FileStream fileStream = hashFsReader.Open(filePath))
				{
					hashFs = await hashFsReader.ReadAsync(fileStream).ConfigureAwait(true);
				}

				INamedHashDirectoryReader namedHashDirectoryReader = _services.GetRequiredService<INamedHashDirectoryReader>();

				IEnumerable<INamedHashEntry> BuildTree(NamedHashDirectory namedHashDirectory)
				{
					return namedHashDirectoryReader.Read(hashFs, namedHashDirectory).Select(row =>
					{
						if (row is NamedHashDirectory currentDirectory)
						{
							return new NamedHashDirectoryTree
							{
								Header = currentDirectory.Header,
								VirtualPath = currentDirectory.VirtualPath,
								Entries = BuildTree(currentDirectory)
							};
						}
						else
						{
							return row;
						}
					});
				}

				trvBrowser.ItemsSource = BuildTree(hashFs.RootDirectory);

				progress.IsIndeterminate = false;
			}
		}

		private async void trvBrowser_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			if (filePath != null && trvBrowser.SelectedItem is HashFile file)
			{
				progress.IsIndeterminate = true;

				IHashFsReader hashFsReader = _services.GetRequiredService<IHashFsReader>();
				IHashEntryReader hashEntryReader = _services.GetRequiredService<IHashEntryReader>();

				using (FileStream fileStream = hashFsReader.Open(filePath))
				{
					trvText.Text = await hashEntryReader.ReadStringAsync(fileStream, file).ConfigureAwait(true);
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
