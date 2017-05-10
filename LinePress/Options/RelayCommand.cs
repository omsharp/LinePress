using System;
using System.Windows.Input;

namespace LinePress.Options
{
   public class RelayCommand<T> : ICommand
   {
      private Predicate<T> canExecute;
      private Action<T> execute;

      public RelayCommand(Predicate<T> canExecute, Action<T> execute)
      {
         this.canExecute = canExecute;
         this.execute = execute;
      }

      public event EventHandler CanExecuteChanged
      {
         add
         {
            if (canExecute != null)
               CommandManager.RequerySuggested += value;
         }
         remove
         {
            if (canExecute != null)
               CommandManager.RequerySuggested -= value;
         }
      }

      public bool CanExecute(object parameter)
      {
         return canExecute((T)parameter);
      }

      public void Execute(object parameter)
      {
         execute((T)parameter);
      }
   }
}
