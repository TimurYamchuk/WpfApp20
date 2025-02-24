using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp20
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            UpdateProcesses();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _timer.Tick += (sender, e) => UpdateProcesses();
            _timer.Start();
        }

        private void UpdateProcesses(object sender, RoutedEventArgs e) => UpdateProcesses();

        private void EndProcess(object sender, RoutedEventArgs e)
        {
            if (ProcessList.SelectedItem is Processes selectedProcess)
            {
                TryKillProcess(selectedProcess);
            }
            else
            {
                MessageBox.Show("Выберите процесс!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TryKillProcess(Processes selectedProcess)
        {
            try
            {
                Process.GetProcessById(selectedProcess.ProcId).Kill();
                UpdateProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProcesses()
        {
            int? selectedProcessId = GetSelectedProcessId();
            ProcessList.Items.Clear();

            var processes = GetRunningProcesses();

            foreach (var process in processes)
            {
                ProcessList.Items.Add(process);
            }

            RestoreSelection(selectedProcessId);
        }

        private int? GetSelectedProcessId()
        {
            return ProcessList.SelectedItem is Processes selectedProcess ? selectedProcess.ProcId : (int?)null;
        }

        private List<Processes> GetRunningProcesses()
        {
            return Process.GetProcesses()
                          .OrderBy(p => p.ProcessName)
                          .Select(p => new Processes(p.Id, p.ProcessName + ".exe", p))
                          .ToList();
        }

        private void RestoreSelection(int? selectedProcessId)
        {
            if (selectedProcessId.HasValue)
            {
                foreach (Processes process in ProcessList.Items)
                {
                    if (process.ProcId == selectedProcessId.Value)
                    {
                        ProcessList.SelectedItem = process;
                        break;
                    }
                }
            }
        }

        private void RunProcess(object sender, RoutedEventArgs e)
        {
            string path = ProcessPath.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Введите путь!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            TryRunProcess(path);
        }

        private void TryRunProcess(string path)
        {
            try
            {
                Process.Start(path);
                UpdateProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
