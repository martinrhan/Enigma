﻿using System;
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
using Enigma.Spacial.TestWPF.Models;
using Enigma.Spacial.TestWPF.Visual;

namespace Enigma.Spacial.TestWPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            TestSpaceViewModel = new TestSpaceViewModel();
            InitializeComponent();
            TestSpaceViewModel.AssignModel(new TestSpace());
        }

        public TestSpaceViewModel TestSpaceViewModel { get; }
    }
}
