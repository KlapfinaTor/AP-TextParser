using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using AP_TextParser_Klapf.Services;
using System.Collections.ObjectModel;
using AP_TextParser_Klapf.Models;
using System.Threading;


namespace AP_TextParser_Klapf.Viewmodels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _filePath;
        private string _statusText;
        private int _progressBarValue;
        private bool _isProcessing;
        public event PropertyChangedEventHandler PropertyChanged;

        public DelegateCommand ProcessCommand { get; set; }
        public DelegateCommand SelectFileCommand { get; set; }
        public DelegateCommand CancelProcessingCommand { get; set; }
        private FileHandlerService _fileHandlerService;
        private CancellationTokenSource src;
        public ObservableCollection<WordData> TableData { get; set; } = new ObservableCollection<WordData>();


        public string StatusText
        {
            get => _statusText;
            set
            {
                if (_statusText != value)
                {
                    _statusText = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusText)));
                }
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                if (_isProcessing != value)
                {
                    _isProcessing = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsProcessing)));
                    this.CancelProcessingCommand.RaiseCanExecuteChanged();
                    this.ProcessCommand.RaiseCanExecuteChanged();
                    this.SelectFileCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProgressBarValue)));
            }
        }


        public string FilePath
        {
            get => _filePath;
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilePath)));
                    this.SelectFileCommand.RaiseCanExecuteChanged();
                    this.ProcessCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public MainWindowViewModel()
        {
            _fileHandlerService = new FileHandlerService();
            IsProcessing = false;

            this.ProcessCommand = new DelegateCommand(
                (o) =>
                {
                    if (String.IsNullOrEmpty(FilePath) | IsProcessing)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                },

                (o) => { _ = StartProcessing(); });
            this.SelectFileCommand = new DelegateCommand(
                (o) => !IsProcessing,
                (o) => { OpenFileDialog(); });
            this.CancelProcessingCommand = new DelegateCommand(
                 (o) => IsProcessing,
                (o) => { src.Cancel(); });

            this.FilePath = "";
        }

        /// <summary>
        /// Starts to process the file that was choosen in the select file dialog. 
        /// The result of the processing is stored in the table TableData.
        /// </summary>
        /// <returns></returns>
        private async Task StartProcessing()
        {
            src = new CancellationTokenSource();
            CancellationToken ct = src.Token;
            ct.Register(() => Console.WriteLine("Aborting File Processing!"));

            if (IsProcessing) { return; }
            try
            {
                ProgressBarValue = 10;
                IsProcessing = true;
                var data = new List<WordData>();
                TableData.Clear();
                StatusText = "";

                await Task.Run(() =>
                {
                    data = _fileHandlerService.ProcessAnsiFile(FilePath);
                });

                ct.ThrowIfCancellationRequested();


                ProgressBarValue = 50;

                if (data != null)
                {
                    foreach (var word in data)
                    {
                        TableData.Add(word);
                    }
                }
                else
                {
                    StatusText = "There was an error while processing the file!";
                    ResetStates();
                }

                ProgressBarValue = 100;
                IsProcessing = false;
            }
            catch (OperationCanceledException ce)
            {
                Console.WriteLine("Processing canceld by user");
                ResetStates();
            }
            catch (Exception ex)
            {
                ResetStates();
                if (ex.Source != null)
                    Console.WriteLine("Exception while Processing: {0}", ex.Source);
            }
        }
        /// <summary>
        /// Resets the table, the processing state and the progressbar
        /// </summary>
        private void ResetStates()
        {
            TableData.Clear();
            IsProcessing = false;
            ProgressBarValue = 0;
        }

        /// <summary>
        /// Open a windows file dialog with a .txt filter for text documents.
        /// </summary>
        private void OpenFileDialog()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = openFileDialog.ShowDialog();
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Filter = "Text documents (.txt)|*.txt";

            if (result == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

    }


}
