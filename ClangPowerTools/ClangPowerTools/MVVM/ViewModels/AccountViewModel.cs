﻿using ClangPowerTools.MVVM.Commands;
using ClangPowerTools.MVVM.LicenseValidation;
using ClangPowerTools.MVVM.Models;
using ClangPowerTools.MVVM.Views;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace ClangPowerTools
{
  public class AccountViewModel : INotifyPropertyChanged
  {
    #region Members

    private AccountModel accountModel;
    private GeneralSettingsModel generalModel;

    private ICommand logoutCommand;

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion


    #region Constructor

    public AccountViewModel()
    {
      accountModel = SettingsProvider.AccountModel;
      generalModel = SettingsProvider.GeneralSettingsModel;
    }

    #endregion


    #region Properties

    public AccountModel AccountModel
    {
      get
      {
        SetAccountNameInTrial();
        return accountModel;
      }
      set
      {
        accountModel = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccountModel"));
      }
    }

    public GeneralSettingsModel GeneralSettingsModel
    {
      get
      {
        return generalModel;
      }
      set
      {
        generalModel = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GeneralSettingsModel"));
      }
    }

    public bool CanExecute
    {
      get
      {
        return true;
      }
    }

    #endregion


    #region Commands

    public ICommand LogoutCommand
    {
      get => logoutCommand ??= new RelayCommand(() => Logout(), () => CanExecute);
    }

    #endregion


    #region Methods

    private void Logout()
    {
      var settingsPathBuilder = new SettingsPathBuilder();
      string path = settingsPathBuilder.GetPath("ctpjwt");

      if (File.Exists(path))
        File.Delete(path);

      SettingsProvider.SettingsView.Close();
      LoginView loginView = new LoginView();
      loginView.ShowDialog();
    }


    private void SetAccountNameInTrial()
    {
      if (accountModel.LicenseType == LicenseType.Trial)
      {
        accountModel.UserName = "Trial Account";
      }
    }

    #endregion
  }
}
